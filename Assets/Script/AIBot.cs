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
        Controll_Jump(false, false);
        isMove = false;
        isStart = true;
        NumberGun = 0;
    }

    void FixedUpdate()
    {
        if (Controller.GamePlayer == false && isStart) // старт
        {
            StartCoroutine(Start_Round());
            isStart = false;
        }

        if (isMove)
        { 
            Go_Position(SelectDistance);

            if(timer > 0)   //если застял       
            {
                timer -= Time.fixedDeltaTime;
                if(timer <= 0)
                {
                    if(PosNow >= Bot.x - 0.5f && PosNow <= Bot.x + 0.5f)
                    {
                        isMove = false;
                        Check_Direction_Distance_Char();
                        Select_Gun();
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

    IEnumerator Start_Round()   //определяем точку для перемещение
    {
        _rigidbody2DBot = GetComponent<Rigidbody2D>();

        gameObject.GetComponent<SpriteRenderer>().sprite = Stay;

        Arrow.SetActive(true);
        Cloud.SetActive(true);
        int wait = UnityEngine.Random.Range(1, 3);

        yield return new WaitForSeconds(wait);
        Cloud.SetActive(false);
        Arrow.SetActive(false);

        Bot = gameObject.GetComponent<Transform>().position;
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
    private void Go_Position(float valueDistance)   //передвигаемся к выбранной точке
    {
        Bot = gameObject.GetComponent<Transform>().position;
        gameObject.GetComponent<SpriteRenderer>().sprite = Idle;

        if (valueDistance > StartPos)
        {
            Controll_Jump(false, true);
            transform.Translate(Vector2.right * SpeedMoveBot * Time.fixedDeltaTime); // idle left
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

            if (valueDistance <= Bot.x)
            {
                isMove = false;
                Check_Direction_Distance_Char();
                if (distance < 7 && isFlagMinDistance)
                {
                    Check_min_distance();
                    isFlagMinDistance = false;
                }
                else
                {
                    Select_Gun();
                    Aiminig();
                }
            }
        }

        if (valueDistance <= StartPos)
        {
            Controll_Jump(true, false);
            transform.Translate(Vector2.left * SpeedMoveBot * Time.fixedDeltaTime); // idle left
            gameObject.GetComponent<SpriteRenderer>().flipX = false;

            if (valueDistance >= Bot.x)
            {
                isMove = false;
                Check_Direction_Distance_Char();
                if(distance < 7 && isFlagMinDistance)
                {
                    Check_min_distance();
                    isFlagMinDistance = false;
                }
                else
                {
                    Select_Gun();
                    Aiminig();
                }
            }
        }
    }
    private void Check_Direction_Distance_Char()    //когда пришли проверяем дистанцию до игрока
    {
        Bot = gameObject.GetComponent<Transform>().position;
        Charactar = Char.GetComponent<Transform>().position;
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
    private void Check_min_distance()   //если дистанция слишком маленькая идем в точку +-4 от игрока
    {
        StartPos = Bot.x;
        if (isCharRight)
        {
            SelectDistance = Charactar.x - 4;
            Go_Position(SelectDistance);
        }
        else
        {
            SelectDistance = Charactar.x + 4;
            Go_Position(SelectDistance);
        } 

        isMove = true;
    }
    private void Select_Gun()   //выбираем оружие в зависимости от полученой дистанции
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
        Controll_Jump(false, false);
        BotAiminig = true;

        if (isCharRight) { gameObject.GetComponent<SpriteRenderer>().flipX = true; }
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;

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
                StartCoroutine(Fire_Gun_0());
            }
            else
            {
                StartCoroutine(Fire_Gun_1());
            }
            
        }

        if (NumberGun == 2)
        {
            StartCoroutine(Fire_Gun_2());
        }

    }
    IEnumerator Fire_Gun_0()    //логика стрельбы 0го оружия
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

        StartCoroutine(Go_Back(1));
    }   
    IEnumerator Fire_Gun_1()    //логика стрельбы 1го оружия
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
        StartCoroutine(Go_Back(1));
    }   
    IEnumerator Fire_Gun_2()    //логика стрельбы 2го оружия
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
        StartCoroutine(Go_Back(1.4f));
    }   
    IEnumerator Go_Back(float wait) //действие после стрельмы 
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
                    Controll_Jump(true, false);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
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
                    Controll_Jump(false, true);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    transform.Translate(Vector2.right * SpeedMoveBot * Time.fixedDeltaTime); // idle right
                    move -= 1f / 100;
                }
            }
        }
        if (run == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Stay;
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
        Delay_End();
        StopAllCoroutines();
    }         
    private void Controll_Jump(bool left, bool right)   //управление прыжками
    {
        TriggerJumpRight.SetActive(right);
        TriggerJumpLeft.SetActive(left);
    }
    public void Delay_End() //конец хода
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Stay;
        Controll_Jump(false, false);
        Controller.TimerStart = false;
        Controller.GamePlayer = true;
        Controller.NextMovePlayer = true;
        isStart = true;
        gameObject.GetComponent<AIBot>().enabled = false;
    }
}
