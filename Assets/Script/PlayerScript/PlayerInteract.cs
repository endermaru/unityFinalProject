using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance { get; private set; }
    private List<GameObject> collidingObjects = new List<GameObject>();
    public List<GameObject> fronts;

    public GameObject InteractMessage;
    public TMP_Text Message;

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
        InteractMessage.SetActive(false);
    }

    public void Start()
    {
        collidingObjects.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            fronts = WindowManager.Instance.FrontObjects(collidingObjects);
            //Canvas frontCanvas = fronts[0].GetComponent<Canvas>();
            foreach (GameObject obj in fronts)
            {
                IComponent component = obj.GetComponent<IComponent>();
                component?.Interact();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 시작 시 오브젝트 추가
        if (!collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
            //Debug.Log("in:" + other.gameObject.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 충돌 끝날 시 오브젝트 제거
        if (collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
            //Debug.Log("out:" + other.gameObject.ToString());
        }
        
    }

    public bool IsInteractValid(GameObject obj)
    {
        List<GameObject> tmpList = collidingObjects.ToList(); ;
        tmpList.Add(obj);
        fronts = WindowManager.Instance.FrontObjects(tmpList);
        return fronts.Contains(obj);
    }

    public void ShowMessage(string message)
    {
        Message.text = message;
        InteractMessage.SetActive(true);
    }
    public void HideMessage()
    {
        InteractMessage.SetActive(false);
    }
}
