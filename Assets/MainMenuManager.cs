using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Fader fader;
        
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {            
           fader.fadeIn();
           Invoke("reloadScene", 2f);
        }
    }

    private void reloadScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex+1) % SceneManager.sceneCountInBuildSettings);
    }
}
