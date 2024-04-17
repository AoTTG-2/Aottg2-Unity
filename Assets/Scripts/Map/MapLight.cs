using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

namespace Map
{
    public class MapLight //TODO: Wait untill rc actually adds this, this is only temporary 
    {
        public Light Light { get; private set; }

        public float cullingDistance = 10.0f; // The distance at which the light will be culled

        public MapLight(Light light)
        {
            this.Light = light;
        }

        public void UpdateCull(Transform targetTransform)
        {
            // Calculate the distance between the light and the target transform
            float distance = Vector3.Distance(Camera.main.transform.position, targetTransform.position);

            // If the distance is greater than the culling distance, disable the light
            if (distance > cullingDistance)
            {
                Light.enabled = false;
            }
            else
            {
                Light.enabled = true;
            }
        }
    }
}