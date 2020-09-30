using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    public static float moveSpeed;
    public Sprite BulletSprite;
    public GameObject MaskBOOM;

    public static int numberGun;

    private Rigidbody2D Rigidbody2D;
    private Animation Animation;

    private bool MaskBoom;

    void Start ()
    {      
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animation = GetComponent<Animation>();
        Rigidbody2D.AddForce (transform.right * moveSpeed, ForceMode2D.Impulse);

        MaskBoom = true;

        if (Controller.GamePlayer)
        {
            numberGun = Aiming.NumberGun;
        }
        else
            numberGun = AIBot.NumberGun;

        if(numberGun == 0)
        {
            Animation.Play("Rotation");
        }

        Invoke("Delay", 7f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Ground" || collision.tag == "Player") && numberGun == 0)
        {
            AfterShot(0.5f);
            Animation.Play("Boom");
            if(MaskBoom == true)
            {
                Instantiate(MaskBOOM, gameObject.transform.position, gameObject.transform.rotation);
                MaskBoom = false;
            }
            
        }

        if (collision.tag == "Ground" && numberGun == 1)
        {
            AfterShot(5f);
            GetComponent<Transform>().rotation = new Quaternion();
        }
    }

    private void AfterShot(float timer)
    {
        GetComponent<SpriteRenderer>().sprite = BulletSprite;
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("Delay", timer);
    }

    private void Delay()
    {
        Destroy(gameObject);    
    }
}
