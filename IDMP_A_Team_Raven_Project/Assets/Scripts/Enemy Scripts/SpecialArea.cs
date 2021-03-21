using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class SpecialArea : MonoBehaviour
{
    private float scaleTimer = 0;
    private float lingerTimer = 0;
    private float diminishTimer = 0;

    protected IEnumerator startExpansion(float maxScaleSize, float maxScaleTime, float maxLingerTime, Coroutine coroutine, float diminishTime) {
        //Get the original scale and set the target scale as a vector
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = new Vector3(maxScaleSize, maxScaleSize, 0f);

        //Keep increasing the size of the area using lerp
        while(scaleTimer < maxScaleTime) {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, scaleTimer / maxScaleTime);

            scaleTimer += Time.deltaTime;

            yield return null;
        }
        coroutine = StartCoroutine(startLinger(maxLingerTime, diminishTime, coroutine));
    }

    private IEnumerator startLinger(float maxLingerTime, float diminishTime, Coroutine coroutine) {
        //Allow the area to linger before destroying it
        while(lingerTimer < maxLingerTime) {
            lingerTimer += Time.deltaTime;
            yield return null;
        }
        lingerTimer = 0;
        coroutine = StartCoroutine(startDiminishing(diminishTime));
    }

    private IEnumerator startDiminishing(float diminishTime) {
        SpriteRenderer spriteRndr = GetComponent<SpriteRenderer>();
        Color originalColour = spriteRndr.color;
        while(diminishTimer < diminishTime) {
            //Lerp between full opacity to none
            float currentAlpha = Mathf.Lerp(1f, 0, diminishTimer / diminishTime);
            spriteRndr.color = 
                new Color(originalColour.a, originalColour.g, originalColour.b, currentAlpha);
            diminishTimer += Time.deltaTime;

            yield return null;
        }

        //Destroy game object once it is invisible
        Destroy(gameObject);
    }






}
