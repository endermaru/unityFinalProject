using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private NodeUI currentNodeUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentNodeUI != null)
        {
            if (currentNodeUI != null)
            {
                Debug.Log($"현재 노드 이름: {currentNodeUI.nodeData.Name}");
            }
            else
            {
                Debug.Log("!!!!!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NodeUI node = collision.GetComponent<NodeUI>();
        if (node != null && node.IsPlayerInRange())
        {
            currentNodeUI = node;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NodeUI node = collision.GetComponent<NodeUI>();
        if (node != null && currentNodeUI == node)
        {
            currentNodeUI = null;
        }
    }
}
