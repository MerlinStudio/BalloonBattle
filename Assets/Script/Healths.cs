using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healths : MonoBehaviour
{
    public Image HealthScale;
    public GameObject CharSprite;
    public GameObject GameOver;
    public bool isCharacter;
    public static bool isGameOver;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HitBoom")
        {
            Bar(30);
        }

        if (collision.tag == "Hummer")
        {
            Bar(30);
        }

        if (collision.tag == "HAME")
        {
            Bar(30);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "HitFire")
        {
            Bar(0.02f);
        }        
    }

    private void Bar(float hit)
    {
        HealthScale.fillAmount -= hit/100;

        if(HealthScale.fillAmount == 0 && isCharacter)   //GameOver бота
        {
            isGameOver = true;
            gameObject.GetComponent<Animator>().SetTrigger("GameOver");
            gameObject.GetComponent<MoveChar>().enabled = false;
            Controller.TimerStart = false;
            Invoke("Delay_end", 2);
        }
        if (HealthScale.fillAmount == 0 && !isCharacter)   //GameOver игрока
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Animation>().Play("BotBOOM");
            Invoke("Delay_end_bot", 0.2f);
        }
    }
    private void Delay_end()
    {
        CharSprite.SetActive(false);
        GameOver.SetActive(true);
    }
    private void Delay_end_bot()
    {
        gameObject.SetActive(false);
    }
}
