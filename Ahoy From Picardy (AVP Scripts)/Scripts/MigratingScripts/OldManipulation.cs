using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace PolySpatial.Samples
{
    /// <summary>
    /// Current you can only select one object at a time and only supports a primary [0] touch
    /// </summary>
    public class OldManipulation : MonoBehaviour
    {
        //Adjuster Manipulation
        PieceSelectionSwapper m_CurrentSelection;

        [SerializeField]
        private Transform _xrRig;
        
        [SerializeField]
        private bool _initialMovement= false;
        [SerializeField]
        private GameObject _uiHandler;

        private Quaternion targetRotation;
        [SerializeField]
        private float rotationTime = 0.5f;
        private float elapsedTime = 0f;

        private void Awake()
        {
            _uiHandler.gameObject.SetActive(false);
        }
        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        void Update()
        {
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);

                if (primaryTouchData.Kind == SpatialPointerKind.DirectPinch || primaryTouchData.Kind == SpatialPointerKind.IndirectPinch)
                {
                    var pieceObject = primaryTouchData.targetObject;

                    
                    if (pieceObject != null && pieceObject.tag == "Adjuster")
                    {
                        if (pieceObject.TryGetComponent(out PieceSelectionSwapper piece))
                        {
                            m_CurrentSelection = piece;
                            m_CurrentSelection.Select(true, primaryTouchData.interactionPosition);

                            if (!_initialMovement)
                            {
                                _initialMovement = true;
                                _uiHandler.gameObject.SetActive(true);
                            }

                            //rotation
                            Vector3 lookPos = _xrRig.position - m_CurrentSelection.transform.position;
                            Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.up);

                            float eulerY = lookRot.eulerAngles.y;
                            targetRotation = Quaternion.Euler(0, eulerY, 0);
                            //m_CurrentSelection.transform.rotation = rotation;

                            RotateTowardsTarget();
                        }
                        
                    }

                    if (activeTouches[0].phase == TouchPhase.Moved)
                    {
                        if (m_CurrentSelection != null)
                        {
                            //position
                            m_CurrentSelection.transform.parent.SetPositionAndRotation(primaryTouchData.interactionPosition, Quaternion.identity);

                            
                        }
                    }

                    if (activeTouches[0].phase == TouchPhase.Ended || activeTouches[0].phase == TouchPhase.Canceled)
                    {
                        if (m_CurrentSelection != null)
                        {
                            m_CurrentSelection.Select(false, primaryTouchData.interactionPosition);
                            m_CurrentSelection = null;
                            elapsedTime = 0f;
                        }
                    }
                }
                else
                {
                    if (m_CurrentSelection != null)
                    {
                        m_CurrentSelection.Select(false, primaryTouchData.interactionPosition);
                        m_CurrentSelection = null;
                    }
                }
            }
        }
        void RotateTowardsTarget()
        {
            if (elapsedTime < rotationTime)
            {
                m_CurrentSelection.transform.rotation = Quaternion.Slerp(
                    m_CurrentSelection.transform.rotation,
                    targetRotation,
                    elapsedTime / rotationTime
                );

                // Update elapsed time
                elapsedTime += Time.deltaTime;
            }
            else
            {
                // Ensure the final rotation is exactly the target rotation
                m_CurrentSelection.transform.rotation = targetRotation;
            }
        }
    }
}
