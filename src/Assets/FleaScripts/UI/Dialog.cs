using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Dialog : UdonSharpBehaviour
{
    //public UnityEngine.UI.InputField input;
    public Text title;
    public Text msg;
    public Button ok;
    public Button yes;
    public Button no;
    UdonSharpBehaviour caller;
    string okEvent;
    string yesEvent;
    string noEvent;
    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void SetMsg(string _title,string _msg)
    {
        title.text = _title;
        msg.text = _msg;
    }
    public void ShowMsgOnly(UdonSharpBehaviour ub,string ev,string _title,string _msg)
    {
        ok.gameObject.SetActive(false);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        caller = ub;
        okEvent = ev;
        SetMsg(_title, _msg);
        core.ui.Push(ub,this);
    }
    
    public void Show(UdonSharpBehaviour ub,string ev,string _title,string _msg)
    {
        ok.gameObject.SetActive(true);
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
        caller = ub;
        okEvent = ev;
        SetMsg(_title, _msg);
        core.ui.Push(ub,this);
    }
    public void ShowSelect(UdonSharpBehaviour ub,string yesev,string noev,
        string _title,string _msg)
    {
        ok.gameObject.SetActive(false);
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);

        caller = ub;
        yesEvent = yesev;
        noEvent = noev;
        SetMsg(_title, _msg);
        core.ui.Push(ub,this);
    }

    public void OnOK()
    {
        core.ui.Pop();
        caller.SendCustomEvent(okEvent);
    }
    public void OnYes()
    {
        core.ui.Pop();
        caller.SendCustomEvent(yesEvent);
    }
    public void OnNo()
    {
        core.ui.Pop();
        caller.SendCustomEvent(noEvent);
    }
}
