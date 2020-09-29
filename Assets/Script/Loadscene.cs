using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loadscene : MonoBehaviour
{
    public static Loadscene instance = null;

    [Header("Loading screen")]
    [SerializeField] private GameObject PanelLoadingScane;
    [SerializeField] private Image LoadingImage;
    [SerializeField] private Text ProgressText;

    private int level;

    private void Start()
    {
        if (instance == null) { instance = this; }
    }

    public void LoadScene(int scene) // Call with button 
    {
        level = scene;
        StartCoroutine(AsyncLoad());
    }
    IEnumerator AsyncLoad()
    {
        PanelLoadingScane.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            LoadingImage.fillAmount = progress;
            ProgressText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }
    }
}
