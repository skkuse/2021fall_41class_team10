using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;


[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class FleaButton : UdonSharpBehaviour
{
    public Renderer mRenderer;
    public FleaCore fleaCore;

    // Start is called before the first frame update
    public virtual void Start()
    {
        fleaCore = GetComponentInParent<FleaCore>();

        mRenderer = GetComponent<Renderer>();
        mRenderer.enabled = true;
    }

    public override void OnDeserialization()
    {
    }

    public override void Interact()
    {
        FleaSocket socket = fleaCore.GetSocket();
        VRCPlayerApi player = Networking.LocalPlayer;

        string[] data = new string[]{"echo","hello"};
        socket.SendRequest(data,this,"Print");
    }

    public void Print(){
        FleaSocket socket = fleaCore.GetSocket(); 
        string[] res = socket.GetDecodedData();
        fleaCore.Log("from server : " + res[0]);
    }

    // Update is called once per frame
    public void Update()
    {
    }


    ///////////// default methods //////////////////
    public void setInteractive()
    {
        DisableInteractive = true;
    }
    public void unsetInteractive()
    {
        DisableInteractive = false;
    }

    public void SendGlobalEvent(string eventName)
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
    }

    public void setActive()
    {
        gameObject.SetActive(true);
    }
    public void setInActive()
    {
        gameObject.SetActive(false);
    }

}
