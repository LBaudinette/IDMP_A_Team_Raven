using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFadeScript : MonoBehaviour
{
    private Coroutine coroutine;

    public void startFade(SpriteRenderer sr, float afterImageDelay) {
        coroutine = StartCoroutine(fadeImage(sr, afterImageDelay));
    }

    private IEnumerator fadeImage(SpriteRenderer sr, float afterImageDelay) {
        //Debug.Log("Start FADE");
        float alpha = 1f;
        float afterImageTimer = 0f;
        while (afterImageTimer < afterImageDelay) {

            alpha = Mathf.Lerp(1, 0, afterImageTimer / afterImageDelay);
            sr.color = new Color(0f, 0.25f, 1f, alpha);
            afterImageTimer += Time.deltaTime;
            yield return null;

        }
        Destroy(gameObject);
    }
}
