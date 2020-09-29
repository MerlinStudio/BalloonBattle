using System;
using System.Collections;
using UnityEngine;

public class AIBot : MonoBehaviour
{
    public static AIBot instance = null;

    public float RangeMoveX_0 , RangeMoveX_1;

    public GameObject Char;
    private Vector2 Charactar;

    public static Rigidbody2D _rigidbody2DBot;

    private Vector2 Bot;
    public float SpeedMoveBot;
    public GameObject[] Gun;
    public GameObject[] PrefabBullet;
    public Transform[] ShootPostion;
    public Sprite Idle , Stay;

    public GameObject TriggerJumpRight, TriggerJumpLeft, Cloud, Arrow;
    private SpriteRenderer SpriteRenderer;
    private Transform PositionBot;

    private bool isCharRight, isMove, isFlagMinDistance;

    public static bool isStart, BotAiminig;

    private float StartPos;
    private float PosNow; // позиция бота в прошлом
    private float timer;

    private float distance, valueY, SelectDistance;

    public static int NumberGun;

    void Start()
    {
        if(instance == null) { instance = this; }
        ControllJump(false, false);
        isMove = false;
        isStart = true;
        NumberGun = 0;
        _rigidbody2DBot = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PositionBot = GetComponent<Transform>();
        Charactar = Char.GetComponent<Transform>().position;
    }

    void FixedUpdate()
    {
        if (Controller.GamePlayer == false && isStart) // старт
        {
            StartCoroutine(StartRound());
            isStart = false;
        }

        if (isMove)
        { 
            GoPosition(SelectDistance);

            if(timer > 0)   //если застял прицеливаемся и стреляем с места застревания       
            {
                timer -= Time.fixedDeltaTime;
                if(timer <= 0)
                {
                    if(PosNow >= Bot.x - 0.5f && PosNow <= Bot.x + 0.5f)
                    {
                        isMove = false;
                        CheckDirectionDistanceChar();
                        SelectGun();
                        Aiminig();
                        timer = 0;
                    }
                    else
                    {
                        PosNow = Bot.x;
                        timer = 2f;
                    }
                }
            }
        }
    }

