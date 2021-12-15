using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaDB : UdonSharpBehaviour
{
    FleaCore core;
    public Storage user;
    public Storage post;
    public Storage review;
    public Storage cart;
    public Storage category;


    public string uid;


    // consts
    public string NONE = "__NONE__";
    public string GUEST = "user0";

    // enums
    // user
    public int UID       = 0;
    public int PW       = 1;
    public int SID      = 2;
    public int TYPE     = 3;
    public int MAJOR    = 4;
    public int EMAIL    = 5;
    public int GENDER   = 6;

    // type
    public string TADMIN = "ADMIN";
    public string TUSER = "USER";
    public string TYES = "YES";
    public string TNO = "NO";

    // post
    public int PID          = 0;
    public int PUID         = 1;
    public int PTITLE       = 2;
    public int PCONTENTS    = 3;
    public int PPRICE       = 4;
    public int PTYPE        = 5;
    public int PONSALE      = 6;
    public int PSCORE       = 7;
    public int PURL         = 8;
    public int PSIZE        = 9; // end

    public int AddPost(string uid, string  title, 
        string contents, int url,
        string price,string type)
    {
        string[] d = new string[PSIZE -1]; // no pid
        d[PUID - 1] = uid;
        d[PTITLE - 1] = title;
        d[PCONTENTS - 1] = contents;
        d[PURL - 1] = url.ToString();
        d[PPRICE - 1] = price;
        d[PTYPE - 1] = type;
        d[PONSALE - 1] = TYES;
        d[PSCORE - 1] = $"{0}";
        return post.Put(core.Pack(d)); // put without key
    }

    // review
    public int RID = 0;
    public int RPID = 1; // post id
    public int RBID = 2; // buyer id
    public int RSID = 3; // seller id
    public int RCONTENTS = 4;
    public int RSCORE = 5; 
    public int RPTITLE = 6; 
    public int AddReview(string pid, string  buyer, string seller,
        string contents,string score)
    {
        string title = post.GetItem(pid)[PTITLE]; // no rid
        string d = core.Pack(new string[] { pid, buyer, seller, 
            contents, score,title});
        return review.Put(d);
    }
    // cart
    public int CID = 0;
    public int CUID = 1;
    public int CSID = 2;
    public int CPID = 3;
    public int CPTITLE = 4;
    public int AddCart(string uid,string seller, string pid)
    {
        string title = post.GetItem(pid)[CPTITLE]; // no cid
        string d = core.Pack(new string[] {uid,seller,pid,title});
        return cart.Put(d);
    }

    // query opt
    public int EXACT = 0;
    public int MATCH = 1;

    // type
    public string PTYPE_PHONE = "phone";
    public string PTYPE_NOTE = "note";
    public string PTYPE_BAG  = "bag";
    public string PTYPE_FOOD  = "food";
    public string PTYPE_TICKET  = "ticket";
    public string PTYPE_ELEC  = "elec";
    public string[] PTYPES = new string[]
    {
        "phone",
        "note",
        "bag" ,
        "food" ,
        "ticket",
        "elec" ,
    };



    void start(){
    }
    public void Init()
    {
        core = GetComponentInParent<FleaCore>();
        user.Init();
        post.Init();
        review.Init();
        cart.Init();
        category.Init();

        uid = NONE;
        //uid = GUEST;
    }
    public bool AddUser(string id, string pw, string sid,  string type,
        string major, string email, string gender)
    {
        if(user.Find(id)>=0)
        {
            return false;
        }
        string[] datas = new string[] { id,pw, sid, type, major, email, gender };
        user.Add(id, core.Pack(datas));
        return true;
    }
    public bool DeleteUser(string id)
    {
        int idx = user.Find(id);
        if (idx < 0)
            return false;
        user.Remove(idx);
        return true;
    }

    public bool Login(string id, string pw)
    {
        int idx = user.Find(id);
        if (idx < 0)
            return false;
        core.Log(user.At(idx));
        string[] up = core.UnPack(user.At(idx));
        string tpw = up[PW];
        core.Log(tpw);

        if (pw != tpw)
            return false;

        uid = id;
        return true;
    }

    public bool Logout()
    {
        if (uid == NONE)
            return false;
        uid = NONE;
        return true;
    }

    public bool IsAdmin(string id)
    {
        if (id == null)
        {
            id = uid;
        }
        string[] val = user.GetItem(id);
        if (val == null)
            return false;
        if (user.GetItem(id)[TYPE] == TADMIN)
            return true;
        return false;
    }

    public string[] Where(string[] data,string q,int dim,int opt)
    {
        int cnt = 0;
        int[] idxs = new int[data.Length];
        string[] ret=null;
        for(int i =0;i<data.Length;i++)
        {
            string val;

            if (dim < 0)
            {
                val = data[i];
            }
            else
            {
                val = core.UnPack(data[i])[dim];
            }

            if(opt==EXACT) // EXACT
            {
                if (val == q)
                    idxs[cnt++] = i;
            }
            else if(opt==MATCH) 
            {
                if (val.IndexOf(q) >= 0)
                    idxs[cnt++] = i;
            }
        }
        ret = new string[cnt];
        for(int i=0;i<cnt;i++)
        {
            ret[i] = data[idxs[i]];
        }
        return ret;
    }

    // URL API
    public VRCUrl[] sampleUrls;
    public VRCUrl[] urls;
    int urlTop=0;
    int urlSize = 100;

    [UdonSynced]
    public VRCUrl surl=null;
    [UdonSynced]
    public int sidx=-1;

    UdonSharpBehaviour listener=null;
    string listenerEvent=null;
    public override void OnDeserialization()
    {
        if(core.IsMaster()) 
        {
            if (sidx >= 0) // geturl
                surl = urls[sidx];
            else // addurl
                sidx = AddUrl(surl);
            core.Sync(this);
        }
        else 
        {
            if (listener != null && listenerEvent != null)
            {
                listener.SendCustomEvent(listenerEvent);
                listener = null;
                listenerEvent = null;
            }
        }
    }

    public void NetworkGetUrl(int idx,UdonSharpBehaviour ub,string ev)
    {
        listener = ub;
        listenerEvent = ev;
        sidx = idx;
        surl = null;
        if(core.IsMaster())
        {
            surl = urls[sidx];
            listener.SendCustomEvent(listenerEvent);
        }
        else
        {
            core.Sync(this);
        }
    }
    public void NetworkAddUrl(VRCUrl url,UdonSharpBehaviour ub,string ev)
    {
        listener = ub;
        listenerEvent = ev;
        sidx = -1;
        surl = url;
        if(core.IsMaster())
        {
            sidx = AddUrl(surl);
            listener.SendCustomEvent(listenerEvent);
        }
        else
        {
            core.Sync(this);
        }
    }

    public void UrlInit()
    {
        urls = new VRCUrl[urlSize];
        for(int i=0;i<sampleUrls.Length;i++)
        {
            urls[i] = sampleUrls[0];
        }
        for(int i=0;i<sampleUrls.Length;i++)
        {
            AddUrl(sampleUrls[i]);
        }
    }
    private int AddUrl(VRCUrl url)
    {
        if (urlTop == urlSize)
            return -1;
        urls[urlTop] = url;
        urlTop++;
        return urlTop-1;
    }

    public override void OnPostSerialization(SerializationResult result)
    {
        if(!result.success)
        {
            core.Log("fleadb  sync fail!");
            core.Sync(this);
        }
    }
}