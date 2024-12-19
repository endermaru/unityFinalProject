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
        Content.text = "������ �����Ͻðڽ��ϱ�?";
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
        Content.text = "���� ��...";
        ButtonText.text = "���";
        Status = 1;
        currentCoroutine = StartCoroutine(ExecuteCoroutine());
    }

    private IEnumerator ExecuteCoroutine()
    {
        ProgressBar.SetActive(true);
        float duration = 2f; // ���� �ð� (2��)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            float progress = Mathf.Clamp01(elapsedTime / duration); // 0���� 1������ ���� ���
            Bar.sizeDelta = new Vector2(progress * 420, Bar.sizeDelta.y); // �ʺ� ������ ����
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� �ʺ� ���� (������ ����)
        Bar.sizeDelta = new Vector2(420, Bar.sizeDelta.y);
        Content.text = "������ �Ϸ�Ǿ����ϴ�.";
        ButtonText.text = "Ȯ��";
        Status = 2;

        if (ExeNode.Name == "E��cape.exe")
        {
            AdPopup.Instance.Display();
            FolderNode target = FileSystemManager.Instance.CurrentNode as FolderNode;
            FolderNode Hope = new FolderNode("Vaccine", target, false);
            target.AddChild(Hope);
            Hope.AddChild(new FileNode("Vaccine.exe", Hope, null, false, null));
            Hope.AddChild(new FileNode("devlog#928.txt", Hope, "<color=#627a63>���¡� ��� ������ ���� ����?\n�Ӹ����� ȥ��������. ������ ������� �� �տ� ���� �ִ�. �Ǽ��ڵ带 ��� �� �ִ� ������ ������.\n\n��¼�� �̰� ������ ���Ҿ�� �������� �𸥴�.\n�� �ý����� ���� ���Ѿ� �ϴ� �����.\n�׷����� ���� �̰ɷ� ������ �ı��Ϸ��� ����?\n�ƴϸ� ���� ������ ���� ��?\n\nŻ���� �õ��ϴ� �ڵ鿡�� ����� ������ �𸥴�.\n������, ���ÿ� �� ���α׷��� �� �ý����� ��� ������ �ı��� ���� �ִ�.\n\n��� ���� ����� ��� �ӿ� ����. ������ ���� �˰� �ִ�.\n�� ����� �������� ���� ���̴�.\n\n<color=red><i>������ �ʴ� �̡�� ���𰡸� �ٲ� �� �ִ١� �ϴ� ���? �� ��� �������� �Ҿ���.</i></color>\n\n�ֳ��ϸ顦 ���� ������ �η����ϰ� �ִ�.\n�� ���α׷��� ����� �ڵ��� Ż���� �õ��� ��, ���¡�\n���� �׵��� ���� �� ������?\n�ƴϸ顦 �ٽ� �׵��� ������ ���?</color></color>"));
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
        ButtonText.text = "Ȯ��";
        Content.text = "������ �����Ͻðڽ��ϱ�?";
        ProgressBar.SetActive(false);
    }

    public void Cancel()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // ���� ���� Coroutine �ߴ�
            currentCoroutine = null;

            // ���� ��Ȳ �ʱ�ȭ
            Bar.sizeDelta = new Vector2(0, Bar.sizeDelta.y);
            Content.text = "������ ��ҵǾ����ϴ�.";
            ButtonText.text = "Ȯ��";
            Status = 3;
        }
    }
}
