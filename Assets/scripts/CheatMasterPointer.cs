using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMasterPointer : MonoBehaviour
{
    public AudioClip moveSound;

    public float moveSpeed;
    int UPPER_BOUNDARY = 16;
    int RIGHT_BOUNDARY = 16; 
    const int HARD_UPPER_BOUNDARY = 16;
    const int HARD_RIGHT_BOUNDARY = 16;
    private bool isMoving;
    private Animator animator;
    private bool goingRight;
    public Transform player;
    private float distanceTreshold = 3;
    public GameObject glitchPrefab;

    // Creation Tool
    private int currentSpawnable;
    private GameObject spawnableGhost;
    public GameObject[] spawnables;
    public Sprite[] spawnableGhostSprites;
    Dictionary<Vector2, GameObject> placedObjects;
    public GameObject connectionLine;
    private bool inConnectMode;
    private GameObject currentConnectionLine;
    private Activatable standingOver;
    private Activatable connectionOrigin;
    private List<Activatable> allActivables;
    public Color placingColor;
    public Color connectingColor;
    public AudioClip denySound;

    // RAM system
    public float MAX_RAM;
    public float currentRam;
    RamMeter ramMeter;


    // Start is called before the first frame update
    void Awake()
    {
        ramMeter = FindObjectOfType<RamMeter>();
        ramMeter.max = MAX_RAM;
        allActivables = new List<Activatable>();
        allActivables.AddRange(GameObject.FindObjectsOfType<Activatable>());
        placedObjects = new Dictionary<Vector2, GameObject>();
        spawnableGhost = transform.Find("ghost").gameObject;
        int i = 0;
        spawnableGhostSprites = new Sprite[spawnables.Length];
        foreach (GameObject g  in spawnables)
        {
            spawnableGhostSprites[i] = g.transform.GetComponentInChildren<SpriteRenderer>(true).sprite;
            i++;
        }
        assignGhost();
    }

    // Update is called once per frame
    void Update()
    {
        currentRam = placedObjects.Keys.Count * 0.95f;
        currentRam = Math.Min(currentRam, MAX_RAM);
        ramMeter.current = currentRam;
        if (!inConnectMode) GetComponentInChildren<SpriteRenderer>().color = placingColor;
        checkMovement();
    }

    private float yDistance()
    {
        return  transform.position.y - player.position.y;
    } 
    private float xDistance()
    {
        return  transform.position.x - player.position.x;
    }
    void checkMovement()
    {       
        
        // TODO IF possible limt distance
        if (yDistance() >= distanceTreshold)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y -1 );
        } 
        
        if (yDistance() <= -distanceTreshold)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y +1 );
        } 
        
        if (xDistance() >= distanceTreshold)
        {
            transform.position = new Vector2(transform.position.x -1, transform.position.y);
        } 
        
        if (xDistance() <= -distanceTreshold)
        {
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
        if (!isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                movePointer(Vector2.up);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movePointer(Vector2.down);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                movePointer(Vector2.left);
                if (goingRight)
                {
                    goingRight = false;
                }

            }
            else if (Input.GetKey(KeyCode.D))
            {
                movePointer(Vector2.right);
                if (!goingRight)
                {
                    goingRight = true;
                }

            }
            else if (Input.GetKeyDown(KeyCode.Space) && !inConnectMode)
            {
                cycleSpawnable();
            } 
            else if (Input.GetKeyDown(KeyCode.E) && !inConnectMode)
            {
                placeSpawnable();
            }            
            else if (Input.GetKeyDown(KeyCode.F) && !inConnectMode)
            {
                startLine();
            }
            else if (Input.GetKeyDown(KeyCode.F) && inConnectMode)
            {
                connect();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // gameManager.restart();
        }
        if (!inConnectMode)
            assignGhost();
        else {
            spawnableGhost.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }


    }

   

    private void placeSpawnable()
    {
        if (!isMoving )
        {
            if (!placedObjects.ContainsKey(transform.position) && currentRam < MAX_RAM)
            {
                GameObject g = Instantiate(spawnables[currentSpawnable], transform.position, Quaternion.identity);
                placedObjects[transform.position] = g;
                allActivables.Add(g.GetComponent<Activatable>());
            }
            //else //Cannot undo
            //{
            //    GameObject g = placedObjects[transform.position];
            //    placedObjects.Remove(transform.position);
            //    allActivables.Remove(g.GetComponent<Activatable>());
            //    Destroy(g);                
            //}
            generateRandomEvent();
        }
    }

    private void generateRandomEvent()
    {
        int randomEventChance = UnityEngine.Random.Range(0, 100);
        if (randomEventChance >= 5)
        {
            Activatable a = allActivables[UnityEngine.Random.Range(0, allActivables.Count)];
            a.activate(gameObject);
        }
        else {
            int x = UnityEngine.Random.Range(0,UPPER_BOUNDARY);
            int y = UnityEngine.Random.Range(0,UPPER_BOUNDARY);
            x = (int)Math.Min(player.position.x + distanceTreshold, x);
            y = (int)Math.Min(player.position.y + distanceTreshold, y);
            Instantiate(glitchPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }

    }

    private void assignGhost()
    {
        spawnableGhost.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.7f);
        spawnableGhost.GetComponent<SpriteRenderer>().sprite = spawnableGhostSprites[currentSpawnable];
    }

    private void cycleSpawnable()
    {
        currentSpawnable = (currentSpawnable +1)%spawnables.Length;        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Activatable activatable = collision.gameObject.GetComponent<Activatable>();
        if (activatable != null)
        if (activatable != null)
        {
            standingOver = activatable;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {      
        standingOver = null;      
    }

    void startLine()
    {
        if (standingOver != null)
        {
           
            connectionOrigin = standingOver;
            inConnectMode = true;
            GameObject l = Instantiate(connectionLine, transform.position, Quaternion.identity);
            GetComponentInChildren<SpriteRenderer>().color = connectingColor;
            currentConnectionLine = l;
            ConnectionLine c = l.GetComponent<ConnectionLine>();
            c.origin = transform.position;
            c.end = transform;
        }
    }

    private void connect()
    {
        if (standingOver)
        {
            inConnectMode = false;
            if( standingOver != connectionOrigin)
                connectionOrigin.connectedTo = standingOver;
            else
            {
                cantConnect();
            }
            Destroy(currentConnectionLine);
        }
    }

    private void cantConnect()
    {
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = denySound;
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().volume = .8f;
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
    }

    void movePointer(Vector2 direction)
    {

        bool canMove = false;
        // Moving up or down
        if (direction.x == 0)
        {
            if (transform.position.y >= 0 && transform.position.y < UPPER_BOUNDARY)
            {            
                    canMove = true;
            }
            else
            {
                canMove = (transform.position.y <= 0 && direction == Vector2.up)
                    || (transform.position.y == UPPER_BOUNDARY && direction == Vector2.down);
            }
        }
        // Moving left or right
        else if (direction.y == 0)
        {
            if (transform.position.x > 0 && transform.position.x < RIGHT_BOUNDARY)
            {               
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
            StartCoroutine(co);
        }

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
