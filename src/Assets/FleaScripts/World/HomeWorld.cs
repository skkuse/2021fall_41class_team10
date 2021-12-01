
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

    public void OnClickRegister()
    {
    }
    public void OnClickLogin()
    {
    }

}
