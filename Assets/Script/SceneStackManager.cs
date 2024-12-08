using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStackManager : MonoBehaviour
{
    private static Stack<string> sceneStack = new Stack<string>(); // �� �̸��� �����ϴ� ����

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void PushCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneStack.Push(currentSceneName); // ���� �� �̸� ����
    }

    public static void LoadScene(string sceneName)
    {
        PushCurrentScene(); // ���� �� �̸��� ���ÿ� ����
        SceneManager.LoadScene(sceneName); // ���ο� �� �ε�
    }

    public static void ReturnToPreviousScene()
    {
        if (sceneStack.Count > 0)
        {
            string previousScene = sceneStack.Pop(); // ���ÿ��� ���� �� �̸� ��������
            SceneManager.LoadScene(previousScene); // ���� �� �ε�
        }
        else
        {
            Debug.LogWarning("���� ���� �����ϴ�.");
        }
    }
}
