
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Video.Components.Base;
using VRC.SDK3.Components;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PostInfo : UdonSharpBehaviour
{
    public BaseVRCVideoPlayer player;
    public Text title;
    public Text price;
    public Text contents;
    public Text review;
    public Text items;
    public Text seller;

    public Button delete;
    public Button add;


    VRCUrl url;
    FleaCore core;
    string pid;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        //player.PlayURL
    }
    private void UserSetting()
    {
        FleaDB db = core.db;
        bool flag = core.db.IsAdmin(null);
        delete.gameObject.SetActive(flag);
        string[] cart = db.cart.Data();
        cart = db.Where(cart, db.uid, db.CUID, db.EXACT);
        // if already exists
        string addText;
        if (cart != null)
            addText = "Del";
        else
            addText = "Add";
        add.GetComponentInChildren<Text>().text = addText;
    }

    public void LoadPostInfo(string _pid)
    {
        UserSetting();
        FleaDB db = core.db;
        UICore ui = core.ui;
        pid = _pid;


        string[] data = db.post.GetItem(pid);
        string fmt = "{0}{1}\n";
        string uid = data[db.PUID];
        title.text = data[db.PTITLE];
        price.text = string.Format(fmt, data[db.PPRICE], "KRW");
        contents.text = data[db.PCONTENTS];
        seller.text = string.Format(fmt, "ID:",uid);
        // TODO get review score
        review.text = string.Format(fmt, "Review:",$"{data[db.PSCORE]}/5");
        int cnt = db.Where(db.post.Data(), uid, db.PUID, db.EXACT).Length;
        items.text = string.Format(fmt, "Items:",cnt);
        core.db.NetworkGetUrl(int.Parse(data[db.PURL]),this,"OnGetUrl");
    }
    public void OnBuy()
    {
        core.tradingRoom.BuyerEnter(pid);
    }
    public void OnAdd()
    {
        core.ui.dialog.ShowSelect(this, "OnYes", "OnNo", "Added to my cart!", 
            "Would you like to open the cart list?");
    }
    public void OnYes()
    {
        core.ui.Push(this,core.ui.cartList);
        core.ui.cartList.LoadCartList("",
            core.ui.cartList.DALL );
    }
    public void OnSeller()
    {
        UICore ui = core.ui;
        FleaDB db = core.db;
        string uid = db.post.GetItem(pid)[db.PUID];
        ui.userInfo.LoadUserInfo(uid);
        ui.Push(this,ui.userInfo);
    }
    public void OnItem()
    {
        UICore ui = core.ui;
        FleaDB db = core.db;
        string uid = db.post.GetItem(pid)[db.PUID];
        ui.postList.LoadPostList(uid, ui.postList.DUID);
        ui.Push(this,ui.postList);
    }
    public void OnReview()
    {
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
