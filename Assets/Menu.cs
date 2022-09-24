using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public ItemDatabaseSO database;
    public void NewGame()
    {
        database.UpdateID();
        UtilityClass.loadSavedFiles = false;
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        database.UpdateID();
        UtilityClass.loadSavedFiles = true;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
