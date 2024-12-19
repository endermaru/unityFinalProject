using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Sprite idleSprite; // ���� ���� ��������Ʈ

    private SpriteRenderer spriteRenderer;
    private Vector3 moveDirection; // ���� �̵� ����
    private Vector3 lastMoveDirection; // ���������� �̵��� ����

    public float screenWidth = 960f; // x ��谪
    public float screenHeight = 540f; // y ��谪


    void Start()
    {
        // SpriteRenderer ������Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastMoveDirection = Vector3.zero; // �ʱⰪ ����
        spriteRenderer.sprite = idleSprite;
    }

    void Update()
    {
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

    void HandleScreenWrap()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, -screenWidth+25, screenWidth);
        position.y = Mathf.Clamp(position.y, -screenHeight-20, screenHeight-55);

        transform.position = position;
    }
}
