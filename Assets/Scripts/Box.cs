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

    public void Drown() 
    {
        if (_boxCollider2D.IsTouchingLayers(_whatStopsMovement) && !_boxCollider2D.IsTouchingLayers(_drownLayer))
        {

            _animator.SetTrigger("InWater");
            _spriteRenderer.sortingOrder = 9;
            gameObject.layer = (int)Mathf.Log(_drownLayer.value, 2);
            gameObject.tag = "Drown";
        }
    }


}
