using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchesController : MonoBehaviour
{
   public int MAX_PIECES_PER_GLITCH = 10;
   Dictionary<string,int> glitchesInGame;

    private void Awake()
    {
        glitchesInGame = new Dictionary<string, int>();
    }
    public bool canSpawnMore(string glitchName)
    {
        Debug.Log(string.Format("Registering {0}", name));
        return glitchesInGame[glitchName] < MAX_PIECES_PER_GLITCH;
    }

    public void registerGlitch(string name)
    {
        Debug.Log(string.Format("Registering {0}", name));
        if (!glitchesInGame.ContainsKey(name))
        {
            glitchesInGame.Add(name,0);
        }
    }

    public void registerSpawn(string name)
    {
        if (glitchesInGame.ContainsKey(name))
        {
            glitchesInGame[name] += 1;
        }
    }
    


}
