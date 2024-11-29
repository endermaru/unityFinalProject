using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private NodeUI currentNodeUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentNodeUI != null)
            {
                //Camera mainCamera = Camera.main;
                //if (mainCamera != null)
                //{
                //    mainCamera.gameObject.SetActive(false); // 카메라 비활성화
                //}
                Debug.Log($"현재 노드 이름: {currentNodeUI.nodeData.Name}");

                //SceneManager.LoadScene("FileExplorerScene");
                SceneStackManager.LoadScene("FileExplorerScene");
            }
            else
            {
                Debug.Log("노드와 상호작용할 수 없습니다.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NodeUI node = collision.GetComponent<NodeUI>();
        if (node != null && node.IsPlayerInRange())
        {
            currentNodeUI = node;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NodeUI node = collision.GetComponent<NodeUI>();
        if (node != null && currentNodeUI == node)
        {
            currentNodeUI = null;
        }
    }
}
