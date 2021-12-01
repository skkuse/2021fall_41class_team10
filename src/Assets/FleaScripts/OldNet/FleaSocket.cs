using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaSocket : UdonSharpBehaviour
{
    //[UdonSynced]
    //int senderID=0;
    //[UdonSynced]
    //int receiverID=0;
    //[UdonSynced]
    //string _data="";

    //UdonSharpBehaviour senderUB=null;
    //string eventListener="";

    //string[] stringSeparators = new string[]{";;;"};
    //bool socketLock=false;

    //public void Lock(){
    //    socketLock = true;
    //}
    //public void UnLock(){
    //    socketLock = false;
    //}


    //FleaCore fleaCore;

    //void Start()
    //{
    //    fleaCore = GetComponentInParent<FleaCore>();
    //}


    //public void AdminDataHandler()
    //{
    //    string[] msgs = GetDecodedData();
    //    // fleaCore.Log(data);
    //    ///////////// server logic ///////////////////////////////////////

    //    UserDB UserDB = fleaCore.userDB;
    //    ProductDB ProductDB = fleaCore.productDB;
    //    string req = msgs[0];
    //    string res="";
    //    if(req == "echo")
    //    {
    //        res = Encode( new string[]{
    //                msgs[1]
    //            });
    //    }
    //    else if(req == "UserDB.Login")
    //    {
    //        string name = msgs[1];
    //        string pwhash = msgs[1];

    //        res = Encode( new string[]{
    //                UserDB.Login(name,pwhash).ToString()
    //            });
    //    }
    //    else if(req == "UserDB.AddUser")
    //    {
    //        string name = msgs[1];
    //        string type = msgs[2];
    //        string pwhash = msgs[3];
    //        string email = msgs[4];

    //        UserDB.AddUser(name, type, pwhash, email);
    //    }
    //    else if(req == "UserDB.GetUID")
    //    {
    //        string name = msgs[1];
    //        int uid = UserDB.GetUID(name);
    //        res = Encode( new string[]{
    //                uid.ToString()
    //            });
    //    }
    //    else if(req == "UserDB.GetUserInfo")
    //    {
    //        string name = msgs[1];
    //        int uid = UserDB.GetUID(name);
    //        res = Encode( new string[]{
    //                UserDB.Name[uid],
    //                UserDB.Type[uid],
    //                // UserDB.PwHash[uid],
    //                UserDB.Email[uid],
    //            });
    //    }

    //    ////////////////////////////////////////////////////
    //    if(senderID == fleaCore.GetAdminPlayerId())
    //    {
    //        senderID = fleaCore.GetAdminPlayerId();
    //        receiverID = fleaCore.GetAdminPlayerId();
    //        ClientDataHandler();
    //        return;
    //    }
    //    SendDataTo(res,senderID);
    //}

    //public void ClientDataHandler()
    //{
    //    // fleaCore.Log(data);

    //    if(senderUB != null)
    //    {
    //        senderUB.SendCustomEvent(eventListener);
    //    }
    //}

    //public void SendRequest(string[] dataArr,UdonSharpBehaviour ub, string eventName)
    //{
    //    string req;
    //    if(socketLock)
    //    {
    //        fleaCore.Log("socket is locked");
    //        return;
    //    }

    //    senderUB = ub;
    //    eventListener = eventName;

    //    req = Encode(dataArr);


    //    if(fleaCore.IsAdmin())
    //    {
    //        _data = req;
    //        senderID = fleaCore.GetAdminPlayerId();
    //        receiverID = fleaCore.GetAdminPlayerId();
    //        AdminDataHandler();
    //        return;
    //    }
    //    SendDataTo(req, fleaCore.GetAdminPlayerId());
    //}

    //public string Encode(string[] msgs)
    //{
    //    string str = "";
    //    foreach(string s in msgs){
    //        str+=s;
    //        str+=stringSeparators[0];
    //    }
    //    return str;
    //}

    //public string[] GetDecodedData()
    //{
    //    return _data.Split(stringSeparators,System.StringSplitOptions.RemoveEmptyEntries);
    //}

    //// on receive
    //public override void OnDeserialization()
    //{
    //    // if i am the receiver
    //    if(receiverID == fleaCore.GetLocalPlayerId()){
    //        if(fleaCore.IsAdmin()) 
    //        {
    //            AdminDataHandler();
    //        }
    //        else{ 
    //            ClientDataHandler();
    //        }
    //    }
    //}

    //private void SendDataTo(string data,int targetId)
    //{
    //    senderID = fleaCore.GetLocalPlayerId();
    //    receiverID = targetId;
    //    _data = data;

    //    fleaCore.Log($"{senderID} --> {receiverID}");

    //    Networking.SetOwner(Networking.LocalPlayer,gameObject);
    //    RequestSerialization();
    //    // if(senderID == receiverID)
    //    //     OnDeserialization();
    //}
}
