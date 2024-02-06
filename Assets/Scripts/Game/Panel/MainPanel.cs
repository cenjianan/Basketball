using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel<MainPanel>
{
    public Button btnLevel;

    public TextMeshProUGUI txtMoney;

    public TextMeshProUGUI txtLevel;

    public void UpdateMoney()
    {
        txtMoney.text = UserData.GetInstance().curMoney.ToString();
    }
    private void Start()
    {
        Application.targetFrameRate = 90;
        //加载场景
        btnLevel.onClick.AddListener(() =>
        {
            StopAllCoroutines();
            ScenesMgr.GetInstance().LoadScene("GameScene", () =>
            {

            });
        });

        txtLevel.text = "LEVEL " + LevelManager.GetInstance().currentLevel;

        txtMoney.text = UserData.GetInstance().curMoney.ToString();
    }
}
