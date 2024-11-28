using UnityEngine;

public class FileSystemIconManager : MonoBehaviour
{
    public GameObject folderIconPrefab; // 폴더 Prefab
    public GameObject fileIconPrefab;   // 파일 Prefab
    public RectTransform fileSystemPanel; // Grid Layout Group이 있는 Panel

    public void DisplayFileSystem(FolderNode root)
    {
        // 기존 아이콘 제거
        foreach (Transform child in fileSystemPanel)
        {
            Destroy(child.gameObject);
        }

        // 파일 시스템의 모든 폴더와 파일 생성
        foreach (var child in root.Children)
        {
            if (child is FolderNode folder)
            {
                CreateIcon(folderIconPrefab, folder.Name, fileSystemPanel);
            }
            else if (child is FileNode file)
            {
                CreateIcon(fileIconPrefab, file.Name, fileSystemPanel);
            }
        }
    }

    private void CreateIcon(GameObject prefab, string name, Transform parent)
    {
        // 아이콘 생성
        GameObject newIcon = Instantiate(prefab, parent);
        var buttonText = newIcon.GetComponentInChildren<UnityEngine.UI.Text>();
        if (buttonText != null)
        {
            buttonText.text = name;
        }

        // 클릭 이벤트 추가 (선택 사항)
        newIcon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Debug.Log($"{name} clicked!");
        });
    }
}
