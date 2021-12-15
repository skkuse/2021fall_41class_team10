using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaCore : UdonSharpBehaviour
{
    public string NONE = "__NONE__";
    public FleaDB db;
    public UICore ui;
    public PortalDialog[] homePortals;
    public MarketRoom[] markets;
    public HomeWorld home;
    public TradingRoom tradingRoom;
    void Start()
    {
        db = GetComponentInChildren<FleaDB>();
        ui = GetComponentInChildren<UICore>();
        tradingRoom = GetComponentInChildren<TradingRoom>();
        home = gameObject.GetComponentInChildren<HomeWorld>();
        markets = gameObject.GetComponentsInChildren<MarketRoom>();
        homePortals = home.gameObject.GetComponentsInChildren<PortalDialog>();
        db.Init();

        for(int i=0;i<homePortals.Length && i< markets.Length;i++)
        {
            string type = db.PTYPES[i % 6];
            homePortals[i].SetPortal(type, markets[i].tp);
            homePortals[i].SetEvent(markets[i], "Sync");
            markets[i].type = type;
        }


        if(IsMaster())
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Config();
        }
    }


    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if(IsMaster())
        {
            Log("bye player" );
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(IsMaster())
        {
            Log("new user sycn...");
            Sync(db);
            Sync(db.user);
            Sync(db.post);
            Sync(db.review);
            Sync(db.cart);
            Sync(db.category);
        }
    }

    // add all helper functions here

    public void Sync(UdonSharpBehaviour ub)
    {
        Networking.SetOwner(Networking.LocalPlayer,ub.gameObject);
        ub.RequestSerialization();
    }
    public void LoadSync(UdonSharpBehaviour ub)
    {
        ub.SendCustomNetworkEvent(NetworkEventTarget.Owner, "RequestSerialization");
    }

    public void Log(string msg){
        Debug.Log($"SKKU [{GetLocalPlayerId()}] : {msg}");
    }

    public void SendGlobalEvent(UdonSharpBehaviour ub, string eventName)
    {
        ub.SendCustomNetworkEvent(NetworkEventTarget.All,eventName);
    }
    public int GetAdminPlayerId()
    {
        return Networking.GetOwner(gameObject).playerId;
    }

    public int GetLocalPlayerId()
    {
        return Networking.LocalPlayer.playerId;
    }

    public bool IsMaster()
    {
        return Networking.IsMaster;
    }


    // object
    public void MoveObject(GameObject go,Vector3 dir)
    {
        go.transform.position += dir;
    }
    public void SetObjectPosition(GameObject go,Vector3 pos)
    {
        go.transform.position = pos;
    }

    public void TeleportPlayerToObject(VRCPlayerApi player, GameObject gameObject)
    {
        player.TeleportTo(
            gameObject.transform.position,
            gameObject.transform.rotation
        );
    }

    // data structure
    public string Pack(string[] strArr)
    {
        return string.Join(";", strArr);
    }
    public string[] UnPack(string str)
    {
        return str.Split(new string[] { ";" },
            System.StringSplitOptions.None);
    }
    public int[] AddToArr(int[] old,int val)
    {
        int len = old.GetLength(0);
        int[] ret = new int[len + 1];
        old.CopyTo(ret, 0);
        ret[len + 1] = val;
        return ret;
    }
    public int[] MergeArr(int[] a, int[] b)
    {
        int alen = a.GetLength(0);
        int blen = b.GetLength(0);
        int[] ret = new int[alen + blen];
        a.CopyTo(ret, 0);
        b.CopyTo(ret, alen);
        return ret;
    }
    public string Hash(string str)
    {
        return str;
    }

    // config
    void Config()
    {
        db.UrlInit();
        db.AddUser("admin", "1234", "2017312097", db.TADMIN,
            NONE, NONE, NONE);

        int userNum = 200;

        for (int i=0;i<userNum;i++)
        {
            string name = $"user{i}";
            string pw = $"pw{i}";
            string sid = $"{(2010+(i*i)%15)*1000000 + (i*i*i)%10000 + 2413 + i*i + i}";
            db.AddUser(name, pw, sid, db.TUSER,
                NONE,
                $"user{i}@skku.edu",
                NONE
                );
        }


        // post
        int[] posts = new int[50];
        for(int i=0;i<posts.Length;i++)
        {
            string uid = $"user{(i * i) % 13}";
            string title = sampleTitles[(i*i+3)%sampleTitles.Length];
            string contents = sampleContents[(i*i*i+3)%sampleContents.Length];
            posts[i] = db.AddPost(uid,
                $"{title}",
                $"{contents}", 0,
                $"{((i*i)%100)*3200}",
                db.PTYPES[(i*i)%6]);

            // add review for contents
            int cnt = (i % 3) * ((i * i + 5) % 8);
            while (--cnt > 0)
            {
                string buyer = $"user{((i * cnt) % 17 + 32)%userNum}";
                string score = $"{(i * cnt + 3) % 5}";
                string review = sampleContents[(i*cnt)%sampleContents.Length];
                db.AddReview(posts[i].ToString(), buyer, uid, review, score);
            }
            // add to cart
            cnt = (i % 6) * ((i * i + 2) % 9);
            while (--cnt > 0)
            {
                string buyer = $"user{((i * i * cnt) % 13 + 23)%userNum}";
                db.AddCart(
                    buyer, uid, posts[i].ToString());
            }
        }
    }
    string[] sampleTitles = new string[] { 
        "Only today!!",
        "Cheap!!",
        "IPHONE13",
        "IPHONE11",
        "MACOBOOK",
        "GOOD NOTE",
        "SHOES",
        "TICKET",
        "FREE",
        "HELLO",
        "SAMPLE",
    };
    string[] sampleContents = new string[] { 
        "It is cheap",
        "as 7 minutes after midnight. The dog was lying on the ",
        "ork sticking out of the dog. The point",
        " is Christopher John Francis Boone. I know all the countries ",
        "ant ‘happy’, like when I’m reading about the Apollo space missions, or when I am still awake at 3 am or 4 am ",
        "now all the countries of the world and their capital cities and every prime numb",
    };
}