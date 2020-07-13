using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class IntermezzoScript : MonoBehaviour
{
    public Fader fader;
    public Text text;
    public bool finished;
    public bool scaleCheatMaster;
    public GameObject[] allUI;
    public GameObject cheatMaster;
    private void Start()
    {
        fader = GameObject.Find("Fader").GetComponent<Fader>();
        StartCoroutine(waitFor(2.0f));
    }

    void Update()
    {
        if (finished)
            changeScene();
        if (scaleCheatMaster)
        {
            cheatMaster.transform.localScale += new Vector3(.000001f, .000001f, 0);
        }
    }

    IEnumerator waitFor(float wait)
    {
        float textTime = 4.0f;
        float interTextTime = 2.0f;
        yield return new WaitForSeconds(wait);
        fader.fadeIn();
        yield return new WaitForSeconds(1.0f);
        text.gameObject.SetActive(true);

        yield return new WaitForSeconds(textTime);
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(interTextTime);
        text.text = "You don't control me, game, you can't tell ME how to play!";
        text.gameObject.SetActive(true);

        hideOldUI();
        showCheatMaster();

        yield return new WaitForSeconds(textTime);
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(interTextTime);



        text.text = "It's time for the...";
        scaleCheatMaster = true;
        text.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(textTime);
        text.gameObject.SetActive(false);
        fader.fadeOut();
        yield return new WaitForSeconds(5.0f);
        fader.fadeIn();
        text.text = "I know I had some instructions... Oh here they are!";
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(textTime);
        text.gameObject.SetActive(false);

        finished = true;
    }

    private void showCheatMaster()
    {
        cheatMaster.SetActive(true);
    }

    private void hideOldUI()
    {
        foreach(GameObject g in allUI)
        {
            g.SetActive(false);
        }
    }

    void changeScene()
    {
        fader.fadeIn();
        Invoke("loadNextScene", 2f);
    }

    private void loadNextScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
