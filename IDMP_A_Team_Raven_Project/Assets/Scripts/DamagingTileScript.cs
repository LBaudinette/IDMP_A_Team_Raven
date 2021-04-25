using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTileScript : MonoBehaviour
{
    public int damage = 10;

    private Coroutine coroutine;
    private float timer = 0;
    private float deactivationDelay = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateTile() {
        gameObject.SetActive(true);
        //coroutine = StartCoroutine(deactivateTile());
        //Maybe play some animation
    }
    //TODO: replace timer with animation flag to deactivate tile
    IEnumerator deactivateTile() {
        while(timer < deactivationDelay) {
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
