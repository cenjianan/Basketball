using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData :BaseManager<UserData>
{
    public int soundOn;

    public int tapticOn;

    public int curMoney;

    public int collectRedBallNum;

    public int collectBlueBallNum;

    public int collectYellowBallNum;

    public int collectGreenBallNum;

    public int collectPurpleBallNum;

    public UserData()
    {
        //初始化数据
        LoadData();
    }

    public void LoadData()
    {
        curMoney = PlayerPrefs.GetInt("CurMoney", 0);
        collectRedBallNum = PlayerPrefs.GetInt("RedBall", 0);
        collectYellowBallNum = PlayerPrefs.GetInt("YellowBall", 0);
        collectBlueBallNum = PlayerPrefs.GetInt("BlueBall", 0);
        collectGreenBallNum = PlayerPrefs.GetInt("GreenBall", 0);
        collectPurpleBallNum = PlayerPrefs.GetInt("PurpleBall", 0);
        soundOn = PlayerPrefs.GetInt("SoundOn", 1);
        tapticOn = PlayerPrefs.GetInt("TapticOn", 1);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CurMoney", curMoney);
        PlayerPrefs.SetInt("RedBall", collectRedBallNum);
        PlayerPrefs.SetInt("YellowBall", collectYellowBallNum);
        PlayerPrefs.SetInt("BlueBall", collectBlueBallNum);
        PlayerPrefs.SetInt("GreenBall", collectGreenBallNum);
        PlayerPrefs.SetInt("PurpleBall", collectPurpleBallNum);
    }

    public void InitData()
    {
        collectRedBallNum = 0;
        collectYellowBallNum = 0;
        collectBlueBallNum = 0;
        collectGreenBallNum = 0;
        collectPurpleBallNum = 0;
        SaveData();
    }

    public void SaveSoundData()
    {
        PlayerPrefs.SetInt("TapticOn", tapticOn);
        PlayerPrefs.SetInt("SoundOn", soundOn);
    }
}
