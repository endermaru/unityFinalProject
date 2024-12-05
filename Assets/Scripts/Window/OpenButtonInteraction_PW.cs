using UnityEngine;

public class OpenButtonInteraction_PW : MonoBehaviour
{
    public GameObject activeImage; // X �̹��� ������Ʈ (Ȱ��ȭ/��Ȱ��ȭ)
    

    private void Start()
    {
        activeImage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� �浹���� ��
        {
            if (activeImage != null)
            {
                activeImage.SetActive(true); // X �̹��� ǥ��
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� ������ ��� ��
        {
            if (activeImage != null)
            {
                activeImage.SetActive(false); // X �̹��� ����
            }
        }
    }
}
