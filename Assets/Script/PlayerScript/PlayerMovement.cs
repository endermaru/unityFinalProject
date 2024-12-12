using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Sprite idleSprite; // ���� ���� ��������Ʈ
    public Sprite[] walkSprites; // �ȴ� �ִϸ��̼� ��������Ʈ
    public float animationSpeed = 0.2f; // �ִϸ��̼� �ӵ�

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection; // ���� �̵� ����
    private Vector3 lastMoveDirection; // ���������� �̵��� ����
    private float animationTimer;
    private int currentFrame;

    public float screenWidth = 960f; // x ��谪
    public float screenHeight = 540f; // y ��谪


    void Start()
    {
        // SpriteRenderer ������Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // �ʱⰪ ����
    }

    void Update()
    {
        if (ScenarioManager.Instance.IsStarting) return;
        // �Է� ó��
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // �밢�� �Է� ó��
        if (moveX != 0 && moveY != 0)
        {
            if (lastMoveDirection.x != 0) // ������ �̵��� ���� ����
            {
                moveY = 0; // ���� �Է� ����
            }
            else if (lastMoveDirection.y != 0) // ������ �̵��� ���� ����
            {
                moveX = 0; // ���� �Է� ����
            }
        }

        moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // ������ �̵� ���� ������Ʈ (���� ���°� �ƴ� ����)
        if (moveDirection != Vector3.zero)
        {
            lastMoveDirection = moveDirection;
        }

        // �̵� ó��
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ��� ó��
        HandleScreenWrap();

        // �ִϸ��̼� ó��
        UpdateAnimation();
    }

    void HandleScreenWrap()
    {
        Vector3 position = transform.position;

        int playerWidthHalf = 25;
        int playerHeightHalf = 35;


        // X ��� ó��
        if (position.x > screenWidth)
        {
            position.x = screenWidth;
        }
        else if (position.x < -screenWidth + playerWidthHalf)
        {
            position.x = -screenWidth + playerWidthHalf;
        }

        // Y ��� ó��
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
            // �ȴ� �ִϸ��̼�
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
            // ���� ����
            if (idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }
}
