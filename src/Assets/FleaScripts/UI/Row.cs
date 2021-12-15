using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Row : UdonSharpBehaviour
{
    public Text info;
    public Button more;

    UdonSharpBehaviour owner;
    string ownerEvent;

    bool dirty=false;

    void Start()
    {
        
    }

    public void SetContents(UdonSharpBehaviour _owner,string _ev,string _info)
    {
        info.text = _info;
        owner = _owner;
        ownerEvent = _ev;
    }
    public void OnMore()
    {
        dirty = true;
        owner.SendCustomEvent(ownerEvent);
    }
    public bool Poll()
    {
        if (dirty)
        {
            dirty = false;
            return true;
        }
        return false;
    }
}
