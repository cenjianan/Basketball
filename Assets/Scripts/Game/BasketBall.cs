using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : Ball
{
    //选择时的圈
    private GameObject chooseFrame;
    private string fxName;
    public override void ChangeSelect(bool isSelect)
    {
        base.ChangeSelect(isSelect);
       if (isSelect)
       {
            GenerateFrame();
       }
       else
       {
            PoolMgr.GetInstance().PushObj(chooseFrame.name,chooseFrame);
        }
    }
    public override void UpdatePosition(int rowIndex, int columIndex, bool dotween = false)
    {
        base.UpdatePosition(rowIndex, columIndex, dotween);
        this.rowIndex = rowIndex;
        this.columIndex = columIndex;
        targetPos = BallManager.GetInstance().GetBallPos(rowIndex, columIndex);
        //BallManager.Instance.balls[rowIndex, columIndex] = this;

        if (dotween)
        {
            // 0.3秒移动到目标点
            transform.DOKill();
            transform.DOLocalMove(targetPos, 0.3f);
            //Debug.Log(targetPos);
        }
        else
        {
            transform.localPosition = targetPos;
        }
    }

    private void GenerateFrame()
    {
        switch (ballType)
        {
            case E_BallType.Red:
                fxName = "Frame/Frame_Red";
                break;
            case E_BallType.Green:
                fxName = "Frame/Frame_Green";
                break;
            case E_BallType.Blue:
                fxName = "Frame/Frame_Blue";
                break;
            case E_BallType.Yellow:
                fxName = "Frame/Frame_Yellow";
                break;
            case E_BallType.Purple:
                fxName = "Frame/Frame_Purple";
                break;

        }

        PoolMgr.GetInstance().GetObj(fxName, (fx) =>
        {
            chooseFrame = fx;
            fx.transform.position = transform.position + Vector3.down * 0.39f;
        });
    }

    //0014E0 蓝
    //520000 红
    //093F00 绿
    //290050 紫
    //7B0E00 黄
}
