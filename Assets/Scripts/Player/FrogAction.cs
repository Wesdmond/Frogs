using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class FrogAction : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnActionStart;
    public UnityEvent OnActionEnd;
}
