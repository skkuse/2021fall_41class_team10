using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Register : UdonSharpBehaviour
{
    public InputField id;
    public InputField pw;
    public InputField pwCheck;
    public InputField sid;
    public InputField major;
    public InputField email;
    public InputField gender;

    public Button register;
    public Button back;


    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }

    public void Clear()
    {
        id.text = "";
        pw.text = "";
        pwCheck.text = "";
        sid.text = "";
        major.text = "";
        email.text = "";
        gender.text = "";
    }
    public void OnRegister()
    {
        FleaDB db = core.db;
        UICore ui = core.ui;
        string[] sids;
        // check routine
        if(id.text==""||sid.text==""||pw.text=="")
        {
            ui.dialog.Show(this, "", "ERROR", $"Enter all required information.");
            return;
        }
        if (db.user.Find(id.text)>=0)
        {
            ui.dialog.Show(this, "", "ERROR", $"{id.text} already exists");
            return;
        }
        if (pwCheck.text != pw.text)
        {
            ui.dialog.Show(this, "", "ERROR", $"check password");
            return;
        }
        //sids = db.Where(db.user.Data(), sid.text,db.SID);
        //if(sids.Length>0)
        //{
        //    ui.dialog.Show(this, "", "ERROR", $"{sid.text} already exists");
        //    return;
        //}
        // TODO email parsing
        if (major.text == "") major.text = db.NONE;
        if (email.text == "") email.text = db.NONE;
        if (gender.text == "") gender.text = db.NONE;

        db.AddUser(id.text, pw.text, sid.text, db.TUSER,
            major.text, email.text, gender.text);

        ui.Pop();
        Clear();
        ui.dialog.Show(this, "", "Welcome!", $"You have successfully registered! Log in!");
        return;
    }

    public void OnBack()
    {
        core.ui.Pop();
    }

    public void OnFailPW()
    {

    }
}
