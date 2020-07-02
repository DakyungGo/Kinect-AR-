using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    private float timer;

    string num;
    public Text NumOfGreat;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        num = Move.traineecnt;
        //Debug.Log(num);
    }

    // Update is called once per frame
    void Update()
    {
        NumOfGreat.text = num;

        timer += Time.deltaTime;
        if (timer > 5)
        {
            SceneManager.LoadScene("Final");

        }
    }
}
