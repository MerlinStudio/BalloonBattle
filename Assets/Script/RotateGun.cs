using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public static bool lookRight;

    public SpriteRenderer Char;
    private Camera Camera;

    private void Start()
    {
        Camera = Camera.main;
    }
    private void Update()
    {
        if (Aiming.isAiming == true)
        {
            Vector2 mousePos = Input.mousePosition;
            Vector3 point = Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 25));
            var angle = Vector2.Angle(Vector2.right, point - transform.position);//угол между вектором от объекта к мыше и осью х        

            if (angle < 90)
            {
                lookRight = true;
                transform.eulerAngles = new Vector3(0f, 0, transform.position.y < point.y ? angle : -angle);
                Char.flipX = true;
                Char.sprite = MoveChar.StaticSpriteIdle;
            }
            else
            {
                lookRight = false;
                transform.eulerAngles = new Vector3(180, 0, transform.position.y < point.y ? -angle : angle);
                Char.flipX = false;
                Char.sprite = MoveChar.StaticSpriteIdle;
            }
        }
    }
}


