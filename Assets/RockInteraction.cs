using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInteraction : MonoBehaviour, ITongueInteractable
{
    public void Interact(Tongue context)
    {
        context.PullFrogToTarget(transform);
    }

    public void AfterInteract()
    {
        
    }
}