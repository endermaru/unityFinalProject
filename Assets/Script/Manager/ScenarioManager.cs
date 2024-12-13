using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    public Image topBar; // 위쪽 검은 막대
    public Image bottomBar; // 아래쪽 검은 막대
    public Camera mainCamera; // 메인 카메라
    public Transform player; // 플레이어 Transform
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 애니메이션 곡선
    public float cinematicZoom = 500f; // 영화 효과 확대 비율 (OrthographicSize 조정)
    public float animationDuration = 1f; // 애니메이션 지속 시간

    public bool IsStarting = false;

    public GameObject DialogBox;
    public TMP_Text Dialog;

    public int CurrentScene = 0;
    public JArray DialogFile;
    private IEnumerator DialogEnumerator;

    private float originalSize; // 카메라의 원래 OrthographicSize 저장
    private Vector3 originalPosition; // 카메라의 원래 위치 저장
    private Vector2 topBarOriginalPos;
    private Vector2 bottomBarOriginalPos;
    private Vector3 cinematicPosition;

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
        DialogBox.SetActive(false);
        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Dialog.json"));
        DialogFile = JArray.Parse(json);
    }

    void Start()
    {
        // 카메라 원래 OrthographicSize 및 위치 저장
        originalSize = mainCamera.orthographicSize;
        originalPosition = mainCamera.transform.position;

        // UI 오브젝트 초기 위치 저장
        topBarOriginalPos = topBar.rectTransform.anchoredPosition;
        bottomBarOriginalPos = bottomBar.rectTransform.anchoredPosition;

        if (false)
        {
            StartCoroutine(MovePlayerToCenter());
        }
        else
        {
            CurrentScene++;
        }

    }

    private IEnumerator MovePlayerToCenter()
    {
        IsStarting = true;
        // 플레이어 시작 위치를 화면 바깥 위쪽으로 설정
        Vector3 startPosition = new Vector3(0, 800, player.transform.localPosition.z);
        Vector3 targetPosition = new Vector3(0, 0, player.transform.localPosition.z);
        player.transform.localPosition = startPosition;

        float duration = 1f; // 이동 시간
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 플레이어 위치를 선형 보간으로 이동
            player.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            yield return null;
        }

        // 정확히 중앙으로 위치 고정
        player.transform.localPosition = targetPosition;
        yield return StartCoroutine(PlayerCrashAnimation(targetPosition));
        yield return new WaitForSeconds(1.5f);
        // StartDialog 호출
        StartDialog();
    }

    private IEnumerator PlayerCrashAnimation(Vector3 centerPosition)
    {
        float crashDuration = 0.5f; // 추락 애니메이션 지속 시간
        float elapsedTime = 0f;

        Vector3 startCrashPosition = centerPosition + new Vector3(0, 25, 0); // 약간 위에서 시작
        Vector3 endCrashPosition = centerPosition; // 중심에 도달

        while (elapsedTime < crashDuration)
        {
            elapsedTime += Time.deltaTime;

            // 빠르게 추락하는 느낌
            player.transform.localPosition = Vector3.Lerp(startCrashPosition, endCrashPosition, Mathf.SmoothStep(0, 1, elapsedTime / crashDuration));

            yield return null;
        }

        // 정확히 중심 위치 고정
        player.transform.localPosition = endCrashPosition;

        // 충격 효과 (예: 진동)
        StartCoroutine(ApplyScreenShake());
    }

    private IEnumerator ApplyScreenShake()
    {
        float shakeDuration = 0.2f; // 진동 지속 시간
        float shakeMagnitude = 5f; // 진동 강도
        float elapsedTime = 0f;

        Vector3 originalPosition = mainCamera.transform.localPosition;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // 진동 효과
            mainCamera.transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;

            yield return null;
        }

        // 카메라 원래 위치로 복구
        mainCamera.transform.localPosition = originalPosition;
    }

    public void StartDialog()
    {
        IsStarting = true;
        cinematicPosition = calculatePosition();
        PlayerInteract.Instance.InteractMessage.SetActive(false);
        DialogEnumerator = ((JArray)DialogFile[CurrentScene++]["List"]).GetEnumerator();
        DialogEnumerator.MoveNext();
        Dialog.text = DialogEnumerator.Current.ToString();
        StartCoroutine(PlayCinematicEffect());
    }

    public void EndDialog()
    {
        IsStarting = false;
        StartCoroutine(PlayCinematicEffect());
    }

    private IEnumerator PlayCinematicEffect()
    {
        if (!IsStarting) DialogBox.SetActive(false);
        float elapsedTime = 0f;
        float startSize = IsStarting ? originalSize : cinematicZoom;
        float targetSize = IsStarting ? cinematicZoom : originalSize;

        Vector3 startPosition = IsStarting ? originalPosition : cinematicPosition;
        Vector3 targetPosition = IsStarting ? cinematicPosition : originalPosition;

        var barMoving = 200f;

        Vector2 topStartPos = IsStarting ? topBarOriginalPos : new Vector2(0, Screen.height - barMoving);
        Vector2 topTargetPos = IsStarting ? new Vector2(0, Screen.height - barMoving) : topBarOriginalPos;

        Vector2 bottomStartPos = IsStarting ? bottomBarOriginalPos : new Vector2(0, -Screen.height + barMoving);
        Vector2 bottomTargetPos = IsStarting ? new Vector2(0, -Screen.height + barMoving) : bottomBarOriginalPos;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            // 카메라 OrthographicSize 조정
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, curveValue);

            // 카메라 위치 조정
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);

            // 검은 막대 위치 조정
            topBar.rectTransform.anchoredPosition = Vector2.Lerp(topStartPos, topTargetPos, curveValue);
            bottomBar.rectTransform.anchoredPosition = Vector2.Lerp(bottomStartPos, bottomTargetPos, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 정확한 위치 설정
        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPosition;
        topBar.rectTransform.anchoredPosition = topTargetPos;
        bottomBar.rectTransform.anchoredPosition = bottomTargetPos;

        if (IsStarting) DialogBox.SetActive(true);
    }

    private Vector3 calculatePosition()
    {
        var ret = player.transform.localPosition;
        ret.x = Mathf.Clamp(ret.x, -160, 160);
        ret.y = Mathf.Clamp(ret.y, -90, 90);
        ret.z = originalPosition.z;
        return ret;
    }

    private void Update()
    {
        if (!IsStarting) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DialogEnumerator.MoveNext())
                Dialog.text = DialogEnumerator.Current.ToString();
            else
            {
                IsStarting = false;
                DialogBox.SetActive(false);
                EndDialog();
            }
        }
    }


}
