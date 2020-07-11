using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    Fader fader;

    private void Awake()
    {
        fader = FindObjectOfType<Fader>();
    }
    public void startGame()
    {
        Debug.Log("Starting game");
        fader.fadeIn();
        Invoke("changeLevel", 1);
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            changeLevel();
    }
    void changeLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
