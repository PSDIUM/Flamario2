using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour {

    public GameObject menuPanal, menuCursor, levelPanel, gameoverPanel, blackCoverPanel, player, mainCamera;
    public TextMeshProUGUI topScoreText, scoreText, coinsText, timeNumText, livesText;
    public Sprite smallMarioStand, smallMarioDed;

    private float timeNumValue, scoreValue;
    private Vector3 player1LocalPos = new Vector3(-315, -152); // Worldspace (-2.8F, 4.2F, -0.3F)
    private Vector3 player2LocalPos = new Vector3(-315, -232); // Worldspace (-2.8F, 3.2F, -0.3F)
    private IEnumerator timer;

    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide the cursor
        //SetCursorState(CursorLockMode.Locked);
        topScoreText.text = "Top- " + PlayerPrefs.GetFloat("TopScore").ToString("000000");
        menuPanal.SetActive(true);
        PlayerPrefs.SetInt("InMenu", 1);
        PlayerPrefs.SetInt("Lives", 3);
        livesText.text = "" + PlayerPrefs.GetInt("Lives");
        timeNumValue = 400;
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
        // Resets top score
        if (Input.GetKeyDown(KeyCode.Y)) {
            PlayerPrefs.SetFloat("TopScore", 000000);
            topScoreText.text = "Top- " + PlayerPrefs.GetFloat("TopScore").ToString("000000");
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitGame();
        }
    }

    public IEnumerator StartGame() {
        menuPanal.SetActive(false);
        levelPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        blackCoverPanel.SetActive(true);
        yield return new WaitForSeconds(0.2F);
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
        yield return new WaitForSeconds(2);
        blackCoverPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetFloat("TopScore", scoreValue);
        SceneManager.LoadScene(0);
    }

    public IEnumerator Death() {
        player.GetComponent<Rigidbody2D>().simulated = false;
        StopCoroutine(timer);
        PlayerPrefs.SetInt("InMenu", 1);
        PlayerPrefs.SetInt("Lives", (PlayerPrefs.GetInt("Lives") - 1));
        livesText.text = "" + PlayerPrefs.GetInt("Lives");
        player.GetComponent<SpriteRenderer>().sprite = smallMarioDed;
        yield return new WaitForSeconds(1);
        if(PlayerPrefs.GetInt("Lives") > 0) {
            levelPanel.SetActive(true);
            player.transform.position = new Vector3(-8, 1, -0.1F);
            mainCamera.transform.position = new Vector3(1, 6, -1);
            player.GetComponent<SpriteRenderer>().sprite = smallMarioStand;
            yield return new WaitForSeconds(2);
            player.GetComponent<Rigidbody2D>().simulated = true;
            StartCoroutine(timer);
            levelPanel.SetActive(false);
            PlayerPrefs.SetInt("InMenu", 0);
        } else if(PlayerPrefs.GetInt("Lives") == 0) {
            gameoverPanel.SetActive(true);
            yield return new WaitForSeconds(2);
            blackCoverPanel.SetActive(true);
        yield return new WaitForSeconds(1);
            SceneManager.LoadScene(0);
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