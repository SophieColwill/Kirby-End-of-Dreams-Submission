using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class KirbyMovment : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D controller;
    KirbyCopyAbilities copyAbilitieManager;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] KirbyAnimator animator;
    public float DisableXAxisMovmentTimer;

    [Header("Walk / Run")]
    [SerializeField] float InhaleSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField, Range(0, 1)] float MoveAdjustDamp;
    [Header("Jump")]
    [SerializeField] float JumpPower;
    [SerializeField] float Gravity_Normal;
    [Header("Float")]
    [SerializeField] float FloatJumpPower;
    [SerializeField] float Gravity_Float;
    [SerializeField] float FloatMoveSpeed;

    [HideInInspector] public bool isGrounded;
    /*[HideInInspector]*/ public bool isFloating;
    [HideInInspector] public float xMovment;
    float xInput;
    bool lookingRight = true;
    #region Getters
    
    public bool GetIsFloating
    {
        get { return isFloating; }
        set
        {
            if (value == false)
            {
                DisableFloating();
            }
        }
    }

    public bool getIsLookingRight
    {
        get { return lookingRight; }
    }

    float GetCurrentSpeed()
    {
        float Output = walkSpeed;

        if (copyAbilitieManager.HasKirbyCurrentlyInhaledSomething())
        {
            Output = InhaleSpeed;
        }
        else if (isFloating)
        {
            Output = FloatMoveSpeed;
        }
        else
        {
            Output = walkSpeed;
        }
        return Output;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        copyAbilitieManager = GetComponent<KirbyCopyAbilities>();
        controller =GetComponent<Rigidbody2D>();
        InputManager.Initialise_Movment(this);
    }

    private void Update()
    {
        if (DisableXAxisMovmentTimer > 0)
        {
            DisableXAxisMovmentTimer -= Time.deltaTime;
            xMovment = 0f;
        }
        else if (copyAbilitieManager.GetIsInhaling())
        {
            xMovment = 0f;
        }
        else
        {
            xMovment = xInput;
        }

        isGrounded = Physics2D.Raycast(GroundCheck.position, Vector2.right, 1.75f, GroundMask);

        if (isGrounded )
        {
            DisableFloating();
        }

        if (xMovment > 0)
        {
            lookingRight = true;
        }
        else if (xMovment < 0)
        {
            lookingRight = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(GroundCheck.position, GroundCheck.position + (Vector3.right * 1.75f));
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = new Vector3(xMovment * GetCurrentSpeed(), controller.velocity.y);
        controller.velocity = Vector3.Slerp(controller.velocity, newVelocity, MoveAdjustDamp);
    }

    public void MoveKirby(float newMoveDir)
    {
        xInput = newMoveDir;
    }
    public void Jump()
    {
        if (isGrounded)
        {
            controller.velocity = new Vector2(controller.velocity.x, JumpPower);
            DisableFloating();
        }
        else if (!copyAbilitieManager.HasKirbyCurrentlyInhaledSomething())
        {
            controller.velocity = new Vector2(controller.velocity.x, FloatJumpPower);
            animator.FloatInputed();
            EnableFloating();
        }
    }
    #region Floating

    void EnableFloating()
    {
        controller.gravityScale = Gravity_Float;
        isFloating = true;
    }

    public void DisableFloating()
    {
        controller.gravityScale = Gravity_Normal;
        isFloating = false;
    }

    #endregion

    public void TryEnterDoor()
    {
        Collider2D[] potentialDoors = Physics2D.OverlapCircleAll(transform.position, 0.2f);

        foreach (Collider2D Lidar in potentialDoors)
        {
            Door door = Lidar.GetComponent<Door>();
            if (door != null)
            {
                door.EnterDoor(this);
                break;
            }
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
