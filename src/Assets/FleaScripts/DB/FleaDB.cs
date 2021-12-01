using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleabDB : UdonSharpBehaviour
{
    FleaCore core;
    UserDB[] userDB;
    ProductDB[] productDB;
    void start(){
        core = GetComponentInParent<FleaCore>();
        userDB = GetComponentsInChildren<UserDB>();
        productDB = GetComponentsInChildren<ProductDB>();
    }
}