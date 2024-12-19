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
    public float cinematicZoom = 510f; // ��ȭ ȿ�� Ȯ�� ���� (OrthographicSize ����)
    public float animationDuration = 1f; // �ִϸ��̼� ���� �ð�

    public bool StopKey = false;
    private bool DialogKey = false;

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

    // scene0 - falling

    // scene1 - getCursor
    public bool getCursor = false;

    // scene2 - open last message and photo
    public bool openLastMessage = false;
    public bool openLastPhoto = false;

    // scene3 - try open zip
    public bool tryOpenZip = false;

    // scene4 - see zipper
    public bool seeZipper = false;

    // scene5 - get zipper
    public bool getZipper = false;

    // scene6 - openPassword
    public bool openPassword = false;

    // scene7 - enterSecrets
    public bool enterSecrets = false;

    // scene8 - removeAds
    public bool removeAds = false;

    // scene9 - enterEscape
    public bool enterEscape = false;

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
        DialogBox.SetActive(false);
        //string json = File.ReadAllText(Path.Combine(Application.dataPath, "Dialog.json"));
        string json = Resources.Load<TextAsset>("Dialog").text;
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
        
        if (true)
        {
            StopKey = true;
            StartCoroutine(MovePlayerToCenter());
        }
        else
        {
            StopKey = false;
            PlayerInteract.Instance.HasCursor = true;
            PlayerInteract.Instance.HasZipper = true;
            PlayerInteract.Instance.HasHidden = true;
            FileSystemManager.Instance.ShowHidden = true;
            CurrentScene=9;
        }

    }

    private IEnumerator MovePlayerToCenter()
    {
        StopKey = true;
        // �÷��̾� ���� ��ġ�� ȭ�� �ٱ� �������� ����
        Vector3 startPosition = new(0, 800, player.transform.localPosition.z);
        Vector3 targetPosition = new(0, 0, player.transform.localPosition.z);
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
        StopKey = true;
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
        cinematicPosition = CalculatePosition();
        PlayerInteract.Instance.InteractMessage.SetActive(false);
        DialogEnumerator = ((JArray)DialogFile[CurrentScene]["List"]).GetEnumerator();
        DialogEnumerator.MoveNext();
        Dialog.text = DialogEnumerator.Current.ToString();
        StartCoroutine(StartDialogSequence());
    }
    private IEnumerator StartDialogSequence()
    {
        CurrentScene += 1;
        StopKey = true;
        yield return StartCoroutine(PlayCinematicEffect(true));
        DialogKey = true;
    }

    public void EndDialog()
    {
        StartCoroutine(EndDialogSequence());
    }

    private IEnumerator EndDialogSequence()
    {
        DialogKey = false;
        yield return StartCoroutine(PlayCinematicEffect(false));
        StopKey = false;
        
    }

    private IEnumerator PlayCinematicEffect(bool isStart)
    {
        StopKey = true;
        DialogKey = false;
        if (!isStart) DialogBox.SetActive(false);
        float elapsedTime = 0f;
        float startSize = isStart ? originalSize : cinematicZoom;
        float targetSize = isStart ? cinematicZoom : originalSize;

        Vector3 startPosition = isStart ? originalPosition : cinematicPosition;
        Vector3 targetPosition = isStart ? cinematicPosition : originalPosition;

        var barMoving = 200f;

        Vector2 topStartPos = isStart ? topBarOriginalPos : new Vector2(0, Screen.height - barMoving);
        Vector2 topTargetPos = isStart ? new Vector2(0, Screen.height - barMoving) : topBarOriginalPos;

        Vector2 bottomStartPos = isStart ? bottomBarOriginalPos : new Vector2(0, -Screen.height + barMoving);
        Vector2 bottomTargetPos = isStart ? new Vector2(0, -Screen.height + barMoving) : bottomBarOriginalPos;

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

        if (isStart) DialogBox.SetActive(true);

        if (!isStart) DialogBox.SetActive(false);
    }

    private Vector3 CalculatePosition()
    {
        var ret = player.transform.localPosition;
        ret.x = Mathf.Clamp(ret.x, -160, 160);
        ret.y = Mathf.Clamp(ret.y, -90, 90);
        ret.z = originalPosition.z;
        return ret;
    }
    private int didx = 0;

    private void Update()
    {
        switch (CurrentScene)
        {
            case 1:
                if (getCursor) StartDialog();
                break;
            case 2:
                if (openLastMessage && openLastPhoto) StartDialog();
                break;
            case 3:
                if (tryOpenZip) StartDialog();
                break;
            case 4:
                if (seeZipper) StartDialog();
                break;
            case 5:
                if (getZipper) StartDialog();
                break;
            case 6:
                if (openPassword) StartDialog();
                break;
            case 7:
                if (enterSecrets) StartDialog();
                break;
            case 8:
                if (removeAds) StartDialog();
                break;
            case 9:
                if (enterEscape) StartDialog();
                break;
            default:
                break;
        }

        if (!DialogKey) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DialogEnumerator.MoveNext())
            {
                Dialog.text = DialogEnumerator.Current.ToString();
                if (CurrentScene == 10)
                {
                    if (didx == 1)
                    {
                        FinalWindow.Instance.back.SetActive(true);
                    }
                    else if (didx == 2)
                    {
                        FinalWindow.Instance.CreateWarningWindow();
                    }
                }
                didx++;
            }
            else
            {
                DialogBox.SetActive(false);
                if (CurrentScene == 5) NodeIconRunner.Instance.setSpeed(500);
                EndDialog();
                if (CurrentScene == 10) FinalWindow.Instance.StartSpawning();
                didx = 0;
            }
        }
        

    }


}
