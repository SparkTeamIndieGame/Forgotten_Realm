using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkPlayerController : NetworkBehaviour
{
    private CharacterController characterController;
    private  Animator animator;
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private InputAction attack;

    private InputAction move;
    
    private NetworkVariable<float> netSpeed = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> netAttack = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        move = InputSystem.actions.FindAction("Move");
        move.performed += Move;
        move.canceled += Move;
        
        attack = InputSystem.actions.FindAction("Attack");
        attack.performed += Attack;
        attack.canceled += Attack;
    }

    void Update()
    {
        if (IsOwner)
        {
            // Передвижение игрока
            Vector3 moveDirection = new Vector3(-moveInput.y, 0, moveInput.x);
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Поворот в сторону движения
            if (moveDirection.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            netSpeed.Value = moveInput.magnitude;
            netAttack.Value = animator.GetBool("Attack");
        }
        else
        {
            animator.SetFloat("Speed", netSpeed.Value);
            animator.SetBool("Attack", netAttack.Value);
        }
        
        
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        animator.SetBool("Attack", context.phase == InputActionPhase.Performed );
        netAttack.Value = (context.phase == InputActionPhase.Performed);
    }
}
