using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FrogMovement : FrogAction
{
    [Header("Settings")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float _moveSpeed = 0.2f;
    [SerializeField] private int _moveDistance = 1;
    [SerializeField] private float _pauseBetweenMovement = 0.075f;
    [SerializeField] private LayerMask _moveThrough;

    [Header("References")]
    [SerializeField] private Collider2D _frogCollider;

    private bool _isMoving = false;
    private Coroutine _tryMoveCoroutine = null;



    #region Public methods
    public void Move(Vector2 direction)
    {
        StopCurrentCoroutine();
        OnActionStart.Invoke();
        // direction = Vector2Int.RoundToInt(direction);
        // int x = Mathf.RoundToInt(direction.x);
        // int y = Mathf.RoundToInt(direction.y);
        _tryMoveCoroutine = StartCoroutine(TryMoveCoroutine(Vector2Int.RoundToInt(direction)));
    }
    public void StopMovement()
    {
        StopCurrentCoroutine();
    }
    #endregion

    #region Private methods
    private void StopCurrentCoroutine()
    {
        if (_tryMoveCoroutine != null)
        {
            StopCoroutine(_tryMoveCoroutine);
            _tryMoveCoroutine = null;
        }
    }
    private IEnumerator TryMoveCoroutine(Vector2Int direction)
    {
        while (true)
        {
            if (!_isMoving)
            {
                StartCoroutine(MoveCoroutine(direction));
            }
            yield return null;
        }
    }

    private IEnumerator MoveCoroutine(Vector2Int direction)
    {
        _isMoving = true;
        Vector3 newPos = transform.position;
        if (direction.x != 0)
        {
            if (CheckCanGo(direction * Vector2.right))
                newPos = newPos + new Vector3(_moveDistance * direction.x, 0, 0);
            
        }
        else if (direction.y != 0)
        {
            if (CheckCanGo(direction * Vector2.up))
                newPos = newPos + new Vector3(0, _moveDistance * direction.y, 0);
        }

        float speed = _moveDistance * _moveSpeed;
        for (int i = 0; i < 1 / _moveSpeed; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed);
            yield return null;
        }

        yield return new WaitForSeconds(_pauseBetweenMovement);
        if (_tryMoveCoroutine == null)
        {
            OnActionEnd.Invoke();
        }
        _isMoving = false;
    }
    #endregion
    
    #region Helper methods

    private bool CheckCanGo(Vector2 direction)
    {
        Vector3 frogColliderSize = _frogCollider.bounds.size;
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + direction, frogColliderSize * 0.95f, 0f);
        if (colliders.Length != 0)
        {
            Collider2D maxCollider2D = colliders[0];
            foreach (Collider2D _collider in colliders)
                maxCollider2D = maxCollider2D.gameObject.layer < _collider.gameObject.layer
                    ? _collider
                    : maxCollider2D;
            if (_moveThrough != (_moveThrough | (1 << maxCollider2D.gameObject.layer)))
                return false;
        }
        return true;
    }
    #endregion

}