using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class TradingRoom : UdonSharpBehaviour
{
    public GameObject buyerChair;
    public GameObject sellerChair;
    public FleaCore core;
    public PostInfo postInfo=null;
    Vector3 backPos;

    [UdonSynced]
    int cnt = 0;
    [UdonSynced]
    string pid=null;
    [UdonSynced]
    string bid=null;
    [UdonSynced]
    string sid=null;

    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void BuyerEnter(string _pid)
    {
        pid = _pid;

        FleaDB db = core.db;
        sid = db.post.GetItem(pid)[db.PUID];
        cnt = 2;

        backPos = Networking.LocalPlayer.GetPosition();
        core.TeleportPlayerToObject(Networking.LocalPlayer,buyerChair);

        core.Sync(this);
    }
    public void CallSeller()
    {
        if(core.db.uid == sid)
        {
            backPos = Networking.LocalPlayer.GetPosition();
            core.TeleportPlayerToObject(Networking.LocalPlayer,buyerChair);
        }
    }
    public void OnCall()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CallSeller");
    }
    public void OnOK()
    {
        UICore ui = core.ui;
        ui.newReview.LoadNewReview(pid);
        //if(core.db.uid == sid)
        //{
        //    cnt--;
        //}
        core.Sync(this);
        //core.ui.Push(PostInfo,)
    }

    public override void OnDeserialization()
    {
        //if (core.db.uid == bid)
        //{
        //    if(cnt == 0)
        //}
    }

    public void OnBack()
    {
        Networking.LocalPlayer.TeleportTo(
            backPos,
            gameObject.transform.rotation
        );
    }
    public void SyncPostInfo()
    {
        FleaDB db = core.db;
        if (postInfo == null)
            return;
        if (pid == null)
            return;
        postInfo.LoadPostInfo(pid);
    }
}
