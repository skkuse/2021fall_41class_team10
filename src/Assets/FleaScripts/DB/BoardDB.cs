using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BoardDB : UdonSharpBehaviour
{
    FleaCore fleaCore;
    const int maxProduct=100;
    int numProduct=0;

    public string[] Title;
    public string[] Owner;
    public string[] Price;
    public string[] Type;

    void Start()
    {
        fleaCore = GetComponentInParent<FleaCore>();
        if(fleaCore.IsAdmin())
        {
            Title = new string[maxProduct];
            Owner = new string[maxProduct];
            Price = new string[maxProduct];
            Type = new string[maxProduct];
        }
    }

    public void AddUser(string name,string type, string pwhash, string email)
    {
        Title[numProduct] = name;
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
            if(Title[i] == name)
                return i;
        }
        return -1;
    }
}
