using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void LoadLoseScene()
    {
        
    }
    
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Resrart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
