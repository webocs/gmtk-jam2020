using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public AudioClip moveSound;
    public AudioClip dieSound;
    public float moveSpeed;
    public BoundaryChecker rightBoundaryChecker;
    public BoundaryChecker leftBoundaryChecker;
    public BoundaryChecker topBoundaryChecker;
    public BoundaryChecker bottomBoundaryChecker;
    public Fader fader;

    const int UPPER_BOUNDARY = 16;
    const int RIGHT_BOUNDARY = 16;
    private bool isMoving;
    private bool isDead;
    private Animator animator;
    private bool goingRight;
    private float oldMoveSpeed; // Used to slow down the player when pushing a heavy object

    // Start is called before the first frame update
    void Awake()
    {
        isDead = false;
        fader = FindObjectOfType<Fader>();
        goingRight = true;
        animator = GetComponentInChildren<Animator>();
        moveSpeed = Constants.MOVE_SPEED;
        oldMoveSpeed = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkMovement();
    }

    private void Update()
    {
        checkInputs();
    }

    private void checkInputs()
    {
        if (!isMoving && !isDead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rightBoundaryChecker.collidingWith && rightBoundaryChecker.collidingWith.GetComponent<Activatable>() != null)
                {
                    rightBoundaryChecker.collidingWith.GetComponent<Activatable>().activate(gameObject);
                }
                if (leftBoundaryChecker.collidingWith && leftBoundaryChecker.collidingWith.GetComponent<Activatable>() != null)
                {
                    leftBoundaryChecker.collidingWith.GetComponent<Activatable>().activate(gameObject);
                }
                if (topBoundaryChecker.collidingWith && topBoundaryChecker.collidingWith.GetComponent<Activatable>() != null)
                {
                    topBoundaryChecker.collidingWith.GetComponent<Activatable>().activate(gameObject);
                }
                if (bottomBoundaryChecker.collidingWith && bottomBoundaryChecker.collidingWith.GetComponent<Activatable>() != null)
                {
                    bottomBoundaryChecker.collidingWith.GetComponent<Activatable>().activate(gameObject);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                reload();
            }
        }
    }

    void checkMovement()
    {
        if (!isMoving && !isDead)
        {
            if (Input.GetKey(KeyCode.W))
            {
                movePlayer(Vector2.up);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movePlayer(Vector2.down);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                movePlayer(Vector2.left);
                if (goingRight)
                {
                    goingRight = false;
                }

            }
            else if (Input.GetKey(KeyCode.D))
            {
                movePlayer(Vector2.right);
                if (!goingRight)
                {
                    goingRight = true;
                }

            } 
          
        }
       

    }

    internal void hurt()
    {
        kill();
    }
    internal void kill()
    {
        if (!isDead)
        {
            isDead = true;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = dieSound;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().volume = .4f;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
            reload();
        }
    }

    private void reload()
    {
        fader.fadeIn();
        Invoke("reloadScene", 1f);
        
    }

    private void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void movePlayer(Vector2 direction)    {
      
        bool canMove = false;
        // Moving up or down
        if (direction.x == 0)
        {
            if (transform.position.y >= 0 && transform.position.y < UPPER_BOUNDARY)
            {
                if (direction == Vector2.up && (!topBoundaryChecker.IsColliding|| canGoTroughOrPush(topBoundaryChecker.collidingWith,direction)) )
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
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = moveSound;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().volume = .5f;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
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
                if(blockingObject.CanBePushed)
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
        if(oldMoveSpeed>0)
            moveSpeed = oldMoveSpeed;
        transform.position = endPosition;
    }
}
