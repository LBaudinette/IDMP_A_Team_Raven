using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float duration = 0.1f;
    private bool stopped = false;

    public void freeze()
    {
        if (!stopped)
        {
            StartCoroutine(HitstopTimer());
        }
    }

    IEnumerator HitstopTimer()
    {
        stopped = true;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        stopped = false;
    }
}
