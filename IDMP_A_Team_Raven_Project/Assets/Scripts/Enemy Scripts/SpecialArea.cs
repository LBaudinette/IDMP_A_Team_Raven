using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class SpecialArea : MonoBehaviour
{
    private float scaleTimer = 0;
    private float lingerTimer = 0;

    protected IEnumerator startExpansion(float maxScaleSize, float maxScaleTime, float maxLingerTime, Coroutine coroutine) {
        //Get the original scale and set the target scale as a vector
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = new Vector3(maxScaleSize, maxScaleSize, 0f);

        //Keep increasing the size of the area using lerp
        while(scaleTimer < maxScaleTime) {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, scaleTimer / maxScaleSize);

            scaleTimer += Time.deltaTime;

            yield return null;
        }
        coroutine = StartCoroutine(startLinger(maxLingerTime));
    }

    private IEnumerator startLinger(float maxLingerTime) {
        //Allow the area to linger before destroying it
        while(lingerTimer < maxLingerTime) {
            lingerTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        lingerTimer = 0;
    }

    

    


}
