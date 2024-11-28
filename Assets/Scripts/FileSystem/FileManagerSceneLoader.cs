using UnityEngine;
using UnityEngine.SceneManagement;

public class FileManagerSceneLoader : MonoBehaviour
{
    private string fileManagerSceneName = "FileManagerScene";

    void Start()
    {
        // FileManagerScene�� �߰������� �ε�
        SceneManager.LoadSceneAsync(fileManagerSceneName, LoadSceneMode.Additive);
    }
}
