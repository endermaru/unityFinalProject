using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();

    private NodeUI currentNodeUI;

    
    private bool IsObjectVisible(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.enabled; // Renderer가 활성 상태인지 확인
        }

        CanvasRenderer canvasRenderer = obj.GetComponent<CanvasRenderer>();
        if (canvasRenderer != null)
        {
            return canvasRenderer.cull == false; // CanvasRenderer가 숨겨지지 않았는지 확인
        }

        // 렌더러가 없으면 항상 "보이지 않음"으로 간주
        return false;
    }

    public void Start()
    {
        collidingObjects.Clear();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (currentNodeUI != null)
        //    {
        //        //Camera mainCamera = Camera.main;
        //        //if (mainCamera != null)
        //        //{
        //        //    mainCamera.gameObject.SetActive(false); // 카메라 비활성화
        //        //}
        //        Debug.Log($"현재 노드 이름: {currentNodeUI.nodeData.Name}");

        //        //SceneManager.LoadScene("FileExplorerScene");
        //        SceneStackManager.LoadScene("FileExplorerScene");
        //    }
        //    else
        //    {
        //        Debug.Log("노드와 상호작용할 수 없습니다.");
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<GameObject> fronts = WindowManager.Instance.FrontObjects(collidingObjects);
            Canvas frontCanvas = fronts[0].GetComponent<Canvas>();
            foreach (GameObject obj in fronts)
            {
                switch (obj.tag)
                {
                    // 바탕화면의 아이콘
                    case "NodeUI":
                        NodeUIDesktop nodeD = obj.GetComponent<NodeUIDesktop>();
                        switch (nodeD.nodeData.NodeType)
                        {
                            case NodeT.Computer:
                                FileSystemManager.Instance.ChangeCurrentNode(FileSystemManager.Instance.Root);
                                WindowManager.Instance.openWindow(WindowManager.Instance.FileExplorer);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;
                            case NodeT.Folder:
                                FileSystemManager.Instance.ChangeCurrentNode(nodeD.nodeData as FolderNode);
                                WindowManager.Instance.openWindow(WindowManager.Instance.FileExplorer);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;
                        }
                        break;

                    // 파일탐색기의 아이콘
                    case "NodeUIBlack":
                        NodeUI node = obj.GetComponent<NodeUI>();
                        switch (node.nodeData.NodeType)
                        {
                            case NodeT.Folder:
                                FileSystemManager.Instance.ChangeCurrentNode(node.nodeData as FolderNode);
                                FileExplorerNodeManager.Instance.DisplayNodes();
                                break;

                        }
                        break;

                    case "CloseButton":
                        WindowManager.Instance.closeWindow(frontCanvas);
                        break;
                }
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    NodeUI node = collision.GetComponent<NodeUI>();
    //    if (node != null && node.IsPlayerInRange())
    //    {
    //        currentNodeUI = node;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    NodeUI node = collision.GetComponent<NodeUI>();
    //    if (node != null && currentNodeUI == node)
    //    {
    //        currentNodeUI = null;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 시작 시 오브젝트 추가
        if (IsObjectVisible(other.gameObject) && 
                !collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
            //Debug.Log("in:" + other.gameObject.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 충돌 끝날 시 오브젝트 제거
        if (IsObjectVisible(other.gameObject) &&
                collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
            //Debug.Log("out:" + other.gameObject.ToString());
        }
    }
}
