using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchesController : MonoBehaviour
{
   public int MAX_PIECES_PER_GLITCH = 10;
   Dictionary<string,int> glitchesInGame;

    private void Awake()
    {
        if(glitchesInGame == null)
        glitchesInGame = new Dictionary<string, int>();
    }
    public bool canSpawnMore(string glitchName)
    {
        if (glitchesInGame == null)
            glitchesInGame = new Dictionary<string, int>();
        if (glitchesInGame.ContainsKey(name))
            return glitchesInGame[glitchName] < MAX_PIECES_PER_GLITCH;
        else return true;
    }

    public void registerGlitch(string name)
    {
        if (glitchesInGame == null)
            glitchesInGame = new Dictionary<string, int>();
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
