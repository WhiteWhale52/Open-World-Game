using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;    

public class MainMenu : MonoBehaviour
{

    public GameObject settings;
    public AudioMixer AllSounds;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenSettings()
    { 
       
        this.gameObject.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        AllSounds.SetFloat("Volume", volume);
    }

}
