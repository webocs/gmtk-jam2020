using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();         
    }

    public void fadeIn()
    {
        animator.SetTrigger("fadeIn");
    }

    public void fadeOut()
    {
        animator.SetTrigger("fadeOut");
    }
}
