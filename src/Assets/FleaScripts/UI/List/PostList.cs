using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PostList : UdonSharpBehaviour
{
    public InputField input;
    public Button search;
    public Button back;
    public Button prev;
    public Button cur;
    public Button next;
    public Dropdown type;
    public Dropdown sort;
    public Text label;
    public GameObject mtp;

    public Row[] rowList;
    int currentPage=1;
    string[] currentData;

    // drop down
    // sort
    public int DREVIEW = 0;
    public int DPRICE = 1;
    // category
    public int DALL = 0;
    public int DTITLE = 1;
    public int DUID = 2;
    public int DTYPE = 3;


    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        rowList = GetComponentsInChildren<Row>();
    }

    string RowFormat(string data)
    {
        string ret="";
        string fmt = "{0,-27}{1,-10}{2,-10}";
        string[] up = core.UnPack(data);
        // row foramt
        ret = string.Format(fmt, up[core.db.PTITLE], up[core.db.PPRICE], up[core.db.PTYPE]);
        // set label <- OH
        label.text = string.Format(fmt, "Title", "Price", "Type");
        return ret;
    }

    public void RenderList(string[] data, int pn)
    {
        int begin = rowList.Length * (pn - 1);
        // sort
        SortRow();
        // first diactive all
        for (int i = 0; i < rowList.Length; i++)
            rowList[i].gameObject.SetActive(false);
        // load contents
        for (int i = 0; i < rowList.Length && begin + i < data.Length; i++)
        {
            rowList[i].gameObject.SetActive(true);
            rowList[i].SetContents(this, "OnMore", RowFormat(data[begin + i]));
        }
        cur.GetComponentInChildren<Text>().text = $"{currentPage}/{TotalPageNum()}";
    }
    
    public void SetData(string[] data)
    {
        currentData = data;
    }
    public void LoadPostList(string _input, int opt)
    {
        input.text = _input;
        type.value = opt;
        OnSearch();
    }

    public void OnSearch() // set data
    {
        string q = input.text;
        int tv = type.value;
        currentData = core.db.post.Data();
        if(q!="")
        {
            int dim = -1; // default is all
            if (tv == DUID)
                dim = core.db.PUID;
            if (tv == DTYPE)
                dim = core.db.PTYPE;
            if (tv == DTITLE)
                dim = core.db.PTITLE;
            currentData = core.db.Where(currentData, q, dim, core.db.MATCH);
        }
        currentPage = 1;
        RenderList(currentData,currentPage);
    }

    int TotalPageNum()
    {
        int rl = rowList.Length;
        int dl = currentData.Length;
        return (dl - 1) / rl + 1;
    }


    public void OnCur()
    {
    }
    public void OnPrev()
    {
        if (currentPage > 1)
            currentPage--;
        else
        {
            //nothing
        }
        RenderList(currentData, currentPage);
    }
    public void OnNext()
    {
        if (currentPage < TotalPageNum())
            currentPage++;
        else
        {
            // nothing
        }
        RenderList(currentData, currentPage);
    }

    public void OnMore()
    {
        UICore ui = core.ui;
        FleaDB db = core.db;
        // poll row
        int idx=-1;
        for(int i=0;i<rowList.Length;i++)
        {
            Row r = rowList[i];
            if(r.Poll())
            {
                idx = i;
                break;
            }
        }
        if (idx < 0)
            return;
        // on more event
        string pid;
        int di;

        di = (currentPage - 1) * rowList.Length + idx;
        pid = core.UnPack(currentData[di])[db.PID];

        ui.Push(this, ui.postInfo);
        ui.postInfo.LoadPostInfo(pid);
    }

    public void OnLow()
    {
        // TODO
    }
    public void OnHigh()
    {
        // TODO
    }

    public void OnBack()
    {
        core.ui.Pop();
    }
    public void OnPush()
    {
        // init phase
        OnSearch();
    }
    public void SortRow()
    {
        Vector3 o = rowList[0].transform.position;
        Vector3 g = rowList[1].transform.position - o; 
        for(int i=0;i<rowList.Length;i++)
        {
            rowList[i].transform.position = o + g * i;
        }
    }
}
