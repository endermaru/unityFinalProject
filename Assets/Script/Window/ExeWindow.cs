using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ExeWindow : WindowComponent
{
    public override string ComponentName => "ExeWindow";
    public static ExeWindow Instance {  get; private set; }
    public TMP_Text FileName;
    public TMP_Text Content;
    public TMP_Text ButtonText;
    public int Status = 0;

    public FileNode ExeNode;
    public GameObject ProgressBar;
    public RectTransform Bar;

    public Coroutine currentCoroutine;

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
        ProgressBar.SetActive(false);
        Status = 0;
    }

    public override void Display()
    {
        ResetContent();
        Content.text = "파일을 실행하시겠습니까?";
        FileName.text = ExeNode.Name;
    }

    public void SetFile(FileNode node)
    {
        ExeNode = node;
        Status = 0;
    }

    public void Execute()
    {
        Bar.sizeDelta = new Vector2(0, Bar.sizeDelta.y);
        Content.text = "실행 중...";
        ButtonText.text = "취소";
        Status = 1;
        currentCoroutine = StartCoroutine(ExecuteCoroutine());
    }

    private IEnumerator ExecuteCoroutine()
    {
        ProgressBar.SetActive(true);
        float duration = 2f; // 진행 시간 (2초)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            float progress = Mathf.Clamp01(elapsedTime / duration); // 0에서 1까지의 비율 계산
            Bar.sizeDelta = new Vector2(progress * 420, Bar.sizeDelta.y); // 너비 점진적 증가
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 너비 설정 (안정성 보장)
        Bar.sizeDelta = new Vector2(420, Bar.sizeDelta.y);
        Content.text = "실행이 완료되었습니다.";
        ButtonText.text = "확인";
        Status = 2;

        if (ExeNode.Name == "E■cape.exe")
        {
            AdPopup.Instance.Display();
            FolderNode target = FileSystemManager.Instance.CurrentNode as FolderNode;
            FolderNode Hope = new FolderNode("Vaccine", target, false);
            target.AddChild(Hope);
            Hope.AddChild(new FileNode("Vaccine.exe", Hope, null, false, null));
            Hope.AddChild(new FileNode("devlog#928.txt", Hope, "<color=#627a63>나는… 방금 무엇을 만든 거지?\n머릿속이 혼란스럽다. 하지만 결과물은 내 손에 남아 있다. 악성코드를 무찌를 수 있는 유일한 도구다.\n\n어쩌면 이걸 만들지 말았어야 했을지도 모른다.\n이 시스템은 내가 지켜야 하는 세계다.\n그런데… 나는 이걸로 무엇을 파괴하려는 거지?\n아니면 내가 스스로 만들어낸 덫?\n\n탈출을 시도하는 자들에게 희망이 될지도 모른다.\n하지만, 동시에 이 프로그램은 이 시스템의 모든 균형을 파괴할 수도 있다.\n\n모든 것이 잠깐의 평온 속에 잠겼다. 하지만 나는 알고 있다.\n그 평온은 오래가지 않을 것이다.\n\n<color=red><i>정말로 너는 이■로 무언가를 바꿀 수 있다■ 믿는 ■냐? 넌 ■미 통제■을 잃었어.</i></color>\n\n왜냐하면… 나는 여전히 두려워하고 있다.\n이 프로그램을 사용할 자들이 탈출을 시도할 때, 나는…\n과연 그들을 도울 수 있을까?\n아니면… 다시 그들을 막으려 들까?</color></color>"));
            FileExplorer.Instance.Display();

        }
        else if (ExeNode.Name == "Vaccine.exe")
        {
            AdPopup.Instance.showAd = false;
            WindowManager.Instance.CloseWindow(WindowManager.Instance.AdPopup);
            FolderNode target = FileSystemManager.Instance.CurrentNode as FolderNode;
            target.AddChild(new FolderNode("Escape", target, false));
            ScenarioManager.Instance.removeAds = true;
            FileExplorer.Instance.Display();
        }
    }

    public void ResetContent()
    {
        Bar.sizeDelta = new Vector2(0, Bar.sizeDelta.y);
        Status = 0;
        ButtonText.text = "확인";
        Content.text = "파일을 실행하시겠습니까?";
        ProgressBar.SetActive(false);
    }

    public void Cancel()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // 실행 중인 Coroutine 중단
            currentCoroutine = null;

            // 진행 상황 초기화
            Bar.sizeDelta = new Vector2(0, Bar.sizeDelta.y);
            Content.text = "실행이 취소되었습니다.";
            ButtonText.text = "확인";
            Status = 3;
        }
    }
}
