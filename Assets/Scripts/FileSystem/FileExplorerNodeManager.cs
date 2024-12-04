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
            Destroy(gameObject); // 중복 방지
        }
    }


    // 특정 노드의 모든 자식을 가져와 표시
    public void DisplayNodes()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // 자식 객체 삭제
        }

        FolderNode current = (FolderNode)FileSystemManager.Instance.CurrentNode;
        Debug.Log(current.Name);


        // 자식 노드 리스트 가져오기
        List<Node> childnodes = current.Children;

        // 각 자식 노드에 대해 ui 생성
        foreach (Node childnode in childnodes)
        {
            if (childnode.NodeType == NodeT.Computer) continue;

            // 프리팹 생성
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);

            // nodeui 초기화
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
