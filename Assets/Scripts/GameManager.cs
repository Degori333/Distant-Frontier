using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
//b3d78b82110b9b278867fa5614111f7a4eaaa342188497eacb680712eb313228

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject mainMenu;
    public SpawnManager spawnManager;
    public bool gameStarted = false;
    public bool gamePaused = false;
    public AudioMixer audioMixer;
    public TextMeshProUGUI percentageMaster;
    public TextMeshProUGUI percentageMusic;
    public TextMeshProUGUI percentageSound;

    private void Start()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 0;
        pauseMenuUI.gameObject.SetActive(false);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !gameStarted)
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1;
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused && gameStarted)
        {
            Time.timeScale = 0;
            pauseMenuUI.gameObject.SetActive(true);
            gamePaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gamePaused && gameStarted)
        {
            Resume();
        }
    }

    public void Resume()
    {
        gamePaused = false;
        if (!spawnManager.shopOpen)
        {
            Time.timeScale = 1;
        }
        pauseMenuUI.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        percentageMaster.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        percentageMusic.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void SetSoundVolume(float value)
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(value) * 20);
        percentageSound.text = Mathf.RoundToInt(value *100) + "%";
    }
}
