using System.Collections;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _whatStopsMovement;
    [SerializeField] private float _speed = 6;
    [SerializeField] private float _maxTongueDistance = 6f;

    [Header("References")]
    [SerializeField] private Transform _tongueTransofrm;
    [SerializeField] private SpriteRenderer _tongueSprite;
    [SerializeField] private BoxCollider2D _tongueCollider;
    [SerializeField] private Transform _frogTransform;

    private float _distance = 0;
    private Transform _objectTransform;
    private Coroutine _coroutineInstance;

    public bool IsRunning { get; private set; } = false;
    public float Speed => _speed;

    public void Rotate(Vector3 direction)
    {
        // rotate by 90 or -90 if x != 0
        float xDegrees = 90 * direction.x;
        // rotate by 180 or 0 if y != 0
        float yDegrees = 90 * Mathf.Abs(direction.y) + 90 * direction.y;

        float rotationDegrees = xDegrees + yDegrees;
        transform.rotation = Quaternion.Euler(0, 0, rotationDegrees); 
    }

    public void Shoot()
    {
        StopTongueCoroutine();
        _coroutineInstance = StartCoroutine(ShootTongue());
        IsRunning = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Door":
                Abort();
                StartRetractTongue(null);
                break;
            case "Item":
                Abort();
                StartRetractTongue(collider.gameObject.transform);
                break;
            case "Rock":
                Abort();
                StartPullFrog(collider.gameObject.transform);
                break;
        }
    }

    private void Abort()
    {
        if (_objectTransform != null)
        {
            FixGrid(_objectTransform);
            _objectTransform = null;
        }
        StopTongueCoroutine();
    }

    private void StartRetractTongue(Transform itemTransform)
    {
        StopTongueCoroutine();
        _coroutineInstance = StartCoroutine(RetractTongue(itemTransform));
        IsRunning = true;
    }

    private void StartPullFrog(Transform targetTransform)
    {
        StopTongueCoroutine();
        _coroutineInstance = StartCoroutine(PullFrog(targetTransform));
        IsRunning = true;
    }

    private void StopTongueCoroutine()
    {
        if (_coroutineInstance == null) return;

        StopCoroutine(_coroutineInstance);
        _coroutineInstance = null;
        IsRunning = false;
    }

    private IEnumerator ShootTongue()
    {
        while (_distance < _maxTongueDistance)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance += deltaDistance;
            ChangeScaleTongue(deltaDistance);
            yield return null;
        }
        
        yield return RetractTongue(null);
    }

    private IEnumerator RetractTongue(Transform itemTransform)
    {
        bool isItemNull = itemTransform == null;
        if (!isItemNull) _objectTransform = itemTransform;

        while (_distance > 0)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance -= deltaDistance;
            ChangeScaleTongue(-deltaDistance);
            if (!isItemNull)
            {
                float distanceBetweenItemAndFrog = Vector2.Distance(
                    itemTransform.position,
                    _frogTransform.position
                );
                if (distanceBetweenItemAndFrog > 1)
                {
                    itemTransform.position = Vector3.MoveTowards(
                        itemTransform.position,
                        _frogTransform.position,
                        deltaDistance
                    );
                }
            }

            yield return null;
        }
        if (!isItemNull)
        {
            FixGrid(itemTransform);
            if (itemTransform.tag == "Item")
                itemTransform.GetComponent<Box>().Drown();
        }
        IsRunning = false;
    }

    private IEnumerator PullFrog(Transform targetTransform)
    {
        while (_distance > 0)
        {
            float deltaDistance = _speed * Time.deltaTime;
            _distance -= deltaDistance;
            ChangeScaleTongue(-deltaDistance);
            float distanceBetweenPullTargetAndFrog = Vector2.Distance(
                targetTransform.position,
                _frogTransform.position
            );
            if (distanceBetweenPullTargetAndFrog > 1)
            {
                _frogTransform.position = Vector3.MoveTowards(
                    _frogTransform.position,
                    targetTransform.position,
                    deltaDistance
                );
            }

            yield return null;
        }

        FixGrid(_frogTransform);
        IsRunning = false;
    }

    private void ChangeScaleTongue(float deltaDistance)
    {
        Vector2 deltaScale = new Vector2(0, deltaDistance);
        transform.localPosition += new Vector3(0, (-deltaScale / 2).y, 0);
        _tongueSprite.size += deltaScale;
        _tongueTransofrm.localPosition += new Vector3(0, (-deltaScale / 2).y, 0);
        _tongueCollider.size += deltaScale;
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
}