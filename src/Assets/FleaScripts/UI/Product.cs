
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.Base;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Product : UdonSharpBehaviour
{
    public Text title;
    public Text price;
    public Text contents;

    FleaCore core;
    string pid;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        //player.PlayURL
    }

    public void LoadPostInfo(string _pid)
    {
        FleaDB db = core.db;
        UICore ui = core.ui;
        pid = _pid;


        string[] data = db.post.GetItem(pid);
        string fmt = "{0}{1}\n";
        string uid = data[db.PUID];
        title.text = data[db.PTITLE];
        price.text = string.Format(fmt, data[db.PPRICE], "KRW");
        contents.text = data[db.PCONTENTS];
    }
    public void OnInfo()
    {
        core.ui.PopAll();
        core.ui.Push(this,core.ui.postInfo);
        core.ui.postInfo.LoadPostInfo(pid);
    }
}
