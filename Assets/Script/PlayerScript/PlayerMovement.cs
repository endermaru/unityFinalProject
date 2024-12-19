using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Sprite idleSprite; // ���� ���� ��������Ʈ

    public Sprite[] loopingSprites; // �ݺ� ����� ��������Ʈ �迭
    public float frameRate = 0.1f; // ������ ��ȯ �ӵ�

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection; // ���� �̵� ����
    private Vector3 lastMoveDirection; // ���������� �̵��� ����

    private float animationTimer = 0f; // �ִϸ��̼� Ÿ�̸�
    private int currentFrame = 0; // ���� ��������Ʈ ������

    public float screenWidth = 960f; // x ��谪
    public float screenHeight = 540f; // y ��谪


    void Start()
    {
        // SpriteRenderer ������Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // �ʱⰪ ����
        spriteRenderer.sprite = idleSprite;

        if (loopingSprites.Length > 0)
        {
            spriteRenderer.sprite = loopingSprites[0]; // ù ��������Ʈ ����
        }
    }

    void Update()
    {
        AnimateSprite();
        if (ScenarioManager.Instance.StopKey) return;
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

        
    }

    void AnimateSprite()
    {
        if (loopingSprites.Length <= 0) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameRate)
        {
            animationTimer = 0f;
            currentFrame = (currentFrame + 1) % loopingSprites.Length; // ���� ������
            spriteRenderer.sprite = loopingSprites[currentFrame]; // ��������Ʈ ������Ʈ
        }
    }


    void HandleScreenWrap()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, -screenWidth+25, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight-20, screenHeight-55);

        transform.position = position;
    }
}
