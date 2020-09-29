using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformation : MonoBehaviour
{
    private float point;
    private Vector2 pos;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBoom")
        {
        }
    }
}
