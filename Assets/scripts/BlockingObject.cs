using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingObject : MonoBehaviour
{
    [Min(0.1f)]
    public float moveSpeed;
    public BoundaryChecker rightBoundaryChecker;
    public BoundaryChecker leftBoundaryChecker;
    public BoundaryChecker topBoundaryChecker;
    public BoundaryChecker bottomBoundaryChecker;

    public bool CanBeTraversed { get; set; }
    public bool CanBePushed { get; set; }
    public bool isMoving;

    private void Awake()
    {
        isMoving = false; 
    }

    bool canGoTo(Vector2 direction)
    {
        if (direction == Vector2.left) return !leftBoundaryChecker.IsColliding || canGoTrough(leftBoundaryChecker.collidingWith);
        if (direction == Vector2.right) return !rightBoundaryChecker.IsColliding || canGoTrough(rightBoundaryChecker.collidingWith);
        if (direction == Vector2.up) return !topBoundaryChecker.IsColliding || canGoTrough(topBoundaryChecker.collidingWith);
        else return !bottomBoundaryChecker.IsColliding || canGoTrough(bottomBoundaryChecker.collidingWith);
    }

    public float tryToPush(Vector2 direction)
    {
        if(!isMoving)
            if (canGoTo(direction))
            {
                IEnumerator co = smoothTranslate(direction);
                StartCoroutine(co);
                return moveSpeed; // If it can move, tell the player at what speed
            }
        return -1; // If can't move, return -1
    }

    private bool canGoTrough(GameObject collidingWith)
    {
        if (collidingWith)
        {
            BlockingObject blockingObject = collidingWith.GetComponent<BlockingObject>();
            if (blockingObject)
                return blockingObject.CanBeTraversed;
            else
                return false;
        }
        else return true; // Is not colliding with anything
    }

    IEnumerator smoothTranslate(Vector2 direction)
    {
        isMoving = true;
        float startime = Time.time;
        Vector2 startPostion = transform.position; //Starting position.
        Vector2 endPosition = (Vector2)transform.position + direction; //Ending position.

        while (Vector2.Distance(transform.position, endPosition) > 0.3f && ((Time.time - startime) * moveSpeed) < 1f)
        {
            float move = Mathf.Lerp(0, 1, (Time.time - startime) * moveSpeed);
            transform.position += (Vector3)(direction * move);
            yield return null;
        }
        isMoving = false;
        transform.position = endPosition;
    }

}
