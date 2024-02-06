using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public bool isDrawLine;
    public bool canDrawLine = true;

    public bool canAddBall;

    public BasketBallHoop basketBallHoop;

    public GameObject successFX;

    [Header("通关所需的数量")]
    public int redBallsNum;

    public int yellowBallsNum;

    public int blueBallsNum;

    public int greenBallsNum;

    public int purpleBallsNum;

    public int anyBallsNum;

    public int nowMovesNum;
    // Start is called before the first frame update
    public void Start()
    {
        Application.targetFrameRate = 90;
        InitData();
        GamePanel.GetInstance().InitNumItems();
    }

    public void InitData()
    {
        redBallsNum = LevelManager.GetInstance().CurLevelData.redBallNeedNum;
        yellowBallsNum = LevelManager.GetInstance().CurLevelData.yellowBallNeedNum;
        blueBallsNum = LevelManager.GetInstance().CurLevelData.blueBallNeedNum;
        greenBallsNum = LevelManager.GetInstance().CurLevelData.greenBallNeedNum;
        purpleBallsNum = LevelManager.GetInstance().CurLevelData.purpleBallNeedNum;
        anyBallsNum = LevelManager.GetInstance().CurLevelData.anyBallNeedNum;
        nowMovesNum = LevelManager.GetInstance().CurLevelData.moveNum;
    }


    public void UpdateMovesNum()
    {
        nowMovesNum--;
        GamePanel.GetInstance().UpdateMovesNum(nowMovesNum);
    }

    public void UpdateBallNum(Ball ball)
    {
        switch (ball.ballType)
        {
            case E_BallType.Red:
                redBallsNum--;
                if (redBallsNum <= 0)
                {
                    GamePanel.GetInstance().UpdateItemState(0);
                    redBallsNum = 0;
                }
                GamePanel.GetInstance().UpdateItemNum(0, redBallsNum);
                UserData.GetInstance().collectRedBallNum++;
                break;
            case E_BallType.Yellow:
                yellowBallsNum--;
                if (yellowBallsNum <= 0)
                {

                    yellowBallsNum = 0;
                    GamePanel.GetInstance().UpdateItemState(1);
                }
                GamePanel.GetInstance().UpdateItemNum(1, yellowBallsNum);
                UserData.GetInstance().collectYellowBallNum++;
                break;
            case E_BallType.Blue:
                blueBallsNum--;
                if (blueBallsNum <= 0)
                {
                    GamePanel.GetInstance().UpdateItemState(2);
                    blueBallsNum = 0;
                }
                GamePanel.GetInstance().UpdateItemNum(2, blueBallsNum);
                UserData.GetInstance().collectBlueBallNum++;
                break;

            case E_BallType.Green:
                greenBallsNum--;
                if (greenBallsNum <= 0)
                {
                    GamePanel.GetInstance().UpdateItemState(3);
                    greenBallsNum = 0;
                }
                GamePanel.GetInstance().UpdateItemNum(3, greenBallsNum);
                UserData.GetInstance().collectGreenBallNum++;
                break;
            
            case E_BallType.Purple:
                purpleBallsNum--;
                if (purpleBallsNum <= 0)
                {
                    GamePanel.GetInstance().UpdateItemState(4);
                    purpleBallsNum = 0;
                }
                GamePanel.GetInstance().UpdateItemNum(4,purpleBallsNum);
                UserData.GetInstance().collectPurpleBallNum++;
                break;
        }

        if (anyBallsNum >0)
        {
            anyBallsNum--;
            if (anyBallsNum<=0)
            {
                GamePanel.GetInstance().UpdateItemState(5);
                anyBallsNum = 0;
            }
            GamePanel.GetInstance().UpdateItemNum(5, anyBallsNum);
        }
    }

    /// <summary>
    /// 成功判断        
    /// </summary>
    public void CheckEnd()
    {

        //通关
        if ((redBallsNum==0||redBallsNum==-1)&& (blueBallsNum == 0 || blueBallsNum == -1)&& (yellowBallsNum == 0 || yellowBallsNum == -1)&& (greenBallsNum == 0 || greenBallsNum == -1)&& (purpleBallsNum == 0 || purpleBallsNum == -1)&& (anyBallsNum == 0 || anyBallsNum == -1))
        {
            //特效
            successFX.gameObject.SetActive(true);
            MusicMgr.GetInstance().PlaySoundOneShot("cheer");

            LevelManager.GetInstance().NextLevel();
            WinPanel.GetInstance().ShowMe();
            UserData.GetInstance().SaveData();
        }
        else if (nowMovesNum == 0)
        {
            FailPanel.GetInstance().ShowMe();
        }
        else
        {
            canDrawLine = true;
        }
    }

    private void OnDisable()
    {
        PoolMgr.GetInstance().Clear();
    }
}
