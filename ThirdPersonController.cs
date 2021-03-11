using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    public float walkSpeed =2;
    public float runSpeed = 6;

    public float speedSmoothTime = 0.2f;
    float speedSmoothVelocity;
    float currentSpeed;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    //uncomment all of the lines referring to the animator, please
    public Animator animator;
    Transform cameraT;

    // Start is called before the first frame update
    void Start()
    {
      cameraT = Camera.main.transform;
      //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = input.normalized;
        if(inputDirection!=Vector2.zero){
          float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
          transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        //toggle whether character is running or not. uses SHIFT KEY or square button
        bool running = false;
        /*if(Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Circle")){
          running = true;
        }*/

        //sets the movement targetSpeed and the walk/run animation to be appropriate for whether we walking or running
        float targetSpeed;
        float animationSpeedPercent;
        if(running==true){
          targetSpeed = runSpeed * inputDirection.magnitude;
          animationSpeedPercent = 1 * inputDirection.magnitude;
        } else {
          targetSpeed = walkSpeed * inputDirection.magnitude;
          animationSpeedPercent = 0.5f * inputDirection.magnitude;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        //just move'n around of course
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);



    }
}
