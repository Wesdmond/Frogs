using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _x;
    private float _y;
    private bool _freeze = false;
    private bool _shootingMode = false;
    private bool _shoot = false;
    private Vector2 movement;

    public Vector2 GetMovement()
    {
        return movement;
    }

    public bool GetShootingMode()
    {
        return _shootingMode;
    }

    public bool GetShoot()
    {
        return _shoot;
    }

    public void SetShoot(bool value)
    {
        _shoot = value;
    }

    public void SetFreeze(bool value)
    {
        _freeze = value;
    }

    float Normale(float x)
    {
        if (x > 0)
            return 1;
        if (x < 0)
            return -1;
        return 0;
    }

    void Update()
    {
        if (!_freeze)
        {
            _x = Input.GetAxisRaw("Horizontal");
            _y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _shootingMode = !_shootingMode;
            }
            if (_shootingMode)
            {
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    _shoot = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                Debug.Log("YOU RESTART");
                GameManager.Instance.Resrart();
            }
            _x = Normale(_x);
            _y = Normale(_y);
            
            movement = new Vector2(_x, _y);
        }
    }
}
