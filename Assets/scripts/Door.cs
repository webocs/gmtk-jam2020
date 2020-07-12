using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public bool status;
    public AudioClip toggleSound;
    
    GameObject openChild;
    GameObject closedChild;

    private void Awake()
    {
        playerCanActivate = false;
        openChild = transform.GetChild(1).gameObject;
        closedChild = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        openChild.SetActive(status);
        closedChild.SetActive(!status);
        CanBeTraversed = status;
    }

    public void toggle()
    {
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().clip = toggleSound;
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().volume = .5f;
        GameObject.Find("SoundPlayer").GetComponent<AudioSource>().Play();
        status = !status;
        if (connectedTo)
        {
            IEnumerator co = smoothActivate();
            StartCoroutine(co);
        }
    }

    public override void activate(GameObject activator)
    {
        
        Debug.Log(activator.name);
        if (playerCanActivate && activator.tag == "Player")
            toggle();
        else if (activator.tag != "Player")
            toggle();
    }

    public override void setStatus(GameObject activator, bool status)
    {
        this.status = status;
    }

    IEnumerator smoothActivate()
    {
        yield return new WaitForSeconds(0.2f);
        connectedTo.activate(gameObject);
        yield return null;
    }
}
