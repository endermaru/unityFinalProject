using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    public List<Canvas> activeWindows = new();
    public Canvas Desktop;
    public Canvas FileExplorer;
    public Canvas TextEditor;
    public Canvas PasswordWindow;
    public Canvas Taskbar;
    public Canvas ZipExtractWindow;
    public Canvas ImageViewer;
    public Canvas AdPopup;
    public Canvas ExeWindow;
    public Canvas FinalWindow;
    public List<Canvas> AllWindows;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        AllWindows = new List<Canvas>()
        {
            Desktop, FileExplorer, TextEditor, PasswordWindow,
            Taskbar, ZipExtractWindow, ImageViewer, AdPopup, ExeWindow, FinalWindow,
        };
    }

    public void Start()
    {
        OpenWindow(Desktop);
        Desktop.sortingOrder = 10;
        OpenWindow(Taskbar);
    }

    public Canvas WhoseCanvas(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            WindowComponent window = current.GetComponent<WindowComponent>();
            if (window != null) { return window.canvas; }
            current = current.parent;
        }
        return null;
    }

    public List<GameObject> FrontObjects(List<GameObject> collidingList)
    {
        var validObjects = collidingList.Where(obj => obj != null).ToList();

        Canvas maxC = Desktop;
        foreach (var obj in validObjects)
        {
            if (obj == null) continue;
            var window = WhoseCanvas(obj);
            if (window == null) continue;
            if (window.sortingOrder > maxC.sortingOrder)
            {
                maxC = window;
            }
        }

        List<GameObject> ret = new()
        {
            maxC.gameObject
        };

        foreach (GameObject obj in validObjects)
        {
            if (obj == null) continue;
            if (obj != maxC.gameObject && WhoseCanvas(obj) == maxC)
            {
                ret.Add(obj);
            }
        }

        return ret.Where(obj => obj != null).ToList();
    }

    

    public void OpenWindow(Canvas window)
    {
        int maxSortingOrder = GetMaxSortingOrderCanvas().sortingOrder;
        window.sortingOrder = maxSortingOrder + 1;
        
        if (!activeWindows.Contains(window))
        {
            activeWindows.Add(window);
        }

        foreach (Canvas canvas in activeWindows)
        {
            WindowComponent windowComponent = canvas.GetComponent<WindowComponent>();
            if (windowComponent != null) windowComponent.SetWindowActive(window==canvas);
        }

        TaskbarWindow.Instance.Display();
    }

    public void CloseWindow(Canvas window)
    {
        activeWindows.Remove(window);
        int minSortingOrder = GetMinSortingOrderCanvas().sortingOrder;
        window.sortingOrder = minSortingOrder - 1;

        WindowComponent windowComponent = GetMaxSortingOrderCanvas().GetComponent<WindowComponent>();
        if (windowComponent != null) windowComponent.SetWindowActive(true);

        TaskbarWindow.Instance.Display();
    }

    public void StopWindow(Canvas window)
    {
        window.sortingOrder = Desktop.sortingOrder - 1;
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
