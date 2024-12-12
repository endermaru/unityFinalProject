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

    public float screenWidth = 960f; // x 경계값
    public float screenHeight = 540f; // y 경계값


    void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // 초기값 설정
    }

    void Update()
    {
        if (ScenarioManager.Instance.IsStarting) return;
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

        // 경계 처리
        HandleScreenWrap();

        // 애니메이션 처리
        UpdateAnimation();
    }

    void HandleScreenWrap()
    {
        Vector3 position = transform.position;

        int playerWidthHalf = 25;
        int playerHeightHalf = 35;


        // X 경계 처리
        if (position.x > screenWidth)
        {
            position.x = screenWidth;
        }
        else if (position.x < -screenWidth + playerWidthHalf)
        {
            position.x = -screenWidth + playerWidthHalf;
        }

        // Y 경계 처리
        if (position.y > screenHeight - playerHeightHalf)
        {
            position.y = screenHeight - playerHeightHalf;
        }
        else if (position.y < -screenHeight)
        {
            position.y = -screenHeight;
        }

        transform.position = position;
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
