using UnityEngine;
using UnityEngine.SceneManagement;

public class FileManagerSceneLoader : MonoBehaviour
{
    private string fileManagerSceneName = "FileManagerScene";

    void Start()
    {
        // FileManagerScene을 추가적으로 로드
        SceneManager.LoadSceneAsync(fileManagerSceneName, LoadSceneMode.Additive);
    }
}
