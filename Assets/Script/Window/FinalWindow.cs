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
    private int maxWarnings = 30; // �ִ� 20�� ����
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

            // �ð� ���� ����
            if (currentWarnings <= 10)
            {
                delay = 0.5f; // ù 10�ʴ� 1�� ����
            }
            else
            {
                delay = 1f; // ���� 10�ʴ� 2�� ����
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
            // ������ ����
            GameObject warningInstance = Instantiate(WarningPrefab, Vector3.zero, Quaternion.identity, canvas.transform);

            // ���� ��ǥ ��� (1920 x 1080 ���� ��)
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransform warningRect = warningInstance.GetComponent<RectTransform>();

            float randomX = Random.Range(200, 1400);
            float randomY = Random.Range(100, 780);
            if (firstTry)
            {
                randomX = 955; randomY = 515;
                firstTry = false;
            }

            // ��ġ ����
            warningRect.anchoredPosition = new Vector2(randomX, randomY);

            // ���� �޽��� ����
            WarningWindow warningWindow = warningInstance.GetComponent<WarningWindow>();
            if (warningWindow != null)
            {
                warningWindow.SetRandomMessage();
            }

            Canvas warningCanvas = warningInstance.GetComponent<Canvas>();
            WindowManager.Instance.OpenWindow(warningCanvas);

            // ������ ��ü�� ����Ʈ�� �߰�
            warningObjects.Add(warningInstance);

            // �ʱ� ���¸� ��Ȱ��ȭ
            warningInstance.SetActive(true);
        }
    }

    private IEnumerator ReactivateWarnings()
    {
        while (!stopSpawning)
        {
            // ��Ȱ��ȭ�� ��ü ���͸�
            var inactiveWarnings = warningObjects.Where(w => w != null && !w.activeSelf).ToList();

            if (inactiveWarnings.Count > 0)
            {
                // �������� �ϳ� ����
                GameObject warningToActivate = inactiveWarnings[Random.Range(0, inactiveWarnings.Count)];
                warningToActivate.SetActive(true); // ��ü Ȱ��ȭ
            }
            else
            {
                // ��� ��ü�� Ȱ��ȭ�Ǿ����� �ڷ�ƾ ����
                break;
            }

            yield return new WaitForSeconds(5f); // Ȱ��ȭ �ֱ�
        }

        reactivationCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
    }

    public void Done()
    {
        stopSpawning = true;

        // �ڷ�ƾ �ߴ�
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
        // CanvasGroup�� ���� ���� ����
        Image targetImage = target.GetComponent<Image>();

        Color color = targetImage.color;
        color.a = 0f; // ���� 0���� ����
        targetImage.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // ���� �� ���������� ����
            color.a = Mathf.Clamp01(elapsedTime / duration);
            targetImage.color = color;
            yield return null;
        }

        // ���������� ������ ǥ��
        color.a = 1f;
        targetImage.color = color;

        // �� ��ȯ
        SceneManager.LoadScene("AfterGame");
    }

}
