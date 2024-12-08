using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileExplorer : WindowComponent
{
    public static FileExplorer Instance { get; private set; }
    public override string ComponentName => "FileExplorer";
    public GameObject NodeIcon;
    public Transform NodeContainer;
    public TMP_Text WindowName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public override void Display()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject);
        }

        FolderNode current = FileSystemManager.Instance.CurrentNode;
        WindowName.text = current.Name;

        // �ڽ� ��� ����Ʈ ��������
        List<Node> childnodes = current.Children;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Node childnode in childnodes)
        {
            if (childnode.NodeType == NodeT.Computer) continue;

            GameObject nodeObject = Instantiate(NodeIcon, NodeContainer);
            NodeIcon nodeIcon = nodeObject.GetComponent<NodeIcon>();
            nodeIcon.Initialize(childnode, "FileExplorer");
        }
    }
    void Start()
    {
        Display();
    }
}
