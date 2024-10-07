using System.Collections;
using UnityEngine;

namespace Utility
{
    class CoroutineWithData
    {
        public Coroutine Coroutine { get; private set; }
        public object Result;
        private IEnumerator _target;
        public bool Done = false;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                Result = _target.Current;
                yield return Result;
            }
            Done = true;
        }
    }
}
