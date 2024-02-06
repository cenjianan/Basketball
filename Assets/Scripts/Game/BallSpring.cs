using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallSpring : Ball
{
    private Animator _animator;

    public Transform jumpPos;

    private void Start()
    {
        _animator = GetComponent<Animator>();   
    }
    public override void UpdatePosition(int rowIndex, int columIndex, bool dotween = false)
    {
        base.UpdatePosition(rowIndex, columIndex, dotween);
        this.rowIndex = rowIndex;
        this.columIndex = columIndex;
    }

    public override void ChangeSelect(bool isSelect)
    {
        transform.DOKill();
        if (isSelect)
        {
            transform.DOScale(0.9f, 0.2f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else
        {
            transform.localScale = Vector3.one*0.8f;
        }
    }

    public void DoShake()
    {
        _animator.SetTrigger("Shake");
    }
}
