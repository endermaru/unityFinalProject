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
            Destroy(gameObject); // 중복 방지
        }
    }

    public void DisplayNodes()
    {
        foreach (Transform child in NodeContainer)
        {
            Destroy(child.gameObject); // 자식 객체 삭제
        }

        FolderNode current = (FolderNode)FileSystemManager.Instance.CurrentNode;

        // 자식 노드 리스트 가져오기
        List<Canvas> activeWindows = WindowManager.Instance.activeWindows;

        // 각 자식 노드에 대해 ui 생성
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

            // 프리팹 생성
            GameObject nodeuiobject = Instantiate(NodeUIPrefab, NodeContainer);
            nodeuiobject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // nodeui 초기화
            NodeUITaskbar nodeUI = nodeuiobject.GetComponent<NodeUITaskbar>();
            nodeUI.Initialize(icon, NodeIconManager);
            if (nodeUI != null)
            {
                nodeUI.Initialize(icon, NodeIconManager);
            }
        }
    }

}
