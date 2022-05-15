using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [SerializeField]
    private int _maxShootingDuration = 6;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private Transform _movePoint;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private LayerMask _whatStopsMovement;

    private Animator _animator;
    private string _activeDirection = "Down";

    void Start()
    {
        _movePoint.parent = null;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Moving mode
        if (!_playerInput.GetShootingMode())
        {
            _animator.SetBool("Shooting", false);
            transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _movePoint.position) == 0f)
            {
                _animator.SetBool("Jumping", false);

                float _x = _playerInput.GetMovement().x;
                float _y = _playerInput.GetMovement().y;

                if (Mathf.Abs(_x) == 1)
                {
                    // There need to play sound
                    _animator.SetBool("Jumping", true);
                    if (_x > 0)
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Right";
                        _animator.SetBool(_activeDirection, true);
                    }
                    else
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Left";
                        _animator.SetBool(_activeDirection, true);
                    }

                    if (!Physics2D.OverlapCircle(transform.position + new Vector3(_x, 0, 0), .2f, _whatStopsMovement))
                    {
                        _movePoint.position = transform.position + new Vector3(_x, 0, 0);
                    }
                }
                else if (Mathf.Abs(_y) == 1)
                {
                    // There need to play sound
                    _animator.SetBool("Jumping", true);
                    if (_y > 0)
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Up";
                        _animator.SetBool(_activeDirection, true);
                    }
                    else
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Down";
                        _animator.SetBool(_activeDirection, true);
                    }

                    if (!Physics2D.OverlapCircle(transform.position + new Vector3(0, _y, 0), .2f, _whatStopsMovement))
                    {
                        _movePoint.position = transform.position + new Vector3(0, _y, 0);
                    }
                }
            }
        }

        // Shooting mode
        else
        {
            _animator.SetBool("Shooting", true);
            float _x = _playerInput.GetMovement().x;
            float _y = _playerInput.GetMovement().y;

            if (Mathf.Abs(_x) == 1)
            {
                // There need to play shooting sound


                if (_x > 0)
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Right";
                    _animator.SetBool(_activeDirection, true);
                }
                else
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Left";
                    _animator.SetBool(_activeDirection, true);
                }
            }

            else if (Mathf.Abs(_y) == 1)
            {
                // There need to play shooting sound


                if (_y > 0)
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Up";
                    _animator.SetBool(_activeDirection, true);
                }
                else
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Down";
                    _animator.SetBool(_activeDirection, true);
                }
            }

        }
    }
}
