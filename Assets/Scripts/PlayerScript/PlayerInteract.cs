using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();
  
    public void Start()
    {
        collidingObjects.Clear();
    }

    private void Update()
    {
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
                            case NodeT.TextFile:
                                FileNode f = nodeD.nodeData as FileNode;
                                if (f.Password != null)
                                {
                                    PassWordManager.Instance.setFile(f);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    TextEditorManager.Instance.setTextFile(nodeD.nodeData as FileNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                                    TextEditorManager.Instance.Display();
                                }
                                break;
                            case NodeT.ZipFile:
                                ZipNode zipNode = nodeD.nodeData as ZipNode;
                                ZipExtractManager.Instance.setFile(zipNode);
                                if (zipNode.Password != null)
                                {
                                    PassWordManager.Instance.setFile(zipNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    WindowManager.Instance.openWindow(WindowManager.Instance.ZipExtractWindow);
                                }
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
                            case NodeT.TextFile:
                                FileNode f = node.nodeData as FileNode;
                                if (f.Password != null)
                                {
                                    PassWordManager.Instance.setFile(f);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    TextEditorManager.Instance.setTextFile(node.nodeData as FileNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                                    TextEditorManager.Instance.Display();
                                }
                                break;

                            case NodeT.ZipFile:
                                ZipNode zipNode = node.nodeData as ZipNode;
                                ZipExtractManager.Instance.setFile(zipNode);
                                if (zipNode.Password != null)
                                {
                                    PassWordManager.Instance.setFile(zipNode);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.PassWordWindow);
                                }
                                else
                                {
                                    WindowManager.Instance.openWindow(WindowManager.Instance.ZipExtractWindow);
                                }
                                break;
                        }
                        break;
                    case "NodeUITaskbar":
                        NodeUITaskbar n =  obj.GetComponent<NodeUITaskbar>();
                        Canvas c = n.nodeData.NodeType switch
                        {
                            NodeT.Folder => WindowManager.Instance.FileExplorer,
                            NodeT.TextFile => WindowManager.Instance.TextEditor,
                            NodeT.ZipFile => WindowManager.Instance.ZipExtractWindow,
                            _ => null,
                        };
                        if (c == null) break;
                        WindowManager.Instance.openWindow(c);
                        break;

                    case "CloseButton":
                        WindowManager.Instance.closeWindow(frontCanvas);
                        break;

                    case "OkButton":
                        if (frontCanvas == WindowManager.Instance.PassWordWindow)
                        {
                            if (PassWordManager.Instance.CheckPassword())
                            {
                                if (PassWordManager.Instance.node?.NodeType == NodeT.TextFile)
                                {
                                    FileNode nodePW = PassWordManager.Instance.node;
                                    WindowManager.Instance.closeWindow(WindowManager.Instance.PassWordWindow);
                                    TextEditorManager.Instance.setTextFile(nodePW);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.TextEditor);
                                    TextEditorManager.Instance.Display();
                                }
                                else if (PassWordManager.Instance.nodeZip?.NodeType == NodeT.ZipFile)
                                {
                                    WindowManager.Instance.closeWindow(WindowManager.Instance.PassWordWindow);
                                    WindowManager.Instance.openWindow(WindowManager.Instance.ZipExtractWindow);
                                   
                                }
                                PassWordManager.Instance.resetNode();
                            }

                            PassWordManager.Instance.ResetContent();
                        }
                        else if (frontCanvas == WindowManager.Instance.ZipExtractWindow)
                        {
                            ZipNode ZipFile = ZipExtractManager.Instance.node;
                            FolderNode parent = ZipFile.Parent as FolderNode;
                            parent.AddChild(ZipFile.ZipRoot);
                            FileExplorerNodeManager.Instance.DisplayNodes();
                            DesktopNodeManager.Instance.DisplayDesktopNodes();
                            WindowManager.Instance.closeWindow(WindowManager.Instance.ZipExtractWindow);
                        }
                       
                        break;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 시작 시 오브젝트 추가
        if (/*IsObjectVisible(other.gameObject) &&*/ 
                !collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
            //Debug.Log("in:" + other.gameObject.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 충돌 끝날 시 오브젝트 제거
        if (/*IsObjectVisible(other.gameObject) &&*/
                collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
            //Debug.Log("out:" + other.gameObject.ToString());
        }
    }
}
