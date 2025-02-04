using UnityEngine;
using Cameras;

namespace Characters
{
    class CameraDetection: BaseDetection
    {
        private InGameCamera _camera;

        public CameraDetection (InGameCamera camera): base(null, false, true)
        {
            _camera = camera;
        }

        public override bool IsNullOrDead()
        {
            return false;
        }

        protected override Vector3 GetPosition()
        {
            return _camera.Cache.Transform.position;
        }

        protected override float GetSpeed()
        {
            if (_camera._follow != null)
                return _camera._follow.CurrentSpeed;
            return 0f;
        }

        protected override void OnRecalculate(BaseCharacter character, float distance)
        {
            character.Animation.OnDistanceUpdate(distance);
            character.FootstepsEnabled = distance < character.MaxFootstepDistance;
            character.SoundsEnabled = distance < character.MaxSoundDistance;
        }
    }
}
