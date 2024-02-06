using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayPush : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DelayBack", 1);
    }

  public void DelayBack()
    {
        PoolMgr.GetInstance().PushObj(gameObject.name, gameObject);
    }
}
