using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerGridMovement : MonoBehaviour
{
    [Range(0.01f, 0.2f)]
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private int _moveDistance = 1;
    [SerializeField] private Transform _movePoint;
    //[SerializeField] private LayerMask _whatStopsMovement;
    //[SerializeField] private LayerMask _drownLayer;
    //[SerializeField] private LayerMask _itemLayer;
    [SerializeField]
    private PlayerInput frogInput;
    [SerializeField]
    private Collider2D frogCollider;
    [SerializeField]
    private LayerMask playerLayer;

    //[Header("MoveCounter")]
    //[SerializeField]
    //private MoveCounter _moveCounter;
    //[SerializeField]
    //private bool enableMoveCounter = false;
    [SerializeField]
    private Animator _animator;

    [Header("Tongue")]
    [SerializeField]
    private Transform _tongueTransform;
    [SerializeField]
    private Tongue _tongue;
    [SerializeField]
    private SpriteRenderer _tongueSpriteRenderer;

    private int _tongueSortingOrder = 11;

    private bool isMoving = false;

    [Header("State")]
    [SerializeField]
    private FrogStates currentState = FrogStates.Idle;

    void Start()
    {
        _movePoint.parent = null;
        _animator.speed = _moveSpeed / 5;
    }

    private Coroutine currentTryMoveCoroutine = null;
    public void Move(CallbackContext context)
    {
        bool canMove = (currentState == FrogStates.Idle) || (currentState == FrogStates.Moving);
        if (!canMove)
        {
            return;
        }
        
        if (context.performed)
        {
            currentState = FrogStates.Moving;
            Vector2 inputVector = context.ReadValue<Vector2>();
            int _x = Mathf.RoundToInt(inputVector.x);
            int _y = Mathf.RoundToInt(inputVector.y);

            if (currentTryMoveCoroutine != null)
            {
                StopCoroutine(currentTryMoveCoroutine);
            }

            currentTryMoveCoroutine = StartCoroutine(TryMoveCoroutine(_x, _y));
        }
        else if (context.canceled)
        {
            if (currentTryMoveCoroutine != null)
            {
                StopCoroutine(currentTryMoveCoroutine);
            }
            currentState = FrogStates.Idle;
        }
    }
    private IEnumerator TryMoveCoroutine(int _x, int _y)
    {
        while (true)
        {
            if (!isMoving)
            {
                StartCoroutine(MoveCoroutine(_x, _y));
            }
            yield return null;
        }
    }
    private IEnumerator MoveCoroutine(int _x, int _y)
    {
        isMoving = true;

        Vector3 frogColliderSize = frogCollider.bounds.size;
        if (_x != 0)
        {
            Collider2D collision = Physics2D.OverlapBox(transform.position + Vector3.right * _x * frogColliderSize.x, frogColliderSize * 0.95f, 0f, playerLayer);
            if (collision == null)
            {
                _movePoint.position = _movePoint.position + new Vector3(_moveDistance * _x, 0, 0);
                _tongueTransform.rotation = Quaternion.Euler(0, 0, 90 * _x);
            }
        }
        else if (_y != 0)
        {
            Collider2D collision = Physics2D.OverlapBox(transform.position + Vector3.up * _y * frogColliderSize.x, frogColliderSize * 0.95f, 0f, playerLayer);
            if (collision == null)
            {
                _movePoint.position = _movePoint.position + new Vector3(0, _moveDistance * _y, 0);
                _tongueTransform.rotation = Quaternion.Euler(0, 0, 90 + 90 * _y);
            }
        }

        float _speed = _moveDistance * _moveSpeed;
        for (int i = 0; i < 1 / _moveSpeed; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _speed);
            yield return null;
        }

        yield return new WaitForSeconds(0.075f);

        isMoving = false;
    }

    public void ChangeShootingMode(CallbackContext context)
    {
        //Debug.Log(context.phase);
        if (!context.started)
        {
            return;
        }

        if (currentState == FrogStates.Idle)
        {
            currentState = FrogStates.ShootingMode;
            frogInput.SwitchCurrentActionMap(FrogConstants.FrogMaps.FrogShootingMap);
            _tongueTransform.gameObject.SetActive(true);
        }
        else
        if (currentState == FrogStates.ShootingMode)
        {
            currentState = FrogStates.Idle;
            frogInput.SwitchCurrentActionMap(FrogConstants.FrogMaps.FrogDefaultMap);
            _tongueTransform.gameObject.SetActive(false);
        }
    }

    public void Shoot(CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        if (currentState == FrogStates.ShootingMode)
        {
            _tongue.StartShootTongue();
        }
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

    enum FrogStates
    {
        Idle,
        Moving,
        ShootingMode,
        shooting
    }
}
