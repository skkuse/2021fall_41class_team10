using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaCore : UdonSharpBehaviour
{
    public FleabDB db;
    public int uid;

    void Start()
    {
        if(IsAdmin())
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        db = GetComponentInChildren<FleabDB>();
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if(IsAdmin())
        {
            Log("bye player" );
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(IsAdmin())
        {
        }
    }

    // add all helper functions here

    public void Sync(UdonSharpBehaviour ub)
    {
        Networking.SetOwner(Networking.LocalPlayer,ub.gameObject);
        ub.RequestSerialization();
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

    public bool IsAdmin()
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

    // config
    void Config()
    {
        //db.
        for(int i=0;i<100;i++)
        {
            /*
            db.userDB.Add(i.ToString(), Pack(new string[]
            {
                (i*i).ToString(),
                (i*2).ToString(),
            }));
            */
        }
    }
}