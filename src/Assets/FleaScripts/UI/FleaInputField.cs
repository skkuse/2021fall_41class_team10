using UdonSharp;
using UnityEngine.UI;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FleaInputField : UdonSharpBehaviour
{
    InputField inputField;
    UdonSharpBehaviour ub;
    string eventName;
    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    public void SetEvent(UdonSharpBehaviour _ub, string _eventName)
    {
        ub = _ub;
        eventName = _eventName;
    }

    public void OnValueChanged()
    {
        if(ub!=null)
            ub.SendCustomEvent(eventName);
    }
}
