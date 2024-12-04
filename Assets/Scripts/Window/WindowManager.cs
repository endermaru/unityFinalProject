using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private List<Canvas> activeWindows = new List<Canvas>(); // Ȱ�� â ����Ʈ
    public Canvas Desktop;
    public Canvas FileExplorer;
    public List<Canvas> AllWindows;

    public Canvas whoseCanvas(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            foreach (Canvas c in activeWindows) 
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
        BringToFront(max);
        foreach (GameObject obj in list)
        {
            if (obj != max.gameObject && whoseCanvas(obj)==max) ret.Add(obj);
        }
        return ret;
    }

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
    }

    public void Start()
    {
        openWindow(Desktop);
        Debug.Log(Desktop.sortingOrder);
        Debug.Log(FileExplorer.sortingOrder);
        FileExplorer.enabled = false;
    }

    public void openWindow(Canvas window)
    {
        Debug.Log("Open");
        // �� â �߰� ��, ���� ��ܿ� ��ġ
        int maxSortingOrder = GetMaxSortingOrderCanvas().sortingOrder;
        window.sortingOrder = maxSortingOrder + 1;

        activeWindows.Add(window);
        window.enabled = true; // �� â Ȱ��ȭ
    }

    public void closeWindow(Canvas window)
    {
        activeWindows.Remove(window);
        window.enabled = false;
    }

    public void BringToFront(Canvas window)
    {
        if (activeWindows.Contains(window))
        {
            // ���� ���� sortingOrder�� ����
            int maxSortingOrder = GetMaxSortingOrderCanvas().sortingOrder;
            window.sortingOrder = maxSortingOrder + 1;
        }
    }

    private Canvas GetMaxSortingOrderCanvas()
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
}
