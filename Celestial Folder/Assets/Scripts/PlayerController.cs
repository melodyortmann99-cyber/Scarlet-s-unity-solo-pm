using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour
{
    Vector3 cameraOffset = new Vector3(0, .5F, .2f);
    Vector2 cameraRotation = Vector2.zero;
    Camera playerCam;
    InputAction lookAxis;
    public Rigidbody rb;
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    GameObject pickupObj;

    public PlayerInput input;
    public Transform weaponSlot;
    public Weapon currentWeapon;

    float inputX;
    float inputY;

    public float speed = 5f;
    public float jumpHeight = 10F;
    public float jumpRayDistance = 1.1f;
    public float interactDistance = 1f;

    public int Health = 5;
    public int maxHealth = 5;

    public bool attacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        interactRay = new Ray(transform.position, transform.forward);
        jumpRay = new Ray(transform.position, -transform.up);
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        lookAxis = GetComponent<PlayerInput>().currentActionMap.FindAction("Look");
        weaponSlot = playerCam.transform.GetChild(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {       
        if(Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Camera Handler
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && attacking)
                currentWeapon.fire();

        // Movement System
        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = inputY * speed;
        tempMove.z = inputX * speed;

        rb.linearVelocity = (tempMove.x * transform.forward) + (tempMove.y * transform.up) + (tempMove.z * transform.right);

    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    attacking = true;
                else
                    attacking = false;
            }
            else if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }
    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }
    public void Interact()
    {
        if (pickupObj)
        {
            if (pickupObj.tag == "weapon")
            {
                if (currentWeapon)
                    DropWeapon();

                pickupObj.GetComponent<Weapon>().equip(this);
            }
            pickupObj = null;
        }
        else
            Reload();
    }
    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.GetComponent<Weapon>().unequip();
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();

        inputX = InputAxis.x;
        inputY = InputAxis.y;
    }
    public void Jump()
    {
        if (Physics.Raycast(jumpRay, jumpRayDistance))
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            Health = 0;
        }

        if ((other.tag == "health") && (Health < maxHealth))
        {
            Health++;
            other.gameObject.SetActive(true);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hazard")
            {
                Health--;
            }
    }
}