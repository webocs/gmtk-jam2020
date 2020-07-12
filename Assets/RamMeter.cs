using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RamMeter : MonoBehaviour
{
    Text text;
    public float current;
    public float max;
    // Start is called before the first frame update
    void Awake()
    {
        current = 0;
        max = 0;
        text =GetComponent<Text>();        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("Ram: {0}/{1}kb",current,max);
    }
}
