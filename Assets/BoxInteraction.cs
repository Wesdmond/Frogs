using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteraction : MonoBehaviour, ITongueInteractable
{
    public void Interact(Tongue context)
    {
        context.PullTargetToFrog(transform);
    }
}
