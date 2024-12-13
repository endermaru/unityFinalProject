using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using TMPro;
using Newtonsoft.Json.Linq;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    public Image topBar; // ���� ���� ����
    public Image bottomBar; // �Ʒ��� ���� ����
    public Camera mainCamera; // ���� ī�޶�
    public Transform player; // �÷��̾� Transform
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // �ִϸ��̼� �
    public float cinematicZoom = 500f; // ��ȭ ȿ�� Ȯ�� ���� (OrthographicSize ����)
    public float animationDuration = 1f; // �ִϸ��̼� ���� �ð�

    public bool IsStarting = false;

    public GameObject DialogBox;
    public TMP_Text Dialog;

    public int CurrentScene = 0;
    public JArray DialogFile;
    private IEnumerator DialogEnumerator;

    private float originalSize; // ī�޶��� ���� OrthographicSize ����
    private Vector3 originalPosition; // ī�޶��� ���� ��ġ ����
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
        // ī�޶� ���� OrthographicSize �� ��ġ ����
        originalSize = mainCamera.orthographicSize;
        originalPosition = mainCamera.transform.position;

        // UI ������Ʈ �ʱ� ��ġ ����
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
        // �÷��̾� ���� ��ġ�� ȭ�� �ٱ� �������� ����
        Vector3 startPosition = new Vector3(0, 800, player.transform.localPosition.z);
        Vector3 targetPosition = new Vector3(0, 0, player.transform.localPosition.z);
        player.transform.localPosition = startPosition;

        float duration = 1f; // �̵� �ð�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // �÷��̾� ��ġ�� ���� �������� �̵�
            player.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            yield return null;
        }

        // ��Ȯ�� �߾����� ��ġ ����
        player.transform.localPosition = targetPosition;
        yield return StartCoroutine(PlayerCrashAnimation(targetPosition));
        yield return new WaitForSeconds(1.5f);
        // StartDialog ȣ��
        StartDialog();
    }

    private IEnumerator PlayerCrashAnimation(Vector3 centerPosition)
    {
        float crashDuration = 0.5f; // �߶� �ִϸ��̼� ���� �ð�
        float elapsedTime = 0f;

        Vector3 startCrashPosition = centerPosition + new Vector3(0, 25, 0); // �ణ ������ ����
        Vector3 endCrashPosition = centerPosition; // �߽ɿ� ����

        while (elapsedTime < crashDuration)
        {
            elapsedTime += Time.deltaTime;

            // ������ �߶��ϴ� ����
            player.transform.localPosition = Vector3.Lerp(startCrashPosition, endCrashPosition, Mathf.SmoothStep(0, 1, elapsedTime / crashDuration));

            yield return null;
        }

        // ��Ȯ�� �߽� ��ġ ����
        player.transform.localPosition = endCrashPosition;

        // ��� ȿ�� (��: ����)
        StartCoroutine(ApplyScreenShake());
    }

    private IEnumerator ApplyScreenShake()
    {
        float shakeDuration = 0.2f; // ���� ���� �ð�
        float shakeMagnitude = 5f; // ���� ����
        float elapsedTime = 0f;

        Vector3 originalPosition = mainCamera.transform.localPosition;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // ���� ȿ��
            mainCamera.transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;

            yield return null;
        }

        // ī�޶� ���� ��ġ�� ����
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

            // ī�޶� OrthographicSize ����
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, curveValue);

            // ī�޶� ��ġ ����
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);

            // ���� ���� ��ġ ����
            topBar.rectTransform.anchoredPosition = Vector2.Lerp(topStartPos, topTargetPos, curveValue);
            bottomBar.rectTransform.anchoredPosition = Vector2.Lerp(bottomStartPos, bottomTargetPos, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ִϸ��̼� ���� �� ��Ȯ�� ��ġ ����
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
