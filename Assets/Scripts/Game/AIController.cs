using com.adjust.sdk;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_State
{
    Enter,
    Ide,
    Back
}

public class AIController : SingletonMono<AIController>
{
    public Animator animator;

    public AudioSource audioSource;

    public List<Transform> arrivePosList;

    public List<GameObject> characterList;

    private float time;

    private int nowIde;

    private Vector3 InitPos;

    public bool canHit;

/*    protected override void Awake()
    {
        base.Awake();

            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        
    }

    // Unity will call OnApplicationPause(false) when an app is resumed
    // from the background
    void OnApplicationPause(bool pauseStatus)
    {
        // Check the pauseStatus to see if we are in the foreground
        // or background
        if (!pauseStatus)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        AdjustEvent adjustEvent = new AdjustEvent("lqtmjm");
        //...
        Adjust.trackEvent(adjustEvent);

        //随机一个角色显示
        InitPos = transform.position;

        if (PlayerPrefs.GetInt("Character", -1) == -1)
        {
            int index = Random.Range(0, characterList.Count);
            characterList[index].gameObject.SetActive(true);
            PlayerPrefs.SetInt("Character", index);
            StartCoroutine(MoveToStore());
        }
        else
        {
            characterList[PlayerPrefs.GetInt("Character", -1)].gameObject.SetActive(true);

            StartCoroutine(InitAim());
            
        }
      
      

        audioSource.volume = UserData.GetInstance().soundOn;
    }

    public IEnumerator InitAim()
    {
        nowIde = Random.Range(1, 4);
        animator.SetTrigger("Ide" + nowIde);
        transform.position = arrivePosList[2].position;
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1f);
        MenuPanel.GetInstance().ShowMe();
        canHit = true;
        StartCoroutine(IdeAim());
    }


   public IEnumerator MoveToStore()
    {
        audioSource.Play();
        transform.DOMove(arrivePosList[0].position, 3).SetEase(Ease.Linear);

        yield return new WaitForSeconds(3);
        transform.DORotate(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.2f);

        MusicMgr.GetInstance().PlaySoundOneShot("DoorOpen");
        Player.GetInstance().door.DORotate(Vector3.up * 95, 0.5f).OnComplete(() => 
        {
            Player.GetInstance().door.DORotate(Vector3.zero, 0.5f).SetDelay(1f);


        });
       
        transform.DOMove(arrivePosList[2].position, 3).SetEase(Ease.Linear);
        yield return new WaitForSeconds(3f);
        audioSource.Stop();

        nowIde = Random.Range(1, 4);
        animator.SetTrigger("Ide"+ nowIde);

        yield return new WaitForSeconds(1f);
        MenuPanel.GetInstance().ShowMe();
        canHit = true;
        StartCoroutine(IdeAim());
    }

    public void Back()
    {
        StopAllCoroutines();
        canHit = false;
        StartCoroutine(MoveBack());
    }
    public IEnumerator MoveBack()
    {
        animator.SetTrigger("Walk");
        transform.DORotate(Vector3.up*180, 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.DOMove(arrivePosList[0].position, 3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.8f);
        audioSource.Play();
        Player.GetInstance().door.DORotate(Vector3.up * -95, 0.5f).OnComplete(() =>
        {
            Player.GetInstance().door.DORotate(Vector3.zero, 0.5f).SetDelay(1f);
            

        });
        yield return new WaitForSeconds(1.2f);
        transform.DORotate(Vector3.up * 270, 0.2f);
        yield return new WaitForSeconds(0.2f);

        transform.DOMove(arrivePosList[3].position, 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        audioSource.Stop();
        transform.position = InitPos;

        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].gameObject.SetActive(false);
        }
        int index = Random.Range(0, characterList.Count);
        characterList[index].gameObject.SetActive(true);
        PlayerPrefs.SetInt("Character", index);
        StartCoroutine(MoveToStore());
    }

    public IEnumerator IdeAim()
    {
        while(true) 
        {

            time += Time.deltaTime;
            if (time >= 10)
            {
                UpdateIde();
                time = 0;
            }
            yield return null;
        }
    }

    public void UpdateIde()
    {
        int tempIndex;
        tempIndex  =Random.Range(1, 4);
        while (tempIndex == nowIde)
        {
            tempIndex = Random.Range(1, 4);
        }
        nowIde = tempIndex;
        animator.SetTrigger("Ide" + nowIde);
    }
}
