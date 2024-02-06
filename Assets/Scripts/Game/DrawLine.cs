using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : SingletonMono<DrawLine>
{
    public LineRenderer line;

    public float maxLength = 1.2f;

    //现在线段的索引
    public int nowIndex = 0;
    private int pointCount =>line.positionCount ;
    private Vector3 lastPos;
    public void UpdateLinePos(Vector3 pos)
    {

        lastPos = line.GetPosition(nowIndex - 1);

        //lineRenderer.positionCount = nowIndex;  
        //判断是否超出可画线的最长长度
        if (Vector3.Distance(pos, lastPos) > maxLength)
        {
            //计算可画线的最终点
            pos = (pos - lastPos).normalized * maxLength + lastPos;
        }

        //比较点的位置，设置最大线段区间
        line.SetPosition(nowIndex, pos);
    }

    /// <summary>
    /// 移除线段点
    /// </summary>
    public void RemoveLinePoint()
    {
        line.positionCount -= 1;
        nowIndex--;
    }

    /// <summary>
    /// 增加线段点
    /// </summary>
    /// <param name="pos"></param>
    public void AddLinePoint(Vector3 pos)
    {
        if (line.positionCount == 0)
        {
            CreatLine(pos);
        }
        else
        {
            line.SetPosition(nowIndex, pos);
            line.positionCount++;
            nowIndex++;
        }

        
       
    }

    /// <summary>
    /// 创建一条线
    /// </summary>
    /// <param name="pos"></param>
    public void CreatLine(Vector3 pos)
    {
        //初始化线段长度
        nowIndex = 0;
        line.positionCount = 2;
        line.SetPosition(0, pos);
        nowIndex++;
    }

    /// <summary>
    /// 销毁线段
    /// </summary>
    public void DestroyLine()
    {
        line.positionCount = 0;
    }
    /// <summary>
    /// 更新线段的颜色
    /// </summary>
    public void UpdateLineColor(Ball ball)
    {
        switch (ball.ballType) 
        {

            case E_BallType.Red:
                line.startColor = new Color(0.3215686f, 0 ,0 , 1 );
                line.endColor = new Color(0.3215686f, 0, 0, 1);
                break;
            case E_BallType.Green:
                line.startColor = new Color(0.03529412f, 0.2470588f, 0, 1);
                line.endColor = new Color(0.03529412f, 0.2470588f, 0, 1);
                break;
            case E_BallType.Blue:
                line.startColor = new Color(0, 0.07843138f, 0.8784314f, 1);
                line.endColor = new Color(0, 0.07843138f, 0.8784314f, 1);
                break;
            case E_BallType.Yellow:
                line.startColor = new Color(0.2830189f, 0.230608f, 0, 1);
                line.endColor = new Color(0.2830189f, 0.230608f, 0, 1);
                break;
            case E_BallType.Purple:
                line.startColor = new Color(0.1607843f, 0, 0.3137255f, 1);
                line.endColor = new Color(0.1607843f, 0, 0.3137255f, 1);
                break;
        }
    }
}
