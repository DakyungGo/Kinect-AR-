using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickExercise : MonoBehaviour
{
    RawImage btnImage;
    RawImage btnImage2;
    GameObject btnParent; //버튼들의 부모

    GameObject follow;    //커서
    Vector3 pos;
    private float timer;

    public static string selected_exercise;

    private void Start()
    {
        btnParent = GameObject.Find("BtnGroup").gameObject; // "부모의 이름"으로 찾습니다.

        follow = GameObject.Find("follow").gameObject;
        pos = follow.transform.position;
        follow.transform.position = new Vector3(400, 0, 200);
        //Debug.Log(pos);

        selected_exercise = "Squat";    //default로 스쿼트가 선택되어 있으므로
    }

    /*
    public void GetBtn()
    {
        Debug.Log("GetBtn");

        GameObject tempBtn = EventSystem.current.currentSelectedGameObject;
        btnImage = tempBtn.GetComponent<RawImage>(); // 해당 오브젝트의 image 컴포넌트를 받음

        int btnLength = btnParent.transform.childCount; // 자식의 갯수를 파악
        Debug.Log(btnLength);
        for (int i = 0; i < btnLength; i++) // 자식의 갯수 만큼 i++ 실행
        {
            //transfrom.Getchild(i)를 통해 버튼을 하나씩 색인
            GameObject tempBtns = btnParent.transform.GetChild(i).gameObject; // 해당 버튼 이미지 컴포넌트를 불러온 후 변경
            btnImage2 = tempBtns.GetComponent<RawImage>();
            if (tempBtns.name == "Squat")
            {
                btnImage2.texture = Resources.Load("Squat_off", typeof(Texture2D)) as Texture2D;
            }
            else if (tempBtns.name == "Lunge")
            {
                btnImage2.texture = Resources.Load("Lunge_off", typeof(Texture2D)) as Texture2D;
            }
            else if (tempBtns.name == "SideHiKick")
            {
                btnImage2.texture = Resources.Load("SideHiKick_off", typeof(Texture2D)) as Texture2D;
            }
            //Debug.Log(tempBtns);
        }

        if (btnImage.name == "Squat")
        {
            Debug.Log("Squat");
            selected_exercise = "Squat";
            btnImage.texture = Resources.Load("Squat_on", typeof(Texture2D)) as Texture2D;
        }
        else if (btnImage.name == "Lunge")
        {
            Debug.Log("Lunge");
            selected_exercise = "Lunge";
            btnImage.texture = Resources.Load("Lunge_on", typeof(Texture2D)) as Texture2D;
        }
        else if (btnImage.name == "SideHiKick")
        {
            Debug.Log("SideHighKick");
            selected_exercise = "SideHighKick";
            btnImage.texture = Resources.Load("SideHiKick_on", typeof(Texture2D)) as Texture2D;
        }


        //Squat x:60~285 , y:355~640 , z:300
        //SideHighKick x:400 ~ 625, y:355~640, z:300
        //Lunge x:735~965 , y:355~640 , z:300        
            
    }
    */

    void Update()
    {
        
        pos = follow.transform.position;
       // Debug.Log(pos);


        if (pos.x >= 60 && pos.x <= 285 && pos.y >= 335 && pos.y <= 640 && pos.z == 200) {
            //스쿼트
            timer += Time.deltaTime;
            //Debug.Log("Timer: " + timer);

            if (timer > 2.5)
            {
                //Debug.Log("Clicked");
                //////
                selected_exercise = "Squat";    //Squat 넘겨줌
                GameObject tempBtn = btnParent.transform.GetChild(3).gameObject;
                btnImage = tempBtn.GetComponent<RawImage>(); // 해당 오브젝트의 image 컴포넌트를 받음

                if (btnImage.name == "Squat")
                {
                    //Debug.Log("Squat");
                    btnImage.texture = Resources.Load("Squat_on", typeof(Texture2D)) as Texture2D;
                }

                GameObject sidehk = btnParent.transform.GetChild(2).gameObject;
                btnImage2 = sidehk.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("SideHiKick_off", typeof(Texture2D)) as Texture2D;

                GameObject lunge = btnParent.transform.GetChild(1).gameObject;
                btnImage2 = lunge.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("Lunge_off", typeof(Texture2D)) as Texture2D;
            }
            
        }
        else if(pos.x >= 400 && pos.x <= 625 && pos.y >= 335 && pos.y <= 640 && pos.z == 200){
            //사이드하이킥    
            timer += Time.deltaTime;
           // Debug.Log("Timer: " + timer);

            if (timer > 2.5)
            {
               // Debug.Log("Clicked");
                /////
                selected_exercise = "SideHiKick";   //SideHiKick 넘겨줌
                GameObject tempBtn = btnParent.transform.GetChild(2).gameObject;
                btnImage = tempBtn.GetComponent<RawImage>(); // 해당 오브젝트의 image 컴포넌트를 받음

                if (btnImage.name == "SideHiKick")
                {
                  //  Debug.Log("SideHiKick");
                    btnImage.texture = Resources.Load("SideHiKick_on", typeof(Texture2D)) as Texture2D;
                }

                GameObject squat = btnParent.transform.GetChild(3).gameObject;
                btnImage2 = squat.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("Squat_off", typeof(Texture2D)) as Texture2D;

                GameObject lunge = btnParent.transform.GetChild(1).gameObject;
                btnImage2 = lunge.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("Lunge_off", typeof(Texture2D)) as Texture2D;
               
            }
        }
        else if (pos.x >= 735 && pos.x <= 965 && pos.y >= 335 && pos.y <= 640 && pos.z == 200)
        {
            //런지
            timer += Time.deltaTime;
           //Debug.Log("Timer: " + timer);

            if (timer > 2.5)
            {
               // Debug.Log("Clicked");
                //////
                selected_exercise = "Lunge";    //Lunge 넘겨줌

                GameObject tempBtn = btnParent.transform.GetChild(1).gameObject;
                btnImage = tempBtn.GetComponent<RawImage>(); // 해당 오브젝트의 image 컴포넌트를 받음

                if (btnImage.name == "Lunge")
                {
                   // Debug.Log("Lunge");
                    btnImage.texture = Resources.Load("Lunge_on", typeof(Texture2D)) as Texture2D;
                }

                GameObject squat = btnParent.transform.GetChild(3).gameObject;
                btnImage2 = squat.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("Squat_off", typeof(Texture2D)) as Texture2D;

                GameObject sidehk = btnParent.transform.GetChild(2).gameObject;
                btnImage2 = sidehk.GetComponent<RawImage>();
                btnImage2.texture = Resources.Load("SideHiKick_off", typeof(Texture2D)) as Texture2D;
                           
            }        
        }
        
        
    }
}
