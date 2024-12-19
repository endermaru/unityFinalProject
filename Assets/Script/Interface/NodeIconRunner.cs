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

    // 이동할 좌표 리스트
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

    // 이동 속도
    private float Speed = 600f;
    private float moveSpeed = 800f;

    // 현재 이동 중인지 여부
    private bool isMoving = false;

    // 현재 목표 좌표 인덱스
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
            // 이미 이동 중이거나 마지막 위치에 도달한 경우 실행하지 않음
            return;
        }
        // 다음 목표 지점으로 이동 시작
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

    // 코루틴: 선형 이동
    private IEnumerator MoveToTarget(Vector3 targetPosition, Vector3 lastPosition)
    {
        
        
        isMoving = true;
        if (currentTargetIndex == 0) moveSpeed = 5;
        else moveSpeed = Speed;

        // 현재 위치
        Vector3 startPosition = iconTransform.position;

        // 초기 거리
        float remainingDistance = Vector3.Distance(startPosition, targetPosition);
        while (remainingDistance > 0.01f) // 목표 지점에 가까워질 때까지 반복
        {
            // 현재 프레임에서 이동할 거리 계산
            float step = moveSpeed * Time.deltaTime;

            // 이동
            iconTransform.position = Vector3.MoveTowards(iconTransform.position, targetPosition, step);

            // 남은 거리 갱신
            remainingDistance = Vector3.Distance(iconTransform.position, targetPosition);
            yield return null;
        }

        // 정확히 목표 지점에 위치 설정
        iconTransform.position = targetPosition;
        isMoving = false;
        Instance.gameObject.SetActive(false);
        iconTransform.position = lastPosition;
        currentTargetIndex += 2;
    }

    private bool isTrembling = false; // 떨림 상태 확인 변수
    private Coroutine tremblingCoroutine; // 떨림 코루틴 저장 변수

    public void getTrembling(float intensity = 1f, float speed = 30f)
    {
        Instance.gameObject.SetActive(true);
        // 이미 떨림 중이라면 다시 실행하지 않음
        if (isTrembling) return;
        currentTargetIndex = 30;

        isTrembling = true;
        tremblingCoroutine = StartCoroutine(Tremble(intensity, speed));
    }

    public void stopTrembling()
    {
        if (!isTrembling) return;

        isTrembling = false;

        // 떨림 코루틴 중지
        if (tremblingCoroutine != null)
        {
            StopCoroutine(tremblingCoroutine);
            tremblingCoroutine = null;
        }

        // 원래 위치로 복원
        if (iconTransform != null)
        {
            iconTransform.localPosition = Vector3.zero; // 초기 위치 복원 (필요에 따라 변경 가능)
        }
    }

    // 코루틴: 무한 떨림
    private IEnumerator Tremble(float intensity, float speed)
    {
        Vector3 originalPosition = iconTransform.position;

        while (isTrembling)
        {
            // X와 Y축으로 랜덤한 떨림 효과 추가
            float offsetX = Mathf.Sin(Time.time * speed) * intensity;
            float offsetY = Mathf.Cos(Time.time * speed) * intensity;

            iconTransform.position = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            yield return null; // 다음 프레임까지 대기
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerInteract.Instance.IsInteractValid(thisIcon))
        {
            HighlightBox.enabled = true;
            PlayerInteract.Instance.ShowMessage("획득 (E)");
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
