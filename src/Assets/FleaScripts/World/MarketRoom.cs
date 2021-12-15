
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class MarketRoom : UdonSharpBehaviour
{
    public string type = "Sample";
    public GameObject tp;
    public Product[] tables;
    public FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        tables = gameObject.GetComponentsInChildren<Product>();
    }
    public void LoadMarketRoom(string _type)
    {
        FleaDB db = core.db;
        type = _type;
        string[] data = db.post.Data();
        data = db.Where(data, type, db.PTYPE, db.EXACT);

        for(int i=0;i<tables.Length;i++)
        {
            Product pd = tables[i];
            tables[i].gameObject.SetActive(false);
        }

        if (data == null)
            return;

        for(int i=0;i<tables.Length && i<data.Length;i++)
        {
            Product pd = tables[i];
            string[] up = core.UnPack(data[i]);
            pd.gameObject.SetActive(true);
            pd.LoadPostInfo(up[db.PID]);
        }
    }
    public void Sync()
    {
        LoadMarketRoom(type);
    }
}
