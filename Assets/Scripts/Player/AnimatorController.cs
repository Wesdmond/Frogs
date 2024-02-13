using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    // Animator variables hash
    private int _isUpHash;
    private int _isDownHash;
    private int _isLeftHash;
    private int _isRightHash;
    private int _isJumpingHash;
    private int _isShootingHash;
    
    // local variables
    private int _currentDirectionHash;
    private Dictionary<Vector2, int> _directionsToHashesMap;
    

    private void Awake()
    {
        _isUpHash = Animator.StringToHash(FrogConstants.FrogAnimations.moveUp);
        _isDownHash = Animator.StringToHash(FrogConstants.FrogAnimations.moveDown);
        _isLeftHash = Animator.StringToHash(FrogConstants.FrogAnimations.moveLeft);
        _isRightHash = Animator.StringToHash(FrogConstants.FrogAnimations.moveRight);
        _isJumpingHash = Animator.StringToHash(FrogConstants.FrogAnimations.jump);
        _isShootingHash = Animator.StringToHash(FrogConstants.FrogAnimations.shoot);

        _animator.SetBool(_isDownHash, true);
        _currentDirectionHash = _isDownHash;
        
        _directionsToHashesMap = new Dictionary<Vector2, int>()
        {
            [Vector2.right] = _isRightHash,
            [Vector2.left] = _isLeftHash,
            [Vector2.up] = _isUpHash,
            [Vector2.down] = _isDownHash
        };
    }
    
    private void HandleNewDirection(int hash)
    {
        _animator.SetBool(_currentDirectionHash, false);
        _currentDirectionHash = hash;
        _animator.SetBool(_currentDirectionHash, true);
    }

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }
    
    public void ChangeDirection(Vector2 direction)
    {
        HandleNewDirection(_directionsToHashesMap[direction]);
    }

    public void EnableShootingMode()
    {
        _animator.SetBool(_isShootingHash, true);
    }
    
    public void DisableShootingMode()
    {
        _animator.SetBool(_isShootingHash, false);
    }
    
    public void StartJumping()
    {
        _animator.SetBool(_isJumpingHash, true);
    }

    public void StopJumping()
    {
        _animator.SetBool(_isJumpingHash, false);
    }
}