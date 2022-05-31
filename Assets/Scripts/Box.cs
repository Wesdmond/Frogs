using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _drownLayer;
    [SerializeField] private LayerMask _whatStopsMovement;

    private bool _isHooked = false;

    public bool isHooked  { get => _isHooked; }

    private void Start()
    {
    }

    public void Drown() 
    {
        if (_boxCollider2D.IsTouchingLayers(_whatStopsMovement))
        {

            _animator.SetTrigger("InWater");
            _spriteRenderer.sortingOrder = 9;
            gameObject.layer = 12;
            gameObject.tag = "Drown";
        }
    }


}
