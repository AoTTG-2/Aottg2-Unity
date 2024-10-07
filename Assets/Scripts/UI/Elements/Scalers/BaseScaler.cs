using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UI
{
    abstract class BaseScaler: MonoBehaviour
    {
        protected virtual void Awake()
        {
            ApplyScale();
        }

        public abstract void ApplyScale();
    }
}
