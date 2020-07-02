using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Final : MonoBehaviour
{
    GameObject follow;    //커서
    Vector3 pos;
    private float timer1;
    private float timer2;

    public string SceneToLoad;


    // Start is called before the first frame update
    void Start()
    {
        follow = GameObject.Find("follow").gameObject;
        pos = follow.transform.position;

        timer1 = 0;
        timer2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        pos = follow.transform.position;
       //Debug.Log(pos);


        if (pos.x >= 290 && pos.x <= 725 && pos.y >= 265 && pos.y <= 315 && pos.z == 200)
        {
            timer1 += Time.deltaTime;

            if (timer1 > 2.5)
            {
                //Debug.Log("Clicked");
                SceneManager.LoadScene("Start");
            }
        }



        if (pos.x >= 290 && pos.x <= 725 && pos.y >= 95 && pos.y <= 158 && pos.z == 200)
        {
            timer2 += Time.deltaTime;

            if (timer2 > 2.5)
            {
               // Debug.Log("Clicked");
                SceneManager.LoadScene("Intro");
            }

        }
    }
}
