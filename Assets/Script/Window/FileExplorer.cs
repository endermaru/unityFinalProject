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

        // 자식 노드 리스트 가져오기
        List<Node> childnodes = current.Children;

        // 각 자식 노드에 대해 ui 생성
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
