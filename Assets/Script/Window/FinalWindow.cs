using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FinalWindow : WindowComponent
{
    public override string ComponentName => "FinalWindow";
    public static FinalWindow Instance { get; private set; }
    public GameObject back;

    public GameObject WarningPrefab;

    private bool firstTry = true;
    private bool stopSpawning = false;
    private int maxWarnings = 30; // 최대 20개 생성
    private int currentWarnings = 0;

    private List<GameObject> warningObjects = new();
    private Coroutine reactivationCoroutine;

    public GameObject front;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        back.SetActive(false);
    }
    public override void Interact()
    {
    }
    public void StartSpawning()
    {
        stopSpawning = false;
        currentWarnings = 0;
        StartCoroutine(SpawnWarnings());
    }

    private IEnumerator SpawnWarnings()
    {
        float delay = 1f;

        while (!stopSpawning && currentWarnings < maxWarnings)
        {
            CreateWarningWindow();
            currentWarnings++;

            // 시간 간격 조정
            if (currentWarnings <= 10)
            {
                delay = 0.5f; // 첫 10초는 1초 단위
            }
            else
            {
                delay = 1f; // 다음 10초는 2초 단위
            }

            yield return new WaitForSeconds(delay);
        }

        if (reactivationCoroutine == null)
        {
            reactivationCoroutine = StartCoroutine(ReactivateWarnings());
        }
    }

    public void CreateWarningWindow()
    {
        if (WarningPrefab != null && canvas != null)
        {
            // 프리팹 생성
            GameObject warningInstance = Instantiate(WarningPrefab, Vector3.zero, Quaternion.identity, canvas.transform);

            // 랜덤 좌표 계산 (1920 x 1080 영역 내)
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransform warningRect = warningInstance.GetComponent<RectTransform>();

            float randomX = Random.Range(200, 1400);
            float randomY = Random.Range(100, 780);
            if (firstTry)
            {
                randomX = 955; randomY = 515;
                firstTry = false;
            }

            // 위치 설정
            warningRect.anchoredPosition = new Vector2(randomX, randomY);

            // 랜덤 메시지 설정
            WarningWindow warningWindow = warningInstance.GetComponent<WarningWindow>();
            if (warningWindow != null)
            {
                warningWindow.SetRandomMessage();
            }

            Canvas warningCanvas = warningInstance.GetComponent<Canvas>();
            WindowManager.Instance.OpenWindow(warningCanvas);

            // 생성된 객체를 리스트에 추가
            warningObjects.Add(warningInstance);

            // 초기 상태를 비활성화
            warningInstance.SetActive(true);
        }
    }

    private IEnumerator ReactivateWarnings()
    {
        while (!stopSpawning)
        {
            // 비활성화된 객체 필터링
            var inactiveWarnings = warningObjects.Where(w => w != null && !w.activeSelf).ToList();

            if (inactiveWarnings.Count > 0)
            {
                // 랜덤으로 하나 선택
                GameObject warningToActivate = inactiveWarnings[Random.Range(0, inactiveWarnings.Count)];
                warningToActivate.SetActive(true); // 객체 활성화
            }
            else
            {
                // 모든 객체가 활성화되었으면 코루틴 종료
                break;
            }

            yield return new WaitForSeconds(5f); // 활성화 주기
        }

        reactivationCoroutine = null; // 코루틴 참조 초기화
    }

    public void Done()
    {
        stopSpawning = true;

        // 코루틴 중단
        if (reactivationCoroutine != null)
        {
            StopCoroutine(reactivationCoroutine);
            reactivationCoroutine = null;
        }
        foreach (var warning in warningObjects)
        {
            warning.SetActive(false);
        }
        ScenarioManager.Instance.StopKey = true;
        if (front != null)
        {
            StartCoroutine(FadeInAndChangeScene(front, 2f));
        }
    }

    private IEnumerator FadeInAndChangeScene(GameObject target, float duration)
    {
        // CanvasGroup을 통해 투명도 조정
        Image targetImage = target.GetComponent<Image>();

        Color color = targetImage.color;
        color.a = 0f; // 투명도 0으로 설정
        targetImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // 알파 값 점진적으로 증가
            color.a = Mathf.Clamp01(elapsedTime / duration);
            targetImage.color = color;
            yield return null;
        }

        // 최종적으로 완전히 표시
        color.a = 1f;
        targetImage.color = color;

        // 씬 전환
        SceneManager.LoadScene("AfterGame");
    }

}
