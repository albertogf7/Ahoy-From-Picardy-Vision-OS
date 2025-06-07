using System.Collections.Generic;
using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace PolySpatial.Samples
{
    /// <summary>
    /// Current you can only select one object at a time and only supports a primary [0] touch
    /// </summary>
    public class HandleManipulation : MonoBehaviour
    {
        [SerializeField] private Transform _xrRig;
        [SerializeField] private GameObject _uiHandler;
        [SerializeField] private float rotationTime = 0.5f;

        private bool _initialMovement = false;
        [SerializeField]
        private float elapsedTime = 0f;

        struct Selection
        {
            public PieceSelectionBehavior Piece;
            public Vector3 PositionOffset;
            public Quaternion RotationOffset;
            public Quaternion TargetRotation; // Added for rotation towards XR Rig
        }

        readonly Dictionary<int, Selection> m_CurrentSelections = new();

        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
            _uiHandler.SetActive(false); // Ensure UI handler is initially disabled
        }

        void Update()
        {
            foreach (var touch in Touch.activeTouches)
            {
                var spatialPointerState = EnhancedSpatialPointerSupport.GetPointerState(touch);
                var interactionId = spatialPointerState.interactionId;

                if (spatialPointerState.Kind == SpatialPointerKind.Touch)
                    continue;

                var pieceObject = spatialPointerState.targetObject;
                if (pieceObject != null && pieceObject.CompareTag("Adjuster")) // Check for "Adjuster" tag
                {
                    if (pieceObject.TryGetComponent(out PieceSelectionBehavior piece) && piece.selectingPointer == -1)
                    {
                        if (!_initialMovement)
                        {
                            _initialMovement = true;
                            _uiHandler.SetActive(true); // Activate UI Handler
                        }

                        var pieceTransform = piece.transform;
                        var interactionPosition = spatialPointerState.interactionPosition;
                        var inverseDeviceRotation = Quaternion.Inverse(spatialPointerState.inputDeviceRotation);
                        var rotationOffset = inverseDeviceRotation * pieceTransform.rotation;
                        var positionOffset = inverseDeviceRotation * (pieceTransform.position - interactionPosition);
                        piece.SetSelected(interactionId);

                        Vector3 lookPos = _xrRig.position - pieceTransform.position;
                        Quaternion targetRotation = Quaternion.LookRotation(lookPos, Vector3.up);

                        if (m_CurrentSelections.TryGetValue(interactionId, out var sselection))
                            sselection.Piece.SetSelected(-1); // Use predefined constant for deselection

                        m_CurrentSelections[interactionId] = new Selection
                        {
                            Piece = piece,
                            RotationOffset = rotationOffset,
                            PositionOffset = positionOffset,
                            TargetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0) // Set target rotation
                        };
                    }
                }

                if (m_CurrentSelections.TryGetValue(interactionId, out var selection))
                {
                    switch (spatialPointerState.phase)
                    {
                        case SpatialPointerPhase.Moved:
                            UpdatePosition(selection, spatialPointerState.interactionPosition, spatialPointerState.inputDeviceRotation);
                            UpdateRotationTowardsTarget(ref selection);
                            break;
                        case SpatialPointerPhase.None:
                        case SpatialPointerPhase.Ended:
                        case SpatialPointerPhase.Cancelled:
                            DeselectPiece(interactionId);
                            break;
                    }
                }
            }
        }

        void UpdatePosition(Selection selection, Vector3 interactionPosition, Quaternion deviceRotation)
        {
            var position = interactionPosition + deviceRotation * selection.PositionOffset;
            selection.Piece.transform.position = position; // Apply position without changing rotation
        }

        void UpdateRotationTowardsTarget(ref Selection selection)
        {
            Vector3 targetDirection = _xrRig.position - selection.Piece.transform.position;
            targetDirection.y = 0; // Ignore vertical difference to focus rotation on Y axis
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Calculate the Y-axis rotation and apply it
            float newYRotation = targetRotation.eulerAngles.y;
            Quaternion currentRotation = selection.Piece.transform.rotation;
            Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, newYRotation, currentRotation.eulerAngles.z);

            selection.Piece.transform.rotation = Quaternion.Slerp(currentRotation, newRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
        }

        void DeselectPiece(int interactionId)
        {
            if (m_CurrentSelections.TryGetValue(interactionId, out var selection))
            {
                selection.Piece.SetSelected(-1); // Use predefined constant for deselection
                m_CurrentSelections.Remove(interactionId);

                // Reset manipulation state
                if (m_CurrentSelections.Count == 0)
                {
                    elapsedTime = 0f;
                }
            }
        }
    }
}