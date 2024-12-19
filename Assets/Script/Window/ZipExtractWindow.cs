using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ZipExtractWindow : WindowComponent
{
    public static ZipExtractWindow Instance { get; private set; }
    public FileNode Node;
    public TMP_Text FileName;

    public override string ComponentName => "ZipExtractWindow";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void SetFile(FileNode node)
    {
        Node = node;
        FileName.text = Node.Name;
    }
}
