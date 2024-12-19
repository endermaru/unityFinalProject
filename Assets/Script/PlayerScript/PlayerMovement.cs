using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Sprite idleSprite; // 정지 상태 스프라이트

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection; // 현재 이동 방향
    private Vector3 lastMoveDirection; // 마지막으로 이동한 방향

    public float screenWidth = 960f; // x 경계값
    public float screenHeight = 540f; // y 경계값


    void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // 초기값 설정
        spriteRenderer.sprite = idleSprite;
    }

    void Update()
    {
        if (ScenarioManager.Instance.StopKey) return;
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
    }

    void HandleScreenWrap()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, -screenWidth+25, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight-20, screenHeight-55);

        transform.position = position;
    }
}
