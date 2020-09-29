using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public RectTransform ContentSpriteChar;
    public Text InputFieldName;
    public Text DropdownLabel;

    public static int NumberSprute;
    private int PosSlider;

    void Start()
    {
        PosSlider = 750;
        NumberSprute = 0;
    }

    public void Left_Button()
    {
        if (PosSlider < 750)
        {
            NumberSprute--;
            PosSlider += 250;
            ContentSpriteChar.DOAnchorPos(new Vector2(PosSlider, 0), 0.7f);
        }
    }

    public void Right_Button()
    {
        if(PosSlider > -750)
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
