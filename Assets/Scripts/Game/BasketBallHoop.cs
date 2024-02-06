using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallHoop : MonoBehaviour
{
    public Transform shootTrans;

    public Transform fxTrans;

    private Animator _animator;

    //public Transform
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();   
    }

    public void DoShake()
    {
        _animator.SetTrigger("Shake");
    }
}
