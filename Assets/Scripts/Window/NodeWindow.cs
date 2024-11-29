using UnityEngine;
using TMPro;

public class NodeWindow : MonoBehaviour
{
    public TMP_Text TitleText;
    public string Path;

    private WindowManager windowManager;

    private void Start()
    {
        // WindowManager 찾기
        windowManager = FindFirstObjectByType<WindowManager>();

        if (windowManager != null)
        {
            // 창 등록
            windowManager.RegisterWindow(gameObject);
        }
    }

    // 창 초기화
    public void Initialize(string title, string path)
    {
        TitleText.text = title;
        Path = path;
    }

    // 창 닫기
    public void CloseWindow()
    {
        Destroy(gameObject);
    }
    public void OnKeyPress()
    {
        // E 키 입력 처리: 창 닫기
        Debug.Log("창 닫기 동작 실행");
        //CloseWindow();
    }
}
