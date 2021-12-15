
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PortalDialog : UdonSharpBehaviour
{
    public Text header;
    public GameObject tp;
    FleaCore core;
    UdonSharpBehaviour ub=null;
    string ev=null;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void SetPortal(string _header,GameObject go)
    {
        header.text = _header;
        tp = go;
        ub = null;
        ev = null;
    }
    public void SetEvent(UdonSharpBehaviour _ub, string _ev)
    {
        ub = _ub;
        ev = _ev;
    }
    public void OnGo()
    {
        if(ub!=null && ev!=null)
        {
            ub.SendCustomEvent(ev);
        }
        core.TeleportPlayerToObject(Networking.LocalPlayer, tp);
    }
}
