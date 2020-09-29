using UnityEngine;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller instance = null;

    public static bool GamePlayer;
    public static bool NextMovePlayer;
    public GameObject MessegeNextMoveBot;
    public GameObject MessegeNextMovePlayer;
    public GameObject PlayerPanel;
    public GameObject YouWin;

    public GameObject Player;
    public GameObject[] Bots;
    public GameObject[] HealthScaleBots;
    public Vector2[] PosHealthScaleBots;
    public GameObject[] Places;

    public Text Name;

    private int SelectBot;
    public static int NumberBots;
    public static int PlayerBalloon;

    public Text Timer;
    public static bool TimerStart;
    public float seconds;

    void Awake()
    {
        if(instance == null) { instance = this; }

        //NumberBots = SaveJson.instance.save.NumberBots;
        //PlayerBalloon = SaveJson.instance.save.PlayerBalloon;
        //Name.text = SaveJson.instance.save.Name;
        NumberBots = 1;
        PlayerBalloon = 2;

        SelectBot = -1;
        GamePlayer = true;
        TimerStart = false;
        PlayerPanel.SetActive(false);

        for (int i = 0; i < Places.Length; i++)  //перемешиваем массив с местами респа
        {
            int j = UnityEngine.Random.Range(0, 5);
            var temp = Places[j];
            Places[j] = Places[i];
            Places[i] = temp;
        }
        Player.transform.position = Places[0].transform.position;   //место появления игрока

        for (int i = 0; i < Bots.Length; i++)  //перемешиваем массив ботов
        {
            if(i != PlayerBalloon)
            {
                int j = UnityEngine.Random.Range(0, 7);
                if(j != PlayerBalloon)
                {
                    var temp = Bots[j];
                    Bots[j] = Bots[i];
                    Bots[i] = temp;
                }
                if (j != PlayerBalloon)
                {
                    var temp = HealthScaleBots[j];
                    HealthScaleBots[j] = HealthScaleBots[i];
                    HealthScaleBots[i] = temp;
                }
            }
        }
        int a = 0;
        int b = 0;
        for (int i = 0; i < NumberBots + a; i++)    //место появления ботов
        {
            if (i != PlayerBalloon)
            {
                Bots[i].SetActive(true);
                HealthScaleBots[i].SetActive(true);
                Bots[i].transform.position = Places[i + 1 + b].transform.position;
                HealthScaleBots[i].GetComponent<RectTransform>().localPosition = PosHealthScaleBots[i + b];
            }
            else { a++; b--; }
        }
    }

    private void Update()
    {
        if (!Bots[0].activeSelf && !Bots[1].activeSelf && !Bots[2].activeSelf && !Bots[3].activeSelf)
        {
            YouWin.SetActive(true);
            TimerStart = false;
            MoveChar.isControllChar = false;
        }

        if (!TimerStart)
        {
            seconds = 30f;
            Timer.GetComponent<Text>().color = new Color(87, 69, 50, 255);
            Time_Snap(seconds);
        }

        if (TimerStart == true && !Healths.isGameOver)
        {
            seconds -= 1 * Time.deltaTime;

            Time_Snap(seconds);

            if (seconds <= 10)
            {
                Timer.GetComponent<Text>().color = new Color(1, 0, 0);
            }

            if (seconds <= 0)
            {
                seconds = 0;
                if(!GamePlayer)
                {
                    AIBot.instance.Delay_End();
                    Bots[SelectBot].GetComponent<AIBot>().enabled = false;
                    Next_Move_Player();
                }
                else
                {
                    MoveChar.isControllChar = false;
                    Aiming.instance.Delay();
                    Next_Move_Bot();
                }
                TimerStart = false;
            }
        }

        if (NextMovePlayer == true)
        {
            Next_Move_Player();
            NextMovePlayer = false;
        }
    }
    public void StartGame()
    {
        NextMovePlayer = true;
    }

    private void Time_Snap(float seconds)
    {
        TimeSpan timer = TimeSpan.FromSeconds(seconds);
        Timer.text = timer.ToString(@"ss\:ff");
    }
    public void Next_Move_Bot()
    {
        TimerStart = false;
        SelectBot++;
        if (SelectBot + 1 > Bots.Length)    //перебераем ботов
        {
            SelectBot = 0;
        }
        if (!Bots[SelectBot].activeSelf)
        {
            Next_Move_Bot();
        }
        Bots[SelectBot].GetComponent<AIBot>().enabled = true;

        PlayerPanel.SetActive(false);
        MessegeNextMoveBot.SetActive(true);
        MessegeNextMoveBot.GetComponent<Animation>().Play("Messege_Next_Move");
        Invoke("Delay_Next_Move_bot", 1f);
    }

    private void Delay_Next_Move_bot()
    {
        HealthScaleBots[SelectBot].GetComponent<Animation>().Play("Health scale");
        MessegeNextMoveBot.SetActive(false);
        TimerStart = true;
        GamePlayer = false;
    }

    public void Next_Move_Player()
    {
        MoveChar.timerClick = 0.2f;
        MessegeNextMovePlayer.SetActive(true);
        MessegeNextMovePlayer.GetComponent<Animation>().Play("Messege_Next_Move");
        if(SelectBot != -1) { HealthScaleBots[SelectBot].GetComponent<Transform>().localScale = new Vector2(1, 1); }   
               
        Invoke("Delay_Next_Move_Player", 1f);
    }

    private void Delay_Next_Move_Player()
    {
        Healths.isGameOver = false;
        TimerStart = true;
        Aiming.isFlagforGun_0 = true;
        MoveChar.timerMove = 1.5f;
        PlayerPanel.SetActive(true);
        MessegeNextMovePlayer.SetActive(false);
        MoveChar.isControllChar = true;
        Aiming.isActionPlayer = true;
        GamePlayer = true;
    }
}
