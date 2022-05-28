using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            if ((SceneManager.GetActiveScene().buildIndex) + 1 < (SceneManager.sceneCount - 1))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else print("This is the last level");
    }
}
