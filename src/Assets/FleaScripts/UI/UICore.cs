using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UICore : UdonSharpBehaviour
{
    public Tester tester;
    public Login login;
    public Register register;
    public Dialog dialog;
    public UserList userList;
    public PostList postList;
    public ReviewList reviewList;
    public CartList cartList;
    public AdList adList;
    // info
    public UserInfo userInfo;
    public PostInfo postInfo;
    public Review reviewInfo;

    // new
    public NewPost newPost;
    public NewReview newReview;

    UdonSharpBehaviour[] stack;
    int top;
    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();

        // ui stack init
        stack = new UdonSharpBehaviour[100];
        top = 0;

        // ui get ui comps
        tester = GetComponentInChildren<Tester>();
        login = GetComponentInChildren<Login>();
        register = GetComponentInChildren<Register>();
        dialog = GetComponentInChildren<Dialog>();

        // list
        userList = GetComponentInChildren<UserList>();
        postList = GetComponentInChildren<PostList>();
        reviewList = GetComponentInChildren<ReviewList>();
        cartList = GetComponentInChildren<CartList>();
        adList = GetComponentInChildren<AdList>();

        // info
        userInfo = GetComponentInChildren<UserInfo>();
        postInfo = GetComponentInChildren<PostInfo>();
        reviewInfo = GetComponentInChildren<Review>();

        // new
        newPost = GetComponentInChildren<NewPost>();
        newReview = GetComponentInChildren<NewReview>();

        // ui default config

        //SetActiveUI(tester, false);
        SetActiveUI(login, false);
        SetActiveUI(register, false);
        SetActiveUI(dialog, false);

        SetActiveUI(userList, false);
        SetActiveUI(postList, false);
        SetActiveUI(reviewList, false);
        SetActiveUI(cartList, false);
        SetActiveUI(adList, false);

        SetActiveUI(userInfo, false);
        SetActiveUI(postInfo, false);
        SetActiveUI(reviewInfo, false);

        SetActiveUI(newPost, false);
        SetActiveUI(newReview, false);

        // init
        //SetActiveUI(tester, true);
        //Push(this, tester);
    }

    public void FloatOn(UdonSharpBehaviour cui, UdonSharpBehaviour pui)
    {
        float s = 0.05f;
        Vector3 z = pui.transform.forward;
        z.x *= s;
        z.y *= s;
        z.z *= s;
        cui.transform.rotation = pui.transform.rotation;
        cui.transform.position = pui.transform.position - z;
    }
    public void Push(UdonSharpBehaviour pusher,UdonSharpBehaviour ui)
    {
        if(top==0)
        {
            stack[top] = pusher;
            top++;
        }
        FloatOn(ui, pusher);
        SetActiveUI(pusher, false);
        SetActiveUI(ui, true);
        stack[top] = ui;
        top++;
        ui.SendCustomEvent("OnPush");
    }
    public void PopAll()
    {
        while(Pop())
        {
            // pop all
        }
    }

    public bool Pop()
    {
        UdonSharpBehaviour current;
        UdonSharpBehaviour next;
        if (top == 0) return false;
        top--;
        if (top == 0) // if root
            return true; // did not swap ui
        // swap ui
        current = stack[top];
        next = stack[top - 1];
        SetActiveUI(current, false); 
        SetActiveUI(next, true);

        return true;
    }

    public void SetActiveUI(UdonSharpBehaviour ui,bool flag)
    {
        if(ui == null)
        {
            core.Log("[UICore] : NULL UI!");
            return;
        }
        if(flag) // activate ui
        {
            ui.DisableInteractive = false;
            ui.gameObject.SetActive(true);
        }
        else // disable ui
        {
            ui.DisableInteractive = true;
            ui.gameObject.SetActive(false);
        }
    }

    public Vector2 GetSize(Transform t)
    {
        Vector3 p1 = t.TransformPoint(0, 0, 0);
        Vector3 p2 = t.transform.TransformPoint(1, 1, 0);
        return new Vector2(p2.x - p1.x, p2.y - p1.y);
    }
}
