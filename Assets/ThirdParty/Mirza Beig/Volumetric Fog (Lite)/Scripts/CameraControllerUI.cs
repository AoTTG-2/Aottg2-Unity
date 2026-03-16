using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

namespace MirzaBeig.VolumetricFogLite
{
    public class CameraControllerUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private int touchCount;

        void Start()
        {

        }

        void Update()
        {

        }

        void LateUpdate()
        {
            CameraController.Instance.inputEnabled = touchCount > 0;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            touchCount++;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            touchCount = Mathf.Max(0, touchCount - 1);
        }
    }
}
