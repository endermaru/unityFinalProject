using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NodeIconManager", menuName = "Scriptable Objects/NodeIconManager")]
public class NodeIconManager : ScriptableObject
{
    public Sprite DefaultIcon; // NodeType에 해당하지 않을 때 사용하는 기본 아이콘
    public Sprite FolderIcon;
    public Sprite TextFileIcon;
    public Sprite ComputerIcon;


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
            default:
                return DefaultIcon;
        }
    }
}
