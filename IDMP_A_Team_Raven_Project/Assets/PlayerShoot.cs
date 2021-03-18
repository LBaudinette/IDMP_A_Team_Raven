using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    public GameObject gun;
    public LineRenderer lr;

    Vector2 mousePos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (Vector3) mousePos - gun.transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        DrawLine();
    }

    private void DrawLine()
    {
        lr.SetPosition(0, gun.transform.position);
        lr.SetPosition(1, mousePos);

    }
}
