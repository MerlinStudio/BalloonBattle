using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Aiming : MonoBehaviour, IDragHandler, IPointerDownHandler,IPointerUpHandler
{
    public static Aiming instance = null;

    public GameObject[] Gun;
    public static int NumberGun;

    public GameObject PanelGuns, Bar, MassegeNoBullet;
    public Image PowerBar;

    public GameObject[] PrefabBullet;
    public Transform[] ShootPostion;

    public Image joystickBG, joystick;

    private Vector2 inputVector;

    public static bool isFlagforGun_0, isFlagforGun_3, isAiming, isActionPlayer;

    private void Start()
    {
        if (instance == null) { instance = this; }

        Bullet.moveSpeed = 5;
        NumberGun = 0;
        joystickBG = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {

        MoveChar.isGun = true;

        if(NumberGun == 0 && !isAiming && MoveChar.timerClick <= 0 && isActionPlayer)
        {
            isAiming = true;
            PowerBar.fillAmount = 0;
            Bullet.moveSpeed = 5;
            StartCoroutine(PowerGun_0());
        }

        if (NumberGun == 1 && !isAiming && MoveChar.timerClick <= 0 && isActionPlayer)
        {
            isAiming = true;
            PowerBar.fillAmount = 1;
            Bullet.moveSpeed = 7;
            StartCoroutine(PowerGun_1());
        }
        if (NumberGun == 2 && !isAiming && MoveChar.timerClick <= 0 && isActionPlayer)
        {
            isAiming = true;
            StartCoroutine(PowerGun_2());
        }
        if(!isActionPlayer && !Healths.isGameOver)
        {

            MassegeNoBullet.GetComponent<Animation>().Play("MassegeNoBullet");
        }

        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
        {
            inputVector = new Vector2(pos.x, pos.y);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBG.rectTransform.sizeDelta.y / 2));
        }
        MoveChar.timerClick = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveChar.timerClick = 0.2f;
    } 
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isAiming && NumberGun == 0 && MoveChar.timerClick <= 0 && MoveChar.isGrounded && isFlagforGun_0)
        {
            Instantiate(PrefabBullet[NumberGun], ShootPostion[NumberGun].position, ShootPostion[NumberGun].rotation);
            isActionPlayer = false;
            StopAllCoroutines();
            Invoke("Delay", 0.2f);
        }
        else if (isAiming && NumberGun == 1)
        {
            isActionPlayer = false;
            StopAllCoroutines();
            Invoke("Delay", 0.2f);
        }
        else if (isAiming && NumberGun == 2)
        {
            isFlagforGun_3 = true;
            MoveChar.isControllChar = false;
            Gun[2].GetComponent<Animation>().Play("Hummer");
            isAiming = false;
            isActionPlayer = false;
            StopAllCoroutines();
            Invoke("Delay", 1.6f);
        }
        else
        {
            if(!isFlagforGun_3)
            {
                Delay();
                StopAllCoroutines(); 
            }    
        }
        MoveChar.timerMove = 1.5f;
    }

    public void Delay()
    {
        isFlagforGun_0 = false;
        isFlagforGun_3 = false;
        MoveChar.isControllChar = true;
        MoveChar.isGun = false; //чтоб не бегал
        isAiming = false;

        Gun[NumberGun].SetActive(false);
        PanelGuns.SetActive(true);
        Bar.SetActive(false);
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    private void Amining(bool BarActiv)
    {
        PanelGuns.SetActive(false);
        Gun[NumberGun].SetActive(true);
        if(NumberGun != 2) { Bar.SetActive(BarActiv); }       
    }

    IEnumerator PowerGun_0()
    {
        yield return new WaitForSeconds(0.2f);
        {
            Amining(true);

            while (PowerBar.fillAmount <= 1)
            {
                yield return new WaitForFixedUpdate();
                {
                    Bullet.moveSpeed += 0.1f;
                    PowerBar.fillAmount += 0.1f / 15;
                    if(PowerBar.fillAmount > 0.2f) { isFlagforGun_0 = true; }
                }
                if (PowerBar.fillAmount >= 1)
                {
                    StopAllCoroutines();
                }
            }
        }
    }

    IEnumerator PowerGun_1()
    {
        yield return new WaitForSeconds(0.5f);
        {
            Amining(true);

            while (PowerBar.fillAmount >= 0)
            {
                yield return new WaitForSeconds(0.1f);
                {
                    int ran = Random.Range(11, 15);
                    Bullet.moveSpeed = ran;
                    Instantiate(PrefabBullet[NumberGun], ShootPostion[NumberGun].position, ShootPostion[NumberGun].rotation);
                    PowerBar.fillAmount -= 1f / 30;
                }
                if(PowerBar.fillAmount <= 0)
                {
                    isActionPlayer = false;
                    StopAllCoroutines();
                    Invoke("Delay", 0.2f);
                }
            }
        }      
    }

    IEnumerator PowerGun_2()
    {
        yield return new WaitForSeconds(0.5f);
        {
            Amining(true);
        }
    }
}
