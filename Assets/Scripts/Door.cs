using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BoxCollider2D _boxCollider2D;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    public void Open()
    {
        _animator.SetBool("Open", true);
        _boxCollider2D.enabled = false;
        _spriteRenderer.sortingLayerName = "Player";
    }

    public void Close()
    {
        _animator.SetBool("Open", false);
        _boxCollider2D.enabled = true;
        _spriteRenderer.sortingLayerName = "Items";
    }
}
