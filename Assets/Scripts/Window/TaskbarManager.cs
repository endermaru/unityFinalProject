using System.Collections.Generic;
using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    public NodeIconManager NodeIconManager;
    public Transform NodeContainer;
    public GameObject NodeUIPrefab;

    public static TaskbarManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
        }
    }

    public void DisplayNodes()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // �ڽ� ��ü ����
        }

        FolderNode current = (FolderNode)FileSystemManager.Instance.CurrentNode;

        // �ڽ� ��� ����Ʈ ��������
        List<Canvas> activeWindows = WindowManager.Instance.activeWindows;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Canvas w in activeWindows)
        {
            Node icon = w.tag switch
            {
                "FileExplorer" => new Node("", null, NodeT.Folder),
                "TextEditor" => new Node("", null, NodeT.TextFile),
                "ZipFile" => new Node("", null, NodeT.ZipFile),
                _ => null,
            };
            if (icon == null) continue;

            // ������ ����
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);
            nodeuiobject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // nodeui �ʱ�ȭ
            NodeUITaskbar nodeUI = nodeuiobject.GetComponent<NodeUITaskbar>();
            nodeUI.Initialize(icon, NodeIconManager);
            if (nodeUI != null)
            {
                nodeUI.Initialize(icon, NodeIconManager);
            }
        }
    }

}
