using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class CartList : UdonSharpBehaviour
{
    public InputField input;
    public Button search;
    public Button back;
    public Button prev;
    public Button cur;
    public Button next;
    public Dropdown type;
    public Text label;

    public Row[] rowList;
    int currentPage=1;
    string[] currentData;

    // drop down
    public int DALL = 0;
    public int DTITLE = 1;
    public int DSID = 2;


    FleaCore core;
    void Start()
    {
        core = GetComponentInParent<FleaCore>();
        rowList = GetComponentsInChildren<Row>();
    }

    string RowFormat(string data)
    {
        string ret="";
        string fmt = "{0,-17}{1,-17}";
        string[] up = core.UnPack(data);
        // row foramt
        ret = string.Format(fmt, 
            up[core.db.CPTITLE], 
            up[core.db.CUID]);
        // set label <- oh
        label.text = string.Format(fmt, "Title","Seller");
        return ret;
    }

    public void RenderList(string[] data, int pn)
    {
        int begin = rowList.Length * (pn - 1);
        // sort position all
        SortRow();
        // first diactive all
        for (int i = 0; i < rowList.Length; i++)
            rowList[i].gameObject.SetActive(false);

        // load contents
        for (int i = 0; (i < rowList.Length) && (begin + i < data.Length); i++)
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

    public void SortRow()
    {
        Vector3 o = rowList[0].transform.position;
        Vector3 g = rowList[1].transform.position - o; 
        for(int i=0;i<rowList.Length;i++)
        {
            rowList[i].transform.position = o + g * i;
        }
    }
    public void LoadCartList(string _input, int opt)
    {
        input.text = _input;
        type.value = opt;
        OnSearch();
    }

    public void OnSearch() // set data
    {
        FleaDB db = core.db;
        string q = input.text;
        int tv = type.value;
        currentData = db.cart.Data();
        currentData = db.Where(currentData, db.uid, db.CUID, db.EXACT);
        if(q!="")
        {
            int dim = -1; // default is all
            if(tv == DTITLE)
                dim = core.db.RPTITLE;
            if(tv == DSID)
                dim = core.db.RSID;
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

        int di = (currentPage - 1) * rowList.Length + idx;
        string pid = core.UnPack(currentData[di])[db.CPID];

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
        // OnSearch();
    }
}
