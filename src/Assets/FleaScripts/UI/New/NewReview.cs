
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NewReview : UdonSharpBehaviour
{
    public UnityEngine.UI.InputField contents;
    public Dropdown score;
    FleaCore core;
    string pid;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
    }
    public void LoadNewReview(string _pid)
    {
        pid = _pid;
        contents.text = "";
        score.value = 0;
    }
    public void OnSubmit()
    {
        FleaDB db = core.db;
        string[] pd = db.post.GetItem(pid);
        // check
        if(contents.text=="" || score.value == 0)
        {
            core.ui.dialog.Show(this, "", "ERROR", "Not enough information!");
            return;
        }
        db.AddReview(pid, db.uid, pd[db.PUID], contents.text, score.value.ToString());
        core.ui.Pop();
        core.ui.dialog.Show(this, "", "SUCCESS", "Thank you!");
    }
}
