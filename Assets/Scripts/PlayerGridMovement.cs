using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private Transform _movePoint;
    [SerializeField]
    private LayerMask _whatStopsMovement;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private LayerMask whatStopsMovement;


    void Start()
    {
        _movePoint.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _movePoint.position) == 0f)
        {
            float _x = _playerInput.GetMovement().x;
            float _y = _playerInput.GetMovement().y;

            if (Mathf.Abs(_x) == 1)
            {
                if (!Physics2D.OverlapCircle(transform.position + new Vector3(_x, 0, 0), .2f, whatStopsMovement))
                {
                    _movePoint.position = transform.position + new Vector3(_x, 0, 0);
                }
            }
            else if (Mathf.Abs(_y) == 1)
            {
                if (!Physics2D.OverlapCircle(transform.position + new Vector3(0, _y, 0), .2f, whatStopsMovement))
                {
                    _movePoint.position = transform.position + new Vector3(0, _y, 0);
                }
            }
        }
    }
}
