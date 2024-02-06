using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel<GamePanel>
{
    public Button btnRestart;

    public Button btnHome;

    public Toggle togSound;

    public Toggle togDown;

    public TextMeshProUGUI txtLevel;

    public TextMeshProUGUI txtMoves;

    public List<BallNumItem> ballNumItems;

    public Transform DownTrans;


    // Start is called before the first frame update
    void Start()
    {
        btnRestart.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            ScenesMgr.GetInstance().LoadSceneAsyn("GameScene", () =>
            {

            });

        });

        btnHome.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            ScenesMgr.GetInstance().LoadSceneAsyn("StoreScene", () =>
            {

            });

        });

        togDown.onValueChanged.AddListener((value) =>
        {
            if(value) 
            {
                DownTrans.DOKill();
                DownTrans.DOScaleY(1, 0.3f);
            }
            else
            {
                DownTrans.DOKill();
                DownTrans.DOScaleY(0, 0.3f);
            }
        });

        togSound.onValueChanged.AddListener((value) =>
        {
            //改声音
            MusicMgr.GetInstance().ChangeSoundAndTapticValue(value);

           
        });

        txtLevel.text = "LEVEL "+ LevelManager.GetInstance().currentLevel;
        txtMoves.text = LevelManager.GetInstance().CurLevelData.moveNum.ToString();
        InitSound();
    }

    public void InitSound()
    {
        togSound.isOn = UserData.GetInstance().soundOn == 1;
    }
    /// <summary>
    /// 更新步数文本
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMovesNum(int num)
    {
        txtMoves.text = num.ToString();
    }

    public void InitNumItems()
    {
        for (int i = 0; i < ballNumItems.Count; i++)
        {
            ballNumItems[i].Init(i);
        }
    }
    public void UpdateItemNum(int index,int num)
    {
        ballNumItems[index].UpdateNum(num); 
    }

    public void UpdateItemState(int index)
    {
        ballNumItems[index].UpdateState();
    }
}
