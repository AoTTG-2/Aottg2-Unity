using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.VolumetricFogLite
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public bool inputEnabled = true;

        [Space]

        public float rotationSpeed = 10.0f;
        public float rotationLerpSpeed = 10.0f;

        [Space]

        public float zoomLerpSpeed = 10.0f;

        [Header("READ ONLY")]

        [SerializeField]
        private float yaw = 0.0f;
        [SerializeField]
        private float pitch = 0.0f;

        Quaternion targetRotation;

        [Space]

        public int zoomMaxSteps = 5;
        public Vector2 zoomMinMax = new Vector2(-1.0f, 2.0f);

        float targetZoom;

        [Space]

        public Transform cameraZoom;

        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            targetRotation = transform.localRotation;
            targetZoom = cameraZoom.localPosition.z;
        }

        void Update()
        {
            // Pinch zoom.

            float zoomRange = zoomMinMax.y - zoomMinMax.x;
            float zoomStep = zoomRange / zoomMaxSteps;

            float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * 10.0f;

            bool pinchZoom = Input.touchCount == 2 && inputEnabled;

            if (pinchZoom)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                deltaMagnitudeDiff /= -200.0f;

                targetZoom += deltaMagnitudeDiff * zoomStep;
            }
            else
            {
                Vector3 mousePosition = Input.mousePosition;

                bool isWithinWindow =

                    mousePosition.x >= 0 && mousePosition.x <= Screen.width &&
                    mousePosition.y >= 0 && mousePosition.y <= Screen.height;

                if (isWithinWindow)
                {
                    targetZoom += mouseWheel * zoomStep;
                }
            }

            targetZoom = Mathf.Clamp(targetZoom, -zoomMinMax.y, -zoomMinMax.x);

            Vector3 targetZoomPosition = cameraZoom.localPosition;
            targetZoomPosition.z = targetZoom;

            cameraZoom.localPosition = Vector3.Lerp(cameraZoom.localPosition, targetZoomPosition, Time.deltaTime * zoomLerpSpeed);

            // Rotate.

            if (inputEnabled)
            {
                // Don't move on first click.

                Vector2 pointerDelta = Vector2.zero;

                if (Input.touchCount == 0)
                {
                    if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
                    {
                        pointerDelta.x = Input.GetAxisRaw("Mouse X");
                        pointerDelta.y = Input.GetAxisRaw("Mouse Y");
                    }
                }
                else if (Input.touchCount == 1)
                {
                    Touch firstTouch = Input.GetTouch(0);

                    if (firstTouch.phase == TouchPhase.Moved)
                    {
                        pointerDelta = firstTouch.deltaPosition / 10.0f;
                    }
                }

                pointerDelta.x /= Screen.width;
                pointerDelta.y /= Screen.height;

                pointerDelta *= rotationSpeed;

                yaw += pointerDelta.x * 360.0f;    // Normalize based on full screen width rotation
                pitch -= pointerDelta.y * 180.0f;  // Normalize based on half screen height rotation (to limit pitch)

                pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);

                targetRotation = Quaternion.Euler(pitch, yaw, 0.0f);
            }

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
        }
    }
}

