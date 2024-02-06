using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : SingletonMono<Player>
{
    public Transform door;

    private Ray ray;

    private RaycastHit _raycastHit;

    private void Start()
    {
        UserData.GetInstance().LoadData();
    }
    private void Update()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
            return;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
#endif

        if (Input.GetMouseButtonDown(0))
        {


            ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out _raycastHit, 100, 1 << LayerMask.NameToLayer("AI")))
            {
                if (_raycastHit.collider.GetComponent<AIController>().canHit)
                {
                    MenuPanel.GetInstance().ShowMe();
                }
            }
        }

    }
}
