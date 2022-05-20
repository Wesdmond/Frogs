using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    [Header("Tongue")]
    [SerializeField]
    private Transform _tongueTransofrm;
    [SerializeField]
    private SpriteRenderer _tongueSprite;
    [SerializeField]
    private BoxCollider2D _tongueCollider;

    [Header("Settings")]
    [SerializeField]
    private float _speed = 0.01f;
    [SerializeField]
    private float _maxTongueDistance = 6f;
    private float _distance = 0;
    private bool isRunning = false;
    private Coroutine _coroutineInstance = null;
    private Transform _itemTransform = null;

    public bool IsRunning { get => isRunning;}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // id of Door layer = 8
        if (collider.gameObject.tag == "Door")
        {
            Abort();
        }

        // id of Item layer = 7
        if (collider.gameObject.tag == "Item")
        {
            if (_coroutineInstance != null)
            {
                StopCoroutine(_coroutineInstance);
            }
            _itemTransform = collider.gameObject.transform;
            RetractTongue();
        }
    }

    void Abort()
    {
        if (_coroutineInstance != null)
        {
            StopCoroutine(_coroutineInstance);
        }
        _itemTransform = null;
        RetractTongue();
    }


    private IEnumerator ShootCoroutine()
    {
        isRunning = true;
        while (_distance < _maxTongueDistance)
        {
            float _deltaDistance = _speed * Time.deltaTime;
            _distance += _deltaDistance;
            Vector2 moveVector = new Vector2(0, _deltaDistance);
            ChangeScaleTongue(moveVector);
            yield return null;
        }
        RetractTongue();
    }

    private void FixGrid(Transform transformToFix)
    {
        transformToFix.position = new Vector3(Mathf.Round(transformToFix.position.x), Mathf.Round(transformToFix.position.y), 0);
    }

    private IEnumerator RetractCoroutine()
    {
        while (_distance > 0)
        {
            float _deltaDistance = _speed * Time.deltaTime;
            Vector2 moveVector = new Vector2(0, _deltaDistance);
            ChangeScaleTongue(-moveVector);
            if (_itemTransform != null)
            {
                if (Vector2.Distance(_itemTransform.position, transform.parent.parent.position) > 1)
                {
                    _itemTransform.position = Vector3.MoveTowards(_itemTransform.position, transform.parent.parent.position, _deltaDistance);
                }
                else
                {
                    FixGrid(_itemTransform);
                }
            }
            _distance -= _deltaDistance;
            yield return null;
        }
        _itemTransform = null;
        isRunning = false;
    }

    public void ShootTongue()
    {
        _coroutineInstance = StartCoroutine(ShootCoroutine());
    }

    public void RetractTongue()
    {
        _coroutineInstance = StartCoroutine(RetractCoroutine());
    }

    private void ChangeScaleTongue(Vector2 deltaScale)
    {
        transform.localPosition += new Vector3(0, (-deltaScale / 2).y, 0);
        _tongueSprite.size += deltaScale;
        _tongueTransofrm.localPosition += new Vector3(0, (-deltaScale / 2).y, 0);
        _tongueCollider.size += deltaScale;
    }

}
