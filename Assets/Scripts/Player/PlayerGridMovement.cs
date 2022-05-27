using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5;
    [SerializeField]
    private int _moveDistance = 500;
    [SerializeField]
    private Transform _movePoint;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private LayerMask _whatStopsMovement;
    [Header("MoveCounter")]
    [SerializeField]
    private MoveCounter _moveCounter;
    [SerializeField]
    private bool enableMoveCounter = false;
    [SerializeField]
    private Animator _animator;

    [Header("Tongue")]
    [SerializeField]
    private Transform _tongueTransform;
    [SerializeField]
    private Tongue _tongue;


    private string _activeDirection = "Down";
    private float _time = 0;
    private float _basicSpeed;


    void Start()
    {
        _basicSpeed = _moveSpeed;
        _movePoint.parent = null;
        _animator.speed = _moveSpeed / 5;
    }

    void Update()
    {

        if (_time <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);
        }
        else
        {
            _time -= _moveSpeed * Time.deltaTime;
        }

        // Moving mode
        if (!_playerInput.GetShootingMode() || Vector3.Distance(transform.position, _movePoint.position) != 0f)
        {

            _animator.SetBool("Shooting", false);
            /*if (_time <= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);
            }
            else
            {
                _time -= _moveSpeed * Time.deltaTime;
            }*/

            if (Vector3.Distance(transform.position, _movePoint.position) == 0.0f)
            {
                _animator.SetBool("Jumping", false);

                float _x = _playerInput.GetMovement().x;
                float _y = _playerInput.GetMovement().y;

                if (Mathf.Abs(_x) == 1)
                {
                    
                    // There need to play sound
                    // AudioManager.Instance.Play("Jump");
                    _animator.SetBool("Jumping", true);
                    _time = 1;
                    if (_x > 0)
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Right";
                        _animator.SetBool(_activeDirection, true);
                        
                        _tongueTransform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Left";
                        _animator.SetBool(_activeDirection, true);
                        _tongueTransform.rotation = Quaternion.Euler(0, 0, 270);
                    }

                    if (!Physics2D.OverlapCircle(transform.position + new Vector3(_x * _moveDistance, 0, 0), .2f, _whatStopsMovement))
                    {
                        _movePoint.position = transform.position + new Vector3(_x * _moveDistance, 0, 0);
                        if (enableMoveCounter)
                        {
                            _moveCounter.Move();
                        }
                    }
                }
                else if (Mathf.Abs(_y) == 1)
                {
                    // There need to play sound
                    _animator.SetBool("Jumping", true);
                    _time = 1;
                    if (_y > 0)
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Up";
                        _animator.SetBool(_activeDirection, true);
                        _tongueTransform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        _animator.SetBool(_activeDirection, false);
                        _activeDirection = "Down";
                        _animator.SetBool(_activeDirection, true);
                        _tongueTransform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (!Physics2D.OverlapCircle(transform.position + new Vector3(0, _y * _moveDistance, 0), .2f, _whatStopsMovement))
                    {
                        _movePoint.position = transform.position + new Vector3(0, _y * _moveDistance, 0);
                        if (enableMoveCounter)
                        {
                            _moveCounter.Move();
                        }
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

            if (_playerInput.GetShoot() && !_tongue.IsRunning)
            {
                if (enableMoveCounter)
                {
                    _moveCounter.Move();
                }
                _tongueTransform.gameObject.SetActive(true);
                _tongue.StartShootTongue();

                _playerInput.SetFreeze(true);
                _playerInput.SetShoot(false);
            }

            _tongueTransform.gameObject.SetActive(_tongue.IsRunning);
            _playerInput.SetFreeze(_tongue.IsRunning);
            if (_tongue.IsRunning)
            {
                _moveSpeed = _tongue.Speed;
            }
            else
            {
                _moveSpeed = _basicSpeed;
            }


            if (Mathf.Abs(_x) == 1)
            {
                // There need to play shooting sound
                // AudioManager.Instance.Play("shoot")


                if (_x > 0)
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Right";
                    _animator.SetBool(_activeDirection, true);
                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Left";
                    _animator.SetBool(_activeDirection, true);
                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

            else if (Mathf.Abs(_y) == 1)
            {
                // There need to play shooting sound
                // AudioManager.Instance.Play("Shoot");

                if (_y > 0)
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Up";
                    _animator.SetBool(_activeDirection, true);
                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    _animator.SetBool(_activeDirection, false);
                    _activeDirection = "Down";
                    _animator.SetBool(_activeDirection, true);
                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
        }
    }
}
