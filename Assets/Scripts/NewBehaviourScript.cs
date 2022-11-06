using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text text;
    string txt = "Hello World!";
    public void Start()
    {
        text.text = txt;
    }
}
