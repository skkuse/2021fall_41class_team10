using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ProductDB : UdonSharpBehaviour
{
    FleaCore fleaCore;
    const int maxProduct=100;
    int numProduct=0;
    public string[] Name;
    public string[] Owner;
    public int[] Amount;
    public string[] Price;
    public string[] Type;
    public bool[] IsValid;

    void Start()
    {
        fleaCore = GetComponentInParent<FleaCore>();
        if(fleaCore.IsAdmin())
        {
            Name = new string[maxProduct];
            Owner = new string[maxProduct];
            Amount = new int[maxProduct];
            Price = new string[maxProduct];
            Type = new string[maxProduct];
            IsValid = new bool[maxProduct];
            for(int i=0;i<maxProduct;i++)
            {
                IsValid[i]=false;
            }
        }
    }

    public void AddUser(string name,string type, string pwhash, string email)
    {
        Name[numProduct] = name;
        Owner[numProduct] = type;
        Price[numProduct] = pwhash;
        Type[numProduct] = email;
    }

    public bool Login(string name,string pwhash)
    {
        if(Price[GetUID(name)] == pwhash)
            return true;
        return false;
    }

    public int GetUID(string name)
    {
        for(int i=0;i<maxProduct;i++)
        {
            if(Name[i] == name)
                return i;
        }
        return -1;
    }
}