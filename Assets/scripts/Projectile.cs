using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector2 direction;
    public float moveSpeed;
  
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + ( new Vector3(direction.x,direction.y,0) * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "boundChecker")
        {
            if (collision.gameObject.tag == "Player")
                collision.gameObject.GetComponent<PlayerController>().hurt();
            Destroy(gameObject);
        }
    }

}
