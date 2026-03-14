using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.VolumetricFogLite
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 rotation;

        void Start()
        {

        }

        void Update()
        {
            transform.Rotate(rotation * Time.deltaTime, Space.Self);
        }
    }
}
