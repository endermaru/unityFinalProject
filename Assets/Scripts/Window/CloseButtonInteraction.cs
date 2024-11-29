using UnityEngine;

public class CloseButtonInteraction : MonoBehaviour
{
    private bool playerInRange = false; // 플레이어가 버튼 범위 안에 있는지 확인
    public GameObject panelToClose; // 연결된 창(패널)
    public GameObject xImage; // X 이미지 오브젝트 (활성화/비활성화)

    private void Start()
    {
        // X 이미지를 초기에는 비활성화
        if (xImage != null)
        {
            xImage.SetActive(false);
        }
    }

    private void Update()
    {
        // 플레이어가 버튼 범위 안에 있고 E 키를 누르면 창 닫기
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ClosePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트와 충돌했을 때
        {
            playerInRange = true;
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
            playerInRange = false;
            if (xImage != null)
            {
                xImage.SetActive(false); // X 이미지 숨김
            }
        }
    }

    private void ClosePanel()
    {
        SceneStackManager.ReturnToPreviousScene();
    }
}
