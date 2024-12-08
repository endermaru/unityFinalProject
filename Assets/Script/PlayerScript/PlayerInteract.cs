using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();
  
    public void Start()
    {
        collidingObjects.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<GameObject> fronts = WindowManager.Instance.FrontObjects(collidingObjects);
            Canvas frontCanvas = fronts[0].GetComponent<Canvas>();
            foreach (GameObject obj in fronts)
            {
                IComponent component = obj.GetComponent<IComponent>();
                if (component != null) 
                {
                    component.Interact(); 
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 시작 시 오브젝트 추가
        if (/*IsObjectVisible(other.gameObject) &&*/ 
                !collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Add(other.gameObject);
            //Debug.Log("in:" + other.gameObject.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 충돌 끝날 시 오브젝트 제거
        if (/*IsObjectVisible(other.gameObject) &&*/
                collidingObjects.Contains(other.gameObject))
        {
            collidingObjects.Remove(other.gameObject);
            //Debug.Log("out:" + other.gameObject.ToString());
        }
    }
}
