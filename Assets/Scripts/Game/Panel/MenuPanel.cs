using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel<MenuPanel>
{
    public Transform frame;

    public Button btnClose;

    public Button btnGet;

    public TextMeshProUGUI txtNum;

    public TextMeshProUGUI txtGetMoney;

    public TextMeshProUGUI txtTime;

    public Image imgProcess;

    public Image ballicon;
    public Image btnBg;
    public List<Sprite> iconList;
    public List<Sprite> btnList;

    private int num;
    public void InitTask()
    {
        switch (TaskManager.GetInstance().CurIndex)
        {
            case 0:
                ballicon.sprite = iconList[0];
                if (UserData.GetInstance().collectRedBallNum >= TaskManager.GetInstance().CurTask.targetNum) 
                {
                    num = TaskManager.GetInstance().CurTask.targetNum;
                }
                else 
                {
                    num = UserData.GetInstance().collectRedBallNum;
                }
                break;
            case 1:
                if (UserData.GetInstance().collectYellowBallNum >= TaskManager.GetInstance().CurTask.targetNum)
                {
                    num = TaskManager.GetInstance().CurTask.targetNum;
                }
                else
                {
                    num = UserData.GetInstance().collectYellowBallNum;
                }
                ballicon.sprite = iconList[1];
                break;
            case 2:
                if (UserData.GetInstance().collectBlueBallNum >= TaskManager.GetInstance().CurTask.targetNum)
                {
                    num = TaskManager.GetInstance().CurTask.targetNum;
                }
                else
                {
                    num = UserData.GetInstance().collectBlueBallNum;
                }
                ballicon.sprite = iconList[2];
                break;
            case 3:
                if (UserData.GetInstance().collectGreenBallNum >= TaskManager.GetInstance().CurTask.targetNum)
                {
                    num = TaskManager.GetInstance().CurTask.targetNum;
                }
                else
                {
                    num = UserData.GetInstance().collectGreenBallNum;
                }
                ballicon.sprite = iconList[3];
                break;
            case 4:
                if (UserData.GetInstance().collectPurpleBallNum >= TaskManager.GetInstance().CurTask.targetNum)
                {
                    num = TaskManager.GetInstance().CurTask.targetNum;
                }
                else
                {
                    num = UserData.GetInstance().collectPurpleBallNum;
                }
                ballicon.sprite = iconList[4];
                break;
        }

        if (num == TaskManager.GetInstance().CurTask.targetNum)
        {
            btnBg.sprite = btnList[0];
        }
        else
        {
            btnBg.sprite = btnList[1];
        }

        imgProcess.fillAmount = (float)num / TaskManager.GetInstance().CurTask.targetNum;
        txtNum.text =  num + "/" + TaskManager.GetInstance().CurTask.targetNum;
        txtGetMoney.text = TaskManager.GetInstance().CurTask.rewardNum.ToString();

    }
    private void Start()
    {
        btnClose.onClick.AddListener(() =>
        {
            HideMe();
        });

        btnGet.onClick.AddListener(() =>
        {

            if (num == TaskManager.GetInstance().CurTask.targetNum)
            {
                UpdateTask();
                MusicMgr.GetInstance().PlaySoundOneShot("getcoin");
                MainPanel.GetInstance().UpdateMoney();
                HideMe();
            }
        });
        gameObject.SetActive(false);
    }

    public void UpdateTask()
    {
        UserData.GetInstance().curMoney += TaskManager.GetInstance().CurTask.rewardNum;
        UserData.GetInstance().InitData();
        TaskManager.GetInstance().RandomTask();

        AIController.GetInstance().Back();   
    }

    public override void ShowMe()
    {
        base.ShowMe();
        frame.transform.DOScale(1, 0.5f).From(0);
        InitTask();
        MusicMgr.GetInstance().PlaySoundOneShot("goal");
    }
}
