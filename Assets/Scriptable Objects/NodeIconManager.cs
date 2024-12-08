using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NodeIconManager", menuName = "Scriptable Objects/NodeIconManager")]
public class NodeIconManager : ScriptableObject
{

    public Sprite DefaultIcon;
    public Sprite FolderIcon;
    public Sprite TextFileIcon;
    public Sprite ComputerIcon;
    public Sprite ZipFileIcon;
    public Sprite EmailIcon;
    public Sprite ExeIcon;

    public Sprite GetIcon(NodeT nodeType)
    {
        switch (nodeType)
        {
            case NodeT.Folder:
                return FolderIcon;
            case NodeT.TextFile:
                return TextFileIcon;
            case NodeT.Computer:
                return ComputerIcon;
            case NodeT.ZipFile:
                return ZipFileIcon;
            case NodeT.Email:
                return EmailIcon;
            case NodeT.Exe:
                return ExeIcon;
            default:
                return DefaultIcon;
        }
    }
}
