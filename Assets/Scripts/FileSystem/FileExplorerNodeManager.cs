using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FileExplorerNodeManager : MonoBehaviour
{
    public GameObject NodeUIPrefab;
    public Transform NodeContainer;
    public NodeIconManager NodeIconManager;

    public static FileExplorerNodeManager Instance { get; private set; }

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


    // Ư�� ����� ��� �ڽ��� ������ ǥ��
    public void DisplayNodes()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // �ڽ� ��ü ����
        }

        FolderNode current = (FolderNode)FileSystemManager.Instance.CurrentNode;
        Debug.Log(current.Name);


        // �ڽ� ��� ����Ʈ ��������
        List<Node> childnodes = current.Children;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Node childnode in childnodes)
        {
            if (childnode.NodeType == NodeT.Computer) continue;

            // ������ ����
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);

            // nodeui �ʱ�ȭ
            NodeUI nodeUI = nodeuiobject.GetComponent<NodeUI>();
            if (nodeUI != null)
            {
                nodeUI.Initialize(childnode, NodeIconManager);
            }
        }
    }
    void Start()
    {
        DisplayNodes();
    }
}
