using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 moveDir;
    Vector3 movePoint;
    public float speed;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        movePoint = transform.position;
    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                movePoint=hit.point;  
            }

        }
        moveDir = (movePoint - transform.position).normalized;
    }

    void LateUpdate()
    {
        characterController.Move(moveDir * speed * Time.deltaTime);
    }
}
