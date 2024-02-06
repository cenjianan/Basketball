using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Ray ray;

    private RaycastHit _raycastHit;

    private Vector3 lastBallPos;

    private Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.GetInstance().canDrawLine)
            return;

        //监听按下
        if (Input.GetMouseButtonDown(0))
        {
           
            ray = CameraManager.GetInstance().mainCam.ScreenPointToRay(Input.mousePosition);
            


            //监听是否点击到球上
            if(Physics.Raycast(ray,out _raycastHit, 100, 1 << LayerMask.NameToLayer("Ball")))
            {
                GameManager.GetInstance().isDrawLine = true;

                ball = _raycastHit.collider.GetComponent<BasketBall>();
                if(ball != null ) 
                {
                    //更改判断消除的颜色
                    BallManager.GetInstance().curType = ball.ballType;
                }
                else
                {
                    ball = _raycastHit.collider.GetComponent<BallSpring>();
                    BallManager.GetInstance().curType = ball.ballType;
                }
                
                DrawLine.GetInstance().UpdateLineColor(ball);
                //添加球到待消除列表
                BallManager.GetInstance().AddOrRemoveBall(ball);


            }

        }

        if (!GameManager.GetInstance().isDrawLine)
            return;
        
        //监听拖拽
        if (Input.GetMouseButton(0))
        {
            ray = CameraManager.GetInstance().mainCam.ScreenPointToRay(Input.mousePosition);
   
            //监听是否点击到球上
   
            if (Physics.Raycast(ray, out _raycastHit, 100, 1 << LayerMask.NameToLayer("Ball")))
            {
                ball = _raycastHit.collider.GetComponent<BasketBall>();
                if (ball == null)
                {
                    ball = _raycastHit.collider.GetComponent<BallSpring>();
                }

                //添加球到待消除列表
                BallManager.GetInstance().AddOrRemoveBall(ball);

            }
            if (Physics.Raycast(ray, out _raycastHit, 100, 1 << LayerMask.NameToLayer("Back")))
            {
                DrawLine.GetInstance().UpdateLinePos(new Vector3( _raycastHit.point.x,0, _raycastHit.point.z));
                OptimizeCheck(new Vector3(_raycastHit.point.x, 0, _raycastHit.point.z));
            }

        }

        //监听抬起
        if (Input.GetMouseButtonUp(0))
        {
            DrawLine.GetInstance().DestroyLine();
            BallManager.GetInstance().CheckEliminate();

            GameManager.GetInstance().isDrawLine = false;
            GameManager.GetInstance().canAddBall = true;
        }


    }

    //优化检测
    public void OptimizeCheck(Vector3 hitPos)
    {
        if (BallManager.GetInstance().LastBall == null)
            return;

        lastBallPos = BallManager.GetInstance().LastBall.transform.position;

        float x = hitPos.x - lastBallPos.x;
        float z = hitPos.z - lastBallPos.z;


        int offsetwidth = 0;
        int offsethegiht = 0;
        if (x <= -1.3f)
        {
            offsetwidth = -1;
            // Debug.Log(BallManager.Instance.lastChooseBall);
        }
        else if (x >= 1.3f)
        {
            offsetwidth = 1;
            // Debug.Log("超出右边界");
        }

        if (offsetwidth != 0)
            BallManager.GetInstance().GetAndAddBall(BallManager.GetInstance().LastBall.rowIndex, BallManager.GetInstance().LastBall.columIndex + offsetwidth, hitPos);

        if (z <= -1.3f)
        {
            offsethegiht = -1;
            // Debug.Log("超出下边界");
        }
        else if (z >= 1.3f)
        {
            offsethegiht = 1;
            //Debug.Log("超出上边界");
        }
        if (offsethegiht != 0)
            BallManager.GetInstance().GetAndAddBall(BallManager.GetInstance().LastBall.rowIndex + offsethegiht, BallManager.GetInstance().LastBall.columIndex, hitPos);
        if (offsethegiht != 0 && offsetwidth != 0)
            BallManager.GetInstance().GetAndAddBall(BallManager.GetInstance().LastBall.rowIndex + offsethegiht, BallManager.GetInstance().LastBall.columIndex + offsetwidth, hitPos);
    }
}
