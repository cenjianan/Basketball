using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : BasePanel<WinPanel>
{
    public Button btnNext;

    public Button btnHome;

    public Image imageShine;

    private void Start()
    {
        btnNext.onClick.AddListener(() =>
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


    public override void HideMe()
    {
        base.HideMe();
        imageShine.transform.DOKill();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        imageShine.transform.DOLocalRotate(new Vector3(0, 0, 360), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }
}
