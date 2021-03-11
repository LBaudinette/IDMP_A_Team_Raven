using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Move object forward
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
        //TODO: Deal damage to player
    }

}
