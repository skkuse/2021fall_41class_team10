
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class HomeWorld : UdonSharpBehaviour
{
    FleaCore core;
    public GameObject RegisterPanel;
    public GameObject LoginPanel;
    public GameObject HomePanel;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(false);
        HomePanel.SetActive(true);
    }

    public void OnClickLogin()
    {
        SwitchPanel(LoginPanel, HomePanel);
    }
    public void OnClickRegister()
    {
        SwitchPanel(RegisterPanel, HomePanel);
    }
    // home panel events
    public void OnClickEnterRegister()
    {
        SwitchPanel(HomePanel, RegisterPanel);
    }
    public void OnClickEnterLogin()
    {
        SwitchPanel(HomePanel, LoginPanel);
    }
    public void OnClickEnterTradingRoom()
    {
        GameObject target = core.GetComponentInChildren<TradingRoom>().gameObject;
        core.TeleportPlayerToObject(Networking.LocalPlayer, target);
    }

    public void UpdateUserBoard()
    {

    }

    void SwitchPanel(GameObject _old, GameObject _new)
    {
        _new.transform.position = _old.transform.position;
        _new.SetActive(true);
        _old.SetActive(false);
    }

}
