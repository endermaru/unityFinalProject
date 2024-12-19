using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class NodeIconRunner : MonoBehaviour, IComponent
{
    string IComponent.ComponentType => "NodeIconRunner";
    public static NodeIconRunner Instance { get; private set; }

    public void Interact()
    {
        Instance.gameObject.SetActive(false);
        //use item
        PlayerInteract.Instance.HasZipper = true;
        stopTrembling();
        ScenarioManager.Instance.getZipper = true;
    }

    public TMP_Text NameText;
    public Image Icon;
    public Image HighlightBox;
    public Node Node;
    public GameObject thisIcon;

    // �̵��� ��ǥ ����Ʈ
    private List<Vector3> targetPositions = new List<Vector3>{
        new Vector3(188, 185, 0), //2 Trap/tmp-extra/

        new Vector3(-10, 185, 0),
        new Vector3(-530, 185, 0), //4 Trap/

        new Vector3(149, 185, 0),
        new Vector3(-175, 185, 0), //6 Trap/tmp2/

        new Vector3(86, 185, 0),
        new Vector3(540, 185, 0),  //8 Trap/tmp2/tmp2-extra/

        new Vector3(258, 185, 0),
        new Vector3(-170, 185, 0), //10 Trap/tmp3/

        new Vector3(417, 185, 0),
        new Vector3(190, 185, 0), //12 Trap/tmp3/tmp3-4

        new Vector3(-64, 185, 0),
        new Vector3(-350, 185, 0), //14 Trap/tmp1

        new Vector3(-140, 185, 0),
        new Vector3(189, 185, 0), //16 Trap/tmp1-4

        new Vector3(-80, 185, 0),
        new Vector3(-350, 185, 0), //18 Trap/tmp1-4/tmp1-4-1

        new Vector3(535, -245, 0),


    };

    // �̵� �ӵ�
    private float Speed = 600f;
    private float moveSpeed = 800f;

    // ���� �̵� ������ ����
    private bool isMoving = false;

    // ���� ��ǥ ��ǥ �ε���
    private int currentTargetIndex = 0;
    public Transform iconTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        Node = new ItemNode("zipper", null);
        NameText.text = Node.Name;
        NameText.color = Color.black;
        HighlightBox.enabled = false;
        Instance.gameObject.SetActive(false);
    }

    public void RunnerRun(int idx)
    {
        if (idx != currentTargetIndex)
        {
            return;
        }
        if (isMoving)
        {
            // �̹� �̵� ���̰ų� ������ ��ġ�� ������ ��� �������� ����
            return;
        }
        // ���� ��ǥ �������� �̵� ����
        Vector3 targetPosition = targetPositions[currentTargetIndex];
        Vector3 lastPosition = targetPositions[currentTargetIndex + 1];
        Instance.gameObject.SetActive(true);
        StartCoroutine(MoveToTarget(targetPosition, lastPosition));
    }

    public void setSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public int getCurrentIndex()
    {
        return currentTargetIndex;
    }

    // �ڷ�ƾ: ���� �̵�
    private IEnumerator MoveToTarget(Vector3 targetPosition, Vector3 lastPosition)
    {
        
        
        isMoving = true;
        if (currentTargetIndex == 0) moveSpeed = 5;
        else moveSpeed = Speed;

        // ���� ��ġ
        Vector3 startPosition = iconTransform.position;

        // �ʱ� �Ÿ�
        float remainingDistance = Vector3.Distance(startPosition, targetPosition);
        while (remainingDistance > 0.01f) // ��ǥ ������ ������� ������ �ݺ�
        {
            // ���� �����ӿ��� �̵��� �Ÿ� ���
            float step = moveSpeed * Time.deltaTime;

            // �̵�
            iconTransform.position = Vector3.MoveTowards(iconTransform.position, targetPosition, step);

            // ���� �Ÿ� ����
            remainingDistance = Vector3.Distance(iconTransform.position, targetPosition);
            yield return null;
        }

        // ��Ȯ�� ��ǥ ������ ��ġ ����
        iconTransform.position = targetPosition;
        isMoving = false;
        Instance.gameObject.SetActive(false);
        iconTransform.position = lastPosition;
        currentTargetIndex += 2;
    }

    private bool isTrembling = false; // ���� ���� Ȯ�� ����
    private Coroutine tremblingCoroutine; // ���� �ڷ�ƾ ���� ����

    public void getTrembling(float intensity = 1f, float speed = 30f)
    {
        Instance.gameObject.SetActive(true);
        // �̹� ���� ���̶�� �ٽ� �������� ����
        if (isTrembling) return;
        currentTargetIndex = 30;

        isTrembling = true;
        tremblingCoroutine = StartCoroutine(Tremble(intensity, speed));
    }

    public void stopTrembling()
    {
        if (!isTrembling) return;

        isTrembling = false;

        // ���� �ڷ�ƾ ����
        if (tremblingCoroutine != null)
        {
            StopCoroutine(tremblingCoroutine);
            tremblingCoroutine = null;
        }

        // ���� ��ġ�� ����
        if (iconTransform != null)
        {
            iconTransform.localPosition = Vector3.zero; // �ʱ� ��ġ ���� (�ʿ信 ���� ���� ����)
        }
    }

    // �ڷ�ƾ: ���� ����
    private IEnumerator Tremble(float intensity, float speed)
    {
        Vector3 originalPosition = iconTransform.position;

        while (isTrembling)
        {
            // X�� Y������ ������ ���� ȿ�� �߰�
            float offsetX = Mathf.Sin(Time.time * speed) * intensity;
            float offsetY = Mathf.Cos(Time.time * speed) * intensity;

            iconTransform.position = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            yield return null; // ���� �����ӱ��� ���
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(thisIcon))
        {
            HighlightBox.enabled = true;
            PlayerInteract.Instance.ShowMessage("ȹ�� (E)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HighlightBox.enabled = false;
            PlayerInteract.Instance.HideMessage();
        }
    }

}
