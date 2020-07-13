using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    public bool IsColliding;
    public GameObject collidingWith;
    List<GameObject> currentCollisions = new List<GameObject>();

    private

    void Start()
    {      
    }
    private void Awake()
    {
        IsColliding = false;
        collidingWith = null;
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag != "boundChecker" && collision.gameObject.tag != "undestroyable")
    //    {
    //        IsColliding = true;
    //        collidingWith = collision.gameObject;
    //    }
    //}

    private void FixedUpdate()
    {
        bool isCollidingWithSomething = false;
        foreach (GameObject collision in currentCollisions)
        {
            if (collision.gameObject)
            {
                if (collision.gameObject.tag != "boundChecker" && collision.gameObject.tag != "undestroyable")
                {
                    isCollidingWithSomething = true;
                    IsColliding = true;
                    collidingWith = collision.gameObject;
                }
            }
        }
        if (!isCollidingWithSomething)
        {
            IsColliding = false;
            collidingWith = null;
        }
    }
   

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Add the GameObject collided with to the list.
        if (collision.gameObject.tag != "boundChecker" && collision.gameObject.tag != "undestroyable" && collision.gameObject.tag != "pressurePlate")
        {
            currentCollisions.Add(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {

        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(col.gameObject);
      
    }
}
