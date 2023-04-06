using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenPartButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int _numOfPart;
    [SerializeField] Harpoon _harpoon;
    public void OnPointerDown(PointerEventData eventData)
    {
        _harpoon.ShotToClosestPart(_numOfPart);
    }
}
