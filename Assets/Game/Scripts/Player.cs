using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController playercontroller;

    // player speed variable
    [SerializeField]
    private float pSpeed = 3.5f;
    //gravity
    private float gravity = 9.81f;

    // Start is called before the first frame update
    void Start()
    {
        playercontroller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * pSpeed;
        velocity.y -= gravity;
        velocity = transform.transform.TransformDirection(velocity);
        playercontroller.Move(velocity * Time.deltaTime);
    }
}
