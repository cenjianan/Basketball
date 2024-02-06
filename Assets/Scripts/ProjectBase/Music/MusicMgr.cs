using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class MusicMgr : SingletonMono<MusicMgr>
{

    private AudioSource audioSource;
    //唯一的背景音乐组件
    // private AudioSource bkMusic = null;
    //音乐大小
    private float bkValue = 1;

    //音效依附对象
    private GameObject soundObj = null;


    private void Start()
    {
        bkValue = UserData.GetInstance().soundOn;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySoundOneShot(string name)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();       
        }
        audioSource.volume = bkValue;
        //当音效资源异步加载结束后 再添加一个音效
        audioSource.PlayOneShot(Resources.Load<AudioClip>("Music/" + name));

    }


    /// <summary>
    /// 改变音效声音大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundAndTapticValue(bool isValueOn)
    {
        //改震动
        Taptic.tapticOn = isValueOn;
        if (isValueOn)
        {

            bkValue = 1;
            UserData.GetInstance().tapticOn = 1;
            UserData.GetInstance().soundOn = 1;
        }
        else
        {
            bkValue = 0;
            UserData.GetInstance().tapticOn = 0;
            UserData.GetInstance().soundOn = 0;
        }
        UserData.GetInstance().SaveSoundData();
    }

   
}
