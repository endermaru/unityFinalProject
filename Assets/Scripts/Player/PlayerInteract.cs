using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private WindowManager windowManager;
    private NodeUI currentNodeUI;

    public GameObject windowPrefab; // â ������
    public Transform canvasTransform; // Canvas�� Transform

    private void Start()
    {
        // WindowManager ã��
        windowManager = FindFirstObjectByType<WindowManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject topWindow = windowManager?.GetTopWindow();

            if (topWindow != null)
            {
                Debug.Log($"���� ���� â: {topWindow.name}");
                //topWindow.GetComponent<NodeWindow>()?.CloseWindow();
                NodeWindow nodeWindow = topWindow.GetComponent<NodeWindow>();
                if (nodeWindow != null)
                {
                    nodeWindow.OnKeyPress(); // â���� E Ű ���� ó��
                }
            }
            else if (currentNodeUI != null)
            {
                Debug.Log($"���� ��� �̸�: {currentNodeUI.nodeData.Name}");
                OpenNodeWindow(currentNodeUI.nodeData);
                // â�� ���ų� �ٸ� �۾� ����
            }
            else
            {
                Debug.Log("���� ��ȣ�ۿ��� �� �����ϴ�.");
            }
        }
    }

    private void OpenNodeWindow(Node nodeData)
    {
        // â ����
        if (windowPrefab != null && canvasTransform != null)
        {
            GameObject window = Instantiate(windowPrefab, canvasTransform);
            NodeWindow nodeWindow = window.GetComponent<NodeWindow>();

            if (nodeWindow != null)
            {
                // â �ʱ�ȭ (��� ������ ���)
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
