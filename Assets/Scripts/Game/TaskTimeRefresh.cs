using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTimeRefresh : MonoBehaviour
{
    DateTime lastTime;

    //时
    private int minutes;

    //分
    private int seconds;

    //刷新间隔时间
    private int updateSeconds = 3600;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    /// <param name="startTimer"></param>
    /// <param name="endTimer"></param>
    /// <returns></returns>
    public int GetSubSeconds(DateTime startTimer, DateTime endTimer)
    {
        TimeSpan startSpan = new TimeSpan(startTimer.Ticks);

        TimeSpan nowSpan = new TimeSpan(endTimer.Ticks);

        TimeSpan subTimer = nowSpan.Subtract(startSpan).Duration();

        return (int)subTimer.TotalSeconds;
    }
}
