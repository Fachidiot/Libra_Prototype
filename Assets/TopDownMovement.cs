using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerInputs inputs;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool debug_GameManager = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        debug_GameManager = GameManager.Instance.TryGetComponent<PlayerInputs>(out inputs);
    }

    void Update()
    {
        // Input
        Movement();
    }

    private void Movement()
    {
        if (debug_GameManager)
        {
            movement.x = inputs.GetAxisHorizontal();
            movement.y = inputs.GetAxisVertical();
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
        // if (movement.y != 0)
        //     spriteRenderer.flipY = movement.y > 0;
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
