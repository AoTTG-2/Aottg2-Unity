using UnityEngine;
using System.Collections.Generic;

namespace Characters
{
    /// <summary>
    /// Warning zone that triggers the Fire1 particle effect on humans entering the steam area
    /// </summary>
    internal class WallColossalSteamWarningZone : MonoBehaviour
    {
        private HashSet<Human> _humansInZone = new HashSet<Human>();
        private WallColossalShifter _owner;
        private bool _isActive;

        internal void Initialize(WallColossalShifter owner)
        {
            _owner = owner;
        }

        internal void SetActive(bool active)
        {
            _isActive = active;
            
            if (!active)
            {
                // Disable fire effect for all humans in zone
                foreach (var human in _humansInZone)
                {
                    if (human != null && !human.Dead)
                        human.ToggleFire1(false);
                }
                _humansInZone.Clear();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive || _owner == null)
                return;

            var human = other.GetComponent<Human>();
            if (human != null && !human.Dead && human.IsMine())
            {
                _humansInZone.Add(human);
                human.ToggleFire1(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_owner == null)
                return;

            var human = other.GetComponent<Human>();
            if (human != null && _humansInZone.Contains(human))
            {
                _humansInZone.Remove(human);
                if (!human.Dead && human.IsMine())
                    human.ToggleFire1(false);
            }
        }

        private void OnDestroy()
        {
            // Clean up all fire effects when destroyed
            foreach (var human in _humansInZone)
            {
                if (human != null && !human.Dead && human.IsMine())
                    human.ToggleFire1(false);
            }
            _humansInZone.Clear();
        }

        private void Update()
        {
            // Clean up dead or invalid humans
            _humansInZone.RemoveWhere(h => h == null || h.Dead);
        }
    }
}
