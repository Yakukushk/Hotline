using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text text;
    string txt = "Hello World!";
    public void Start()
    {
        Method();
    }

    public void Method() {
        
        for (var i = 0; i < 10; i++) {
            text.text = txt;
        }    
    }
}
