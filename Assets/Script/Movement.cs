using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public ParticleSystem gunFlash;
    public float flashTimer = 0f;
    public float flashTime = 0.1f; 

    CharacterController characterController;
    public float speed;
    int horizontal;
    int vertical;

    bool movePressed;
    bool lookPressed;
    public bool canShoot;

    Vector3 defaultDir;
    Vector3 defaultUp;
    int dirMulti = 1;
    float angle;

    Vector2 moveValue;
    Vector2 lookValue;
    Vector3 lookDir;
    Vector2 moveDir;

    InputControls control;
    Animator animator;

    #region - Awake / OnEnable / OnDisable - 

    private void Awake()
    {
        control = new InputControls();

        control.Gameplay.Move.performed += ctx => //print(ctx.ReadValue<Vector2>());
        {
            moveValue = ctx.ReadValue<Vector2>();
            movePressed = true;
                
        };
        control.Gameplay.Move.canceled += ctx => movePressed = false;

        control.Gameplay.LookShoot.performed += ctx =>
        {
            lookValue = ctx.ReadValue<Vector2>();
            lookPressed = true;
        };
        control.Gameplay.LookShoot.canceled += ctx => lookPressed = false;
    }

    private void OnEnable()
    {
        control.Gameplay.Enable();
    }    
    
    private void OnDisable()
    {
        control.Gameplay.Disable();
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        SetVectors();
        SetAnimator();
    }

    void SetVectors()
    {
        lookDir = transform.forward;
        defaultDir = transform.forward;
        defaultUp = -transform.right;
    }

    void SetAnimator()
    {
        animator = GetComponent<Animator>();

        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }



    // Update is called once per frame
    private void Update()
    {
        animator.SetFloat(horizontal, moveDir.x);
        animator.SetFloat(vertical, -moveDir.y);
    }

    private void LateUpdate()
    {
        CalculateVectors();
        characterController.Move(new Vector3(moveValue.x, 0, moveValue.y) * Time.deltaTime * speed);
    }

    private void CalculateVectors()
    {
        print(lookValue);

        lookDir.x = lookValue.x * 1000000;
        lookDir.z = lookValue.y * 1000000;
        transform.LookAt(lookDir);

        if (Vector3.Dot(defaultUp, lookDir) >= 0)
            dirMulti = -1;
        else
            dirMulti = 1;

        angle = Vector3.Angle(defaultDir, transform.forward) * dirMulti;

        moveDir.x = moveValue.x * Mathf.Cos(angle / 180 * Mathf.PI) - moveValue.y * Mathf.Sin(angle / 180 * Mathf.PI);
        moveDir.y = moveValue.x * Mathf.Sin(angle / 180 * Mathf.PI) + moveValue.y * Mathf.Cos(angle / 180 * Mathf.PI);

        //print(" normal " + moveValue + " angle " + angle + " and " + moveDir + " cosQ = " + Mathf.Cos(angle / 180 * Mathf.PI));

        if (!movePressed)
            moveValue = Vector2.zero;

        if (lookPressed && Vector2.Distance(Vector2.zero, lookValue) > 0.95f)
        {
            canShoot = true;
            Shooting();
        }
        else
            canShoot=false;

        flashTimer += Time.deltaTime;
    } 

    private void Shooting()
    {
        if(flashTime < flashTimer)
        {
            gunFlash.Play();
            flashTimer = 0;
        }
    }

}
