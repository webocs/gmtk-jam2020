using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public AudioClip moveSound;
    public float moveSpeed;
    public BoundaryChecker rightBoundaryChecker;
    public BoundaryChecker leftBoundaryChecker;
    public BoundaryChecker topBoundaryChecker;
    public BoundaryChecker bottomBoundaryChecker;
    [Min(1)]
    public float MIN_MOVE_TIMER;
    [Min(1)]
    public float MAX_MOVE_TIMER;


    private float newMoveTimer;
    const int UPPER_BOUNDARY = 100;
    const int RIGHT_BOUNDARY = 100;
    private bool isMoving;
    private Animator animator;
    private bool goingRight;
    private float oldMoveSpeed; // Used to slow down the player when pushing a heavy object

    // Start is called before the first frame update
    void Awake()
    {
        goingRight = true;
        animator = GetComponentInChildren<Animator>();
        oldMoveSpeed = -1;
        MAX_MOVE_TIMER = Math.Max(MIN_MOVE_TIMER, MAX_MOVE_TIMER);
        MIN_MOVE_TIMER = Math.Min(MIN_MOVE_TIMER, MAX_MOVE_TIMER);
        newMoveTimer = UnityEngine.Random.Range(MIN_MOVE_TIMER, MAX_MOVE_TIMER);
    }

    // Update is called once per frame
    void Update()
    {
        if (newMoveTimer < 0)
        {
            checkMovement();
            newMoveTimer = UnityEngine.Random.Range(MIN_MOVE_TIMER, MAX_MOVE_TIMER);
        }
        else
        {
            newMoveTimer -= Time.deltaTime;
        }
    }

    void checkMovement()
    {
        int nextMove = getNextMovement();
        // 0 is left, clockwise
        if (!isMoving)
        {
            if (nextMove == 1)
            {
                move(Vector2.up);
            }
            else if (nextMove == 3)
            {
                move(Vector2.down);
            }
            else if (nextMove == 0)
            {
                move(Vector2.left);
                if (goingRight)
                {
                    animator.SetTrigger("goLeft");
                    goingRight = false;
                }

            }
            else if (nextMove == 2)
            {
                move(Vector2.right);
                if (!goingRight)
                {
                    animator.SetTrigger("goRight");
                    goingRight = true;
                }

            }        
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // gameManager.restart();
        }

    }

    private int getNextMovement()
    {
        return UnityEngine.Random.Range(0, 3);
    }

    void move(Vector2 direction)
    {

        bool canMove = false;
        // Moving up or down
        if (direction.x == 0)
        {
            if (transform.position.y >= 0 && transform.position.y < UPPER_BOUNDARY)
            {
                if (direction == Vector2.up && (!topBoundaryChecker.IsColliding || canGoTroughOrPush(topBoundaryChecker.collidingWith, direction)))
                    canMove = true;
                if (direction == Vector2.down && (!bottomBoundaryChecker.IsColliding || canGoTroughOrPush(bottomBoundaryChecker.collidingWith, direction)))
                    canMove = true;
            }
            else
            {
                canMove = (transform.position.y == 0 && direction == Vector2.up)
                    || (transform.position.y == UPPER_BOUNDARY && direction == Vector2.down);
            }
        }
        // Moving left or right
        else if (direction.y == 0)
        {
            if (transform.position.x > 0 && transform.position.x < RIGHT_BOUNDARY)
            {
                if (direction == Vector2.right && (!rightBoundaryChecker.IsColliding || canGoTroughOrPush(rightBoundaryChecker.collidingWith, direction)))
                    canMove = true;
                if (direction == Vector2.left && (!leftBoundaryChecker.IsColliding || canGoTroughOrPush(leftBoundaryChecker.collidingWith, direction)))
                    canMove = true;
            }
            else
            {
                canMove = (transform.position.x == 0 && direction == Vector2.right)
                    || (transform.position.x == RIGHT_BOUNDARY && direction == Vector2.left);
            }
        }

        if (canMove)
        {
            IEnumerator co = smoothTranslate(direction);
            //GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = moveSound;
            //GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
            StartCoroutine(co);
        }

    }

    private bool canGoTroughOrPush(GameObject collidingWith, Vector2 direction)
    {
        if (collidingWith)
        {
            BlockingObject blockingObject = collidingWith.GetComponent<BlockingObject>();
            if (blockingObject)
            {
                if (blockingObject.CanBePushed)
                {
                    float pushMoveSpeed = blockingObject.tryToPush(direction);
                    if (pushMoveSpeed < 0) return false;
                    else
                    {
                        oldMoveSpeed = moveSpeed;
                        moveSpeed = pushMoveSpeed;
                        return true;
                    }
                }
                return blockingObject.CanBeTraversed;

            }
            else
                return false;
        }
        else return true; // Is not colliding with anything
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        if (oldMoveSpeed > 0)
            moveSpeed = oldMoveSpeed;
        transform.position = endPosition;
    }
}
