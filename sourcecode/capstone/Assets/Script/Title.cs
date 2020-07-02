using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string SceneToLoad;

    GameObject follow;    //커서
    Vector3 pos;
    private float timer;

    private void Start() 
    {
       // Debug.Log("Start");
        follow = GameObject.Find("follow").gameObject;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    void Update()
    {
       // Debug.Log("Update");

        pos = follow.transform.position;
        //Debug.Log(pos);
        

        if (pos.x >= 360 && pos.x <= 600 && pos.y >= 180 && pos.y <= 200 &&pos.z == 200) {
            timer += Time.deltaTime;
           // Debug.Log("Timer: "+ timer);

            if (timer > 2.5) {
               // Debug.Log("Clicked");
                SceneManager.LoadScene(SceneToLoad);
            }
            
        }

        /*
        Debug.Log("1");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("2");
            SceneManager.LoadScene(SceneToLoad);
        }
        */
    }
}
