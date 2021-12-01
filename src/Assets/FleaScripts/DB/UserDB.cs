
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UserDB : UdonSharpBehaviour
{
    FleaCore core;
    const int size = 30;
    const string NONE = "__NULL__";
    [UdonSynced]
    string[] keyArr;
    [UdonSynced]
    string[] itemArr;
    [UdonSynced]
    int count;

    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        if(core.IsAdmin())
        {
            keyArr = new string[size];
            itemArr = new string[size];
            for(int i=0;i<size;i++)
            {
                keyArr[i] = NONE;
            }
        }
    }
    public int Add(string key, string item)
    {
        int idx = Find(NONE);
        if (idx < 0)
            return -1;
        keyArr[idx] = key;
        itemArr[idx] = item;
        count++;

        return idx;
    }
    public bool Remove(int idx)
    {
        if (idx < 0 || idx >= size)
            return false;
        if (keyArr[idx] == NONE)
            return false;
        keyArr[idx] = NONE;
        count--;
        return true;
    }
    public int Count()
    {
        return count;
    }
    public int Find(string key)
    {
        for (int i = 0; i < Count(); i++)
        {
            if (keyArr[i] == key)
            {
                return i;
            }
        }
        return -1;
    }
    public int[] FindAll(string item, int dim=0, int begin=0, int end=0)
    {
        // TODO
        return new int[10];
    }
    public string At(int idx)
    {
        return itemArr[idx];
    }

    public void Sync()
    {
        core.Sync(this);
    }
}
