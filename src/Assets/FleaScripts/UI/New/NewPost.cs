using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NewPost : UdonSharpBehaviour
{
    public InputField title;
    public InputField price;
    public VRCUrlInputField youtube;
    public InputField contents;

    public Button register;
    public Button back;


    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }

    public void Clear()
    {
        title.text = "";
        contents.text = "";
        price.text = "";
        youtube.SetUrl(core.db.sampleUrls[0]);
    }
    public void OnUpload()
    {
        FleaDB db = core.db;
        UICore ui = core.ui;
        VRCUrl url = null;
        string[] sids;
        // check routine
        if (title.text == "" || contents.text == "" || price.text == "")
        {
            ui.dialog.Show(this, "", "ERROR", $"Enter all required information.");
            return;
        }
        url = youtube.GetUrl();
        if(url != null) // has yo(utube url
        {
            ui.dialog.ShowMsgOnly(this, "OnAddUrl", "WAIT", "Uploading...");
            db.NetworkAddUrl(url, this, "OnAddUrl");
        }
        else
        {
            Upload();
        }
        return;
    }

    public void Upload()
    {
        FleaDB db = core.db;
        //core.ui.Pop(); // new post panel
        core.ui.PopAll();
        core.ui.dialog.Show(this, "", "SUCCESS", $"Uploaded your post");
        db.AddPost(db.uid, title.text, contents.text, db.sidx, price.text, "NORM");
        db.post.Sync();
    }
    public void OnAddUrl()
    {
        Upload();
        core.ui.Pop(); // wait msg
    }

    public void OnBack()
    {
        core.ui.Pop();
    }
}
