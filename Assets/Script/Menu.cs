using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public Button slideLeft, slideRight, startInMenu, Gun0, Gun1, Gun2, pause, backPause, nextMove, startGame, showAd, menuInPause, menuInWon, menuInGameOver;
    [Space]
    [SerializeField] private RectTransform ContentSpriteChar;
    [SerializeField] private Text InputFieldName, DropdownLabel;
    [SerializeField] private GameObject MenuPause, MenuWon, MenuGameOver, StartPanel;
    private RectTransform RectTransformGun0, RectTransformGun1, RectTransformGun2;

    public static int NumberSprute;
    public static int PosSlider;

    [Header ("Menu-true, Game-false")]
    public bool MenuOrGame;

    void Start()
    {
        if (MenuOrGame)
        {
            slideLeft.onClick.AddListener(LeftButton);
            slideRight.onClick.AddListener(RightButton);
            startInMenu.onClick.AddListener(SaveData);
        }
        else
        {
            RectTransformGun0 = Gun0.gameObject.GetComponent<RectTransform>();
            RectTransformGun1 = Gun1.gameObject.GetComponent<RectTransform>();
            RectTransformGun2 = Gun2.gameObject.GetComponent<RectTransform>();

            menuInPause.onClick.AddListener(() => ActionMenu(MenuPause));
            menuInWon.onClick.AddListener(() => ActionMenu(MenuWon));
            menuInGameOver.onClick.AddListener(() => ActionMenu(MenuGameOver));
            Gun0.onClick.AddListener(() => MoveButton(50, 0, 0, 0));
            Gun1.onClick.AddListener(() => MoveButton(0, 50, 0, 1));
            Gun2.onClick.AddListener(() => MoveButton(0, 0, 50, 2));
            pause.onClick.AddListener(() => Pause(0,true));
            backPause.onClick.AddListener(() => Pause(1, false));
            nextMove.onClick.AddListener(Controller.instance.NextMoveBot);
            startGame.onClick.AddListener(StartGame);
            showAd.onClick.AddListener(AD.instance.UserOptToWatchAd);
        }
        PosSlider = 750;
        NumberSprute = 0;
    }

    public void MoveButton(int posYGun0, int posYGun1, int posYGun2, int NumberGun)
    {
        RectTransformGun0.DOAnchorPos(new Vector2(-150, posYGun0), 0.2f);
        RectTransformGun1.DOAnchorPos(new Vector2(0, posYGun1), 0.2f);
        RectTransformGun2.DOAnchorPos(new Vector2(150, posYGun2), 0.2f);
        Aiming.NumberGun = NumberGun;
    }
    void LeftButton()
    {
        if (NumberSprute > 0)
        {
            NumberSprute--;
            PosSlider += 250;
            ContentSpriteChar.DOAnchorPos(new Vector2(PosSlider, 0), 0.7f);
        }
    }

    void RightButton()
    {
        if (NumberSprute < 6)
        {
            NumberSprute++;
            PosSlider -= 250;
            ContentSpriteChar.DOAnchorPos(new Vector2(PosSlider, 0), 0.7f);
        }
    }

    void Pause(int timeScale, bool menuPanel)
    {
        Time.timeScale = timeScale;
        MenuPause.SetActive(menuPanel);
    }
    void StartGame()
    {
        StartPanel.SetActive(false);
        Controller.instance.StartGame();
    }
    void ActionMenu(GameObject gameObject)
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Loadscene.instance.LoadScene(0);
    }
    public void SaveData()
    {
        SaveJson.instance.save.NumberBots = int.Parse(DropdownLabel.GetComponent<Text>().text);
        SaveJson.instance.save.Name = InputFieldName.GetComponent<Text>().text;
        SaveJson.instance.save.PlayerBalloon = NumberSprute;
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(SaveJson.instance.save));
        Loadscene.instance.LoadScene(1);
    }
}
