using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class net : NetworkManager {

    public Text text;

    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("OnPlayerConnected");
    }

    void FixedUpdate()
    {

        //text.text = _view.owner.ToString();

    }
}
