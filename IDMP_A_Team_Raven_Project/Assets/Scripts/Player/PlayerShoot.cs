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

    private bool aiming;

    Vector2 mousePos;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        weaponSprite.enabled = false;
        lr.enabled = false;
        aiming = false;
    }

    // Update is called once per frame
    void Update()
    {
       /* processInputs();

        // disable line renderer and weapon sprite when RMB is released
        if (Input.GetMouseButtonUp(1))
        {
            weaponSprite.enabled = false;
            lr.enabled = false;
            aiming = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            aiming = true;
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
            {
                GameObject firedArrow = Instantiate(arrowPrefab, firePoint.position, weapon.transform.rotation);
                firedArrow.GetComponent<Rigidbody2D>().AddForce(firePoint.right * arrowForce, ForceMode2D.Impulse);
            }
        }*/
    }

    private void processInputs()
    {

    }

    private void DrawLine()
    {
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, mousePos);
    }

    public bool isAiming()
    {
        return aiming;
    }

}
