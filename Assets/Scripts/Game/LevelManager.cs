using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseManager<LevelManager>
{
    public Level level;

    public List<int> CurBallsTypeList => level.dataList[(currentLevel - 1) % 10].ballsTypeList;
    public List<int> CurSpringsColumNum => level.dataList[(currentLevel - 1) % 10].springsColumNum;
    public List<string> CurSpringsName => level.dataList[(currentLevel - 1) % 10].springsName;
    public LevelData CurLevelData => level.dataList[(currentLevel - 1)%10];

    public int currentLevel;
    public LevelManager()
    {
        LoadData();
    }

    public void LoadData()
    {
        level = JsonMapper.ToObject<Level>(Resources.Load<TextAsset>("Level/LevelData").text);
        currentLevel = PlayerPrefs.GetInt("CurLevel", 1);

    }

    public void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurLevel", currentLevel);
    }

    public List<int> GetCurBallsList()
    {
        return level.dataList[(currentLevel - 1) % 10].ballsList;
    }


}
