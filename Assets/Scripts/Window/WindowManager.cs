using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    private List<GameObject> activeWindows = new List<GameObject>(); // Ȱ�� â ����Ʈ

    public void RegisterWindow(GameObject window)
    {
        // â ��� (����Ʈ �� ���� �߰�)
        activeWindows.Add(window);
    }

    public void UnregisterWindow(GameObject window)
    {
        // â ����
        activeWindows.Remove(window);
    }

    public GameObject GetTopWindow()
    {
        // ���� ���� â ��ȯ
        if (activeWindows.Count > 0)
        {
            return activeWindows[activeWindows.Count - 1];
        }
        return null;
    }
}
