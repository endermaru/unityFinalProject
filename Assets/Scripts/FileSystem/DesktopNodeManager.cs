using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DesktopNodeManager : MonoBehaviour
{
    public static DesktopNodeManager Instance { get; private set; }
    public GameObject NodeUIPrefab; // 노드 UI 프리팹
    public Transform NodeContainer; // Grid Layout Group이 적용된 컨테이너
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
    

    // 특정 노드의 모든 자식을 가져와 표시
    public void DisplayDesktopNodes()
        {
        FolderNode desktop = (FolderNode)FileSystemManager.Instance.FindNode("Desktop");

        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // 자식 객체 삭제
        }

        // 자식 노드 리스트 가져오기
        List<Node> childnodes = desktop.Children;

        // 각 자식 노드에 대해 ui 생성
        foreach (Node childnode in childnodes)
        {
            if (childnode.Name == FSConstants.ParentName) continue;
            // 프리팹 생성
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);

            // nodeui 초기화
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
