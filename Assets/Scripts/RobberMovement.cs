using UnityEngine;

public class RobberMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveInput = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(h, 0f, v).normalized;

        if (animator != null)
        {
            animator.SetBool("isWalking", moveInput.magnitude > 0f);
        }
    }

    void FixedUpdate()
    {
        if (moveInput.magnitude > 0f)
        {
            // Move with Rigidbody
            Vector3 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            // Rotate to face movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(targetRot);
        }
    }
}
