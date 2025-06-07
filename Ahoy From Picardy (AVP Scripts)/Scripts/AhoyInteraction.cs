using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace PolySpatial.Samples
{
    public class AhoyInteraction : MonoBehaviour
    {
        [SerializeField]
        Transform m_InputAxisTransform;

        void OnEnable()
        {
            // enable enhanced touch support to use active touches for properly pooling input phases
            EnhancedTouchSupport.Enable();
        }

        void Update()
        {
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);
                if (activeTouches[0].phase == TouchPhase.Began)
                {
                    // allow balloons to be popped with a poke or indirect pinch
                    if (primaryTouchData.Kind == SpatialPointerKind.IndirectPinch || primaryTouchData.Kind == SpatialPointerKind.Touch)
                    {
                        var interactedObject = primaryTouchData.targetObject;
                        if (interactedObject != null && interactedObject.tag == "Pipete")
                        {
                            if (interactedObject.TryGetComponent(out PipeteBehaviour pipete))
                            {
                                pipete.Drop();
                            }
                        }

                        else if (interactedObject != null && interactedObject.tag == "SingleTap")
                        {
                            if (interactedObject.TryGetComponent(out SingleTapAnimation singleTap))
                            {
                                singleTap.SingleTap();
                            }
                        }

                        else if (interactedObject != null && interactedObject.tag == "Pillar")
                        {
                            if (interactedObject.TryGetComponent(out PillarButton pillar))
                            {
                                pillar.CallTracks();
                            }
                        }

                        else if (interactedObject != null && interactedObject.tag == "UIButton")
                        {
                            if (interactedObject.TryGetComponent(out UIButton button))
                            {
                                button.UIButtonPress();
                            }
                        }

                    

                        else if (interactedObject != null && interactedObject.tag == "ChangeScene")
                        {
                            if (interactedObject.TryGetComponent(out ChangeScene loader))
                            {
                                loader.LoadMainScene();
                            }
                        }

                        else if (interactedObject != null && interactedObject.tag == "Exit")
                        {
                            if(interactedObject.TryGetComponent(out GameManager exitManager))
                            {
                               // exitManager.RunExitAnim();
                            }
                        }


                    }

                    // update input gizmo
                    m_InputAxisTransform.SetPositionAndRotation(primaryTouchData.interactionPosition, primaryTouchData.inputDeviceRotation);
                }

                // visualize input gizmo while input is maintained
                if (activeTouches[0].phase == TouchPhase.Moved)
                {
                    m_InputAxisTransform.SetPositionAndRotation(primaryTouchData.interactionPosition, primaryTouchData.inputDeviceRotation);
                }
            }
        }
    }
}