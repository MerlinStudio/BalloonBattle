using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveJson : MonoBehaviour
{
    public static SaveJson instance = null;

    public Save save = new Save();

    [Serializable]
    public class Save
    {
        public string Name;
        public int PlayerBalloon;
        public int NumberBots;
    }

    void Start()
    {
        if (instance == null) { instance = this; }

        if (!PlayerPrefs.HasKey("Save"))
        {
            PlayerPrefs.SetString("Save", JsonUtility.ToJson(save));
        }
        else
            save = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Save"));
    }
}
