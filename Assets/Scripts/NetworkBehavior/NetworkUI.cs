using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    public void StartServerOnClick()
    {
        NetworkManager.Singleton.StartServer();
        Debug.Log("Server started");
    }
    public void StartHostOnClick()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host started");
    }
    public void StartClientOnClick()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client started");
    }
}
