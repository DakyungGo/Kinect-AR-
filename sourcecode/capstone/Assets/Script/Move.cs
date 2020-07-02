using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
public class Move : MonoBehaviour
{
    //    [DllImport("NtKinectDll")]
    //    private static extern System.IntPtr getKinect();
    //    [DllImport("NtKinectDll")]
    //    private static extern int setSkeleton(System.IntPtr kinect, System.IntPtr data, System.IntPtr state, System.IntPtr id);

    int bodyCount = 6;
    int jointCount = 25;

    //public Material BoneMaterial;
    public GameObject BodySourceManager;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    public GameObject Trainnner;
    public GameObject Trainnne;
    CharacterSkeleton skeleton_Trainnner;
    CharacterSkeleton skeleton_Trainnne;
    public bool mirror = true;
    public bool move = true;
    private System.IntPtr kinect;

    //trainer
    public string[] lines;
    public string[] lines_right;
    public int trainer_i = -1;
    public int trainer_j = 0;
    public float[] trainer_data1;
    public int[] trainer_state;
    public int speed = 0;
    public int trainer_count = 0;
    public bool trainer_check = true;
    public bool trainer_check_outside = true;
    public float trainer_timer = 0;
    public bool trainer_other_side = false;
    public int trainer_speed = 0;
    //trainer ==

    public Text AngleLeftKnee;
    public Text AngleRightKnee;
    public Text KneeToeLeft;
    public Text KneeToeRight;
    public Text LeftLegUp;
    public Text RightLegUp;

    public Text IfSpineIsStraight;
    public Text HipBalance;
    public Text PullHipBack;
    public Text Great;

    public Text Trainner_count, Trainee_count;
    string trainercnt;
    public static string traineecnt;


    string hipbal, leftkneewarning, rightkneewarning, leftsidehighkick, rightsidehighkick;
    string spinestraight, pullhipback, great;

    string exercise;
    int workout_flag, workout_count, workout_flag2;
    bool workout;

    int sidehk_flag, lunge_flag_l, lunge_flag_r;

    private float timer;
    private float flag_timer;
    int flag;

    public GameObject count3;
    public GameObject count2;
    public GameObject count1;
    public GameObject HipBalance1, PullHipBack2, LegUp3, SpineStraight4, Great5, KneeDown6;
    public GameObject ex_start;
    public GameObject Squat_Instruction, SideHighKick_Instruction, Lunge_Left_Instruction, Lunge_Right_Instruction;
    public GameObject Right_Start;
    public GameObject Squat_Title, SideHighKick_Title, Lunge_Title;

    int tot;
    string total;

