using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTarget;
    public float xOffset;
    public float yOffset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(
            cameraTarget.position.x + xOffset,
            cameraTarget.position.y + yOffset,
            transform.position.z);
    }
}