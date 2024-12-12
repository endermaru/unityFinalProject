using System.Collections.Generic;
using UnityEngine;

public class Desktop : WindowComponent
{
    public static Desktop Instance { get; private set; }
    public override string ComponentName => "Desktop";
    public GameObject NodeIcon;
    public Transform NodeContainer;

    FolderNode desktop;

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

        // �ڽ� ��� ����Ʈ ��������
        List<Node> childnodes = desktop.Children;

        // �� �ڽ� ��忡 ���� ui ����
        foreach (Node childnode in childnodes)
        {
            if (childnode.Name == FSConstants.ParentName) continue;

            if (!FileSystemManager.Instance.ShowHidden && childnode.Hidden) continue;

            GameObject nodeObject = Instantiate(NodeIcon, NodeContainer);
            NodeIcon nodeIcon = nodeObject.GetComponent<NodeIcon>();
            nodeIcon.Initialize(childnode, "Desktop");
        }
    }
    void Start()
    {
        desktop = (FolderNode)FileSystemManager.Instance.FindNode("Desktop");
        Display();
    }
}
