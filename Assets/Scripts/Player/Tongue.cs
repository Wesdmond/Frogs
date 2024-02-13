using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tongue : FrogAction
{
    [Header("Settings")]
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _maxTongueDistance = 6f;
    [SerializeField] private float _tongueWaitTime = 0.1f;

    [Header("References")]
    [SerializeField] private Transform _frogTransform;
    [SerializeField] private SpriteRenderer _tongueSprite;
    [SerializeField] private BoxCollider2D _tongueCollider;

    private float _distance = 0f;
    private float _offset = 0.5f;
    private Transform _targetTransform;

    private Coroutine _coroutineInstance;
    
    #region Unity methods

    private void Awake()
    {
        OnActionStart.AddListener(() => _tongueSprite.enabled = true);
        OnActionEnd.AddListener(() => _tongueSprite.enabled = false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        collider.TryGetComponent<ITongueInteractable>(out var itemInterface);
        if (itemInterface == null)
        {
            RetractTongue();
            return;
        }
        itemInterface.Interact(this);
    }
    #endregion

    #region Public methods available to frog controller
    public void Rotate(Vector3 direction)
    {
        int tongueOrderInLayers = direction.y < 0
            ? FrogConstants.FrogTongueLayers.TongueDefaultOrderInLayers
            : FrogConstants.FrogTongueLayers.TongueNotLookingDownOrderInLayers;
        _tongueSprite.sortingOrder = tongueOrderInLayers;
        transform.position = _frogTransform.position + direction * _tongueSprite.size.x / 2;
        
        // rotate by 90 or -90 if x != 0
        float xDegrees = 90 * direction.x;
        // rotate by 180 or 0 if y != 0
        float yDegrees = 90 * Mathf.Abs(direction.y) + 90 * direction.y;

        float rotationDegrees = xDegrees + yDegrees;
        transform.rotation = Quaternion.Euler(0, 0, rotationDegrees); 
    }

    public void Shoot()
    {
        OnActionStart.Invoke();
        _coroutineInstance = StartCoroutine(ShootCoroutine());
    }
    #endregion

    #region public methods available to interactable objects
    public void PullFrogToTarget(Transform targetTransform)
    {
        Abort();
        _targetTransform = targetTransform;
        _coroutineInstance = StartCoroutine(PullFrogToTargetCoroutine(targetTransform));
    }

    public void PullTargetToFrog(Transform targetTransform)
    {
        Abort();
        _targetTransform = targetTransform;
        _coroutineInstance = StartCoroutine(PullTargetToFrogCoroutine(targetTransform));
    }

    public void RetractTongue()
    {
        Abort();
        _coroutineInstance = StartCoroutine(RetractTongueCoroutine());
    }

    public void BreakTongue()
    {
        Abort();
        OnActionEnd.Invoke();
    }
    #endregion

    #region Private methods
    private void Abort()
    {
        if (_targetTransform != null)
        {
            FixGrid(_targetTransform);
            _targetTransform = null;
        }
        StopTongueCoroutine();
    }

    private IEnumerator ShootCoroutine()
    {
        while (_distance + _offset < _maxTongueDistance)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance += deltaDistance;
            ResizeTongue(deltaDistance);
            yield return null;
        }

        yield return new WaitForSeconds(_tongueWaitTime);
        RetractTongue();
    }
    private IEnumerator RetractTongueCoroutine()
    {
        while (_distance > 0)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance -= deltaDistance;
            ResizeTongue(-deltaDistance);
            yield return null;
        }

        OnActionEnd.Invoke();
    }

    private IEnumerator PullTargetToFrogCoroutine(Transform target)
    {
        while (_distance > 0)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance -= deltaDistance;
            ResizeTongue(-deltaDistance);
            float distanceBetweenPullTargetAndFrog = Vector3.Distance(target.position, _frogTransform.position);

            if (distanceBetweenPullTargetAndFrog > 1)
            {
                target.position = Vector3.MoveTowards(
                    target.position,
                    _frogTransform.position,
                    deltaDistance
                );
            }

            yield return null;
        }

        FixGrid(target);
        _targetTransform = null;
        OnActionEnd.Invoke();
    }

    private IEnumerator PullFrogToTargetCoroutine(Transform target)
    {
        while (_distance > 0)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance -= deltaDistance;
            ResizeTongue(-deltaDistance);
            float distanceBetweenPullTargetAndFrog = Vector3.Distance(target.position, _frogTransform.position);

            if (distanceBetweenPullTargetAndFrog > 1)
            {
                _frogTransform.position = Vector3.MoveTowards(
                    _frogTransform.position,
                    target.position,
                    deltaDistance
                );
            }

            yield return null;
        }

        FixGrid(_frogTransform);
        _targetTransform = null;
        OnActionEnd.Invoke();
    }
    #endregion

    #region Helper methods
    private void StopTongueCoroutine()
    {
        if (_coroutineInstance == null)
        {
            return;
        }

        StopCoroutine(_coroutineInstance);
        _coroutineInstance = null;
    }

    private void ResizeTongue(float deltaDistance)
    {
        Vector2 newSpriteSize = new Vector2(_tongueSprite.size.x, _tongueSprite.size.y + deltaDistance);
        _tongueSprite.size = newSpriteSize;
        
        Vector2 newColliderSize = new Vector2(_tongueCollider.size.x, _tongueCollider.size.y + deltaDistance);
        _tongueCollider.size = newColliderSize;

        transform.localPosition += -transform.up * deltaDistance / 2;
    }

    private static void FixGrid(Transform transformToFix)
    {
        Vector3 position = transformToFix.position;
        position = new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            0
        );
        transformToFix.position = position;
    }

    #endregion
}