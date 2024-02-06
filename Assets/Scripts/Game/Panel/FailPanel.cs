using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : BasePanel<FailPanel>
{
    public Button btnRetry;

    public Button btnHome;

    // Start is called before the first frame update
    void Start()
    {
        btnRetry.onClick.AddListener(() =>
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
        gameObject.SetActive(false);
    }


}
