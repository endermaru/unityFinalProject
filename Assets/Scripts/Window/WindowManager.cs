using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private List<Canvas> activeWindows = new List<Canvas>(); // 활성 창 리스트
    public Canvas Desktop;
    public Canvas FileExplorer;
    public Canvas TextEditor;
    public Canvas PassWordWindow;
    public List<Canvas> AllWindows;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        AllWindows = new List<Canvas>()
        {
            Desktop, FileExplorer, TextEditor, PassWordWindow,
        };
    }

    public void Start()
    {
        openWindow(Desktop);
        Desktop.sortingOrder = 10;
        //openWindow(PassWordWindow);
    }

    public Canvas whoseCanvas(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            foreach (Canvas c in AllWindows) 
            {
                if (current == c.transform) return c;
            }
            current = current.parent;
        }
        return null;
    }

    public List<GameObject> FrontObjects(List<GameObject> list)
    {
        Canvas max = Desktop;
        foreach (var obj in list)
        {
            var window = whoseCanvas(obj);
            if (window.sortingOrder > max.sortingOrder)
            {
                max = window;
            }
        }
        List<GameObject> ret = new List<GameObject>
        {
            max.gameObject
        };
        if (max != Desktop)
        {
            BringToFront(max);
        }
        else
        {
            adjustHeaders(Desktop);
        }
        

        foreach (GameObject obj in list)
        {
            if (obj != max.gameObject && whoseCanvas(obj)==max) ret.Add(obj);
        }
        
        return ret;
    }

    

    public void openWindow(Canvas window)
    {
        // 새 창 추가 시, 가장 상단에 배치
        int maxSortingOrder = GetMaxSortingOrderCanvas().sortingOrder;
        window.sortingOrder = maxSortingOrder + 1;
        
        if (!activeWindows.Contains(window))
        {
            activeWindows.Add(window);
            
        }
        BringToFront(window);
    }

    public void closeWindow(Canvas window)
    {
        activeWindows.Remove(window);
        int minSortingOrder = GetMinSortingOrderCanvas().sortingOrder;
        window.sortingOrder = minSortingOrder - 1;

        Canvas max = GetMaxSortingOrderCanvas();
        if (max != Desktop)
            adjustHeaders(max);
    }

    public void BringToFront(Canvas window)
    {
        if (activeWindows.Contains(window))
        {
            // 가장 높은 sortingOrder로 설정
            int maxSortingOrder = GetMaxSortingOrderCanvas().sortingOrder;
            window.sortingOrder = maxSortingOrder + 1;

            adjustHeaders(window);
        }
        
    }

    private void adjustHeaders(Canvas max)
    {
        foreach (Canvas w in activeWindows)
        {
            switch (w.tag)
            {
                case "FileExplorer":
                    FileExplorerNodeManager.Instance.inactiveHeader.GetComponent<Canvas>().enabled = (w != max);
                    break;
                case "TextEditor":
                    TextEditorManager.Instance.inactiveHeader.GetComponent<Canvas>().enabled = (w != max);
                    break;
                default:
                    break;
            }
        }
    }

    public Canvas GetMaxSortingOrderCanvas()
    {
        Canvas maxOrderCanvas = Desktop;
        foreach (var window in activeWindows)
        {
            if (window.sortingOrder > maxOrderCanvas.sortingOrder)
            {
                maxOrderCanvas = window;
            }
        }
        return maxOrderCanvas;
    }

    public Canvas GetMinSortingOrderCanvas()
    {
        Canvas minOrderCanvas = Desktop;
        foreach (var window in activeWindows)
        {
            if (window.sortingOrder < minOrderCanvas.sortingOrder)
            {
                minOrderCanvas = window;
            }
        }
        return minOrderCanvas;
    }
}
