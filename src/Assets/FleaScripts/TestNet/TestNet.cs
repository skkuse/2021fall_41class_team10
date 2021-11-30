using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TestNet : UdonSharpBehaviour
{
    public FleaCore core;
    InputField input;
    [UdonSynced]
    string[] data;

    public virtual void Start()
    {
        core = GetComponentInParent<FleaCore>();
        GetComponentInChildren<FleaButton>().SetEvent(this, "Test");
        GetComponentInChildren<FleaInputField>().SetEvent(this, "Test");

        data = new string[1000];
        for(int i=0;i<data.GetLength(0);i++)
        {
            data[i] = "heyyyeyedddddddddddyeyeyey";
        }
    }

    public void Test()
    {
        core.Log("test!!!");
    }

    public override void Interact()
    {
        core.Sync(this);
    }

    public override void OnPostSerialization(SerializationResult result)
    {
        if(result.success)
        {
            int len = result.byteCount;
            core.Log($"sent {len}");
        }
        else
        {
            core.Log($"fail");
        }
    }

    public override void OnDeserialization()
    {
    }
}
