using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float maxAliveTime;

    private float aliveTime;

    // Start is called before the first frame update
    void Start()
    {
        aliveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > maxAliveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // on enemy collision, damage them and potentially apply knockback based on current rb2d force
    }
}
