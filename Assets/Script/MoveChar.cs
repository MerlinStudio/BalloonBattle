using UnityEngine;
using UnityEngine.UI;

public class MoveChar : MonoBehaviour
{
    Vector3 point = new Vector3();
    Vector2 mousePos = new Vector2();

    public float Charactar;

    public GameObject Arrow;

    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Transform GroundCheck;
    public static float _checkRadius;
    [SerializeField] private LayerMask GroundLayer;
    public static bool isGrounded;

    public Sprite[] BalloonStay;
    public Sprite[] BalloonIdle;
    public Color[] ColorArm;
    public GameObject CharSprite;
    public SpriteRenderer[] Arm;
    public Image Portrait;
    public static Sprite StaticSpriteIdle;

    public static bool isControllChar, isGun;
    public float SpeedMove;
    public static float timerClick, timerMove, lastClickTime;

    private bool isMove;
    private float PosNow; // позиция бота в прошлом
    void Start()
    {
        StaticSpriteIdle = BalloonIdle[Controller.PlayerBalloon];
        CharSprite.GetComponent<SpriteRenderer>().sprite = BalloonStay[Controller.PlayerBalloon];
        Portrait.sprite = BalloonStay[Controller.PlayerBalloon];
        for (int i = 0; i < Arm.Length; i++)
        {
            Arm[i].color = ColorArm[Controller.PlayerBalloon];
        }
        _checkRadius = 0.5f;
        lastClickTime = 1f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    { 
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, _checkRadius, GroundLayer); // check ground

        MoveJumpLogic();
        Char_No_Move();

        if (timerClick > 0) { timerClick -= Time.fixedDeltaTime; }
    }

    public void GetClick()
    {
        if (timerClick > 0 && isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * 7.5f, ForceMode2D.Impulse);

            timerClick = 0;
        }

        else
        {
            isMove = true;
            point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 25));
        }
    }

    private void MoveJumpLogic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            if(Controller.GamePlayer && isControllChar)
            {
                if (mousePos.y < 700 && mousePos.y > 100)
                {
                    GetClick();
                }
                if (mousePos.y < 100 && (mousePos.x < 400 || mousePos.x > 880))
                {
                    GetClick();
                }
            }
        }

        if (isGun) { point.x = Charactar; }

        if (!isMove && isGrounded && !Aiming.isAiming) { 
            CharSprite.GetComponent<SpriteRenderer>().sprite = BalloonStay[Controller.PlayerBalloon];}

        if (!Controller.GamePlayer) { isMove = false; }

        if (isMove)
        {
            Arrow.SetActive(false);
            CharSprite.GetComponent<SpriteRenderer>().sprite = BalloonIdle[Controller.PlayerBalloon];
            Charactar = gameObject.GetComponent<Transform>().position.x;
            if (Charactar < point.x)
            {
                CharSprite.GetComponent<SpriteRenderer>().flipX = true;
                transform.Translate(Vector2.right * SpeedMove * Time.fixedDeltaTime);   // idle right
                if (Charactar + 0.5f >= point.x)
                {
                    isMove = false;
                }
            }

            else
            {
                CharSprite.GetComponent<SpriteRenderer>().flipX = false;
                transform.Translate(Vector2.left * SpeedMove * Time.fixedDeltaTime); // idle left
                if (Charactar - 0.5f < point.x)
                {
                    isMove = false;
                }
            } 
        }

        if (!Controller.GamePlayer) { Arrow.SetActive(false) ; }
    }

    private void Char_No_Move()
    {
        if (timerMove > 0)
        {
            timerMove -= Time.fixedDeltaTime;
            if (timerMove <= 0)
            {
                if (PosNow >= Charactar - 0.5f && PosNow <= Charactar + 0.5f && !isGun && isControllChar && Controller.GamePlayer)
                {
                    isMove = false;
                    Arrow.SetActive(true);
                    timerMove = 0;
                }
                else
                {
                    PosNow = Charactar;
                    timerMove = 1.5f;
                }
            }
        }
    }
}
