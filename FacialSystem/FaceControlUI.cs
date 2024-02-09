using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class FaceControlUI : MonoBehaviour 
{
    public CubismModel CubismModel;
    public float Value = (float)0.0;

    [SerializeField]
    public CubismParameterBlendMode BlendMode = CubismParameterBlendMode.Override;

    private Dictionary<string, int> DicParaID;
    private Dictionary<string, int> DicParaName;
    private Dictionary<string, int> DicPartsID;
    private Dictionary<string, int> DicPartsName;
    private float[] MaximumValueArray;
    private float[] MinimumValueArray;
    private float[] DefaultValueArray;
    private bool ONOFF = true;

    // Start is called before the first frame update
    void Start()
    {
        DicParaID = new Dictionary<string, int>();
        DicParaName = new Dictionary<string, int>();
        DicPartsID = new Dictionary<string, int>();
        DicPartsName = new Dictionary<string, int>();
        MaximumValueArray = new float[CubismModel.Parameters.Length];
        MinimumValueArray = new float[CubismModel.Parameters.Length];
        DefaultValueArray = new float[CubismModel.Parameters.Length];
        for (int i = 0; i < CubismModel.Parameters.Length; i++)
        {
            DicParaID.Add(CubismModel.Parameters[i].Id,i);
            DicParaName.Add(CubismModel.Parameters[i].name, i);
            MaximumValueArray[i] = CubismModel.Parameters[i].MaximumValue;
            MinimumValueArray[i] = CubismModel.Parameters[i].MinimumValue;
            DefaultValueArray[i] = CubismModel.Parameters[i].DefaultValue;
        }
        Debug.Log(CubismModel.Parameters[3].name);
        Debug.Log(CubismModel.Parameters[3].Id);
        for (int i = 0; i < CubismModel.Parts.Length; i++)
        {
            DicPartsID.Add(CubismModel.Parts[i].Id, i);
            DicPartsName.Add(CubismModel.Parts[i].name, i);
        }
    }

    // Update
    void LateUpdate()
    {
        if (ONOFF)
        {
            //CubismModel.Parameters[3].Value = Value;
            CubismModel.Parameters[3].BlendToValue(BlendMode,Value);
        }

        //CubismModel.Parameters[3].BlendToValue


    }
}
