using UnityEngine;

public class CloseButtonInteraction : MonoBehaviour
{
    public GameObject xImage; // X 이미지 오브젝트 (활성화/비활성화)
    public WindowManager WindowManager;

    private void Start()
    {
        // X 이미지를 초기에는 비활성화
        if (xImage != null)
        {
            xImage.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트와 충돌했을 때
        {
            if (xImage != null)
            {
                xImage.SetActive(true); // X 이미지 표시
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트가 범위를 벗어날 때
        {
            if (xImage != null)
            {
                xImage.SetActive(false); // X 이미지 숨김
            }
        }
    }
}
