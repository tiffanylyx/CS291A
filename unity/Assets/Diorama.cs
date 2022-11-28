using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Diorama : MonoBehaviour
{
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private Vector3 minResultingScale = Vector3.one * .1f;
    [SerializeField] private ARRaycastManager raycastManager;
    private readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private PinchState pinchState = PinchState.Off;
    private float initialPinchDistance;
    private Vector3 initialScale;

    private enum PinchState
    {
        Off = 0,
        Start,
        Stay,
        Stop
    }

    private void Update()
    {
        HandleTap();
        HandlePinch();
    }
    private void HandleTap()
    {
        if (Input.touchCount != 1)
            return;
        Touch touch = Input.GetTouch(0);
        if (raycastManager.Raycast(touch.position, hits, TrackableType.AllTypes))
        {
            var closestHit = hits[0].pose;
            transform.position = closestHit.position;
        }
    }

    private void HandlePinch()
    {
        if (Input.touchCount == 2)
        {
            Touch touchA = Input.touches[0];
            Touch touchB = Input.touches[1];
            float pinchDistance = Vector2.Distance(touchA.position, touchB.position);

            if (pinchState == PinchState.Off)
            {
                pinchState = PinchState.Start;
                OnPinchStart(touchA, touchB, pinchDistance);
            }
            else
            {
                pinchState = PinchState.Stay;
                OnPinchStay(touchA, touchB, pinchDistance);
            }
        }
        else
        {
            /* Handle OnPinchStop
            if (pinchState == PinchState.Start || pinchState == PinchState.Stay)
            {
                pinchState = PinchState.Stop;
                OnPinchStop();
            }
            */
            pinchState = PinchState.Off;
        }
        Debug.Log($"[DIORAMA] pinchState:{pinchState}");
    }

    private void OnPinchStart(Touch a, Touch b, float pinchDistance)
    {
        initialPinchDistance = pinchDistance;
        initialScale = transform.localScale;
        Debug.Log($"[DIORAMA] PinchStart \nA:{a.position}\nB:{b.position}\ninitialPinchDistance:{initialPinchDistance}\ninitialScale:{initialScale}");
    }

    private void OnPinchStay(Touch a, Touch b, float pinchDistance)
    {
        Vector3 resultScale = initialScale * (pinchDistance / initialPinchDistance) * scaleFactor;
        transform.localScale = Vector3.Max(resultScale, minResultingScale);
        Debug.Log($"[DIORAMA] PinchStay \nA:{a.position}\nB:{b.position}\ninitialPinchDistance:{initialPinchDistance}\ninitialScale:{initialScale}");
    }
}
