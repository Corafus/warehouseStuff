using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeSpan : MonoBehaviour
{
    public int life = 30;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(life > 0){
          life--;
        } else {
          Destroy(gameObject);
        }
    }
}
