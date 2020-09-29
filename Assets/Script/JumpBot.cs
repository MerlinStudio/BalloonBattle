using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBot : MonoBehaviour
{
    //public GameObject Bot;

    private float timerJump;
    private void FixedUpdate()
    {
        if(timerJump > 0)
        {
            timerJump -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.tag == "Ground" || collision.tag == "Player") && timerJump <= 0 && !AIBot.BotAiminig && !Controller.GamePlayer)
        {
            AIBot._rigidbody2DBot.AddForce(Vector2.up * 7.5f, ForceMode2D.Impulse);
            timerJump = 1f;
        }
    }
}
