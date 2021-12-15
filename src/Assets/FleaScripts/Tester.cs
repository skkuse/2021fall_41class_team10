
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Tester : UdonSharpBehaviour
{
    public Button login;
    public Button userList;
    public Button postList;

    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }

    public void OnLogin()
    {
        UICore ui = core.ui;
        ui.Push(this, ui.login);
    }
    public void OnUserList()
    {
        UICore ui = core.ui;
        UserList ul = ui.userList;

        ul.LoadUserList("", ul.DALL);
        ui.Push(this, ui.userList);
    }

    public void OnUserInfo()
    {
        UICore ui = core.ui;
    }
    public void OnPostInfo()
    {
        UICore ui = core.ui;
        ui.Push(this, ui.postInfo);
    }

    public void OnPostList()
    {
        UICore ui = core.ui;
        ui.Push(this, ui.postList);
    }
    public void OnReviewList()
    {
        UICore ui = core.ui;
        ReviewList rl = ui.reviewList;
        ui.Push(this, ui.reviewList);
        //rl.LoadReviewList(core.db.uid, rl.DSID);
        rl.LoadReviewList("", rl.DALL);
    }

    public void OnNewPost()
    {
        UICore ui = core.ui;
        NewPost np = ui.newPost;
        np.Clear();
        ui.Push(this,np);
    }
    public void OnNewReview()
    {
        UICore ui = core.ui;
        NewReview nr = ui.newReview;
        ui.Push(this,nr);
        nr.LoadNewReview("0");
    }

}
