using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public GameObject test;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowPosition;
    [SerializeField] private LayerMask _layerMaskObjects;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeCd = 0.1f;

    private bool timeOut;
    public static Harpoon instance;
    Vector2 cameraScale;


    private void Awake()
    {
        Vector2 s1 = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 s2 = Camera.main.ScreenToWorldPoint(Vector2.zero);
        cameraScale = new Vector2(s1.x - s2.x, s1.y - s2.y);

        if (instance != null)
        {
            Debug.LogError("Harpoon.instance != null");
            return;
        }
        instance = this;
    }
    public void ShotToClosestPart(int num)
    {
        if (timeOut)
            return;
        float screenW = Screen.width;
        float screenH = Screen.height;
        Vector2 v;
        Collider2D[] colliders;
        switch (num)
        {
            case 0:
                v = Camera.main.ScreenToWorldPoint(new Vector2(screenW * 3 / 4, screenH * 3 / 4));
                break;
            case 1:
                v = Camera.main.ScreenToWorldPoint(new Vector2(screenW * 3 / 4, screenH * 1 / 4));
                break;
            case 2:
                v = Camera.main.ScreenToWorldPoint(new Vector2(screenW * 1 / 4, screenH * 1 / 4));
                break;
            case 3:
                v = Camera.main.ScreenToWorldPoint(new Vector2(screenW * 1 / 4, screenH * 3 / 4));
                break;
            default:
                Debug.LogError("ShotToClosestPart switch default");
                v = Vector2.zero;
                break;
        }
        colliders = Physics2D.OverlapBoxAll(v, cameraScale / 2, 0, _layerMaskObjects);
        float minDist = cameraScale.y / 4;
        Vector2 v2 = v;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (Vector2.Distance(v, colliders[i].transform.position) < minDist)
            {
                v2 = colliders[i].transform.position;
            }
        }
        Shot(v2);
    }
    public void Shot(Vector3 pos)
    {
        if (timeOut)
            return;

        Vector3 difference = pos - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

        GameObject arrow = Instantiate(_arrowPrefab, _arrowPosition.transform.position, transform.rotation);
        arrow.GetComponent<Arrow>().Shot(transform);

        StartCoroutine(TimeOut());
    }
    IEnumerator TimeOut()
    {
        timeOut = true;
        yield return new WaitForSeconds(_timeCd);
        timeOut = false;
    }
}
