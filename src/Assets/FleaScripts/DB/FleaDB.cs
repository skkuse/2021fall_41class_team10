using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleabDB : UdonSharpBehaviour
{
    FleaCore core;
    public UserDB userDB;
    public ProductDB productDB;
    void start(){
        core = GetComponentInParent<FleaCore>();
        userDB = GetComponentInChildren<UserDB>();
        productDB = GetComponentInChildren<ProductDB>();
    }
}