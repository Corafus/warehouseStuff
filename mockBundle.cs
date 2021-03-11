using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mockBundle : MonoBehaviour
{


    public Vector3[] path;
    public GameObject stack1;
    public GameObject stack2;
    public GameObject stack3;
    float stack2Width = 1000f;

    public string from;
    public GameObject inputFrom;
    //public string index;
    //public GameObject inputIndex;
    public string to;
    public GameObject inputTo;
    public float speed = 8f;
    public GameObject plopSound;



    void Start()
    {
      //StackBlocks();


    }

    void Update()
    {


    }

    public void StackBlocks(){
      from = inputFrom.GetComponent<Text>().text;
      //index= inputIndex.GetComponent<Text>().text;
      to = inputTo.GetComponent<Text>().text;
      stack1 = GameObject.Find(from);
      stack2 = GameObject.Find(to);
      compareSizes();
    }

    void compareSizes(){


      if(stack2Width > stack1.transform.GetChild(stack1.transform.childCount-1).GetChild(0).localScale.z){
        //Debug.Log("searching stack1");
        stackSearch(stack1, stack2);

      } else {
        Debug.Log("searching stack2");
        stackSearch(stack2, stack1);

      }


      //yield return StartCoroutine(FollowPath(stack3, stack2));
      //stack2Width = stack2.transform.GetChild(stack2.transform.childCount-1).GetChild(0).localScale.z;
    }

    IEnumerator FollowPath(GameObject stack3, GameObject destinationStack){
      foreach(Vector3 waypoint in path){
        yield return StartCoroutine(Move(waypoint, stack3));
        speed+=8f;
        if(speed==16){
          GameObject sound = (GameObject)Instantiate(plopSound, Vector3.zero, Quaternion.Euler(Vector3.forward));
        }
      }
      speed = 8f;
      for(var i = stack3.transform.childCount -1; i >=0; i--){ //this should be to work for both destination and starting stacks
        Transform finishedBlock = stack3.transform.GetChild(i);
        finishedBlock.parent = null;
        finishedBlock.parent = destinationStack.transform; //this should be to work for both destination and starting stacks
        //saying 'stack2' will break it
      }


      Debug.Log("Made the drop");
      if(stack2.transform.childCount ==0){
        stack2Width = 1000f;
      } else {
        stack2Width = stack2.transform.GetChild(stack2.transform.childCount-1).GetChild(0).localScale.z;
      }

      //Debug.Log(stack2Width);
    }

    IEnumerator Move(Vector3 destination, GameObject stack){
      while(stack.transform.position!=destination){
        stack.transform.position = Vector3.MoveTowards(stack.transform.position, destination, speed * Time.deltaTime);
        yield return null;
      }
    }

    public void setPath(GameObject smallTop, GameObject bigTop){
      float smallTopHeight = smallTop.transform.position.y;
      for(var j = 0; j < smallTop.transform.childCount; j++){
        smallTopHeight+= smallTop.transform.GetChild(j).GetChild(0).localScale.y + 0.2f;
      }
      float bigTopHeight = bigTop.transform.position.y;
      for(var j = 0; j < bigTop.transform.childCount; j++){
        bigTopHeight+= bigTop.transform.GetChild(j).GetChild(0).localScale.y + 0.2f;
      }
      path[0].x = smallTop.transform.position.x;
      if(smallTopHeight > bigTopHeight){
        path[0].y = stack3.transform.position.y + 0.5f;
        path[1].y = stack3.transform.position.y + 0.5f;
      }
      else if(smallTopHeight < bigTopHeight){
        path[0].y = bigTopHeight + 0.5f;
        path[1].y = bigTopHeight + 0.5f;
      }
      path[0].z = smallTop.transform.position.z;
      path[1].x = bigTop.transform.position.x;
      path[1].z = bigTop.transform.position.z;
      path[2].x = bigTop.transform.position.x;
      path[2].y = bigTopHeight;
      path[2].z = bigTop.transform.position.z;
    }

    IEnumerator splitStack(int verticalPosition, GameObject smallTop, GameObject bigTop){
      stack3.transform.position = smallTop.transform.GetChild(verticalPosition).position;
      for(var i = smallTop.transform.childCount-1; i >= verticalPosition; i--){
        Transform firstBlock = smallTop.transform.GetChild(i);
        firstBlock.transform.parent = null;
        firstBlock.transform.parent = stack3.transform;
      }
      //Debug.Log("Collected from starting stack at index " + verticalPosition.ToString());
      setPath(smallTop, bigTop);
      yield return StartCoroutine(FollowPath(stack3, bigTop));

      compareSizes();
    }

    void stackSearch(GameObject smallTop, GameObject bigTop){
      //remember that stack1 doesn't refer to the stack1 in our game here

      if(bigTop.transform.childCount ==0){
        stack2Width = 1000f;
      } else {
        stack2Width = bigTop.transform.GetChild(bigTop.transform.childCount-1).GetChild(0).localScale.z;
      }


      for(var i = smallTop.transform.childCount-1; i>=0; i--){
        if(smallTop.transform.GetChild(i).GetChild(0).localScale.z <= stack2Width
        && i==0){
          StartCoroutine(splitStack(i, smallTop, bigTop));
          //Debug.Log("Got to the bottom of the stack");
          break;
        }



        //there's no condition for if i@0 is greater than the other stack, but I don't think that can happen,
        //think it's covered by this next condition
        /*if(smallTop.transform.GetChild(i).GetChild(0).localScale.z < stack2Width &&
        smallTop.transform.GetChild(i).GetChild(0).localScale.z < smallTop.transform.GetChild(i-1).GetChild(0).localScale.z){*/

        if(smallTop.transform.GetChild(i).GetChild(0).localScale.z <= smallTop.transform.GetChild(i-1).GetChild(0).localScale.z &&
        smallTop.transform.GetChild(i-1).GetChild(0).localScale.z <= stack2Width){
          //this hasn't worked somehow
          continue;
        } else {
          StartCoroutine(splitStack(i, smallTop, bigTop));
          //Debug.Log("Splitting the stack at " + i.ToString());
          break;
        }
      }

    }
}
