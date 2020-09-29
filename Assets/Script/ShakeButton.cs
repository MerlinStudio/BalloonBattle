using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeButton : MonoBehaviour
{
    public static ShakeButton instance = null;
    private RectTransform ThisObject;

    void Start()
    {
        if(instance == null) { instance = this; }
        ThisObject = gameObject.GetComponent<RectTransform>();
        StartCoroutine(ShakeButtonAd());
    }

    IEnumerator ShakeButtonAd()
    {
        yield return new WaitForSeconds(3f);
        {
            ThisObject.DOShakeRotation(2, new Vector3(0, 0, 30), 10, 10, false);
            ThisObject.DOShakeAnchorPos(2, 10, 10, 10, true, false);
        }
        yield return new WaitForSeconds(2f);
        {
            StartCoroutine(ShakeButtonAd());
        }
        
    }
}
