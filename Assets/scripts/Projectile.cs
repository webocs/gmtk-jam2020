using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector2 direction;
    public float moveSpeed;
    public AudioClip destroySound;

    private void Awake()
    {
        moveSpeed = Constants.PROJECTILE_SPEED;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + ( new Vector3(direction.x,direction.y,0) * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "boundChecker" && collision.gameObject.tag != "undestroyable")
        {
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = destroySound;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().volume = .2f;
            GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
            if (collision.gameObject.tag == "Player")
                collision.gameObject.GetComponent<PlayerController>().hurt();
            Destroy(gameObject);
        }
    }

}
