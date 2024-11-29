using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private WindowManager windowManager;
    private NodeUI currentNodeUI;

    public GameObject windowPrefab; // 창 프리팹
    public Transform canvasTransform; // Canvas의 Transform

    private void Start()
    {
        // WindowManager 찾기
        windowManager = FindFirstObjectByType<WindowManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject topWindow = windowManager?.GetTopWindow();

            if (topWindow != null)
            {
                Debug.Log($"가장 위의 창: {topWindow.name}");
                //topWindow.GetComponent<NodeWindow>()?.CloseWindow();
                NodeWindow nodeWindow = topWindow.GetComponent<NodeWindow>();
                if (nodeWindow != null)
                {
                    nodeWindow.OnKeyPress(); // 창에서 E 키 동작 처리
                }
            }
            else if (currentNodeUI != null)
            {
                Debug.Log($"현재 노드 이름: {currentNodeUI.nodeData.Name}");
                OpenNodeWindow(currentNodeUI.nodeData);
                // 창을 열거나 다른 작업 수행
            }
            else
            {
                Debug.Log("노드와 상호작용할 수 없습니다.");
            }
        }
    }

    private void OpenNodeWindow(Node nodeData)
    {
        // 창 생성
        if (windowPrefab != null && canvasTransform != null)
        {
            GameObject window = Instantiate(windowPrefab, canvasTransform);
            NodeWindow nodeWindow = window.GetComponent<NodeWindow>();

            if (nodeWindow != null)
            {
                // 창 초기화 (노드 데이터 기반)
                nodeWindow.Initialize(nodeData.Name, "path");
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
