using UnityEngine;

public class OpenButtonInteraction_PW : MonoBehaviour
{
    public GameObject activeImage; // X 이미지 오브젝트 (활성화/비활성화)
    

    private void Start()
    {
        activeImage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트와 충돌했을 때
        {
            if (activeImage != null)
            {
                activeImage.SetActive(true); // X 이미지 표시
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트가 범위를 벗어날 때
        {
            if (activeImage != null)
            {
                activeImage.SetActive(false); // X 이미지 숨김
            }
        }
    }
}
