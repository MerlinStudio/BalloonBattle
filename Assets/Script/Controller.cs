using UnityEngine;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static Controller instance = null;

    public GameObject MessegeNextMoveBot, MessegeNextMovePlayer, PlayerPanel, YouWin, Player;

    public GameObject[] Bots;
    public GameObject[] HealthScaleBots;
    public GameObject[] Places;
    public Vector2[] PosHealthScaleBots;

    public Text Name;
    public Text Timer;

    public static int NumberBots, PlayerBalloon;
    public static bool GamePlayer, NextMovePlayer, TimerStart;

    public float seconds;

    private int SelectBot;
    private Text Text;
    void Awake()
    {
        if(instance == null) { instance = this; }

        //NumberBots = SaveJson.instance.save.NumberBots;
        //PlayerBalloon = SaveJson.instance.save.PlayerBalloon;
        //Name.text = SaveJson.instance.save.Name;
        NumberBots = 1;
        PlayerBalloon = 2;

        Text = Timer.GetComponent<Text>();

        //for (int i = 0; i < HealthScaleBots.Length; i++)
        //{
        //    Transforms[i] = HealthScaleBots[i].GetComponent<RectTransform>();
        //    IBot[i] = Bots[i].GetComponent<AIBot>();
        //}

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

        for (int i = 0; i < Bots.Length; i++)  //перемешиваем всех ботов 
        {
            if(i != PlayerBalloon)
            {
                int j = UnityEngine.Random.Range(0, 7);
                if(j != PlayerBalloon)  //боты
                {
                    var temp = Bots[j];
                    Bots[j] = Bots[i];
                    Bots[i] = temp;
                }
                if (j != PlayerBalloon) //их FillAmount
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
            Text.color = new Color(87, 69, 50, 255);
            TimeSnap(seconds);
        }

        if (TimerStart == true && !Healths.isGameOver)
        {
            seconds -= 1 * Time.deltaTime;

            TimeSnap(seconds);

            if (seconds <= 10)
            {
                Text.color = new Color(1, 0, 0);
            }

            if (seconds <= 0)
            {
                seconds = 0;
                if(!GamePlayer)
                {
                    AIBot.instance.DelayEnd();
                    Bots[SelectBot].GetComponent<AIBot>().enabled = false;
                    NextMovePlayerNow();
                }
                else
                {
                    MoveChar.isControllChar = false;
                    Aiming.instance.Delay();
                    NextMoveBot();
                }
                TimerStart = false;
            }
        }

        if (NextMovePlayer == true)
        {
            NextMovePlayerNow();
            NextMovePlayer = false;
        }
    }
    public void StartGame()
    {
        NextMovePlayer = true;
    }

    private void TimeSnap(float seconds)
    {
        TimeSpan timer = TimeSpan.FromSeconds(seconds);
        Timer.text = timer.ToString(@"ss\:ff");
    }
    public void NextMoveBot()
    {
        TimerStart = false;
        SelectBot++;
        if (SelectBot + 1 > Bots.Length)    //перебераем ботов
        {
            SelectBot = 0;
        }
        if (!Bots[SelectBot].activeSelf)
        {
            NextMoveBot();
        }
        Bots[SelectBot].GetComponent<AIBot>().enabled = true;

        PlayerPanel.SetActive(false);
        MessegeNextMoveBot.SetActive(true);
        MessegeNextMoveBot.GetComponent<Animation>().Play("MessegeNextMove");
        Invoke("DelayNextMovebot", 1f);
    }

    private void DelayNextMovebot() // вызов с инвока
    {
        HealthScaleBots[SelectBot].GetComponent<Animation>().Play("HealthScale");
        MessegeNextMoveBot.SetActive(false);
        TimerStart = true;
        GamePlayer = false;
    }

    public void NextMovePlayerNow()
    {
        MoveChar.timerClick = 0.2f;
        MessegeNextMovePlayer.SetActive(true);
        MessegeNextMovePlayer.GetComponent<Animation>().Play("MessegeNextMove");
        if(SelectBot != -1) { HealthScaleBots[SelectBot].GetComponent<Transform>().localScale = new Vector2(1, 1); }   
               
        Invoke("DelayNextMovePlayer", 1f);
    }

    private void DelayNextMovePlayer() // вызов с инвока
    {
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
