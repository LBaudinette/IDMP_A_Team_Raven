using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerShoot : MonoBehaviour
{
    public Camera cam;
    public GameObject weapon;
    public SpriteRenderer weaponSprite;
    public LineRenderer lr;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowForce;
    public float defaultAimLineLength = 10f;

    private bool aiming;
    private bool shoot;
    private PlayerControls playerControls;

    void Start()
    {
        weaponSprite.enabled = false;
        lr.enabled = false;
        aiming = false;
    }

    // Update is called once per frame
    void Update()
    {   
        processInputs();
        if (aiming)
        {
            Aiming();
            if (shoot)
            {
                Shoot();
            }
        }
    }

    private void processInputs()
    {
        playerControls.Player.Aim.canceled += _ => OnAimStop();
        playerControls.Player.Aim.started += _ => OnAimStart();
        if (aiming)
        {
            playerControls.Player.Attack.started += _ => shoot = true;
            Debug.Log("aiming and input shoot");
        } else
        {
            shoot = false;
        }
    }

    private void OnAimStart()
    {
        // enable aiming func, line renderer and weapon sprite when RMB is pressed
        lr.enabled = true;
        weaponSprite.enabled = true;
        aiming = true;
    }

    private void OnAimStop()
    {
        // disable aiming func, line renderer and weapon sprite when RMB is released
        weaponSprite.enabled = false;
        lr.enabled = false;
        aiming = false;
    }

    private void Aiming()
    {
        // get aim angle
        Vector2 lookDir = Vector2.right;

        // if aim direction bind is being actively used
        if (playerControls.Player.AimDirection.activeControl != null)
        {
            // if aim dir is being controlled by mouse
            if (playerControls.Player.AimDirection.activeControl.displayName.Equals("Position"))
            {
                lookDir = cam.ScreenToWorldPoint(playerControls.Player.AimDirection.ReadValue<Vector2>()) - weapon.transform.position;
            }
            else
            {
                lookDir = playerControls.Player.AimDirection.ReadValue<Vector2>();
            }
        }
        
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, lookDir.normalized, Mathf.Infinity, LayerMask.GetMask("Collisions"));

        // get end of raycast, if cast didn't hit anything calc a point in the direction to be endpoint
        Vector2 endPoint;
        if (hit.collider != null)
        {
            endPoint = hit.point;
        } else
        {
            endPoint = (Vector2) firePoint.position + lookDir.normalized * defaultAimLineLength;
        }
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // rotate weapon to face mouse direction
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // flip sprite if crossbow on other side of player
        if (angle <= -90f || angle > 90)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }

        DrawLine(endPoint);
    }

    private void Shoot()
    {
        GameObject firedArrow = Instantiate(arrowPrefab, firePoint.position, weapon.transform.rotation);
        firedArrow.GetComponent<Rigidbody2D>().AddForce(firePoint.right * arrowForce, ForceMode2D.Impulse);
        shoot = false;
    }

    private void DrawLine(Vector3 endPoint)
    {
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, endPoint);
    }

    public bool isAiming()
    {
        return aiming;
    }

    public void setControls(PlayerControls controls)
    {
        playerControls = controls;
    }

}