    //lsy
    void Trainer_Run()
    {
        if (!trainer_check)//운동 끝났을 때
        {
            trainer_timer += Time.deltaTime;
            if (trainer_timer > 3)
            {
                //Debug.Log("3초 됨 운동 끝!!!!!"); //결과씬이나 다른씬으로
                trainer_check_outside = false;

                Squat_Title.SetActive(false);
                SideHighKick_Title.SetActive(false);
                Lunge_Title.SetActive(false);


                SceneManager.LoadScene("Result"); // Result씬으로 이동
               
            }
            return;
        }
        //Thread.Sleep(50);
        if (trainer_j >= lines.Length - 3)
        {
            if (trainer_count < 20 && exercise == "Squat")
            {
                trainer_count++;
                //Debug.Log("카운트 : " + trainer_count);
                trainercnt = trainer_count + "/20";
                Trainner_count.text = trainercnt;
            }
            if (trainer_count < 10 && exercise == "SideHiKick")
            {
                trainer_count++;
               // Debug.Log("카운트 : " + trainer_count);
                trainercnt = trainer_count + "/10";
                Trainner_count.text = trainercnt;
            }
            if (trainer_count < 10 && exercise == "Lunge")
            {
                trainer_count++;
               // Debug.Log("카운트 : " + trainer_count);
                trainercnt = trainer_count + "/10";
                Trainner_count.text = trainercnt;
            }
            if (trainer_count >= 20 && exercise == "Squat")
            {
                trainer_check = false;//스쿼트일때 운동 끝나는 시점!!!!!
            }
            if (trainer_count >= 10 && exercise == "SideHiKick")
            {
                if (!trainer_other_side)
                {
                    if (trainer_timer > 3)
                    {
                        Right_Start.SetActive(false);

                        trainer_count = 0;
                        lines = lines_right;
                        trainer_other_side = true;
                        trainer_timer = 0;
                    }
                    else
                    {
                        Right_Start.SetActive(true);

                        trainer_timer += Time.deltaTime;
                        return;
                    }
                }
                else
                {
                    trainer_check = false;//사이드하이킥일때 운동 끝나는 시점!!!!!
                }
            }
            if (trainer_count >= 10 && exercise == "Lunge")
            {
                if (!trainer_other_side)
                {
                    if (trainer_timer > 3)
                    {
                        Lunge_Right_Instruction.SetActive(false);

                        trainer_speed = 10;
                        trainer_count = 0;
                        lines = lines_right;
                        trainer_other_side = true;
                        trainer_timer = 0;
                    }
                    else
                    {
                        Lunge_Right_Instruction.SetActive(true);

                        trainer_timer += Time.deltaTime;
                        return;
                    }
                }
                else
                {
                    trainer_check = false;//런지일때 운동 끝나는 시점!!!!!
                }
            }
            trainer_j = 0;
            return;
        }
        trainer_i = -1;
        trainer_data1 = new float[bodyCount * jointCount * 3];
        trainer_state = new int[bodyCount * jointCount];
        for (int k = 0; k < 25; k++)
        {
            trainer_i++;
            trainer_data1[trainer_i] = float.Parse(lines[trainer_j]);
            trainer_i++;
            trainer_data1[trainer_i] = float.Parse(lines[trainer_j + 1]);
            trainer_i++;
            trainer_data1[trainer_i] = float.Parse(lines[trainer_j + 2]);
            if ((float.Parse(lines[trainer_j]) + float.Parse(lines[trainer_j + 1]) + float.Parse(lines[trainer_j + 2])) != 0)
            {
                trainer_state[trainer_i - 2] = 1;
            }
            skeleton_Trainnner.trainer_set(trainer_data1, trainer_state, 0, true, true);
            trainer_j = trainer_j + 3;
        }
        if (speed < trainer_speed)
        {
            trainer_j -= 75;
            speed++;
        }
        else
        {
            //    Debug.Log("다음");
            //   Debug.Log(trainer_j);
            speed = 0;

        }
    }
    //lsy end

    void Trainee_Count()
    {

        if (exercise == "Squat")
        {

            workout_flag = 0;
            workout_flag2 = 0;
            total = "20";
            //tot = 20;
        }
        else if (exercise == "SideHiKick")
        {
            workout_flag = 0;
            tot = 10;
            total = "20";

        }
        else if (exercise == "Lunge")
        {
            workout_flag = 0;
            tot = 10;
            total = "20";
        }
        workout_count++;
        workout = false;
        //Debug.Log("트레이니: " + workout_count);
        traineecnt = workout_count + "/" + total;
        Trainee_count.text = traineecnt;
    }

