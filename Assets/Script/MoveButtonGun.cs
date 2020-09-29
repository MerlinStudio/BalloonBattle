using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveButtonGun : MonoBehaviour
{
    private RectTransform ThisObject;
    void Start()
    {
        ThisObject = gameObject.GetComponent<RectTransform>();
    }
    public void MoveButtonUp(int posX)
    {
        ThisObject = gameObject.GetComponent<RectTransform>();
        ThisObject.DOAnchorPos(new Vector2(posX, 50), 0.2f);
        if (ThisObject.position.y != 50)
        {

        }
    }
    public void MoveButtonDown(int posX)
    {
            ThisObject.DOAnchorPos(new Vector2(posX, 0), 0.2f);
        if (ThisObject.position.y != 0)
        {

        }
    }
}
