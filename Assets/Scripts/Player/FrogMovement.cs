using System.Collections;
using System.Collections.Generic;
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

        int _x = Mathf.RoundToInt(direction.x);
        int _y = Mathf.RoundToInt(direction.y);
        _tryMoveCoroutine = StartCoroutine(TryMoveCoroutine(_x, _y));
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
    private IEnumerator TryMoveCoroutine(int _x, int _y)
    {
        while (true)
        {
            if (!_isMoving)
            {
                StartCoroutine(MoveCoroutine(_x, _y));
            }
            yield return null;
        }
    }

    private IEnumerator MoveCoroutine(int _x, int _y)
    {
        _isMoving = true;
        Vector3 newPos = transform.position;
        Vector3 frogColliderSize = _frogCollider.bounds.size;
        if (_x != 0)
        {
            Collider2D collision = Physics2D.OverlapBox(transform.position + Vector3.right * _x * frogColliderSize.x, frogColliderSize * 0.95f, 0f, ~_moveThrough);
            if (collision == null)
            {
                newPos = newPos + new Vector3(_moveDistance * _x, 0, 0);
            }
        }
        else if (_y != 0)
        {
            Collider2D collision = Physics2D.OverlapBox(transform.position + Vector3.up * _y * frogColliderSize.x, frogColliderSize * 0.95f, 0f, ~_moveThrough);
            if (collision == null)
            {
                newPos = newPos + new Vector3(0, _moveDistance * _y, 0);
            }
        }

        float _speed = _moveDistance * _moveSpeed;
        for (int i = 0; i < 1 / _moveSpeed; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, _speed);
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
}
