using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        AudioManager.Instance.Play("MainTheme");
    }

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
