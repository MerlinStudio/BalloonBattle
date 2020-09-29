using UnityEngine;
using UnityEngine.UI;

public class MoveChar : MonoBehaviour
{
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public static float _checkRadius;
    public static bool isGrounded;

    Vector3 point = new Vector3();
    Vector2 mousePos = new Vector2();

    public Sprite[] BalloonStay;
    public Sprite[] BalloonIdle;
    public Color[] ColorArm;
    public GameObject Arrow;
    public SpriteRenderer CharSprite;
    public SpriteRenderer[] Arm;
    public Image Portrait;

    public float SpeedMove, PosCharactar;

    public static Sprite StaticSpriteIdle;
    public static bool isControllChar, isGun;
    public static float timerClick, timerMove, lastClickTime;

    private bool isMove;
    private float PosNow; // позиция бота в прошлом
    private Rigidbody2D Rigidbody2D;
    private SpriteRenderer SpriteRenderer;
    private Camera Camera;
    private Transform PositionChar;

    void Start()
    {
        SpriteRenderer = CharSprite.GetComponent<SpriteRenderer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        PositionChar = GetComponent<Transform>();
        Camera = Camera.main;

        StaticSpriteIdle = BalloonIdle[Controller.PlayerBalloon];
        SpriteRenderer.sprite = BalloonStay[Controller.PlayerBalloon];
        Portrait.sprite = BalloonStay[Controller.PlayerBalloon];
        for (int i = 0; i < Arm.Length; i++)
        {
            Arm[i].color = ColorArm[Controller.PlayerBalloon];
        }
        _checkRadius = 0.5f;
        lastClickTime = 1f;
    }
    void FixedUpdate()
    { 
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, _checkRadius, GroundLayer); // check ground

        MoveJumpLogic();
        CharNoMove();

        if (timerClick > 0) { timerClick -= Time.fixedDeltaTime; }
    }
    public void GetClick()
    {
        if (timerClick > 0 && isGrounded)
        {
            Rigidbody2D.AddForce(Vector2.up * 7.5f, ForceMode2D.Impulse);

            timerClick = 0;
        }
        else
        {
            isMove = true;
            point = Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 25));
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
        if (isGun) { point.x = PosCharactar; }
        if (!isMove && isGrounded && !Aiming.isAiming) {SpriteRenderer.sprite = BalloonStay[Controller.PlayerBalloon];}
        if (!Controller.GamePlayer) { isMove = false; }
        if (isMove)
        {
            Arrow.SetActive(false);
            SpriteRenderer.sprite = BalloonIdle[Controller.PlayerBalloon];
            PosCharactar = PositionChar.position.x;
            if (PosCharactar < point.x)
            {
                SpriteRenderer.flipX = true;
                transform.Translate(Vector2.right * SpeedMove * Time.fixedDeltaTime);   // idle right
                if (PosCharactar + 0.5f >= point.x)
                {
                    isMove = false;
                }
            }
            else
            {
                SpriteRenderer.flipX = false;
                transform.Translate(Vector2.left * SpeedMove * Time.fixedDeltaTime); // idle left
                if (PosCharactar - 0.5f < point.x)
                {
                    isMove = false;
                }
            } 
        }
        if (!Controller.GamePlayer) { Arrow.SetActive(false) ; }
    }
    private void CharNoMove()
    {
        if (timerMove > 0)
        {
            timerMove -= Time.fixedDeltaTime;
            if (timerMove <= 0)
            {
                if (PosNow >= PosCharactar - 0.5f && PosNow <= PosCharactar + 0.5f && !isGun && isControllChar && Controller.GamePlayer)
                {
                    isMove = false;
                    Arrow.SetActive(true);
                    timerMove = 0;
                }
                else
                {
                    PosNow = PosCharactar;
                    timerMove = 1.5f;
                }
            }
        }
    }
}
