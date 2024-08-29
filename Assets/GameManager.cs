using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camerafollow camerafollow;
    
    public void SetPlayer(Transform player)
    {
        camerafollow.player = player;
    }
}
