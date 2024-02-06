using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Level
{
    public List<LevelData> dataList = new List<LevelData>();
}

public class LevelData
{
    public int id;

    public List<int> ballsList = new List<int>();

    public List<int> ballsTypeList = new List<int>();

    public List<string> springsName = new List<string>();

    public List<int> springsColumNum = new List<int>();

    //通关条件
    public int redBallNeedNum;
    public int yellowBallNeedNum;
    public int blueBallNeedNum;
    public int greenBallNeedNum;
    public int purpleBallNeedNum;
    public int anyBallNeedNum;
    public int moveNum;
}


