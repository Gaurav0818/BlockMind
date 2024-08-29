using Unity.Netcode;
using UnityEngine;

public class ConnectionButton : MonoBehaviour
{
    public void JoinServerButton()
    {
        NetworkManager.Singleton.StartClient();
    }
    
    public void HostServerButton()
    {
        NetworkManager.Singleton.StartHost();
    }
}
