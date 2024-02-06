using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum E_BallType
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple
}

public class Ball : MonoBehaviour
{
    public E_BallType ballType;

    //是否是弹簧
    public bool isballSpring;
    //行列索引
    public int rowIndex;

    public int columIndex;

    protected Vector3 targetPos;

    /// <summary>
    /// 移动坐标
    /// </summary>
    public virtual void UpdatePosition(int rowIndex, int columIndex, bool dotween = false)
    {
       

    }

    public virtual void ChangeSelect(bool isSelect)
    {
        transform.DOKill();
        if (isSelect) 
        {
            transform.DOScale(1.1f, 0.2f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else 
        {
            transform.localScale = Vector3.one; 
        }
    }

    public void Jump(int rowIndex, int columIndex)
    {
        this.rowIndex = rowIndex;
        this.columIndex = columIndex;
        targetPos = BallManager.GetInstance().GetBallPos(rowIndex, columIndex);


        //创建特效
        PoolMgr.GetInstance().GetObj("FX/BallCircle", (ballcircle) =>
        {

            ballcircle.transform.SetParent(transform, false);
            ballcircle.transform.localScale = Vector3.one*1.5f;
            ballcircle.transform.localPosition = Vector3.up * 0.4f;
        });

        transform.DOLocalJump(targetPos, 0.8f, 1, 0.2f).OnComplete(() =>
        {
            MusicMgr.GetInstance().PlaySoundOneShot("ball_floor");
            transform.DOScale(new Vector3(0.8f,0.8f,1.3f), 0.1f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.05f);
            });
        });

       
    }

    public void JumpToShoot(Ball ball)
    {
        StartCoroutine(JumpShoot(ball));  
    }

    private IEnumerator JumpShoot(Ball ball)
    {
        targetPos = (ball as BallSpring).jumpPos.position;

        transform.DOJump(targetPos, 0.8f, 1, 0.2f);

        yield return new WaitForSeconds(0.2f);

        (ball as BallSpring).DoShake();
        transform.DOJump(GameManager.GetInstance().basketBallHoop.shootTrans.position, 4f, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(0, 0.3f);
        //相机震动
        CameraManager.GetInstance().mainCam.transform.DOShakePosition(0.4f, 0.05f, 10, 80);
        GameManager.GetInstance().basketBallHoop.DoShake();

        //进球特效
        PoolMgr.GetInstance().GetObj("FX/Score", (ballcircle) =>
        {

            ballcircle.transform.position = GameManager.GetInstance().basketBallHoop.fxTrans.position;
        });

        //更新数量
        GameManager.GetInstance().UpdateBallNum(ball);

        MusicMgr.GetInstance().PlaySoundOneShot("ball_net");
        //震动脚本
        Taptic.Light();
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