    IEnumerator StartRound()   //определяем точку для перемещение
    {

        SpriteRenderer.sprite = Stay;

        Arrow.SetActive(true);
        Cloud.SetActive(true);
        int wait = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(wait);
        Cloud.SetActive(false);
        Arrow.SetActive(false);

        Bot = PositionBot.position;
        StartPos = Bot.x;

        SelectDistance = UnityEngine.Random.Range(-6, 6);

        if (SelectDistance + StartPos > RangeMoveX_1)
        {
            SelectDistance = RangeMoveX_1;
        }
        else if (SelectDistance + StartPos < RangeMoveX_0)
        {
            SelectDistance = RangeMoveX_0;
        }
        else SelectDistance += StartPos;
        timer = 2f;
        isFlagMinDistance = true;
        isMove = true;

    }
    private void GoPosition(float valueDistance)   //передвигаемся к выбранной точке
    {
        Bot = PositionBot.position;
        SpriteRenderer.sprite = Idle;

        if (valueDistance > StartPos)
        {
            ControllJump(false, true);
            transform.Translate(Vector2.right * SpeedMoveBot * Time.fixedDeltaTime); // idle left
            SpriteRenderer.flipX = true;

            if (valueDistance <= Bot.x)
            {
                isMove = false;
                CheckDirectionDistanceChar();
                if (distance < 7 && isFlagMinDistance)
                {
                    CheckMinDistance();
                    isFlagMinDistance = false;
                }
                else
                {
                    SelectGun();
                    Aiminig();
                }
            }
        }

        if (valueDistance <= StartPos)
        {
            ControllJump(true, false);
            transform.Translate(Vector2.left * SpeedMoveBot * Time.fixedDeltaTime); // idle left
            SpriteRenderer.flipX = false;

            if (valueDistance >= Bot.x)
            {
                isMove = false;
                CheckDirectionDistanceChar();
                if(distance < 7 && isFlagMinDistance)
                {
                    CheckMinDistance();
                    isFlagMinDistance = false;
                }
                else
                {
                    SelectGun();
                    Aiminig();
                }
            }
        }
    }
    private void CheckDirectionDistanceChar()    //когда пришли проверяем дистанцию до игрока
    {
        Bot = PositionBot.position;
        float _distance = Bot.x - Charactar.x;
        valueY = Bot.y - Charactar.y;

        if (_distance > 0)
        {
            isCharRight = false;
        }

        else
        {
            isCharRight = true;
        }

        distance = Math.Abs(_distance);
    }
    private void CheckMinDistance()   //если дистанция слишком маленькая идем в точку +-4 от игрока
    {
        StartPos = Bot.x;
        if (isCharRight)
        {
            SelectDistance = Charactar.x - 4;
            GoPosition(SelectDistance);
        }
        else
        {
            SelectDistance = Charactar.x + 4;
            GoPosition(SelectDistance);
        } 

        isMove = true;
    }
    private void SelectGun()   //выбираем оружие в зависимости от полученой дистанции
    {
        if (distance >= 15)
        {
            NumberGun = 0;
        }

        if (distance > 7 && distance < 15)
        {
            NumberGun = 1;
        }

        if (distance <= 7)
        {
            NumberGun = 2;
        }
    }
    private void Aiminig()  //прицеливаемся
    {
        ControllJump(false, false);
        BotAiminig = true;

        if (isCharRight) { SpriteRenderer.flipX = true; }
        else SpriteRenderer.flipX = false;

        if (NumberGun == 0 || NumberGun == 1)
        {
            float angle;
            //int add = UnityEngine.Random.Range(-3, 3);
            if (NumberGun == 0)
            {
                angle = (distance - 10f)/2 + 40f /*+ add*/;
            }
            else
            {

                if (valueY > 1 && distance <= 15)    // бот выш игрока
                {
                    valueY *= 3 * (-1);
                }
                if (valueY < -1 && distance <= 15)  // бот ниже игрока
                {
                    valueY *= 2.5f;
                }
                angle = 65f - (distance - 7) /*+ add*/ + valueY;
            }

            if (!isCharRight)
            {
                Gun[NumberGun].transform.eulerAngles = new Vector3(0f, 180f, true ? angle : -angle);
            }
            else
            {
                Gun[NumberGun].transform.eulerAngles = new Vector3(0f, 0f, true ? angle : -angle);
            }

            if (NumberGun == 0)
            {
                StartCoroutine(FireGun0());
            }
            else
            {
                StartCoroutine(FireGun1());
            }
            
        }

        if (NumberGun == 2)
        {
            StartCoroutine(FireGun2());
        }

    }
    IEnumerator FireGun0()    //логика стрельбы 0го оружия
    {
        Cloud.SetActive(true);
        int wait = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(wait);
        Cloud.SetActive(false);
        Gun[NumberGun].SetActive(true);

        yield return new WaitForSeconds(1f);
        //float random = UnityEngine.Random.Range(-1, 1f);
        Bullet.moveSpeed = distance / 4 + 8 + 20/ distance /*+ random*/ - valueY / 5;

        Instantiate(PrefabBullet[NumberGun], ShootPostion[NumberGun].position, ShootPostion[NumberGun].rotation);

        StartCoroutine(GoBack(1));
    }   
    IEnumerator FireGun1()    //логика стрельбы 1го оружия
    {
        float Power = 1;
        Cloud.SetActive(true);
        int wait = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(wait);
        Cloud.SetActive(false);
        Gun[NumberGun].SetActive(true);
        yield return new WaitForSeconds(1f);
        while (Power >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            {
                float power = UnityEngine.Random.Range(8, 12);
                Bullet.moveSpeed = power - (15 - distance)/2 + (valueY/5 * (-1)+2);
                Instantiate(PrefabBullet[NumberGun], ShootPostion[NumberGun].position, ShootPostion[NumberGun].rotation);
                Power -= 1f / 30;
            }
        }
        StartCoroutine(GoBack(1));
    }   
    IEnumerator FireGun2()    //логика стрельбы 2го оружия
    {
        Cloud.SetActive(true);
        int wait = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(wait);
        Cloud.SetActive(false);
        Gun[NumberGun].SetActive(true);
        yield return new WaitForSeconds(1f);

        if (!isCharRight)
        {
            Gun[2].transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Gun[2].transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        Gun[NumberGun].GetComponent<Animation>().Play("Hummer");
        StartCoroutine(GoBack(1.4f));
    }   
    IEnumerator GoBack(float wait) //действие после стрельмы 
    {
        BotAiminig = false;
        yield return new WaitForSeconds(wait);
        Gun[NumberGun].SetActive(false);

        int run = UnityEngine.Random.Range(0, 3);
        float move = UnityEngine.Random.Range(0, 2);

        if (run == 0)
        {
            while (move >= 0)
            {
                yield return new WaitForFixedUpdate();
                {
                    ControllJump(true, false);
                    SpriteRenderer.flipX = false;
                    transform.Translate(Vector2.left * SpeedMoveBot * Time.fixedDeltaTime); // idle left
                    move -= 1f / 100;
                }
            }
        }
        if (run == 1)
        {
            while (move >= 0)
            {
                yield return new WaitForFixedUpdate();
                {
                    ControllJump(false, true);
                    SpriteRenderer.flipX = true;
                    transform.Translate(Vector2.right * SpeedMoveBot * Time.fixedDeltaTime); // idle right
                    move -= 1f / 100;
                }
            }
        }
        if (run == 2)
        {
            SpriteRenderer.sprite = Stay;
            Cloud.SetActive(true);
            while (move >= 0)
            {
                yield return new WaitForFixedUpdate();
                {
                    move -= 1f / 100;
                }
            }
            Cloud.SetActive(false);
        }
        DelayEnd();
        StopAllCoroutines();
    }         
    private void ControllJump(bool left, bool right)   //управление прыжками
    {
        TriggerJumpRight.SetActive(right);
        TriggerJumpLeft.SetActive(left);
    }
    public void DelayEnd() //конец хода
    {
        SpriteRenderer.sprite = Stay;
        ControllJump(false, false);
        Controller.TimerStart = false;
        Controller.GamePlayer = true;
        Controller.NextMovePlayer = true;
        isStart = true;
        gameObject.GetComponent<AIBot>().enabled = false;
    }
}
