using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public Transform player;
    Vector3 pos;
    Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        pos = Vector3.zero;
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos.x = player.position.x;
        pos.z = player.position.z;

        transform.position = pos + defaultPos;
    }
}
