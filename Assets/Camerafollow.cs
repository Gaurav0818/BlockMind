using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform player;
    Vector3 pos;
    Vector3 defaultPos;
    Camera camera;
    GameObject Object;
    // Start is called before the first frame update
    void Start()
    {
        pos = Vector3.zero;
        defaultPos = transform.position;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;
        
        pos.x = player.position.x;
        pos.z = player.position.z;

        transform.position = pos + defaultPos;

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)) ;

        Physics.Raycast(ray,out hit);

        if(hit.collider)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (Object)
                {
                    Object.GetComponent<MeshRenderer>().enabled = true;
                    Object = null;
                }
            }
            else
            {
                if( hit.collider.gameObject != Object)
                {
                    if (Object)
                    {
                        Object.GetComponent<MeshRenderer>().enabled = true;
                    }
                    Object = hit.collider.gameObject;
                    Object.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
            
    }
}
