using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.VolumetricFogLite
{
    public class DemoController : MonoBehaviour
    {
        public int startTargetFrameRate = 60;

        public enum StartResolution
        {
            _960x540,
            _1280x720,
            _1920x1080,

            Native
        }

        public StartResolution startResolution = StartResolution._1920x1080;

        void Start()
        {
            Application.targetFrameRate = startTargetFrameRate;

            switch (startResolution)
            {
                case StartResolution._960x540:
                    {
                        Screen.SetResolution(960, 540, false);
                        break;
                    }
                case StartResolution._1280x720:
                    {
                        Screen.SetResolution(1280, 720, false);
                        break;
                    }
                case StartResolution._1920x1080:
                    {
                        Screen.SetResolution(1920, 1080, false);
                        break;
                    }
                default:
                case StartResolution.Native:
                    {
                        break;
                    }
            }
        }

        void Update()
        {

        }
    }
}
