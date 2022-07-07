using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITongueInteractable
{
    public void Interact(Tongue context);
    public void Abort();
}
