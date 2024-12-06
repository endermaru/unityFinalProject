using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ZipExtractManager : MonoBehaviour
{
    public static ZipExtractManager Instance { get; private set; }
    public ZipNode node;
    public TMP_Text FileName;

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
    }
    public void setFile(ZipNode f)
    {
        node = f;
        Debug.Log("zip: "+node.Name);
        FileName.text = node.Name;
    }
}
