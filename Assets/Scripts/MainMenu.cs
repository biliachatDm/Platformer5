using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputField hName;

    private void Start()
    {
    }

    public void PlayBtPressed()
    {
        PlayerPrefs.SetString("hName", hName.text);
        SceneManager.LoadScene(1);
    }
}
