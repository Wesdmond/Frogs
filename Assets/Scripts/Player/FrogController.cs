using System;
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
    [SerializeField] private AnimatorController _animatorController;

    [Header("State")]
    [SerializeField] private FrogStates _currentState = FrogStates.Idle;
    
    [SerializeField] private float paramSpeed = 5.33f;
    
    private void Awake()
    {
        _animatorController.SetAnimatorSpeed(paramSpeed);
        
        _tongue.OnActionStart.AddListener(() => _currentState = FrogStates.Shooting);
        _tongue.OnActionEnd.AddListener(() => _currentState = FrogStates.ShootingMode);

        _frogMovement.OnActionStart.AddListener(() => _currentState = FrogStates.Moving);
        _frogMovement.OnActionStart.AddListener(_animatorController.StartJumping);
        
        _frogMovement.OnActionEnd.AddListener(() => _currentState = FrogStates.Idle);
        _frogMovement.OnActionEnd.AddListener(_animatorController.StopJumping);
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

                        _animatorController.ChangeDirection(direction);
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

                _animatorController.ChangeDirection(direction);
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
            _animatorController.EnableShootingMode();
            _tongue.gameObject.SetActive(true);
        }
        else if (_currentState == FrogStates.ShootingMode)
        {
            _currentState = FrogStates.Idle;
            _frogInput.SwitchCurrentActionMap(FrogConstants.FrogMaps.FrogDefaultMap);
            _animatorController.DisableShootingMode();
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

    public void Restart()
    {
        GameManager.Instance.Resrart();
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
}