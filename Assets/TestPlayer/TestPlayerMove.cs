using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerMove : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private InputAction move;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        move = InputSystem.actions.FindAction("Move");
        move.performed += Move;
        move.canceled += Move;
    }

    void Update()
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
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("Speed", moveInput.magnitude);
    }
}