using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveCoolTime = 0.1f;
    [SerializeField] private MazeManager mazeManager;

    private Rigidbody2D rb;
    private PlayerInputs inputs;
    private Vector2 movementDirection;
    private float coolTime;

    void Start()
    {
        inputs = GameManager.Instance.GetComponent<PlayerInputs>();
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.position = new Vector2(Mathf.Round(rb.position.x), Mathf.Round(rb.position.y));
        }
    }

    void FixedUpdate()
    {
        if (movementDirection != Vector2.zero)
        {
            // 수평 이동 시 스프라이트 방향 전환
            if (movementDirection.x != 0 && spriteRenderer != null)
            {
                spriteRenderer.flipX = movementDirection.x < 0;
            }

            // 현재 Rigidbody 위치를 기준으로 목표 위치를 계산합니다.
            Vector2 targetPosition = rb.position + movementDirection;

            // MovePosition을 사용해 물리적으로 안전하게 이동합니다.
            rb.MovePosition(targetPosition);

            // 키 입력 한 번에 한 칸만 움직이도록 이동 후에 방향을 초기화합니다.
            movementDirection = Vector2.zero;

            coolTime = 0;
        }

        // CoolTime Set
        coolTime += Time.deltaTime;
        transform.localPosition = new Vector3(Mathf.RoundToInt(transform.localPosition.x), Mathf.RoundToInt(transform.localPosition.y), transform.localPosition.z);
    }

    void Update()
    {
        // 새로운 이동 명령이 없을 때만 입력을 받습니다.
        if (movementDirection == Vector2.zero && coolTime > moveCoolTime)
        {
            if (inputs.GetMoveUp())
                movementDirection = Vector2.up;
            else if (inputs.GetMoveDown())
                movementDirection = Vector2.down;
            else if (inputs.GetMoveLeft())
                movementDirection = Vector2.left;
            else if (inputs.GetMoveRight())
                movementDirection = Vector2.right;

            if (movementDirection != Vector2.zero)
            {
                if (!mazeManager.isStart)
                    mazeManager.StartMaze();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            mazeManager.FinishMaze();
        }
    }
}
