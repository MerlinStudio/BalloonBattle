using UnityEngine;
using DG.Tweening;

public class ShakeButton : MonoBehaviour
{
    private RectTransform ThisObject;

    void Start()
    {
        ThisObject = GetComponent<RectTransform>();
        ThisObject.DOShakePosition(5, new Vector2(10,10), 3, 1, false, true).SetLoops(-1, LoopType.Restart).SetDelay(2);
    }   
}
