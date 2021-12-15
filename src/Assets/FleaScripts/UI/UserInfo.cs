
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UserInfo : UdonSharpBehaviour
{
    //public Button back;
    //public Button productMore;
    //public Button reviewMore;
    public Button ban;
    public Button apply;
    public Text info;
    public Text review;
    public Text item;
    public Text header;
    public Toggle admin;

    FleaCore core;

    string _uid;

    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }

    private void UserSetting()
    {
        // admin setting
        bool flag = core.db.IsAdmin(null);
        ban.gameObject.SetActive(flag);
        apply.gameObject.SetActive(flag);
        admin.gameObject.SetActive(flag);
    }
    public void LoadUserInfo(string uid)
    {
        FleaDB db = core.db;
        _uid = uid;
        UserSetting();
        
        // get user data
        string[] data = db.user.GetItem(uid);
        string fmt = "{0,10}:{1,-20}\n";
        info.text  = string.Format(fmt, "UID"           , data[db.UID]);
        info.text += string.Format(fmt, "Student ID"    , data[db.SID]);
        info.text += string.Format(fmt, "major"         , data[db.MAJOR]);
        info.text += string.Format(fmt, "Email"         , data[db.EMAIL]);
        info.text += string.Format(fmt, "Gender"        , data[db.GENDER]);
        if (data[db.TYPE] == db.TADMIN)
            admin.isOn = true;
        else
            admin.isOn = false;
        // get review score
        // TODO 
        // get item number
        // TODO
    }
    public void OnReview()
    {
        // TODO open review list
    }
    public void OnItem()
    {
        // TODO open product list
    }
    public void OnApply()
    {
        // check are u sure?
        // TODO
        // update admin info
        core.db.user.SetItem(_uid, core.db.TADMIN, core.db.TYPE);
    }
    public void OnBan()
    {
        // TODO ru sure?
        // delete user
        core.db.DeleteUser(_uid);
    }
    public void OnBack()
    {
        core.ui.Pop();
    }
}
