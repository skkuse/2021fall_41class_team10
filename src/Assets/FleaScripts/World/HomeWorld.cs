
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
        core = GetComponent<FleaCore>();
        RegisterPanel.SetActive(false);
        LoginPanel.SetActive(false);
        HomePanel.SetActive(true);
    }

    public void OnClickEnterRegister()
    {
        core.Log("click enter register");
        SwitchPanel(HomePanel, RegisterPanel);
    }
    public void OnClickEnterLogin()
    {
        core.Log("click enter login");
        SwitchPanel(HomePanel, LoginPanel);
    }

    void SwitchPanel(GameObject _old, GameObject _new)
    {
        _new.transform.position = _old.transform.position;
        _new.SetActive(true);
        _old.SetActive(false);
    }

}
