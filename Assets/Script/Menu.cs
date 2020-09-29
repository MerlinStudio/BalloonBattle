using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform ContentSpriteChar;
    [SerializeField] private Text InputFieldName, DropdownLabel;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private RectTransform Gun1, Gun2;
    private RectTransform GameObject;

    public static int NumberSprute;
    public static int PosSlider;

    void Start()
    {
        GameObject = GetComponent<RectTransform>();
        PosSlider = 750;
        NumberSprute = 0;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "Slide left":
                LeftButton();
                break;

            case "Slide right":
                RightButton();
                break;

            case "Start":
                SaveData();
                Loadscene.instance.LoadScene(1);
                break;

            case "Gun 0":
                Aiming.NumberGun = 0;
                MoveButton(0,150,-150);
                break;

            case "Gun 1":
                Aiming.NumberGun = 1;
                MoveButton(-150,150,0);
                break;

            case "Gun 2":
                Aiming.NumberGun = 2;
                MoveButton(-150,0,150);
                break;

            case "Pause":
                Time.timeScale = 0;
                MenuPanel.SetActive(true);
                break;

            case "BackPause":
                Time.timeScale = 1;
                MenuPanel.SetActive(false);
                break;

            case "Menu":
                Time.timeScale = 1;
                MenuPanel.SetActive(false);
                Loadscene.instance.LoadScene(0);
                break;

            case "Button next":
                Controller.instance.NextMoveBot();
                break;

            case "Start game":
                MenuPanel.SetActive(false);
                Controller.instance.StartGame();
                break;

            case "Show ad":
                AD.instance.UserOptToWatchAd();
                break;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    }
    public void MoveButton(int posXGun1, int posXGun2, int posXGameObj)
    {
        GameObject.DOAnchorPos(new Vector2(posXGameObj, 50), 0.2f);
        Gun1.DOAnchorPos(new Vector2(posXGun1, 0), 0.2f);
        Gun2.DOAnchorPos(new Vector2(posXGun2, 0), 0.2f);
    }
    public void LeftButton()
    {
        if (NumberSprute > 0)
        {
            NumberSprute--;
            PosSlider += 250;
            ContentSpriteChar.DOAnchorPos(new Vector2(PosSlider, 0), 0.7f);
        }
    }

    public void RightButton()
    {
        if (NumberSprute < 6)
        {
            NumberSprute++;
            PosSlider -= 250;
            ContentSpriteChar.DOAnchorPos(new Vector2(PosSlider, 0), 0.7f);
        }
    }

    public void SaveData()
    {
        SaveJson.instance.save.NumberBots = int.Parse(DropdownLabel.GetComponent<Text>().text);
        SaveJson.instance.save.Name = InputFieldName.GetComponent<Text>().text;
        SaveJson.instance.save.PlayerBalloon = NumberSprute;
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(SaveJson.instance.save));
    }
}
