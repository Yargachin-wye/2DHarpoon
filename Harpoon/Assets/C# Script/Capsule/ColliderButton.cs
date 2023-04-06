using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColliderButton : MonoBehaviour
{
    private Harpoon harpoon = null;
    private Rigidbody2D rigidbody2D = null;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        harpoon = Harpoon.instance;
    }

    private void OnMouseDown()
    {
        harpoon.Shot(transform.position);
        Debug.Log("OnMouseDown");
    }
    private void Update()
    {
        rigidbody2D.position = transform.parent.position;
    }
    /*public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == 5) //5 = UI layer
            {
                return true;
            }
        }
        return false;
    }*/
}
