using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageScript : MonoBehaviour
{
    protected float afterImageDelay = 0.3f;         //The delay between creating new after images
    private float afterImageTimer = 0f;
    private Coroutine coroutine;

    //creates an after image trail from origin to newPos
    public void createAfterImageTrail(Transform origin, Transform newPos, Sprite sprite) {
        Vector2 direction = (Vector2)(newPos.position - origin.position);

        GameObject afterImage = new GameObject("Ghost");
        SpriteRenderer afterImageSR = afterImage.AddComponent<SpriteRenderer>();

        AfterImageFadeScript timerScript = afterImage.AddComponent<AfterImageFadeScript>();
        afterImageSR.sprite = sprite;
        Instantiate(afterImage, origin.position, origin.rotation);

    }

    //creates an after image at origin
    public void createAfterImage(Transform origin, Sprite sprite) {
        //Create new after image game object with a sprite renderer and timer
        GameObject afterImage = new GameObject("Ghost");
        SpriteRenderer afterImageSR = afterImage.AddComponent<SpriteRenderer>();
        AfterImageFadeScript timerScript = afterImage.AddComponent<AfterImageFadeScript>();
        afterImageSR.sprite = sprite;

        //Instantiate the after image and start fading it
        Instantiate(afterImage, origin.position, origin.rotation);
        timerScript.startFade(afterImageSR, afterImageDelay);
    }

    IEnumerator createTrail(Transform origin, Transform newPos, Sprite sprite, Vector2 direction) {

        //while()
        yield return null;
    }
}
