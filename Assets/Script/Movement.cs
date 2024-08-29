using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader; // Reference to the InputReader ScriptableObject
    public ParticleSystem gunFlash; // Particle system for gun flash effect
    public float flashTimer = 0f; // Timer for gun flash effect
    public float flashTime = 0.2f; // Duration of the gun flash effect
    float defaultFlashTime = 0.2f; // Default duration of the gun flash effect

    CharacterController characterController; // Character controller component
    public float speed; // Movement speed
    int horizontal; // Animator parameter for horizontal movement
    int vertical; // Animator parameter for vertical movement
    int reload; // Animator parameter for reload action
    int granade; // Animator parameter for grenade action
    
    bool lookPressed; // Flag to check if look is pressed
    public bool canShoot; // Flag to check if the player can shoot

    Vector3 defaultDir; // Default direction vector
    Vector3 defaultUp; // Default up vector
    int dirMulti = 1; // Direction multiplier
    float angle; // Angle for movement direction

    public float shootPower = 10; // Power of the shoot

    Vector2 moveValue; // Value for movement input
    Vector2 lookValue; // Value for look input
    Vector3 lookDir; // Direction vector for looking
    Vector2 moveDir; // Direction vector for movement

    Animator animator; // Animator component

    [SerializeField] GameObject _bulletPrefab; // Prefab for the bullet
    [SerializeField] GameObject _gunTip; // Gun tip position for shooting
    GameObject _bullet; // Bullet game object

    #region - Awake / OnEnable / OnDisable -

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;
        
        GameManager.Instance.SetPlayer(transform);
        
        // Initialize components and variables
        characterController = GetComponent<CharacterController>();
        SetVectors();
        SetAnimator();
        defaultFlashTime = flashTime;
        
        // Subscribe to input events
        //_inputReader.MoveEvent += OnMove;
        //_inputReader.LookEvent += OnLook;
        _inputReader.ShootEvent += OnShoot;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;
        
        // Unsubscribe from input events
        //_inputReader.MoveEvent -= OnMove;
        //_inputReader.LookEvent -= OnLook;
        _inputReader.ShootEvent -= OnShoot;
    }

    #endregion

    void SetVectors()
    {
        // Set default direction and up vectors
        lookDir = transform.forward;
        defaultDir = transform.forward;
        defaultUp = -transform.right;
    }

    void SetAnimator()
    {
        // Initialize animator parameters
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        reload = Animator.StringToHash("IsReload");
        granade = Animator.StringToHash("IsGrenade");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsOwner)
            return;
        //Update Values
        UpdateMoveValues();
        UpdateLookValues();
        
        CalculateVectors();
        characterController.Move(new Vector3(moveValue.x, 0, moveValue.y) * Time.deltaTime * speed);
        
        flashTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;
        // Update animator parameters
        animator.SetFloat(horizontal, moveDir.y);
        animator.SetFloat(vertical, -moveDir.x);
    }

    private void CalculateVectors()
    {
        // Calculate look direction and movement direction based on input
        if (lookValue != Vector2.zero)
        {
            lookDir.x = lookValue.x * 1000000;
            lookDir.z = lookValue.y * 1000000;
            transform.LookAt(lookDir);
        }


        dirMulti = Vector3.Dot(defaultUp, lookDir) >= 0 ? -1 : 1;
        angle = Vector3.Angle(defaultDir, transform.forward) * dirMulti;

        moveDir.x = moveValue.x * Mathf.Cos(angle / 180 * Mathf.PI) - moveValue.y * Mathf.Sin(angle / 180 * Mathf.PI);
        moveDir.y = moveValue.x * Mathf.Sin(angle / 180 * Mathf.PI) + moveValue.y * Mathf.Cos(angle / 180 * Mathf.PI);
        
        if (lookPressed && Vector2.Distance(Vector2.zero, lookValue) > 0.95f)
        {
            canShoot = true;
            Shooting();
        }
        else
        {
            canShoot = false;
        }
    }

    private void UpdateMoveValues()
    {
        // Handle movement input
        moveValue = _inputReader.MoveValue;
    }
    
    private void UpdateLookValues()
    {
        // Handle look input
        lookValue = _inputReader.LookValue;
    }

    private void OnShoot(bool isShooting)
    {
        // Handle shooting input
        canShoot = isShooting;

        if (canShoot && flashTimer >= flashTime)
        {
            flashTime = defaultFlashTime;
            gunFlash.Play();
            flashTimer = 0;
            Shoot();
        }
    }

    private void Shooting()
    {
        // Handle shooting logic
        if (flashTime < flashTimer)
        {
            flashTime = defaultFlashTime;
            gunFlash.Play();
            flashTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        // Instantiate and shoot the bullet
        Instantiate(_bulletPrefab, _gunTip.transform.position, _gunTip.transform.rotation)
            .GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shootPower);
    }

    public void Reload()
    {
        // Handle reload action
        animator.SetBool(reload, true);
        flashTime = 2f;
    }

    public void ThrowGranade()
    {
        // Handle grenade action
        animator.SetBool(granade, true);
        flashTime = 2f;
    }
}