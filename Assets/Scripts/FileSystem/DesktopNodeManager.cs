using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DesktopNodeManager : MonoBehaviour
{
    public static DesktopNodeManager Instance { get; private set; }
    public GameObject NodeUIPrefab; // ��� UI ������
    public Transform NodeContainer; // Grid Layout Group�� ����� �����̳�
    public NodeIconManager NodeIconManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    // Ư�� ����� ��� �ڽ��� ������ ǥ��
    public void DisplayDesktopNodes()
        {
        FolderNode desktop = (FolderNode)FileSystemManager.Instance.FindNode("Desktop");

        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // �ڽ� ��ü ����
        }

        // �ڽ� ��� ����Ʈ ��������
        List<Node> childnodes = desktop.Children;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Node childnode in childnodes)
        {
            if (childnode.Name == FSConstants.ParentName) continue;
            // ������ ����
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);

            // nodeui �ʱ�ȭ
            NodeUIDesktop nodeUI = nodeuiobject.GetComponent<NodeUIDesktop>();
            if (nodeUI != null)
            {
                nodeUI.Initialize(childnode, NodeIconManager);
            }
        }
    }
    void Start()
    {
        DisplayDesktopNodes();
    }

}
