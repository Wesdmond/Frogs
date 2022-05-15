using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private Animator _buttonAnimator;

    [Header("Button events")]
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonDepressed;

    private string pressButtonTrigger = "Button Pressed";
    private string depressButtonTrigger = "Button Depressed";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnButtonPressed.Invoke();
        _buttonAnimator.SetTrigger(pressButtonTrigger);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnButtonDepressed.Invoke();
        _buttonAnimator.SetTrigger(depressButtonTrigger);
    }


}
