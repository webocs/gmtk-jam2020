using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    public bool IsColliding;
    public GameObject collidingWith;

    private void Awake()
    {
        IsColliding = false;
        collidingWith = null;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "boundChecker" && collision.gameObject.tag != "undestroyable")
        {
            IsColliding = true;
            collidingWith = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsColliding = false;
        collidingWith = null;

    }
}
