using UnityEngine;

public class FileSystemIconManager : MonoBehaviour
{
    public GameObject folderIconPrefab; // ���� Prefab
    public GameObject fileIconPrefab;   // ���� Prefab
    public RectTransform fileSystemPanel; // Grid Layout Group�� �ִ� Panel

    public void DisplayFileSystem(FolderNode root)
    {
        // ���� ������ ����
        foreach (Transform child in fileSystemPanel)
        {
            Destroy(child.gameObject);
        }

        // ���� �ý����� ��� ������ ���� ����
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
        // ������ ����
        GameObject newIcon = Instantiate(prefab, parent);
        var buttonText = newIcon.GetComponentInChildren<UnityEngine.UI.Text>();
        if (buttonText != null)
        {
            buttonText.text = name;
        }

        // Ŭ�� �̺�Ʈ �߰� (���� ����)
        newIcon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Debug.Log($"{name} clicked!");
        });
    }
}
