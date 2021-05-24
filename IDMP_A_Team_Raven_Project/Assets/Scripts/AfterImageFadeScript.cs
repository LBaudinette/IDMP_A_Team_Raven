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
        float alpha = 1f;
        float afterImageTimer = 0f;
        while (afterImageTimer < afterImageDelay) {

            alpha = Mathf.Lerp(1, 0, afterImageTimer / afterImageDelay);
            sr.color = new Color(1f, 1f, 1f, alpha);
            yield return null;

        }
        Destroy(gameObject);
    }
}
