using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;

    public GameObject pauseMenuUI;

    public MainStats playerRef;

    public InventorySO inventory;
    public InventorySO equipment;
    public ItemDatabaseSO itemDatabase;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPause = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPause = true; 
    }

    public void SaveGame()
    {
        playerRef.SaveStats();
        inventory.Save();
        equipment.Save();
    }

    public void RestartGame()
    {
        gameIsPause = false;
        UtilityClass.loadSavedFiles = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        itemDatabase.UpdateID();
        inventory.Clear();
        equipment.Clear();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLastSavedFile()
    {
        gameIsPause = false;
        inventory.Clear();
        equipment.Clear();
        itemDatabase.UpdateID();
        UtilityClass.loadSavedFiles = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
