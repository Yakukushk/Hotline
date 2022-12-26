using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairCursor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mouseCursorPos = " + mouseCursorPos);

        Debug.Log("mousePosition = " + Input.mousePosition);
        Debug.Log("mouseCursorPos = " + mouseCursorPos);
        transform.position = mouseCursorPos;
    }
}
