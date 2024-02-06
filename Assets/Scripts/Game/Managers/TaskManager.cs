using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TaskManager :BaseManager<TaskManager>
{
    public Task task;

    public TaskData CurTask => task.tasks[task.curIndex];
    public int CurIndex => task.curIndex;
    public TaskManager() 
    {
        string path = Application.persistentDataPath+"/TaskData.json";
        if (File.Exists(path))
        {
            task = JsonMgr.Instance.LoadData<Task>("TaskData");
        }
        else
        {
            task = JsonMapper.ToObject<Task>(Resources.Load<TextAsset>("Level/TaskData").text);

            RandomTask();
        }
    
    }

    public void SaveData()
    {
        JsonMgr.Instance.SaveData(task,"TaskData");
    }

    public void RandomTask()
    {
        int randomNum = UnityEngine.Random.Range(1, 11);   

        switch (randomNum) 
        {
            case 1:
            case 2:
                task.curIndex = 0;
            break;
            case 3:
            case 4:
                task.curIndex = 1;
            break;
            case 5:
            case 6:
            case 7:
            case 8:
                task.curIndex = 2;
                break;
            case 9:
                task.curIndex = 3;
                break;
            case 10:
                task.curIndex = 4;
                break;



        }
        task.freshTime =DateTime.Now;
        SaveData();
    }

}

public class Task
{
    public List<TaskData> tasks = new List<TaskData>();

    public int curIndex =-1;

    public DateTime freshTime = DateTime.Now;
}

public class TaskData
{
    public int needIndex;

    public int targetNum;

    public int rewardNum;
}