using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject playerDeathBG;

    public GameObject playerDeathPanel;

    public static bool gameIsPause = false;

    public GameObject pauseMenuUI;

    public MainStats playerRef;

    public InventorySO inventory;
    public InventorySO equipment;
    public ItemDatabaseSO itemDatabase;

    private void Start()
    {
        gameIsPause = false;
        playerRef.OnPlayerDeath += PlayerDeath;
    }
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

    public void PlayerDeath()
    {
        StartCoroutine(PlayerDeathCoroutine());
    }

    IEnumerator PlayerDeathCoroutine()
    {
        playerDeathBG.SetActive(true);
        gameIsPause = true;
        yield return new WaitForSeconds(3.5f);
        playerDeathPanel.SetActive(true);
        StopAllCoroutines();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!playerRef.isDead)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gameIsPause = false;
        }
    }

    public void Pause()
    {
        playerDeathBG.SetActive(false);
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
