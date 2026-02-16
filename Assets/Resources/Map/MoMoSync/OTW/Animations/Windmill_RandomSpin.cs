using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill_RandomSpin : MonoBehaviour
{
    [SerializeField] Animator _animator;

    void Start()
    {
        float randomDelay = Random.Range(0f, 3.4f);
        Invoke(nameof(RandomSpin), randomDelay);

    }

    private void RandomSpin()
    {
        _animator.SetTrigger("Trigger_Spin");
    }

}
