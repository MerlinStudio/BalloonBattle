using UnityEngine;
using DG.Tweening;

public class ShakeButton : MonoBehaviour
{
    private RectTransform ThisObject;

    void Start()
    {
        ThisObject = GetComponent<RectTransform>();
        ThisObject.DOShakeAnchorPos(5, 15, 4, 1, true, true).SetLoops(-1,LoopType.Restart);
    }   
}
