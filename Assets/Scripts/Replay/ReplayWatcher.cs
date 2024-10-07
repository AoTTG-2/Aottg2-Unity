using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Replay
{
    class ReplayWatcher: MonoBehaviour
    {
        public bool Playing;
        public float Speed;
        public float CurrentTime;
        public float MaxTime;
        private ReplayScript _script;
        private int _currentEvent;

        public void LoadScript(ReplayScript script)
        {
            _script = script;
            _currentEvent = 0;
            Playing = false;
            CurrentTime = script.Events[0].Time;
            MaxTime = script.Events[script.Events.Count - 1].Time;
            HandleEvent(script.Events[0]);
        }

        private void FixedUpdate()
        {
            if (!Playing)
                return;
            CurrentTime += Time.fixedDeltaTime * Speed;
            while (_currentEvent < _script.Events.Count - 1)
            {
                ReplayScriptEvent nextEvent = _script.Events[_currentEvent + 1];
                if (CurrentTime < nextEvent.Time)
                    break;
                _currentEvent++;
                HandleEvent(nextEvent);
            }
            if (CurrentTime >= MaxTime)
            {
                CurrentTime = MaxTime;
                Playing = false;
            }
        }

        private void HandleEvent(ReplayScriptEvent currentEvent)
        {
            if (currentEvent.Category == ReplayEventCategory.Map.ToString())
                HandleMapEvent(currentEvent);
            else if (currentEvent.Category == ReplayEventCategory.Human.ToString())
                HandleHumanEvent(currentEvent);
            else if (currentEvent.Category == ReplayEventCategory.Titan.ToString())
                HandleTitanEvent(currentEvent);
            else if (currentEvent.Category == ReplayEventCategory.Camera.ToString())
                HandleCameraEvent(currentEvent);
            else if (currentEvent.Category == ReplayEventCategory.Chat.ToString())
                HandleChatEvent(currentEvent);
        }

        private void HandleMapEvent(ReplayScriptEvent currentEvent)
        {
            if (currentEvent.Action == ReplayEventMapAction.SetMap.ToString())
            {
                string map = currentEvent.Parameters[0];
            }
        }

        private void HandleHumanEvent(ReplayScriptEvent currentEvent)
        {
        }

        private void HandleTitanEvent(ReplayScriptEvent currentEvent)
        {
        }

        private void HandleCameraEvent(ReplayScriptEvent currentEvent)
        {
        }

        private void HandleChatEvent(ReplayScriptEvent currentEvent)
        {
        }
    }
}
