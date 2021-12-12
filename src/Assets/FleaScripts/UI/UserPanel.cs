using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UserPanel : UdonSharpBehaviour
{
    public UnityEngine.UI.InputField input;
    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void OnClickSearch()
    {
        core.Log("hello");
        core.Log(input.text);
        input.text = "a";
    }
}
