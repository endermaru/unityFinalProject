using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStackManager : MonoBehaviour
{
    private static Stack<string> sceneStack = new Stack<string>(); // 씬 이름을 저장하는 스택

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void PushCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        sceneStack.Push(currentSceneName); // 현재 씬 이름 저장
    }

    public static void LoadScene(string sceneName)
    {
        PushCurrentScene(); // 현재 씬 이름을 스택에 저장
        SceneManager.LoadScene(sceneName); // 새로운 씬 로드
    }

    public static void ReturnToPreviousScene()
    {
        if (sceneStack.Count > 0)
        {
            string previousScene = sceneStack.Pop(); // 스택에서 이전 씬 이름 가져오기
            SceneManager.LoadScene(previousScene); // 이전 씬 로드
        }
        else
        {
            Debug.LogWarning("이전 씬이 없습니다.");
        }
    }
}
