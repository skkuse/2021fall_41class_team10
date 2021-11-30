
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UserDB : UdonSharpBehaviour
{
    FleaCore fleaCore;
    const string None ="None";
    const int maxUser=100;
    int numUser=0;
    public string[] Name;
    public string[] Type;
    public string[] PwHash;
    public string[] Email;

    void Start()
    {
        fleaCore = GetComponentInParent<FleaCore>();
        if(fleaCore.IsAdmin())
        {
            Name = new string[maxUser];
            Type = new string[maxUser];
            PwHash = new string[maxUser];
            Email = new string[maxUser];
        }
    }

    public void AddUser(string name,string type, string pwhash, string email)
    {
        Name[numUser] = name;
        Type[numUser] = type;
        PwHash[numUser] = pwhash;
        Email[numUser] = email;
        numUser++;
    }

    public bool Login(string name,string pwhash)
    {
        if(PwHash[GetUID(name)] == pwhash)
            return true;
        return false;
    }

    public int GetUID(string name)
    {
        for(int i=0;i<maxUser;i++)
        {
            if(Name[i] == name)
                return i;
        }
        return -1;
    }
}
