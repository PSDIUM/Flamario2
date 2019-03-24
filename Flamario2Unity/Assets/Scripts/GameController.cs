using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public GameObject menuPanal, menuCursor, levelPanel, gameoverPanel, blackCoverPanel, player;
    public TextMeshProUGUI scoreText, coinsText, timeNumText;

    private float timeNumValue, scoreValue;
    private Vector3 player1LocalPos = new Vector3(-315, -152); // Worldspace (-2.8F, 4.2F, -0.3F)
    private Vector3 player2LocalPos = new Vector3(-315, -232); // Worldspace (-2.8F, 3.2F, -0.3F)
    private IEnumerator timer;

    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide the cursor
        //SetCursorState(CursorLockMode.Locked);
        menuPanal.SetActive(true);
        PlayerPrefs.SetInt("InMenu", 1);
        timeNumValue = 400;
        PlayerPrefs.SetInt("FlagBase", 0);
        timer = Timer();
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
        Debug.Log(player.transform.position.y);
    }

    public IEnumerator StartGame() {
        menuPanal.SetActive(false);
        levelPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        blackCoverPanel.SetActive(true);
        yield return new WaitForSeconds(0.2F);
        //timeNumText.text = "400";
        StartCoroutine(timer);
        levelPanel.SetActive(false);
        blackCoverPanel.SetActive(false);
        PlayerPrefs.SetInt("InMenu", 0);
    }

    public IEnumerator Timer() {
        while (timeNumValue > 0) {
            timeNumText.text = "" + timeNumValue;
            yield return new WaitForSeconds(1.0F);
            timeNumValue--;
        }
    }

    public IEnumerator FinishFlag() {
        player.GetComponent<Rigidbody2D>().simulated = false;
        StopCoroutine(timer);
        PlayerPrefs.SetInt("InMenu", 1);
        while (player.transform.position.y > 1) {
            player.transform.position = new Vector2(179.2F, player.transform.position.y - 0.05F);
            yield return null;
        }
        while (player.transform.position.x < 185.5) {
            player.transform.position = new Vector2(player.transform.position.x + 0.06F, 1);
            yield return null;
        }
        player.SetActive(false);
        StartCoroutine(FinalScore());
    }

    public IEnumerator FinalScore() {
        while (timeNumValue >= 0) {
            timeNumText.text = "" + timeNumValue;
            timeNumValue--;
            scoreValue += 50;
            scoreText.text = "Mario\n" + scoreValue.ToString("000000");
            yield return null;
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