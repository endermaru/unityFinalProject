using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassWordManager : MonoBehaviour
{
    public static PassWordManager Instance { get; private set; }
    public FileNode node;
    public TMP_Text Content;
    public GameObject invalid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        invalid.SetActive(false);
    }
    public void setFile(FileNode f)
    {
        node = f;
        ResetContent();
        invalid.SetActive(false);
    }
    public void Update()
    {
        if (Content.text.Length < 27)
        {
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    Content.text += (key - KeyCode.Alpha0).ToString();
                }
            }
        }
    }

    public void ResetContent()
    {
        Content.text = string.Empty;
    }

    public bool CheckPassword()
    {
        return Content.text == node.Password;
    }
}
