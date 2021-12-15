using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Storage : UdonSharpBehaviour
{
    FleaCore core;
    const int size = 1000;
    const string NONE = "__NONE__";
    [UdonSynced]
    string[] keyArr;
    [UdonSynced]
    string[] itemArr;
    [UdonSynced]
    int count;

    void start()
    {
    }
    public void Init()
    {
        core = GetComponentInParent<FleaCore>();
        if(core.IsMaster()) // TODO
        {
            keyArr = new string[size];
            itemArr = new string[size];
            for(int i=0;i<size;i++)
            {
                keyArr[i] = NONE;
                itemArr[i] = NONE;
            }
        }
    }
    public string[] Data()
    {
        string[] data = new string[count];
        int di = 0;
        for(int i=0;i<size;i++)
        {
            if(keyArr[i]!=NONE)
            {
                data[di++] = itemArr[i];
            }
        }
        return data;
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
    public int Put(string item)
    {
        int idx = Find(NONE);
        if (idx < 0)
            return -1;
        keyArr[idx] = idx.ToString();
        // idx + items
        itemArr[idx] = core.Pack(new string[] { idx.ToString(), item });
        count++;

        return idx;
    }

    public bool Remove(int idx)
    {
        if (idx <= 0 || idx > size)
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
        for (int i = 0; i < size; i++)
        {
            if (keyArr[i] == key)
            {
                return i;
            }
        }
        return -1;
    }
    public string[] GetItem(string key)
    {
        int idx = Find(key);
        if (idx < 0)
            return null;
        return core.UnPack(itemArr[idx]);
    }
    public bool SetItem(string key,string value, int dim)
    {
        int idx = Find(key);
        if (idx < 0)
            return false;
        if(dim<0)
        {
            // change all 
            // TODO
        }
        else
        {
            string[] up = core.UnPack(At(idx));
            up[dim] = value;
            itemArr[idx] = core.Pack(up);
        }
        return true;
    }
    public int[] FindAll(string[] qs = null, int sort = -1)
    {
        int cnt = 0;
        int[] idxs = new int[size];
        for(int i=0;i<size;i++)
        {
            if (itemArr[i] == NONE)
                continue;

            string[] up = core.UnPack(itemArr[i]);
            bool match = true;
            int qlen = 0;
            if (qs == null)
                qlen = 0;
            else
                qlen = qs.Length;

            // skip if qs is null match -> true
            for (int j = 0; j < qlen; j++) 
            {
                if (up[j] != qs[j] && qs[j] != NONE)
                {
                    match = false;
                    break;
                }
            }

            if(match)
            {
                idxs[cnt] = i;
                cnt++;
            }
        }

        if (sort != -1)
        {
            //sort
            // TODO
        }

        int[] ret = new int[cnt];
        idxs.CopyTo(ret, 0);
        return ret;
    }
    public string At(int idx)
    {
        return itemArr[idx];
    }

    public void Sync()
    {
        core.Sync(this);
    }
    public override void OnPostSerialization(SerializationResult result)
    {
        if(!result.success)
        {
            core.Log("storage sync fail!");
            core.Sync(this);
        }
    }
}
