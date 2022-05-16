using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoveCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _counterText;
    [SerializeField]
    private int _counter = 10;

    public void Move()
    {
        if (_counter > 0)
        {
            _counter -= 1;
        }
        else
        {
            Debug.Log("YOU LOSE!");
            GameManager.Instance.LoadLoseScene();
        }
    }

    void Update()
    {
        _counterText.text = _counter.ToString();
    }
}
