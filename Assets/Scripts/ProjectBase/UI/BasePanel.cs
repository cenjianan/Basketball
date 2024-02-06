using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类 
/// 帮助我门通过代码快速的找到所有的子控件
/// 方便我们在子类中处理逻辑 
/// 节约找控件的工作量
/// </summary>
public abstract class BasePanel<T> : SingletonMono<T> where T :MonoBehaviour
{
    /// <summary>
    /// 隐藏
    /// </summary>
    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 显示
    /// </summary>
    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }
}
