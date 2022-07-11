using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class FrogController : MonoBehaviour
{
    [SerializeField] private PlayerInput _frogInput;

    [SerializeField] private FrogMovement _frogMovement;
    [SerializeField] private Tongue _tongue;
    [SerializeField] private Animator _animator;

    [Header("State")]
    [SerializeField] private FrogStates _currentState = FrogStates.Idle;

    //[Header("MoveCounter")]
    //[SerializeField]
    //private MoveCounter _moveCounter;
    //[SerializeField]
    //private bool enableMoveCounter = false;

    private void Awake()
    {
        _tongue.OnActionStart.AddListener(() => _currentState = FrogStates.Shooting);
        _tongue.OnActionEnd.AddListener(() => _currentState = FrogStates.ShootingMode);

        _frogMovement.OnActionStart.AddListener(() => _currentState = FrogStates.Moving);
        _frogMovement.OnActionEnd.AddListener(() => _currentState = FrogStates.Idle);
    }

    public void Move(CallbackContext context)
    {
        bool canMove = (_currentState == FrogStates.Idle) || (_currentState == FrogStates.Moving);
        if (canMove)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    {
                        RotateTongue(context);
                        Vector2 inputVector = context.ReadValue<Vector2>();
                        Vector3 direction = InputToDir(inputVector);

                        _frogMovement.Move(direction);
                        _tongue.Rotate(direction);
                        break;
                    }

                case InputActionPhase.Canceled:
                    {
                        _frogMovement.StopMovement();
                        break;
                    }
            }
        }

        
    }
    public void RotateTongue(CallbackContext context)
    {
        if (_currentState == FrogStates.ShootingMode)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Vector2 inputVector = context.ReadValue<Vector2>();
                Vector3 direction = InputToDir(inputVector);

                _tongue.Rotate(direction);
            }
        }
    }

    public void ChangeShootingMode(CallbackContext context)
    {
        // We are only interested in "started" phase.
        if (!context.started)
        {
            return;
        }

        if (_currentState == FrogStates.Idle)
        {
            _currentState = FrogStates.ShootingMode;
            _frogInput.SwitchCurrentActionMap(FrogConstants.FrogMaps.FrogShootingMap);
            _tongue.gameObject.SetActive(true);
        }
        else if (_currentState == FrogStates.ShootingMode)
        {
            _currentState = FrogStates.Idle;
            _frogInput.SwitchCurrentActionMap(FrogConstants.FrogMaps.FrogDefaultMap);
            _tongue.gameObject.SetActive(false);
        }
    }

    public void Shoot(CallbackContext context)
    {
        // We are only interested in "started" phase.
        if (!context.started)
        {
            return;
        }

        if (_currentState == FrogStates.ShootingMode)
        {
            _tongue.Shoot();
        }
    }
    private Vector3 InputToDir(Vector2 input)
    {
        if(input.x != 0)
        {
            return new Vector3(input.x, 0, 0);
        } 
        if(input.y != 0)
        {
            return new Vector3(0, input.y, 0);
        }
        return Vector3.zero;
    }

    public enum FrogStates
    {
        Idle,
        Moving,
        ShootingMode,
        Shooting
    }

    //void Update()
    //{

    //    if (_time <= 0)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        _time -= _moveSpeed * Time.deltaTime;
    //    }

    //    // Moving mode
    //    if (!_playerInput.GetShootingMode() || Vector3.Distance(transform.position, _movePoint.position) != 0f)
    //    {
    //        _animator.SetBool("Shooting", false);

    //        if (Mathf.RoundToInt(Vector3.Distance(transform.position, _movePoint.position)) == 0)
    //        {
    //            _animator.SetBool("Jumping", false);

    //            float _x = _playerInput.GetMovement().x;
    //            float _y = _playerInput.GetMovement().y;

    //            if (_y < 0) _tongueSortingOrder = 11;
    //            else if (_y != 0 || _x != 0) _tongueSortingOrder = 9;

    //            if (Mathf.Abs(_x) == 1)
    //            {
    //                // There need to play sound
    //                // AudioManager.Instance.Play("Jump");

    //                _animator.SetBool("Jumping", true);
    //                _time = 1;
    //                if (_x > 0)
    //                {
    //                    _animator.SetBool(_activeDirection, false);
    //                    _activeDirection = "Right";
    //                    _animator.SetBool(_activeDirection, true);
    //                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 90);
    //                }
    //                else
    //                {
    //                    _animator.SetBool(_activeDirection, false);
    //                    _activeDirection = "Left";
    //                    _animator.SetBool(_activeDirection, true);
    //                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 270);
    //                }

    //                // Check if there drowning object
    //                if (Physics2D.OverlapCircle(transform.position + new Vector3(_x * _moveDistance, 0, 0), .2f, _drownLayer) != null && 
    //                    Physics2D.OverlapCircle(transform.position + new Vector3(_x * _moveDistance, 0, 0), .2f, _itemLayer) == null)
    //                {
    //                    _movePoint.position = transform.position + new Vector3(_x * _moveDistance, 0, 0);
    //                }

    //                // Check if there no tile collider
    //                else if (Physics2D.OverlapCircle(transform.position + new Vector3(_x * _moveDistance, 0, 0), .2f, _whatStopsMovement) == null)
    //                {
    //                    _movePoint.position = transform.position + new Vector3(_x * _moveDistance, 0, 0);
    //                    if (enableMoveCounter) _moveCounter.Move();
    //                }

    //            }
    //            else if (Mathf.Abs(_y) == 1)
    //            {
    //                // There need to play sound
    //                // AudioManager.Instance.Play("Jump");

    //                _animator.SetBool("Jumping", true);
    //                _time = 1;
    //                if (_y > 0)
    //                {
    //                    _animator.SetBool(_activeDirection, false);
    //                    _activeDirection = "Up";
    //                    _animator.SetBool(_activeDirection, true);
    //                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 180);
    //                }
    //                else
    //                {
    //                    _animator.SetBool(_activeDirection, false);
    //                    _activeDirection = "Down";
    //                    _animator.SetBool(_activeDirection, true);
    //                    _tongueTransform.rotation = Quaternion.Euler(0, 0, 0);
    //                }

    //                // Check if there drowning object
    //                if (Physics2D.OverlapCircle(transform.position + new Vector3(0, _y * _moveDistance, 0), .2f, _drownLayer) != null &&
    //                    Physics2D.OverlapCircle(transform.position + new Vector3(0, _y * _moveDistance, 0), .2f, _itemLayer) == null)
    //                {
    //                    _movePoint.position = transform.position + new Vector3(0, _y * _moveDistance, 0);
    //                }

    //                // Check if there no tile collider
    //                else if (Physics2D.OverlapCircle(transform.position + new Vector3(0, _y * _moveDistance, 0), .2f, _whatStopsMovement) == null)
    //                {
    //                    _movePoint.position = transform.position + new Vector3(0, _y * _moveDistance, 0);
    //                    if (enableMoveCounter) _moveCounter.Move();
    //                }

    //            }
    //        }
    //    }

    //    // Shooting mode
    //    else
    //    {
    //        _animator.SetBool("Shooting", true);
    //        float _x = _playerInput.GetMovement().x;
    //        float _y = _playerInput.GetMovement().y;
    //        if (_y < 0) _tongueSortingOrder = 11;
    //        else if (_y != 0 || _x != 0) _tongueSortingOrder = 9;

    //        if (_playerInput.GetShoot() && !_tongue.IsRunning)
    //        {
    //            if (enableMoveCounter) _moveCounter.Move();

    //            _tongueTransform.gameObject.SetActive(true);
    //            _tongueSpriteRenderer.sortingOrder = _tongueSortingOrder;
    //            _tongue.StartShootTongue();

    //            _playerInput.SetFreeze(true);
    //            _playerInput.SetShoot(false);
    //        }

    //        _tongueTransform.gameObject.SetActive(_tongue.IsRunning);
    //        _playerInput.SetFreeze(_tongue.IsRunning);
    //        if (_tongue.IsRunning)
    //        {
    //            _moveSpeed = _tongue.Speed;
    //        }
    //        else
    //        {
    //            _moveSpeed = _basicSpeed;
    //        }


    //        if (Mathf.Abs(_x) == 1)
    //        {
    //            // There need to play shooting sound
    //            // AudioManager.Instance.Play("shoot")

    //            if (_x > 0)
    //            {
    //                _animator.SetBool(_activeDirection, false);
    //                _activeDirection = "Right";
    //                _animator.SetBool(_activeDirection, true);
    //                _tongueTransform.rotation = Quaternion.Euler(0, 0, 90);
    //            }
    //            else
    //            {
    //                _animator.SetBool(_activeDirection, false);
    //                _activeDirection = "Left";
    //                _animator.SetBool(_activeDirection, true);
    //                _tongueTransform.rotation = Quaternion.Euler(0, 0, 270);
    //            }
    //        }

    //        else if (Mathf.Abs(_y) == 1)
    //        {
    //            // There need to play shooting sound
    //            // AudioManager.Instance.Play("Shoot");

    //            if (_y > 0)
    //            {
    //                _animator.SetBool(_activeDirection, false);
    //                _activeDirection = "Up";
    //                _animator.SetBool(_activeDirection, true);
    //                _tongueTransform.rotation = Quaternion.Euler(0, 0, 180);
    //            }
    //            else
    //            {
    //                _animator.SetBool(_activeDirection, false);
    //                _activeDirection = "Down";
    //                _animator.SetBool(_activeDirection, true);
    //                _tongueTransform.rotation = Quaternion.Euler(0, 0, 0);
    //            }
    //        }

    //    }
    //}
}
