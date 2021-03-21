using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    public GameObject weapon;
    public SpriteRenderer weaponSprite;
    public LineRenderer lr;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowForce;

    Vector2 mousePos;
    void Start()
    {
        weaponSprite.enabled = false;
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // disable line renderer and weapon sprite when RMB is released
        if (Input.GetMouseButtonUp(1))
        // if (Input.GetButtonUp("CTRLAim"))
        {
            weaponSprite.enabled = false;
            lr.enabled = false;
        }

        if (Input.GetMouseButton(1))
        // if (Input.GetButton("CTRLAim"))
        {
            // get mouse position and angle relative to player
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = (Vector3)mousePos - weapon.transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            // rotate weapon to face mouse direction
            weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            weaponSprite.enabled = true;
            lr.enabled = true;
            // flip sprite if crossbow on other side of player
            if (angle <= -90f || angle > 90)
            {
                weaponSprite.flipY = true;
            }
            else
            {
                weaponSprite.flipY = false;
            }

            DrawLine();

            if (Input.GetMouseButtonDown(0))
            //if (Input.GetButtonDown("CTRLFire"))
            {
                GameObject firedArrow = Instantiate(arrowPrefab, firePoint.position, weapon.transform.rotation);
                firedArrow.GetComponent<Rigidbody2D>().AddForce(firePoint.right * arrowForce, ForceMode2D.Impulse);
            }
        }
    }

    private void DrawLine()
    {
        lr.SetPosition(0, weapon.transform.position);
        lr.SetPosition(1, mousePos);
    }

}
