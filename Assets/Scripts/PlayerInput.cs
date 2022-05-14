using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _x;
    private float _y;
    private bool freeze = false;
    private Vector2 movement;

    public Vector2 GetMovement()
    {
        return movement;
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
        if (!freeze)
        {
            _x = Input.GetAxisRaw("Horizontal");
            _y = Input.GetAxisRaw("Vertical");

            //_x = Normale(_x);
            //_y = Normale(_y);
            
            movement = new Vector2(_x, _y);
            Debug.Log(movement);
        }
    }
}
