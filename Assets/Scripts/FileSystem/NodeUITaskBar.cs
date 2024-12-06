using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;


public class NodeUITaskbar : MonoBehaviour
{
    public TMP_Text NameText;
    public Image Icon;
    public Image HighlightBox;

    public Node nodeData;
    private bool isPlayerInRange;
    private NodeIconManager iconManager;

    public void Initialize(Node node, NodeIconManager manager)
    {
        nodeData = node;
        iconManager = manager;
        // 이름과 아이콘 설정
        NameText.text = node.Name;
        Icon.sprite = iconManager.GetIcon(node.NodeType);

        // 초기 강조 테두리 비활성화
        HighlightBox.enabled = false;

        isPlayerInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (HighlightBox != null)
                HighlightBox.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (HighlightBox != null)
                HighlightBox.enabled = false;
        }
    }

    public bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

}