    void Start()
    {
        timer = 0;
        Trainner_count.text = "";
        Trainee_count.text = "";
        total = "20";
        tot = 20;

        skeleton_Trainnner = new CharacterSkeleton(Trainnner);
        skeleton_Trainnne = new CharacterSkeleton(Trainnne);

        //lsy
        trainer_state = new int[bodyCount * jointCount];
        trainer_data1 = new float[bodyCount * jointCount * 3];
        //lsy end

        exercise = ClickExercise.selected_exercise; //ClickExercise에서 선택한 운동이 무엇인지 String으로 넘어옴.
   
       // Debug.Log(exercise);    //Squat, SideHiKick, Lunge 에 따라서 Trainer움직이고, 사용자에게 instruction주기 

        if (exercise == "Squat")
        {
            
            lines = File.ReadAllLines(@"Trainer_txt/squart.txt");
           // Debug.Log("읽음");
        }
        else if (exercise == "SideHiKick")
        {
            
            lines = File.ReadAllLines(@"Trainer_txt/leg_right.txt");
            lines_right = File.ReadAllLines(@"Trainer_txt/leg_left.txt");
            //Debug.Log("읽음");
        }
        else if (exercise == "Lunge")
        {          
            lines = File.ReadAllLines(@"Trainer_txt/runzi_right.txt");
            lines_right = File.ReadAllLines(@"Trainer_txt/runzi_left.txt"); 
           // Debug.Log("읽음");
        }
        else
        {
            lines = File.ReadAllLines(@"Trainer_txt/squart.txt");
            //Debug.Log("읽음");
        }

        workout_count = 0;
        workout_flag = 0;
        workout_flag2 = 0;
        workout = false;

        sidehk_flag = 0;
        lunge_flag_l = 0;
        lunge_flag_r = 0;
        flag = 0;
        flag_timer = 0;

        if (exercise == "Squat")
        {
            Squat_Instruction.SetActive(true);
        }
        else if (exercise == "SideHiKick")
        {
            SideHighKick_Instruction.SetActive(true);
        }
        else if (exercise == "Lunge")
        {
            Lunge_Left_Instruction.SetActive(true);
        }

        //HipBalance1.SetActive(false);
        //PullHipBack2.SetActive(false);
        //LegUp3.SetActive(false);
        //SpineStraight4.SetActive(false);
        //Great5.SetActive(false);
        //KneeDown6.SetActive(false);
    }

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    void Update()
    {

        timer += Time.deltaTime;
        //Debug.Log("Timer: " + timer);
        if (timer < 3)
        {
            if (exercise == "Squat")
            {
                Squat_Instruction.SetActive(true);
            }
            else if (exercise == "SideHiKick")
            {
                SideHighKick_Instruction.SetActive(true);
            }
            else if (exercise == "Lunge")
            {
                Lunge_Left_Instruction.SetActive(true);
            }
        }
        else if (timer < 4)
        {
            Squat_Instruction.SetActive(false);
            SideHighKick_Instruction.SetActive(false);
            Lunge_Left_Instruction.SetActive(false);

            count3.SetActive(true);
            count2.SetActive(false);
            count1.SetActive(false);
            ex_start.SetActive(true);

        }
        else if (timer < 5) {
            count3.SetActive(false);
            count2.SetActive(true);

        }
        else if (timer < 6)
        {
            count2.SetActive(false);
            count1.SetActive(true);
            ex_start.SetActive(true);

        }
        else
        {
            count1.SetActive(false);
            ex_start.SetActive(false);

            //lsy
            if (trainer_check_outside)
            {

                Trainer_Run();

            }
            //lsy end

            float[] data1 = new float[bodyCount * jointCount * 3];
            int[] state = new int[bodyCount * jointCount];
            int[] id = new int[bodyCount];
            GCHandle gch = GCHandle.Alloc(data1, GCHandleType.Pinned);
            GCHandle gch2 = GCHandle.Alloc(state, GCHandleType.Pinned);
            GCHandle gch3 = GCHandle.Alloc(id, GCHandleType.Pinned);

            if (BodySourceManager == null)
            {
                return;
            }

            _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
            if (_BodyManager == null)
            {
                return;
            }

            Kinect.Body[] data = _BodyManager.GetData();
            if (data == null)
            {
                return;
            }

            List<ulong> trackedIds = new List<ulong>();
            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {

                    trackedIds.Add(body.TrackingId);
                }
            }

            List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

            // First delete untracked bodies
            foreach (ulong trackingId in knownIds)
            {
                if (!trackedIds.Contains(trackingId))
                {
                    Destroy(_Bodies[trackingId]);
                    _Bodies.Remove(trackingId);
                }
            }

            foreach (var body in data)
            {
                if (body == null)
                {
                    continue;
                }

                if (body.IsTracked)
                {
                    if (!_Bodies.ContainsKey(body.TrackingId))
                    {

                        _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                    }
                    RefreshBodyObject(body, _Bodies[body.TrackingId]);
                }
            }
        }

    }

    private GameObject CreateBodyObject(ulong id)
    {

        GameObject body = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            //lr.SetVertexCount(2);
            //lr.material = BoneMaterial;
            //lr.SetWidth(0.05f, 0.05f);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        float[] data1 = new float[bodyCount * jointCount * 3];
        int[] state = new int[bodyCount * jointCount];
        int[] id1 = new int[bodyCount];
        GCHandle gch = GCHandle.Alloc(data1, GCHandleType.Pinned);
        GCHandle gch2 = GCHandle.Alloc(state, GCHandleType.Pinned);
        GCHandle gch3 = GCHandle.Alloc(id1, GCHandleType.Pinned);

        int i = -1;
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            //Debug.Log(i);

            i++;
            data1[i] = sourceJoint.Position.X;
            i++;
            data1[i] = sourceJoint.Position.Y;
            i++;
            data1[i] = sourceJoint.Position.Z;
            if ((sourceJoint.Position.X + sourceJoint.Position.Y + sourceJoint.Position.Z) != 0)
            {
                state[i - 2] = 1;
            }
            skeleton_Trainnne.trainee_set(data1, state, 0, true, true, body.Joints[jt], jointObj);

            /*
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                //Debug.Log(jointObj.localPosition);
                //Debug.Log(GetVector3FromJoint(targetJoint.Value));
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
            */
        }
        
        ///////////////////////////
        Angles MyAngles = new Angles();
        byte[] ReadAngles = MyAngles.GetVector(body);

        if (exercise == "Squat")
        {
            //Squat_Title
            Squat_Title.SetActive(true);

            if (ReadAngles[1] - ReadAngles[2] <= 5)
            {
                HipBalance1.SetActive(false);
            }
            else
            {
                //"양쪽 힘이 동일하지 않습니다. \n균형을 잡으세요!";
                HipBalance1.SetActive(true);
            }

            if (ReadAngles[5] > 95 || ReadAngles[6] > 95)
            {
                //"엉덩이를 뒤로 더 빼세요!";
                PullHipBack2.SetActive(true);
            }
            else
            {
                PullHipBack2.SetActive(false);
            }

            if (ReadAngles[3] < 95 && ReadAngles[4] < 95)
            {
                Great5.SetActive(true);
            }
            else
            {
                great = "";
                Great5.SetActive(false);
            }

            ////////////운동 횟수 카운트//////////////////////
            if (ReadAngles[3] < 95 && ReadAngles[4] < 95 && workout
                && workout_flag >= 95 && workout_flag2 >= 95)
            {
                Trainee_Count();
            }
            else
            {
                workout_flag = ReadAngles[3];
                workout_flag2 = ReadAngles[4];
                workout = true;
            }

        }
        else if (exercise == "SideHiKick")
        {
            //Title
            SideHighKick_Title.SetActive(true);

            if ((ReadAngles[7] < 45) && (ReadAngles[7] > 10)
                && (sidehk_flag < ReadAngles[7]))
            {
                LegUp3.SetActive(true); //다리를 더 높이 들어올리세요
                Great5.SetActive(false);

            }
            else
            {
                LegUp3.SetActive(false);
                Great5.SetActive(false);

                //이전각도
                sidehk_flag = ReadAngles[7];
            }

            if (ReadAngles[7] >= 45)
            {
                Great5.SetActive(true); //Great
            }
            else
            {
                Great5.SetActive(false);
            }

            ////////운동 횟수 카운트////////////////////
            if (ReadAngles[7] > 45 && workout && workout_flag <= 45)
            {
                Trainee_Count();
            }
            else
            {
                workout_flag = ReadAngles[7];
                workout = true;
            }



        }
        else if (exercise == "Lunge")
        {
            //Lunge_Title
            Lunge_Title.SetActive(true);

            if (!trainer_other_side)
            {
                if (ReadAngles[3] < 130 && ReadAngles[3] >= 91 && lunge_flag_l > ReadAngles[3])
                {
                    great = "";
                    Great5.SetActive(false);
                    KneeDown6.SetActive(true); //무릎을 더 굽혀주세요.  
                    

                }
                else if (ReadAngles[3] < 91)
                {
                    great = "GREAT!";
                    Great5.SetActive(true);
                    KneeDown6.SetActive(false);

                }
                else {
                    Great5.SetActive(false);
                    //KneeDown6.SetActive(false);
                }


                //////////////운동 횟수 카운트//////////////////
                //왼쪽
                if (ReadAngles[3] < 95 && workout && workout_flag >= 95)
                {
                    Trainee_Count();
                }
                else
                {
                    workout_flag = ReadAngles[3];
                    workout = true;
                }
            }
            else
            {
                if (ReadAngles[4] < 130 && ReadAngles[4] >= 91 && lunge_flag_r > ReadAngles[4])
                {
                    great = "";
                    Great5.SetActive(false);
                    KneeDown6.SetActive(true); //무릎을 더 굽혀주세요.  

                }
                else if (ReadAngles[4] < 91)
                {
                    great = "GREAT!";
                    Great5.SetActive(true);
                    KneeDown6.SetActive(false);

                }
                else {
                    Great5.SetActive(false);
                    //KneeDown6.SetActive(false);
                }
               

                //////////////운동 횟수 카운트//////////////////
                //오른쪽
                if (ReadAngles[4] < 95 && workout && workout_flag >= 95)
                {
                    Trainee_Count();
                }
                else
                {
                    workout_flag = ReadAngles[4];
                    workout = true;
                }
            }


            lunge_flag_l = ReadAngles[3];
            lunge_flag_r = ReadAngles[4];
        }

        //공통된 사항
        if (ReadAngles[0] < 170 || ReadAngles[0] > 190)
        {
            spinestraight = "허리를 곧게 펴세요!";
            SpineStraight4.SetActive(true);
        }
        else
        {
            spinestraight = "";
            SpineStraight4.SetActive(false);
        }
        
        ///////////////////////////
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    public class Angles
    {
        public double AngleBetweenTwoVectors(Vector3 Va, Vector3 Vb)
        {
            double dotProduct;

            Va.Normalize();
            Vb.Normalize();
            dotProduct = Vector3.Dot(Va, Vb);

            return (double)Math.Acos(dotProduct) * (180 / Math.PI);

        }

        public byte[] GetVector(Kinect.Body body)
        {
            Vector3 SpineShoulder = new Vector3(body.Joints[Kinect.JointType.SpineShoulder].Position.X, body.Joints[Kinect.JointType.SpineShoulder].Position.Y, body.Joints[Kinect.JointType.SpineShoulder].Position.Z);
            Vector3 RightShoulder = new Vector3(body.Joints[Kinect.JointType.ShoulderRight].Position.X, body.Joints[Kinect.JointType.ShoulderRight].Position.Y, body.Joints[Kinect.JointType.ShoulderRight].Position.Z);
            Vector3 LeftShoulder = new Vector3(body.Joints[Kinect.JointType.ShoulderLeft].Position.X, body.Joints[Kinect.JointType.ShoulderLeft].Position.Y, body.Joints[Kinect.JointType.ShoulderLeft].Position.Z);
            Vector3 RightElbow = new Vector3(body.Joints[Kinect.JointType.ElbowRight].Position.X, body.Joints[Kinect.JointType.ElbowRight].Position.Y, body.Joints[Kinect.JointType.ElbowRight].Position.Z);
            Vector3 LeftElbow = new Vector3(body.Joints[Kinect.JointType.ElbowLeft].Position.X, body.Joints[Kinect.JointType.ElbowLeft].Position.Y, body.Joints[Kinect.JointType.ElbowLeft].Position.Z);
            Vector3 RightWrist = new Vector3(body.Joints[Kinect.JointType.WristRight].Position.X, body.Joints[Kinect.JointType.WristRight].Position.Y, body.Joints[Kinect.JointType.WristRight].Position.Z);
            Vector3 LeftWrist = new Vector3(body.Joints[Kinect.JointType.WristLeft].Position.X, body.Joints[Kinect.JointType.WristLeft].Position.Y, body.Joints[Kinect.JointType.WristLeft].Position.Z);
            Vector3 UpVector = new Vector3((float)0.0, (float)1.0, (float)0.0);
            //////////////////////////////////////
            Vector3 SpineMid = new Vector3(body.Joints[Kinect.JointType.SpineMid].Position.X, body.Joints[Kinect.JointType.SpineMid].Position.Y, body.Joints[Kinect.JointType.SpineMid].Position.Z);
            Vector3 SpineBase = new Vector3(body.Joints[Kinect.JointType.SpineBase].Position.X, body.Joints[Kinect.JointType.SpineBase].Position.Y, body.Joints[Kinect.JointType.SpineBase].Position.Z);
            Vector3 HipLeft = new Vector3(body.Joints[Kinect.JointType.HipLeft].Position.X, body.Joints[Kinect.JointType.HipLeft].Position.Y, body.Joints[Kinect.JointType.HipLeft].Position.Z);
            Vector3 KneeLeft = new Vector3(body.Joints[Kinect.JointType.KneeLeft].Position.X, body.Joints[Kinect.JointType.KneeLeft].Position.Y, body.Joints[Kinect.JointType.KneeLeft].Position.Z);
            Vector3 AnkleLeft = new Vector3(body.Joints[Kinect.JointType.AnkleLeft].Position.X, body.Joints[Kinect.JointType.AnkleLeft].Position.Y, body.Joints[Kinect.JointType.AnkleLeft].Position.Z);
            Vector3 HipRight = new Vector3(body.Joints[Kinect.JointType.HipRight].Position.X, body.Joints[Kinect.JointType.HipRight].Position.Y, body.Joints[Kinect.JointType.HipRight].Position.Z);
            Vector3 KneeRight = new Vector3(body.Joints[Kinect.JointType.KneeRight].Position.X, body.Joints[Kinect.JointType.KneeRight].Position.Y, body.Joints[Kinect.JointType.KneeRight].Position.Z);
            Vector3 AnkleRight = new Vector3(body.Joints[Kinect.JointType.AnkleRight].Position.X, body.Joints[Kinect.JointType.AnkleRight].Position.Y, body.Joints[Kinect.JointType.AnkleRight].Position.Z);
            Vector3 FootRight = new Vector3(body.Joints[Kinect.JointType.FootRight].Position.X, body.Joints[Kinect.JointType.FootRight].Position.Y, body.Joints[Kinect.JointType.FootRight].Position.Z);
            Vector3 FootLeft = new Vector3(body.Joints[Kinect.JointType.FootLeft].Position.X, body.Joints[Kinect.JointType.FootLeft].Position.Y, body.Joints[Kinect.JointType.FootLeft].Position.Z);


            double StraightSpine = AngleBetweenTwoVectors(SpineShoulder - SpineMid, SpineBase - SpineMid); //스쿼트, 런지 - Joint 3개로도 가능

            double AngleLeftHip = AngleBetweenTwoVectors(SpineBase - SpineShoulder, HipLeft - KneeLeft); //스쿼트, 런지
            double AngleRightHip = AngleBetweenTwoVectors(SpineBase - SpineShoulder, HipRight - KneeRight);  //스쿼트, 런지
            //double AngleLeftKnee = AngleBetweenTwoVectors(KneeLeft - HipLeft, KneeLeft - AnkleLeft);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능
            double AngleLeftKnee = AngleBetweenTwoVectors(HipLeft - KneeLeft, AnkleLeft - KneeLeft);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능
            //double AngleRightKnee = AngleBetweenTwoVectors(KneeRight - HipRight, KneeRight - AnkleRight);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능
            double AngleRightKnee = AngleBetweenTwoVectors(HipRight - KneeRight, AnkleRight - KneeRight);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능

            double KneeToeLeft = AngleBetweenTwoVectors(AnkleLeft - FootLeft, KneeLeft - FootLeft); //스쿼트 - Joint 3개로도 가능
            double KneeToeRight = AngleBetweenTwoVectors(AnkleRight - FootRight, KneeRight - FootRight); //스쿼트 - Joint 3개로도 가능
            //double LeftLegUp = AngleBetweenTwoVectors(SpineMid - SpineShoulder, KneeLeft - HipLeft);    //사이드 하이킥
            //double RightLegUp = AngleBetweenTwoVectors(SpineMid - SpineShoulder, KneeRight - HipRight);    //사이드 하이킥
            double LeftLegUp = AngleBetweenTwoVectors(KneeRight - HipRight, KneeLeft - HipLeft);    //사이드 하이킥
            double RightLegUp = AngleBetweenTwoVectors(KneeLeft - HipLeft, KneeRight - HipRight);    //사이드 하이킥
            //LeftLegUp이랑 RightLegUp이랑 각도 동일


            byte[] Angles = { Convert.ToByte(StraightSpine),
                              Convert.ToByte(AngleLeftHip), Convert.ToByte(AngleRightHip),
                              Convert.ToByte(AngleLeftKnee), Convert.ToByte(AngleRightKnee),
                              Convert.ToByte(KneeToeLeft), Convert.ToByte(KneeToeRight),
                              Convert.ToByte(LeftLegUp), Convert.ToByte(RightLegUp)};

            return Angles;
        }
    }
}