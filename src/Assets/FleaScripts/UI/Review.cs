
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.Base;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Review : UdonSharpBehaviour
{
    public BaseVRCVideoPlayer player;
    public Text title;
    public Text price;
    public Text contents;
    public Text score;
    public Text seller;
    public Text buyer;
    public Button delete;


    VRCUrl url;
    FleaCore core;
    string rid;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        //player.PlayURL
    }
    private void AdminSetting()
    {
        bool flag = core.db.IsAdmin(null);
        delete.gameObject.SetActive(flag);
    }

    public void LoadReviewInfo(string _rid)
    {
        FleaDB db = core.db;
        UICore ui = core.ui;
        AdminSetting();
        rid = _rid;

        string[] rdata = db.review.GetItem(rid);
        string fmt = "{0}{1}\n";
        string sid = rdata[db.RSID];
        string bid = rdata[db.RBID];
        string score = rdata[db.RSCORE];
        string pid = rdata[db.RPID];
        string[] pdata = db.post.GetItem(pid);

        title.text = pdata[db.PTITLE];
        price.text = string.Format(fmt, pdata[db.PPRICE], "KRW");
        contents.text = "Review : \n"+rdata[db.RCONTENTS];
        seller.text = string.Format(fmt, "Seller :",sid);
        buyer.text = string.Format(fmt, "Buyer :",bid);
        this.score.text = string.Format(fmt, "Score :",$"{score}/5");
        core.db.NetworkGetUrl(int.Parse(pdata[db.PURL]),this,"OnGetUrl");
    }
    public void OnSeller()
    {
        UICore ui = core.ui;
        FleaDB db = core.db;
        string uid = db.review.GetItem(rid)[db.RSID];
        ui.userInfo.LoadUserInfo(uid);
        ui.Push(this,ui.userInfo);
    }
    public void OnBuyer()
    {
        UICore ui = core.ui;
        FleaDB db = core.db;
        string uid = db.review.GetItem(rid)[db.RBID];
        ui.userInfo.LoadUserInfo(uid);
        ui.Push(this,ui.userInfo);
    }
    public void OnBack()
    {
        core.ui.Pop();
    }
    public void OnPush()
    {
    }
    public void OnGetUrl()
    {
        player.PlayURL(core.db.surl);
    }
}
