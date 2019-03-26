using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Animator anim;
	private Vector2 startPos;
	private Vector2 endPos;
	private float speed = 0.1f;
    public bool coinBlock = false;
    public GameObject coin;
    private int coinCounter;
    public GameObject hitBlock;

    void Start() {
    	startPos = transform.position;
    	endPos = new Vector2(startPos.x, startPos.y + 0.3f);
        if (coinBlock)
            coinCounter = 4;
    }

	public void HitAnimation() {
		StartCoroutine(AnimateUp(startPos, endPos, speed));
        if (coinBlock && coinCounter > 0)
        {
            StartCoroutine(CoinPlay());
            coinCounter--;
            GameController.coinScore++;
        }
        if (coinBlock && coinCounter == 0)
        {
            Instantiate(hitBlock, gameObject.transform.position, Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

	private IEnumerator AnimateUp(Vector2 startPos, Vector2 endPos, float t) {
		float elapsedTime = 0;

		while (elapsedTime < t) {
			transform.position = Vector2.Lerp(startPos, endPos, elapsedTime / t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		transform.position = endPos;
		StartCoroutine(AnimateDown(endPos, startPos, speed));
	}

	private IEnumerator AnimateDown(Vector2 startPos, Vector2 endPos, float t) {
		float elapsedTime = 0;

		while (elapsedTime < t) {
			transform.position = Vector2.Lerp(startPos, endPos, elapsedTime / t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		transform.position = endPos;
	}

    private IEnumerator CoinPlay()
    {
        GameObject newCoin = Instantiate(coin, gameObject.transform, true);
        newCoin.GetComponent<Animator>().Play("Coins");
        yield return new WaitForSeconds(0.8f);
        Destroy(newCoin);
    }

}
