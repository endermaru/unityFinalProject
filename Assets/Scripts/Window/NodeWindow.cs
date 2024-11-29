using UnityEngine;
using TMPro;

public class NodeWindow : MonoBehaviour
{
    public TMP_Text TitleText;
    public string Path;

    private WindowManager windowManager;

    private void Start()
    {
        // WindowManager ã��
        windowManager = FindFirstObjectByType<WindowManager>();

        if (windowManager != null)
        {
            // â ���
            windowManager.RegisterWindow(gameObject);
        }
    }

    // â �ʱ�ȭ
    public void Initialize(string title, string path)
    {
        TitleText.text = title;
        Path = path;
    }

    // â �ݱ�
    public void CloseWindow()
    {
        Destroy(gameObject);
    }
    public void OnKeyPress()
    {
        // E Ű �Է� ó��: â �ݱ�
        Debug.Log("â �ݱ� ���� ����");
        //CloseWindow();
    }
}
