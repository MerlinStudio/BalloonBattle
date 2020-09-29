using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public void SelectGun(int num)
    {
        Aiming.NumberGun = num;
    }
}
