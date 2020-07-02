using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System.IO;
using System;


public class BodySourceView : MonoBehaviour
{
    public Material BoneMaterial;
    public Material xbot;
    public GameObject BodySourceManager;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    public Text IfSpineIsStraight;
    public Text HipBalance;   
    public Text AngleLeftKnee;
    public Text AngleRightKnee;
    public Text KneeToeLeft;
    public Text KneeToeRight;
    public Text LeftLegUp;
    public Text RightLegUp;
    public Text PullHipBak;
    public Text Great;

    string hipbal, leftkneewarning, rightkneewarning, leftsidehighkick, rightsidehighkick;
    string spinestraight, pullhipback, great;

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

        IfSpineIsStraight.text = "허리를 곧게: Tracking";
        HipBalance.text = "양쪽 힙 균형: Calculating";
        AngleLeftKnee.text = "왼쪽 무릎 각도: Tracking";
        AngleRightKnee.text = "오른쪽 무릎 각도: Tracking";
        KneeToeLeft.text = "왼쪽 무릎과 발끝: Tracking";
        KneeToeRight.text = "오른쪽 무릎과 발끝: Tracking";
        LeftLegUp.text = "왼쪽 다리 들어올린 각도: Tracking";
        RightLegUp.text = "오른쪽 다리 들어올린 각도: Tracking";


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

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            //////////////////
            //lr.material = xbot;

            /////////////////
            lr.SetWidth(0.05f, 0.05f);


            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    int num = 0;
    string str = "";

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {

        //StreamWriter sw = new StreamWriter(new FileStream("a.txt", FileMode.Create));
       // sw.WriteLine("Hello");
        //sw.Close();
       // Debug.Log("바디트래킹됨=================" + num++);
        //NewBehaviourScript.test();
       // str += "바디트래킹됨=================" + num++ + "\n";



        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {

            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            ///////////////////////////
            Angles MyAngles = new Angles();
            byte[] ReadAngles = MyAngles.GetVector(body);
            // ReadAngles[0].ToString();
            //Debug.Log("HipLeft " + ReadAngles[1].ToString());
            //Debug.Log("HipRight " + ReadAngles[2].ToString());
            //Debug.Log("KneeLeft " + ReadAngles[3].ToString());
            //Debug.Log("KneeRight " + ReadAngles[4].ToString());

                  
            if (ReadAngles[1] - ReadAngles[2] <= 5)
            {
                hipbal = "양쪽 균형이 잡혀있습니다.";
            }
            else {
                hipbal = "양쪽 힘이 동일하지 않습니다. 균형을 잡으세요";
            }

            if (ReadAngles[5] > 90)
            {
                leftkneewarning = "왼쪽 무릎이 발끝을 넘어갔습니다.";
            }
            else {
                leftkneewarning = "";
            }
            if (ReadAngles[6] > 90)
            {
                rightkneewarning = "오른쪽 무릎이 발끝을 넘어갔습니다.";
            }
            else
            {
                rightkneewarning = "";
            }

            if (ReadAngles[7] < 45)
            {
                leftsidehighkick = "왼쪽 다리를 좀 더 높이 들어올리세요";
            }
            else {
                leftsidehighkick = "";
            }

            if (ReadAngles[8] < 45)
            {
                rightsidehighkick = "오른쪽 다리를 좀 더 높이 들어올리세요";
            }
            else
            {
                rightsidehighkick = "";
            }


            IfSpineIsStraight.text = "허리를 곧게: " + ReadAngles[0].ToString();
            HipBalance.text = "양쪽 힙 균형: " + hipbal;
            AngleLeftKnee.text = "왼쪽 무릎 각도: " + ReadAngles[3].ToString();
            AngleRightKnee.text = "오른쪽 무릎 각도: " + ReadAngles[4].ToString();
            KneeToeLeft.text = "올바르지 않은 자세: " + leftkneewarning;
            KneeToeRight.text = "올바르지 않은 자세: " + rightkneewarning;
            LeftLegUp.text = "왼쪽 다리 운동중: " + leftsidehighkick;
            RightLegUp.text = "오른쪽 다리 운동중: " + rightsidehighkick;

            ///////////////////////////

            Debug.Log(body.Joints[Kinect.JointType.SpineBase].Position.X);
            if (body.Joints[jt].JointType.ToString() == "SpineBase")
            {
                Debug.Log(sourceJoint.Position.X);

                //str += sourceJoint.Position.X;
                //str += "\n";
                Debug.Log(sourceJoint.Position.Y);

                //str += sourceJoint.Position.Y;
                //str += "\n";
                Debug.Log(sourceJoint.Position.Z);
            }
            //str += body.Joints[jt].JointType;
            //str += "\n";


            //str += sourceJoint.Position.Z;
            //str += "\n";
            //Debug.Log("<<<<<<<<<<<<<<<<<");

           // str += "<<<<<<<<<<<<<<<<<<<\n";


            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
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

            return (double)Math.Acos(dotProduct) * (180/ Math.PI);

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

            /*
            double AngleRightElbow = AngleBetweenTwoVectors(RightElbow - RightShoulder, RightElbow - RightWrist);
            double AngleRightShoulder = AngleBetweenTwoVectors(UpVector, RightShoulder - RightElbow);
            double AngleLeftElbow = AngleBetweenTwoVectors(LeftElbow - LeftShoulder, LeftElbow - LeftWrist);
            double AngleLeftShoulder = AngleBetweenTwoVectors(UpVector, LeftShoulder - LeftElbow);
            */

            double StraightSpine = AngleBetweenTwoVectors(SpineShoulder - SpineMid, SpineBase - SpineMid); //스쿼트, 런지 - Joint 3개로도 가능

            double AngleLeftHip = AngleBetweenTwoVectors(SpineBase - SpineShoulder, HipLeft - KneeLeft); //스쿼트, 런지
            double AngleRightHip = AngleBetweenTwoVectors(SpineBase - SpineShoulder, HipRight - KneeRight);  //스쿼트, 런지
            double AngleLeftKnee = AngleBetweenTwoVectors(KneeLeft - HipLeft, KneeLeft - AnkleLeft);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능
            double AngleRightKnee = AngleBetweenTwoVectors(KneeRight - HipRight, KneeRight - AnkleRight);    //스쿼트, 런지, 사이드하이킥 - Joint 3개로도 가능

            double KneeToeLeft = AngleBetweenTwoVectors(AnkleLeft - FootLeft, KneeLeft - FootLeft); //스쿼트 - Joint 3개로도 가능
            double KneeToeRight = AngleBetweenTwoVectors(AnkleRight - FootRight, KneeRight - FootRight); //스쿼트 - Joint 3개로도 가능
            double LeftLegUp = AngleBetweenTwoVectors(SpineMid - SpineShoulder, KneeLeft - HipLeft);    //사이드 하이킥
            double RightLegUp = AngleBetweenTwoVectors(SpineMid - SpineShoulder, KneeRight - HipRight);    //사이드 하이킥


            byte[] Angles = { Convert.ToByte(StraightSpine), 
                              Convert.ToByte(AngleLeftHip), Convert.ToByte(AngleRightHip), 
                              Convert.ToByte(AngleLeftKnee), Convert.ToByte(AngleRightKnee),
                              Convert.ToByte(KneeToeLeft), Convert.ToByte(KneeToeRight),
                              Convert.ToByte(LeftLegUp), Convert.ToByte(RightLegUp)};

            return Angles;
        }
    }


}
