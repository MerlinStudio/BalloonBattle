using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    public static float moveSpeed;
    public Sprite BulletSprite;
    public GameObject MaskBOOM;

    public static int numberGun;

    private Rigidbody2D _rigidbody2D;

    void Start ()
    {      
        _rigidbody2D = GetComponent<Rigidbody2D> ();
        _rigidbody2D.AddForce (transform.right * moveSpeed, ForceMode2D.Impulse);

        if (Controller.GamePlayer)
        {
            numberGun = Aiming.NumberGun;
        }
        else
            numberGun = AIBot.NumberGun;

        if(numberGun == 0)
        {
            gameObject.GetComponent<Animation>().Play("Rotation");
        }

        Invoke("Delay", 7f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((collision.tag == "Ground" || collision.tag == "Player") && numberGun == 0)
        {
            AfterShot(0.5f);
            gameObject.GetComponent<Animation>().Play("Boom");
            //Instantiate(MaskBOOM, gameObject.transform.position, gameObject.transform.rotation);
        }

        if (collision.tag == "Ground" && numberGun == 1)
        {
            AfterShot(5f);
            gameObject.GetComponent<Transform>().rotation = new Quaternion();
        }
    }

    private void AfterShot(float timer)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = BulletSprite;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("Delay", timer);
    }

    private void Delay()
    {
        Destroy(gameObject);    
    }
}
