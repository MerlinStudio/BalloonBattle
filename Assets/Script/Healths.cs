using UnityEngine;
using UnityEngine.UI;

public class Healths : MonoBehaviour
{
    public Image HealthScale;
    public GameObject CharSprite, GameOver;
    public bool isCharacter;
    public static bool isGameOver;

    private Animation Animation;
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;
    private MoveChar MoveChar;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Animation = GetComponent<Animation>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        MoveChar = GetComponent<MoveChar>();
    }

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
            MoveChar.enabled = false;
            Controller.TimerStart = false;
            Animator.SetTrigger("GameOver");
            Invoke("DelayEnd", 2);
        }
        if (HealthScale.fillAmount == 0 && !isCharacter)   //GameOver игрока
        {
            SpriteRenderer.enabled = false;
            Animation.Play("BotBOOM");
            Invoke("DelayEndBot", 0.2f);
        }
    }
    private void DelayEnd() // вызов с инвока
    {
        CharSprite.SetActive(false);
        GameOver.SetActive(true);
    }
    private void DelayEndBot()  // вызов с инвока
    {
        gameObject.SetActive(false);
    }
}
