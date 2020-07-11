using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : MonoBehaviour
{
    public GameObject glitchPiece;


    private float MIN_REPLICATE_TIMER;
    private float MAX_REPLICATE_TIMER;
    private float replicateTimer;
    private Vector2[] positionsArray = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
    private List<Vector2> positions;
    private int maxReplications;
    private int currentReplications;
    private GlitchesController glitchesController;
    private void Awake()
    {
        glitchesController = GameObject.Find("GlitchesController").GetComponent<GlitchesController>();
        glitchesController.registerGlitch(gameObject.name);
        MIN_REPLICATE_TIMER = 1;
        MAX_REPLICATE_TIMER = 3;
        positions = new List<Vector2>();
        positions.AddRange(positionsArray);      
        replicateTimer = UnityEngine.Random.Range(MIN_REPLICATE_TIMER, MAX_REPLICATE_TIMER);
        maxReplications = UnityEngine.Random.Range(2, 5);
        currentReplications = 0;
    }

    private void Update()
    {
        if (glitchesController.canSpawnMore(gameObject.name))
        {
            if (currentReplications < maxReplications)
            {
                if (replicateTimer < 0)
                {
                    GameObject newGlitchPiece = glitchPiece;
                    Vector3 newPosition = transform.position + (Vector3)positions[UnityEngine.Random.Range(0, positions.Count)];
                    if (newPosition.x >= 0 && newPosition.y >= 0)
                    {
                        positions.Remove(newPosition);
                        Instantiate(newGlitchPiece, newPosition, Quaternion.identity);
                        replicateTimer = UnityEngine.Random.Range(MIN_REPLICATE_TIMER, MAX_REPLICATE_TIMER);
                        currentReplications += 1;
                        glitchesController.registerSpawn(gameObject.name);
                    }
                }
                else
                {
                    replicateTimer -= Time.deltaTime;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(string.Format("Colliding with {0}", collision.gameObject.name));
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag!="walls" && collision.gameObject.tag != "glitch")
        {
            Destroy(collision.gameObject);
        }
    }
}
