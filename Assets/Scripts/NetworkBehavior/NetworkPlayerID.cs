using System;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NetworkPlayerID : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI playerIDText;
    
    NetworkVariable<ulong> playerIdNetwork = new NetworkVariable<ulong>();
    bool isPlayerIdSet = false;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playerIdNetwork.Value = OwnerClientId + 1;
        }
        base.OnNetworkSpawn();
        
    }

    private void Update()
    {
        if (!isPlayerIdSet)
        {
            SetPlayerIdText();
            isPlayerIdSet = true;
        }
    }
    
    public void SetPlayerIdText()
    {
        playerIDText.text = $"P{playerIdNetwork.Value}";
    }
}
