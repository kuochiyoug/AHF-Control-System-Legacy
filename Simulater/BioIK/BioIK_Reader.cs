using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;
using BioIK;


public class BioIK_Reader : MonoBehaviour
{

    [SerializeField]private BioIK.BioIK bioik;
    public MotionRecorder Logger;

    [Tooltip("The Frame Information file")]
    public string FrameXMLSettingFile;

    [Tooltip("Drag IK Target that you want to record")]
    public GameObject[] IKTarget_To_Record;

    [Tooltip("Reference Origin, Null will be World Origin")]
    public GameObject IKTarget_Origin;
    protected Vector3[] RelativePosition;
    protected Quaternion[] RelativeRot;


    private List<string> BioIKMotorNameList = new List<string>();
    private List<string> RotateAxisList = new List<string>();
    private List<string> DataDescription = new List<string>();
    private double[] WriteDataArray;
    //public double[] AngleArray;
    //public double[] VelocityArray;
    //public double[] AccelArray;
    public int Target_Read_FPS;
    private int DataLength;

    private bool Logger_trigger = false;


    private Vector3 GetRelativePosition(GameObject obj)
    {
        if (IKTarget_Origin == null)
        {
            
            return obj.transform.position;
        }
        else
        {
            return IKTarget_Origin.transform.InverseTransformPoint(obj.transform.position);
        }
    }

    private Quaternion GetRelativeRot(GameObject obj)
    {
        if (IKTarget_Origin == null)
        {
            return obj.transform.rotation;
        }
        else
        {
            return Quaternion.Inverse(IKTarget_Origin.transform.rotation) * obj.transform.rotation;
        }
    }

    private void SetupBioIKJoints(string FrameXMLFile)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(FrameXMLFile);
        Debug.Log("Read XML File Success!");
        XmlNodeList JointNodeList = xmlDoc.DocumentElement.SelectNodes("/AHF/JointList/Joint");
        
        foreach (XmlNode node in JointNodeList)
        {
            BioIKMotorNameList.Add(node.SelectSingleNode("BioIK/Name").InnerText);
            RotateAxisList.Add(node.SelectSingleNode("BioIK/RotateAxis").InnerText);
        }
        DataDescription.Add("POSITION");
        DataDescription.Add("VELOCITY");
        DataDescription.Add("ACCELERATION");
        //AngleArray = new double[BioIKMotorNameList.Count];
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject robot_ik_model = GameObject.Find("Root");
        if (robot_ik_model)
        {
            bioik = robot_ik_model.GetComponent<BioIK.BioIK>();
        }
        else
        {
            bioik = GetComponent<BioIK.BioIK>();
        }
        Application.targetFrameRate = Target_Read_FPS;
        if (Logger != null)
        {
            Logger.CreateLogFile();
            Logger_trigger = true;
        }
        SetupBioIKJoints(FrameXMLSettingFile);
        if (IKTarget_To_Record.Length != 0)
        {
            RelativePosition = new Vector3[IKTarget_To_Record.Length];
            RelativeRot = new Quaternion[IKTarget_To_Record.Length];
            foreach (GameObject IKTargetGO in IKTarget_To_Record)
            {
                DataDescription.Add("Target_" + IKTargetGO.name +"_Transform");
            }
        }
        DataLength = BioIKMotorNameList.Count * 3 + IKTarget_To_Record.Length * 7;
        WriteDataArray = new double[DataLength];

        if (Logger_trigger)
        {
            double FrameRate = (1.0 / (double)Target_Read_FPS);
            Logger.WriteDataDescription(BioIKMotorNameList.ToArray(), DataDescription.ToArray(), FrameRate, "BIOIK");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int DataCount = 0;
        for (int i = 0; i< BioIKMotorNameList.Count ; i++)
        {
            //Debug.Log(BioIKMotorNameList[i] + " " + RotateAxisList[i]);
            if (String.Equals(RotateAxisList[i], "X"))// StringComparison.OrdinalIgnoreCase))
            {
                WriteDataArray[DataCount] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.X.CurrentValue;
                WriteDataArray[DataCount + 1] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.X.CurrentVelocity;
                WriteDataArray[DataCount + 2] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.X.CurrentAcceleration;
                //AngleArray[i] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.X.CurrentValue;
            }
            else if (String.Equals(RotateAxisList[i], "Y", StringComparison.OrdinalIgnoreCase))
            {
                WriteDataArray[DataCount] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Y.CurrentValue;
                WriteDataArray[DataCount + 1] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Y.CurrentVelocity;
                WriteDataArray[DataCount + 2] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Y.CurrentAcceleration;
                //AngleArray[i] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Y.CurrentValue;
            }
            else if (String.Equals(RotateAxisList[i], "Z", StringComparison.OrdinalIgnoreCase))
            {
                WriteDataArray[DataCount] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Z.CurrentValue;
                WriteDataArray[DataCount + 1] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Z.CurrentVelocity;
                WriteDataArray[DataCount + 2] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Z.CurrentAcceleration;
                //AngleArray[i] = bioik.FindSegment(BioIKMotorNameList[i]).Joint.Z.CurrentValue;
            }
            else
            {
                Debug.LogError("Wrong Rotate Axis Type, Please check the XML file");
                break;
            }
            DataCount = DataCount + 3;
        }

        if (IKTarget_To_Record.Length != 0)
        {
            RelativePosition = new Vector3[IKTarget_To_Record.Length];
            RelativeRot = new Quaternion[IKTarget_To_Record.Length];
            foreach (GameObject IKTargetGO in IKTarget_To_Record)
            {
                Vector3 position = GetRelativePosition(IKTargetGO);
                Quaternion rot = GetRelativeRot(IKTargetGO);
                WriteDataArray[DataCount] = position.x;
                WriteDataArray[DataCount+1] = position.y;
                WriteDataArray[DataCount+2] = position.z;
                WriteDataArray[DataCount+3] = rot.x;
                WriteDataArray[DataCount+4] = rot.y;
                WriteDataArray[DataCount+5] = rot.z;
                WriteDataArray[DataCount+6] = rot.w;
            }
            DataCount = DataCount + 7;
        }
        //Debug.Log(BioIKMotorNameList[0] + " " + RotateAxisList[0]);
        //Debug.Log(bioik.FindSegment(BioIKMotorNameList[0]).Joint.Y.TargetValue.ToString());
        if (Logger_trigger)
        { 
            Logger.RecordOneFrame(WriteDataArray);
        }
    }

    void OnDestroy()
    {
        if (Logger_trigger)
        {
            Logger.CloseFile();
        }
    }
}
