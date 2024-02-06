using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : SingletonMono<BallManager>
{
    //行
    public static int ROW =5;

    //列
    public static int COLUM = 5;

    //球大小
    public static float SIZE = 1.2f;

    public List<GameObject> ballPrefab;

    //现在选择的类型
    public E_BallType curType;

    //球二维数组
    public Ball[,] balls;
    //消除选择列表
    public List<Ball> chooseList = new List<Ball>();

    private Dictionary<int,int> eliminateDic = new Dictionary<int,int>();

    private Ball lastChooseBall;

    public Ball LastBall => lastChooseBall;

    private string fxName;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        SpawnerBall();
        SpwanerSprings();
    }

    /// <summary>
    /// 生成篮球
    /// </summary>
    public void SpawnerBall()
    {
        balls = new Ball[ROW +1, COLUM];
        List<int> curBallsList = LevelManager.GetInstance().GetCurBallsList();
        int tempindex = 0;
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COLUM; j++)
            {

                //指定一个类型
                GameObject ball = Instantiate(ballPrefab[curBallsList[tempindex]]);
                ball.transform.SetParent(transform, false);
                ball.transform.localPosition = GetBallPos(i, j);

                //加入数组中
                balls[i, j] = ball.GetComponent<BasketBall>();
                ball.GetComponent<BasketBall>().UpdatePosition(i, j);
                tempindex++;
            }
        }
    }

    /// <summary>
    /// 生成弹簧
    /// </summary>
    public void SpwanerSprings()
    {

        for (int i = 0; i < LevelManager.GetInstance().CurSpringsColumNum.Count; i++)
        {
            GameObject springs = ResMgr.GetInstance().Load<GameObject>(LevelManager.GetInstance().CurSpringsName[i]);
            springs.transform.position = balls[4, LevelManager.GetInstance().CurSpringsColumNum[i]].transform.position + Vector3.forward * 1.1f;
            springs.GetComponent<BallSpring>().UpdatePosition(5, LevelManager.GetInstance().CurSpringsColumNum[i]);
        }
    }
    public void AddOrRemoveBall(Ball ball)
    {
        //判断是否时相同颜色的球
        //判断是否在上一个球的可到达地方
        if (ball == null || ball.ballType != curType)
            return;

        if (!IsCanAddBall(ball))
            return;
        //是否前面已经包含这个点
        if (chooseList.Contains(ball))
        {
            //移除前面已经选择的点
            RemoveToChooseBall(ball);
        }
        else
        {
            if (GameManager.GetInstance().canAddBall)
            {
                //是弹簧的时候不能继续添加球
                if (ball.isballSpring&&chooseList.Count!=0)
                    GameManager.GetInstance().canAddBall = false;
                //震动脚本
                Taptic.Light();

                //放入列表
                chooseList.Add(ball);


                //播放音效
                index = chooseList.Count > 8 ? 8 : chooseList.Count;
                MusicMgr.GetInstance().PlaySoundOneShot("combo" + index);

                ball.ChangeSelect(true);

                if (!ball.isballSpring)
                {
                    GenerateChooseFX(ball);


                    //生成底部的圆圈
                }

                //更改线段
                DrawLine.GetInstance().AddLinePoint(ball.transform.position);
            }
            else
                return;

            

        }
        lastChooseBall = ball;

    }

    /// <summary>
    /// 得到对应的球体然后加入列表当中
    /// </summary>
    /// <param name="row"></param>
    /// <param name="colum"></param>
    public void GetAndAddBall(int row, int colum, Vector3 hitPos)
    {
        // Debug.Log(x + "-" + y);

        if (row >= 0 && colum >= 0 && colum <= COLUM - 1 && row <= ROW - 1)
        {
            AddOrRemoveBall(balls[row, colum]);
            DrawLine.GetInstance().UpdateLinePos(hitPos);
        }

    }

    public void RemoveToChooseBall(Ball ball)
    {
        index = chooseList.Count;
        //减少到目标球为止
        while (chooseList[chooseList.Count - 1] != ball)
        {
            DrawLine.GetInstance().RemoveLinePoint();

            chooseList[chooseList.Count - 1].ChangeSelect(false);

            //是弹簧的时候不能继续添加球
            if (chooseList[chooseList.Count - 1].isballSpring)
                GameManager.GetInstance().canAddBall = true;

            chooseList.RemoveAt(chooseList.Count - 1);
        }
        if (index != chooseList.Count)
        {
            //播放音效
            index = chooseList.Count > 8 ? 8 : chooseList.Count;
            MusicMgr.GetInstance().PlaySoundOneShot("combo" + index);
        }

    }


    /// <summary>
    /// 清除列表
    /// </summary>
    public void ClearList()
    {
        for (int i = 0; i < chooseList.Count; i++)
        {
            chooseList[i].ChangeSelect(false);
        }
        chooseList.Clear();
    }


    /// <summary>
    /// 是否可以添加球
    /// </summary>
    /// <param name="ball"></param>
    /// <returns></returns>
    private bool IsCanAddBall(Ball ball)
    {
        if (chooseList.Count == 0)
            return true;

        return (Mathf.Abs(lastChooseBall.rowIndex - ball.rowIndex) <= 1 && Mathf.Abs(lastChooseBall.columIndex - ball.columIndex) <= 1);
    }

    public Vector3 GetBallPos(int rowIndex,int columIndex)
    {
        return new Vector3((columIndex - COLUM / 2f) * SIZE + SIZE / 2f, (rowIndex - ROW / 2f) * SIZE + SIZE / 2f, 0);
    }


    /// <summary>
    /// 消除所有选择的球
    /// </summary>
    public IEnumerator EliminateChooseBall()
    {

        GameManager.GetInstance().canDrawLine = false;
        for (int i = 0; i < chooseList.Count; i++)
        {
            chooseList[i]?.ChangeSelect(false);

            if (eliminateDic.ContainsKey(chooseList[i].columIndex))
            {
                eliminateDic[chooseList[i].columIndex]++;
            }
            else
            {
                eliminateDic.Add(chooseList[i].columIndex, 1);
            }
            //音效
            MusicMgr.GetInstance().PlaySoundOneShot("broken");
            GenerateBrokenFX(chooseList[i].ballType,chooseList[i].transform.position);
            Destroy(balls[chooseList[i].rowIndex, chooseList[i].columIndex].gameObject);
            balls[chooseList[i].rowIndex, chooseList[i].columIndex] = null;
        }

        yield return new WaitForSeconds(0.2f);
        //移动球填充位置
        MoveBall();

        yield return new WaitForSeconds(0.3f);
        //生成新的球
        GenerateNewBall();
        //清空列表
        chooseList.Clear();
        eliminateDic.Clear();

        GameManager.GetInstance().CheckEnd();
       
    }

    public IEnumerator ShootEliminateChooseBall()
    {
        GameManager.GetInstance().canDrawLine = false;
        Ball spring = null;
        //先找到弹簧组件

        //弹簧在第一个
        if (chooseList[0].isballSpring)
        {
            spring = chooseList[0];
            spring.ChangeSelect(false);
            chooseList.RemoveAt(0);

            //添加到字典中
            for (int i = 0; i < chooseList.Count; i++)
            {
                if (eliminateDic.ContainsKey(chooseList[i].columIndex))
                {
                    eliminateDic[chooseList[i].columIndex]++;
                }
                else
                {
                    eliminateDic.Add(chooseList[i].columIndex, 1);
                }
                chooseList[i].ChangeSelect(false);
            }

            while (chooseList.Count != 0)
            {
                chooseList[0].JumpToShoot(spring);

                int tempColum = chooseList[chooseList.Count-1].columIndex;
                balls[chooseList[chooseList.Count - 1].rowIndex, chooseList[chooseList.Count-1].columIndex] = null;
                for (int i = chooseList.Count - 1; i > 0; i--)
                {
                    chooseList[i].Jump(chooseList[i - 1].rowIndex, chooseList[i - 1].columIndex);
                }

                yield return new WaitForSeconds(0.1f);
                RemoveDic(tempColum);
                yield return new WaitForSeconds(0.1f);
                chooseList.RemoveAt(0);
            }
        }
        //弹簧在最后一个
        else
        {
            spring = chooseList[chooseList.Count - 1];
            spring.ChangeSelect(false);
            chooseList.RemoveAt(chooseList.Count - 1);

            //添加到字典中
            for (int i = 0; i < chooseList.Count; i++)
            {
                if (eliminateDic.ContainsKey(chooseList[i].columIndex))
                {
                    eliminateDic[chooseList[i].columIndex]++;
                }
                else
                {
                    eliminateDic.Add(chooseList[i].columIndex, 1);
                }
                chooseList[i].ChangeSelect(false);
            }


            while (chooseList.Count != 0)
            {
                chooseList[chooseList.Count - 1].JumpToShoot(spring);

                int tempColum = chooseList[0].columIndex;
                balls[chooseList[0].rowIndex, chooseList[0].columIndex] = null;



/*                for (int i = chooseList.Count - 2; i >= 0; i--)
                {
                    chooseList[i].Jump(chooseList[i + 1].rowIndex, chooseList[i +1].columIndex);
                }
*/              
                if(chooseList.Count > 1) 
                {
                    for (int i = 0; i < chooseList.Count - 1; i++)
                    {
                        chooseList[i].Jump(chooseList[i + 1].rowIndex, chooseList[i + 1].columIndex);
                    }
                }

               

                
                yield return new WaitForSeconds(0.1f);
                RemoveDic(tempColum);
                yield return new WaitForSeconds(0.1f);
                chooseList.RemoveAt(chooseList.Count - 1);
            }

        }



        yield return new WaitForSeconds(0.5f);

        GameManager.GetInstance().CheckEnd();

    }

    private void RemoveDic(int colum)
    {
        if (--eliminateDic[colum] == 0)
        {
            //更新整一列
            StartCoroutine(UpdateOneColum(colum));  

        }
    }

    private IEnumerator UpdateOneColum(int columIndex)
    {
        int updateRowIndex = ROW - 1;
        for (int j = ROW - 1; j >= 0; j--)
        {
            if (balls[j, columIndex] != null)
            {
                if (j != updateRowIndex)
                {
                    balls[updateRowIndex, columIndex] = balls[j, columIndex];
                    balls[j, columIndex] = null;

                    balls[updateRowIndex, columIndex].UpdatePosition(updateRowIndex, columIndex, true);
                }
                updateRowIndex--;
            }
        }
        yield return new WaitForSeconds(0.3f);

        for (int j = ROW - 1; j >= 0; j--)
        {
            if (balls[j, columIndex] == null)
            {
                //随机一个类型
                GameObject ball = Instantiate(ballPrefab[LevelManager.GetInstance().CurBallsTypeList[Random.Range(0, LevelManager.GetInstance().CurBallsTypeList.Count)]]);
                ball.transform.SetParent(transform, false);
                ball.transform.localPosition = new Vector3((columIndex - COLUM / 2f) * SIZE + SIZE / 2f, -5.5f, 0);

                //加入数组中
                balls[j, columIndex] = ball.GetComponent<BasketBall>();
                ball.GetComponent<BasketBall>().UpdatePosition(j, columIndex, true);
            }
        }
    }
    //更新球的位置
    public void MoveBall()
    {
        for (int i = 0; i < COLUM; i++)
        {
            if (eliminateDic.ContainsKey(i))
            {
                int updateRowIndex = ROW-1;
                for (int j = ROW-1; j >= 0; j--)
                {
                    if (balls[j, i] != null)
                    {
                        if (j != updateRowIndex)
                        {
                            balls[updateRowIndex, i] = balls[j, i];
                            balls[j, i] = null;

                            balls[updateRowIndex, i].UpdatePosition(updateRowIndex, i, true);
                        }                      
                        updateRowIndex--;   
                    }
                }
            }
        }      
    }

    public void GenerateNewBall()
    {
        for (int i = 0; i < COLUM; i++)
        {
            if (eliminateDic.ContainsKey(i))
            {
                for (int j = ROW - 1; j >= 0; j--)
                {
                    if (balls[j, i] == null)
                    {
                        //随机一个类型
                        GameObject ball = Instantiate(ballPrefab[LevelManager.GetInstance().CurBallsTypeList[Random.Range(0,LevelManager.GetInstance().CurBallsTypeList.Count)]]);
                        ball.transform.SetParent(transform, false);
                        ball.transform.localPosition = new Vector3((i - COLUM / 2f) * SIZE + SIZE / 2f,-5.5f, 0);

                        //加入数组中
                        balls[j, i] = ball.GetComponent<BasketBall>();
                        ball.GetComponent<BasketBall>().UpdatePosition(j, i,true);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 检测消除
    /// </summary>
    public void CheckEliminate()
    {

        //投篮消除
        if (chooseList[0].isballSpring || chooseList[chooseList.Count - 1].isballSpring)
        {
            if (chooseList.Count > 3)
            {
                //更新步数
                GameManager.GetInstance().UpdateMovesNum();

                StartCoroutine(ShootEliminateChooseBall());
            }
            else
                ClearList();
        }
        //直接消除
        else 
        {
            

            if (chooseList.Count > 2)
            {
                //更新步数
                GameManager.GetInstance().UpdateMovesNum();

                StartCoroutine(EliminateChooseBall());
                
            }
            else
                ClearList();

        }


        
    }

    private void GenerateBrokenFX(E_BallType ballType,Vector3 pos)
    {
        switch (ballType)
        {
            case E_BallType.Red:
                fxName = "FX/BrokenRed";
                break;
            case E_BallType.Green:
                fxName = "FX/BrokenGreen";
                break;
            case E_BallType.Blue:
                fxName = "FX/BrokenBlue";
                break;
            case E_BallType.Yellow:
                fxName = "FX/BrokenYellow";
                break;
            case E_BallType.Purple:
                fxName = "FX/BrokenPurple";
                break;

        }

        PoolMgr.GetInstance().GetObj(fxName, (fx) =>
        {
            fx.transform.position = pos;
        });
    }
    private void GenerateChooseFX(Ball ball)
    {
        switch (ball.ballType) 
        {
            case E_BallType.Red:
                fxName = "FX/Sp_Red";
                break;
            case E_BallType.Green:
                fxName = "FX/Sp_Green";
                break; 
            case E_BallType.Blue:
                fxName = "FX/Sp_Blue";
                break;
            case E_BallType.Yellow:
                fxName = "FX/Sp_Yellow";
                break;
            case E_BallType.Purple:
                fxName = "FX/Sp_Purple";
                break;
        
        }

        PoolMgr.GetInstance().GetObj(fxName, (fx) =>
        {
            fx.transform.SetParent(ball.transform,false);
            fx.transform.localPosition = Vector3.zero;
        });
    }

}
