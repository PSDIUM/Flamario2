using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide the cursor
        //SetCursorState(CursorLockMode.Locked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void SetCursorState(CursorLockMode wantedMode) {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
    
    public void QuitGame() {
        #if UNITY_EDITOR  
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        #if UNITY_STANDALONE
                Application.Quit();
        #endif
    }
}