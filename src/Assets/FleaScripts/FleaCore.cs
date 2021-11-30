using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaCore : UdonSharpBehaviour
{
    public UserDB userDB;
    public ProductDB productDB;
    FleaSocket[] fleaSockets;
    public VRCPlayerApi adminPlayer;
    public VRCPlayerApi localPlayer;

    public string uid;

    //max player is 20
    int maxPlayer;

    [UdonSynced]
    int[] Sessions;

    int GetSidByPid(int pid)
    {
        for(int i=0;i<maxPlayer;i++)
        {
            if(Sessions[i]==pid)
            {
                return i;
            }
        }
        return -1;
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if(IsAdmin())
        {
            // TODO
            // Sessions[sid] = -1;
            Log("bye player" );
            RequestSerialization();
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(IsAdmin())
        {
            int pid = VRCPlayerApi.GetPlayerId(player);
            int newSid;
            newSid = GetSidByPid(-1);
            Log($"new player :  {pid} , {newSid}");
            Sessions[newSid] = pid;

            RequestSerialization();
        }
    }


    void Start()
    {
        fleaSockets = GetComponentsInChildren<FleaSocket>();
        userDB = GetComponentInChildren<UserDB>();
        productDB = GetComponentInChildren<ProductDB>();
        maxPlayer = fleaSockets.GetLength(0);

        if(IsAdmin())
        {
            Log("Max palyer :" + maxPlayer);
            Sessions = new int[maxPlayer];
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            for(int i=0;i<maxPlayer;i++)
            {
                Sessions[i] = -1; // no User
            }
        }
        adminPlayer = Networking.GetOwner(gameObject);
        localPlayer = Networking.LocalPlayer;

    }

    public FleaSocket GetSocket(){
        int sid = GetSidByPid(GetLocalPlayerId());
        // int sid = GetSidTag(localPlayer);
        Log("My sid is " + sid);
        return fleaSockets[sid];
    }


    // add all helper functions here

    public void Sync(GameObject go)
    {
        Networking.SetOwner(Networking.LocalPlayer,go);
        RequestSerialization();
    }

    public void Log(string msg){
        Debug.Log($"SKKU [{GetLocalPlayerId()}] : {msg}");
    }

    // database

    public void SendGlobalEvent(UdonSharpBehaviour ub, string eventName)
    {
        ub.SendCustomNetworkEvent(NetworkEventTarget.All,eventName);
    }
    public int GetAdminPlayerId()
    {
        return VRCPlayerApi.GetPlayerId(adminPlayer);
    }

    public int GetLocalPlayerId()
    {
        return VRCPlayerApi.GetPlayerId(Networking.LocalPlayer);
    }

    public bool IsAdmin()
    {
        return Networking.IsMaster;
        // return isAdmin;
    }

    public void moveObject(UdonSharpBehaviour ub,Vector3 dir)
    {
        ub.gameObject.transform.position += dir;
    }

    public void TeleportPlayerToObject(VRCPlayerApi player, GameObject gameObject)
    {
        player.TeleportTo(
            gameObject.transform.position,
            gameObject.transform.rotation
        );
    }

}

// public class FleaDB{

// }