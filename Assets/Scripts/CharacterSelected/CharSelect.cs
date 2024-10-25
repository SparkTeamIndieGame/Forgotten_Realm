using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class SpawnManager : NetworkBehaviour
{
    public GameObject WariorPrefab;
    public GameObject MagePrefab;
    public Button MageButton;
    public Button WarriorButton;
    

    void Start()
    {
        MageButton.onClick.AddListener(() => SpawnObject(1));
        WarriorButton.onClick.AddListener(() => SpawnObject(2));
    }
    void SpawnObject(int objectIndex)
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
        {
            SpawnObjectServerRpc(objectIndex, NetworkManager.Singleton.LocalClientId);
        }
    }
    

    [ServerRpc(RequireOwnership = false)]
    void SpawnObjectServerRpc(int objectIndex, ulong clientId, ServerRpcParams rpcParams = default)
    {
        GameObject objectToSpawn = objectIndex == 1 ? MagePrefab : WariorPrefab;
        GameObject instance = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
        NetworkObject networkObject = instance.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId);
    }
}