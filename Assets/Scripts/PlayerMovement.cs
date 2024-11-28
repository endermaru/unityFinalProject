using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite idleSprite; // 정지 상태 스프라이트
    public Sprite[] walkSprites; // 걷는 애니메이션 스프라이트
    public float animationSpeed = 0.2f; // 애니메이션 속도

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection; // 현재 이동 방향
    private Vector3 lastMoveDirection; // 마지막으로 이동한 방향
    private float animationTimer;
    private int currentFrame;

    void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // 초기값 설정
    }

    void Update()
    {
        // 입력 처리
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 대각선 입력 처리
        if (moveX != 0 && moveY != 0)
        {
            if (lastMoveDirection.x != 0) // 마지막 이동이 수평 방향
            {
                moveY = 0; // 수직 입력 무시
            }
            else if (lastMoveDirection.y != 0) // 마지막 이동이 수직 방향
            {
                moveX = 0; // 수평 입력 무시
            }
        }

        moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // 마지막 이동 방향 업데이트 (정지 상태가 아닐 때만)
        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
        }

        // 이동 처리
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 애니메이션 처리
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (moveDirection != Vector3.zero)
        {
            // 걷는 애니메이션
            if (walkSprites != null && walkSprites.Length > 0)
            {
                animationTimer += Time.deltaTime;
                if (animationTimer >= animationSpeed)
                {
                    animationTimer = 0f;
                    currentFrame = (currentFrame + 1) % walkSprites.Length;
                    spriteRenderer.sprite = walkSprites[currentFrame];
                }
            }
        }
        else
        {
            // 정지 상태
            if (idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }
}
