using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    private List<GameObject> activeWindows = new List<GameObject>(); // 활성 창 리스트

    public void RegisterWindow(GameObject window)
    {
        // 창 등록 (리스트 맨 위에 추가)
        activeWindows.Add(window);
    }

    public void UnregisterWindow(GameObject window)
    {
        // 창 제거
        activeWindows.Remove(window);
    }

    public GameObject GetTopWindow()
    {
        // 가장 위의 창 반환
        if (activeWindows.Count > 0)
        {
            return activeWindows[activeWindows.Count - 1];
        }
        return null;
    }
}
