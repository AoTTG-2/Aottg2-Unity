using System;
using UnityEngine;

namespace Replay
{
    class BaseReplayObject: MonoBehaviour
    {
        public int ObjectId;

        public virtual void SetState(BaseReplayState state)
        {
        }
    }
}
