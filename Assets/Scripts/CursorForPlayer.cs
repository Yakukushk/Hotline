using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorForPlayer : MonoBehaviour
{
    [SerializeField] private Texture2D cursorSight;
    void Start()
    {
        Cursor.SetCursor(cursorSight, Vector2.zero, CursorMode.ForceSoftware);
    }
}
