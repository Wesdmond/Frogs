using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVertical : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BoxCollider2D _boxCollider2D;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Vertical")]
    [SerializeField]
    private Transform _visualTransform;

    private Vector3 _spriteCloseOffset;
    private bool _firstOpen = true;

    private void Start()
    {
        _spriteCloseOffset = new Vector3(0, 0.2f);
        print(_spriteCloseOffset);
    }

    public void Open()
    {
        _animator.SetBool("Open", true);
        _boxCollider2D.enabled = false;
        _spriteRenderer.sortingLayerName = "Player";


        if (!_firstOpen)
        {
            _visualTransform.position += _spriteCloseOffset;
        }
        else _firstOpen = false;
    }

    public void Close()
    {
        _animator.SetBool("Open", false);
        _boxCollider2D.enabled = true;
        _spriteRenderer.sortingLayerName = "Items";
        _visualTransform.position -= _spriteCloseOffset;
    }
}
