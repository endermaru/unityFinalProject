using UnityEngine;

public class CloseButtonInteraction : MonoBehaviour
{
    private bool playerInRange = false; // �÷��̾ ��ư ���� �ȿ� �ִ��� Ȯ��
    public GameObject panelToClose; // ����� â(�г�)
    public GameObject xImage; // X �̹��� ������Ʈ (Ȱ��ȭ/��Ȱ��ȭ)

    private void Start()
    {
        // X �̹����� �ʱ⿡�� ��Ȱ��ȭ
        if (xImage != null)
        {
            xImage.SetActive(false);
        }
    }

    private void Update()
    {
        // �÷��̾ ��ư ���� �ȿ� �ְ� E Ű�� ������ â �ݱ�
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ClosePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� �浹���� ��
        {
            playerInRange = true;
            if (xImage != null)
            {
                xImage.SetActive(true); // X �̹��� ǥ��
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� ������ ��� ��
        {
            playerInRange = false;
            if (xImage != null)
            {
                xImage.SetActive(false); // X �̹��� ����
            }
        }
    }

    private void ClosePanel()
    {
        SceneStackManager.ReturnToPreviousScene();
    }
}
