using Photon;
using System;
using UnityEngine;
using System.Collections;
using Settings;
using Photon.Realtime;

namespace Effects {
    class LineRendererEffect : BaseEffect
    {
        protected float _totalTime;
        protected LineRenderer _renderer;

        public override void Setup(Player owner, float liveTime, object[] settings)
        {
            base.Setup(owner, liveTime, settings);
            _renderer = GetComponent<LineRenderer>();
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, (Vector3)settings[0]);
            _renderer.SetPosition(1, (Vector3)settings[1]);
            _renderer.startWidth = (float)settings[2];
            _renderer.endWidth = (float)settings[3];
            _totalTime = (float)settings[4];
            _timeLeft = _totalTime;
        }

        protected override void Update()
        {
            base.Update();
            Color color = new Color(1f, 1f, 1f, _timeLeft / _totalTime);
            _renderer.startColor = color;
            _renderer.endColor = color;
        }
    }
}