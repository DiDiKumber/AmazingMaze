using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static event Action<bool> ShieldActive;

    [SerializeField] Player player;
    [SerializeField] Button btnShield;
    [SerializeField] GameObject preloader;
    [SerializeField] Animator preloaderAnim;

    [SerializeField] GameObject pausePanel;

    float shieldTime = 2f;
    bool shieldIsActive;

    private void Start()
    {
        preloaderAnim.CrossFade("preloaderOff", 0.1f, -1, 0f);
        Invoke("ClosePreloader", 1f);
    }

    void ClosePreloader()
    {
        preloader.SetActive(false);
    }

    public void OnPointerDown()
    {
        shieldIsActive = true;
        ShieldActive?.Invoke(shieldIsActive);
    }

    public void OnPointerUp()
    {
        shieldIsActive = false;
        ShieldActive?.Invoke(shieldIsActive);
        ResetTimer();
    }


    private void Update()
    {
        if(shieldTime > 0 && shieldIsActive)
        {
            shieldTime -= Time.deltaTime;
        }
        else
        {
            shieldIsActive = false;
            ShieldActive?.Invoke(shieldIsActive);
            
        }
    }

    void ResetTimer()
    {
        shieldTime = 2f;
    }

    public void FinishLevel()
    {
        preloader.SetActive(true);
        preloaderAnim.CrossFade("preloaderOn", 0.1f, -1, 0f);
        Invoke("SetNextLevel",2f);
    }

    void SetNextLevel()
    {
        SceneManager.LoadScene(0);
    }




    //*************************************************
    public void SetPause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void SetContinue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void SetExit()
    {
        Application.Quit();
    }
}

