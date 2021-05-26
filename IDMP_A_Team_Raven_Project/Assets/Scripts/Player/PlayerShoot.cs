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

    [Header("Inventory Variables")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem boltInventoryItem;
    [SerializeField] private SignalSender boltFiredSignal;
    [SerializeField] private int boltFiredCost;

    private bool aiming;
    private bool shoot;
    private PlayerControls playerControls;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Sprite crossbowArmVertical;
    [SerializeField] private Sprite crossbowArmHorizontal;

    void Start()
    {
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
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
        }
        else
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
        playerAnimator.SetBool("Aiming", true);
    }

    private void OnAimStop()
    {
        // disable aiming func, line renderer and weapon sprite when RMB is released
        weaponSprite.enabled = false;
        lr.enabled = false;
        aiming = false;
        playerAnimator.SetBool("Aiming", false);
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

        //if moving lock aiming angle in one of 4 directions
        if (playerMovement.state == PlayerMovement.State.Moving)
        {
            float currentMovementX = playerMovement.movementDir.x;
            float currentMovementY = playerMovement.movementDir.y;
            if (Mathf.Abs(currentMovementX) > Mathf.Abs(currentMovementY))
            {
                currentMovementY = 0;
            }
            else
            {
                currentMovementX = 0;
            }

            if (currentMovementX > 0)
            {
                lookDir = new Vector2(1, 0);
            }
            if (currentMovementX < 0)
            {
                lookDir = new Vector2(-1, 0);
            }
            if (currentMovementY > 0)
            {
                lookDir = new Vector2(0, 1);
            }
            if (currentMovementY < 0)
            {
                lookDir = new Vector2(0, -1);
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, lookDir.normalized, Mathf.Infinity, LayerMask.GetMask("Collisions"));

        // get end of raycast, if cast didn't hit anything calc a point in the direction to be endpoint
        Vector2 endPoint;
        if (hit.collider != null)
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = (Vector2)firePoint.position + lookDir.normalized * defaultAimLineLength;
        }
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // rotate weapon to face mouse direction
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        float xAngle = 0;
        float yAngle = 0;

        if (45 < angle && angle <= 135)
        {
            firePoint.localPosition = new Vector3(1.375f, 0f, 0f);

            weapon.transform.localPosition = new Vector3(0.1f, 0.4f, 0f);
            weaponSprite.sprite = crossbowArmVertical;
            weaponSprite.flipY = false;
            weaponSprite.sortingOrder = -1;
            yAngle = 1;
        }
        if (-135 < angle && angle < -45)
        {
            firePoint.localPosition = new Vector3(1.375f, 0f, 0f);
            weapon.transform.localPosition = new Vector3(-0.3f, 0.5f, 0f);
            weaponSprite.sprite = crossbowArmVertical;
            weaponSprite.flipY = false;
            weaponSprite.sortingOrder = 1;
            yAngle = -1;
        }

        if (-45 <= angle && angle <= 0 || 0 <= angle && angle <= 45)
        {
            firePoint.localPosition = new Vector3(1.25f, 0.05f, 0f);
            if (playerMovement.state == PlayerMovement.State.Moving)
            {
                weapon.transform.localPosition = new Vector3(0.05f, 0.3f, 0f);
            }
            else
            {
                weapon.transform.localPosition = new Vector3(-0.25f, 0.3f, 0f);
            }
            weaponSprite.sprite = crossbowArmHorizontal;
            weaponSprite.flipY = false;
            weaponSprite.sortingOrder = 1;
            xAngle = 1;
        }
        if (-180 <= angle && angle <= -135 || 135 < angle && angle <= 180)
        {
            firePoint.localPosition = new Vector3(1.25f, -0.05f, 0f);
            if (playerMovement.state == PlayerMovement.State.Moving)
            {
                weapon.transform.localPosition = new Vector3(0.05f, 0.3f, 0f);
            }
            else
            {
                weapon.transform.localPosition = new Vector3(0.25f, 0.3f, 0f);
            }
            weaponSprite.sprite = crossbowArmHorizontal;
            weaponSprite.flipY = true;
            weaponSprite.sortingOrder = 1;
            xAngle = -1;
        }

        playerAnimator.SetFloat("AngleX", xAngle);
        playerAnimator.SetFloat("AngleY", yAngle);

        DrawLine(endPoint);
    }

    private void Shoot()
    {
        if (boltInventoryItem.numberHeld > 0)
        {
            GameObject firedArrow = Instantiate(arrowPrefab, firePoint.position, weapon.transform.rotation);
            firedArrow.GetComponent<Rigidbody2D>().AddForce(firePoint.right * arrowForce, ForceMode2D.Impulse);
            boltInventoryItem.DeacreaseAmount(boltFiredCost);
            boltFiredSignal.Raise();
            shoot = false;
        }
    }

    private void DrawLine(Vector3 endPoint)
    {
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, endPoint);
    }

    public bool IsAiming()
    {
        return aiming;
    }

    public void setControls(PlayerControls controls)
    {
        playerControls = controls;
    }
}
