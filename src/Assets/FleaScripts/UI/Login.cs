using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Login : UdonSharpBehaviour
{
    public InputField id;
    public InputField password;
    public Button login;
    public Button register;
    public Toggle admin;
    public UdonSharpBehaviour sample;

    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void Clear()
    {
        id.text = "";
        password.text = "";
        admin.isOn = false;
    }
    public void OnRegister()
    {
        UICore ui = core.ui;
        ui.Push(this,ui.register);
    }
    public void OnLogin()
    {
        FleaDB db = core.db;
        UICore ui = core.ui;

        // admin check
        if(admin.isOn && !db.IsAdmin(id.text))
        {
            ui.dialog.Show(this, "OnNothing", "ERROR", "You do not have administrator privileges.");
            return;
        }

        // login info check
        if(db.Login(id.text, password.text))
        {
            ui.dialog.Show(this, "OnSuccess", "SUCCESS!", "Welcome to skku flea market!");
        }
        else
        {
            ui.dialog.Show(this,"Clear","ERROR", "INVALID LOGIN INFO");
        }
    }
    public void OnNothing()
    {

    }
    public void OnSuccess()
    {
        core.ui.Push(this, core.ui.adList);
    }
    public void OnPush()
    {
    }
    public void OnPop()
    {

    }
}
