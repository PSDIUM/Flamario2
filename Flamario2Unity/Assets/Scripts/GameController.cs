using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public GameObject menuPanal, menuCursor, levelPanel, gameoverPanel, blackCoverPanel;
    public TextMeshProUGUI scoreText, coinsText, timeNumText;

    private float timeNumValue;
    private Vector3 player1LocalPos = new Vector3(-315, -152); // Worldspace (-2.8F, 4.2F, -0.3F)
    private Vector3 player2LocalPos = new Vector3(-315, -232); // Worldspace (-2.8F, 3.2F, -0.3F)

    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide the cursor
        //SetCursorState(CursorLockMode.Locked);
        menuPanal.SetActive(true);
        PlayerPrefs.SetInt("InMenu", 1);
        timeNumValue = 400;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("InMenu") == 1 && menuPanal.activeInHierarchy) {
            if(Input.GetAxis("Vertical") < 0) {
                menuCursor.transform.localPosition = player2LocalPos;
            } else if(Input.GetAxis("Vertical") > 0) {
                menuCursor.transform.localPosition = player1LocalPos;
            }
            if(Input.GetButtonDown("Jump") && menuCursor.transform.localPosition == player1LocalPos) {
                StartCoroutine(StartGame());
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public IEnumerator StartGame() {
        menuPanal.SetActive(false);
        levelPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        blackCoverPanel.SetActive(true);
        yield return new WaitForSeconds(0.2F);
        //timeNumText.text = "400";
        StartCoroutine(StartCountdown());
        levelPanel.SetActive(false);
        blackCoverPanel.SetActive(false);
        PlayerPrefs.SetInt("InMenu", 0);
        yield return null;
    }

    public IEnumerator StartCountdown() {
        while (timeNumValue > 0) {
            timeNumText.text = "" + timeNumValue;
            yield return new WaitForSeconds(1.0F);
            timeNumValue--;
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