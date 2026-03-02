using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Open : MonoBehaviour

{
    // Start is called before the first frame update
    [SerializeField] private Animator _animator;
    void Start()
    {
        _animator.SetTrigger("Trigger_Open");
    }
}
