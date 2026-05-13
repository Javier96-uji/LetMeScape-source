using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CatController : MonoBehaviour
{
    [Header("Movimiento y Rotacion")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f; // Que tan rapido gira el gato
    public float jumpForce = 5f;

    /*[Header("Ataque (Zarpazo)")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public float attackSpeedMultiplier = 0.2f;

    [Header("Escalada (SphereCast)")]
    public Transform chestRaycastPoint;
    public float ledgeDetectionLength = 0.6f; // Distancia hacia adelante
    public float ledgeSphereCastRadius = 0.2f; // Grosor de la esfera (nuevo)
    public float heightToCheck = 1.0f; // Altura para buscar el suelo arriba
    public LayerMask whatIsLedge; // Nueva capa para indicar que se puede escalar*/

    private Rigidbody rb;
    private bool isGrounded;
    private bool isClimbing;
    private Transform mainCamera;

    private InputSystem_Actions inputActions;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
        anim = GetComponentInChildren<Animator>();

        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (anim != null) anim.SetBool("IsGrounded", isGrounded);

        if (isClimbing) return;

        MoveAndRotate();
        HandleJump();
        //HandleAttack();
        //CheckLedgeGrab();
    }

    void MoveAndRotate()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        //Calculamos la direccion relativa a la camara
        Vector3 camForward = mainCamera.forward;
        Vector3 camRight = mainCamera.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        //Creamos la direccion de movimiento basada en hacia donde mira la camara
        Vector3 direction = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        bool isSprinting = inputActions.Player.Sprint.IsPressed();
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        bool isAttacking = false;
        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            
            if (stateInfo.IsName("Attack"))
            {
                isAttacking = true;
            }
        }

        /*if (isAttacking)
        {
            currentSpeed *= attackSpeedMultiplier; // Reduce la velocidad
        }*/

        if (direction.magnitude >= 0.1f)
        {
            // 1. Rotacion suave hacia donde estamos caminando
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 2. Movimiento
            Vector3 movement = direction * currentSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
            if (anim != null) anim.SetFloat("Speed", currentSpeed);
            SoundManager.instance.PlayWalk();
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            if (anim != null) anim.SetFloat("Speed", 0f);
            SoundManager.instance.StopLoop();
        }
    }

    void HandleJump()
    {
        if (inputActions.Player.Jump.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            if (anim != null) anim.SetTrigger("Jump");
        }
        
    }

    /*void HandleAttack()
    {

        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            if (anim != null) anim.SetTrigger("Attack");
            if (attackPoint != null)
            {
                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
                foreach (Collider enemy in hitEnemies)
                {
                    Debug.Log("Golpeaste a: " + enemy.name);
                }
            }
        }
    }

    void CheckLedgeGrab()
    {
        if (isGrounded || chestRaycastPoint == null) return;

        // 1. SphereCast: Lanzamos una esfera hacia adelante
        if (Physics.SphereCast(chestRaycastPoint.position, ledgeSphereCastRadius, transform.forward, out RaycastHit wallHit, ledgeDetectionLength, whatIsLedge))
        {
            // 2. Calculamos desde donde lanzar el rayo hacia abajo
            Vector3 rayDownStart = wallHit.point + (transform.forward * 0.1f) + (Vector3.up * heightToCheck);

            // 3. Raycast hacia abajo para encontrar la parte superior plana
            if (Physics.Raycast(rayDownStart, Vector3.down, out RaycastHit ledgeHit, heightToCheck, whatIsLedge))
            {
                if (ledgeHit.point.y > transform.position.y)
                {
                    StartCoroutine(ClimbLedge(ledgeHit.point));
                }
            }
        }
    }

    IEnumerator ClimbLedge(Vector3 targetPosition)
    {
        isClimbing = true;
        rb.isKinematic = true;

        if (anim != null) anim.SetBool("IsClimbing", true);

        float forwardOffset = 0.5f;
        Vector3 finalPos = targetPosition + (Vector3.up * 0.5f) + (transform.forward * forwardOffset);
        Vector3 intermediatePos = new Vector3(transform.position.x, finalPos.y, transform.position.z);

        float climbDuration = 0.15f;

        // Fase 1: Arriba
        float timeElapsed = 0f;
        Vector3 startPos = transform.position;
        while (timeElapsed < climbDuration)
        {
            transform.position = Vector3.Lerp(startPos, intermediatePos, timeElapsed / climbDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Fase 2: Adelante
        timeElapsed = 0f;
        startPos = transform.position;
        while (timeElapsed < climbDuration)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, timeElapsed / climbDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = finalPos;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = false;
        isClimbing = false;

        if (anim != null) anim.SetBool("IsClimbing", false);
    }*/

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}