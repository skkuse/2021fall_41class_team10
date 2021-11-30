
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class FleaButton : UdonSharpBehaviour
{
    Button button;
    UdonSharpBehaviour ub;
    string eventName;
    void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetEvent(UdonSharpBehaviour _ub, string _eventName)
    {
        ub = _ub;
        eventName = _eventName;
    }

    public void OnClick()
    {
        if(ub!=null)
            ub.SendCustomEvent(eventName);
    }
}
