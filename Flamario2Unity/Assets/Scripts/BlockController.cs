using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

	private Animator anim;
	private Vector2 startPos;
	private Vector2 endPos;
	private float speed = 0.1f;

    void Start() {
    	startPos = transform.position;
    	endPos = new Vector2(startPos.x, startPos.y + 0.3f);
    }

	public void HitAnimation() {
		StartCoroutine(AnimateUp(startPos, endPos, speed));
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

}
