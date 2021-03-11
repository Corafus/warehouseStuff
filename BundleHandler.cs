using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleHandler : MonoBehaviour
{

    public TextAsset textJSON;
    public ShelfHandler shelfHandler;
    public GameObject stackPrefab;
    public GameObject bundlePrefab;


    [System.Serializable]
    public class Bundle
    {
      public int partNumber;
      public Location location;
      public Vector3 position;
      public int quantity;
      public Vector3 size;
    }

    [System.Serializable]
    public class BundleList
    {
      public List<Bundle> bundles = new List<Bundle>();
    }

    [System.Serializable]
    public class Location
    {
      public string aisle;
      public int column;
      public string shelf;
    }

    [System.Serializable]
    public class ShelfColumn
    {
      public float availableWidth;
      public float availableHeight;
      public float position;
    }

    public BundleList myBundleList = new BundleList();
    public Bundle mybundle = new Bundle();

    // Start is called before the first frame update
    void Start()
    {
        //this is how I speculate we will save data from Unity to the json file
        //string json = JsonUtility.ToJson(myBundleList);

        myBundleList = JsonUtility.FromJson<BundleList>(textJSON.text);

        for(int i = 0; i < myBundleList.bundles.Count; i++){
          Bundle currentBundle = myBundleList.bundles[i];

          string bundleAisle = currentBundle.location.aisle;
          int bundleColumn =  currentBundle.location.column;
          string bundleShelf = currentBundle.location.shelf;

          string bundleName = bundleAisle + bundleColumn.ToString() + bundleShelf + "bundle";
          InstantiateBundles(i, bundleName);



          Vector3 bundlePosition = Vector3.zero;

          for(var j = 0; j < shelfHandler.myShelfList.aisles.Length; j++){
            ShelfHandler.Aisle currentAisle = shelfHandler.myShelfList.aisles[j];
            if(currentAisle.aisle == bundleAisle){

              for(var k = 0; k < shelfHandler.myShelfList.aisles[j].columns.Length; k++){
                ShelfHandler.Column currentColumn = currentAisle.columns[k];
                if(currentColumn.column == bundleColumn){




                  //float currentHeight = 0f;
                  for(var l = 0; l < currentColumn.shelves.Length; l++){

                    ShelfHandler.Shelf currentShelf = currentColumn.shelves[l];


                  }
                }
              }
            }
          }
       }
    }

    void InstantiateBundles(int i, string bundleName){
      Bundle currentBundle = myBundleList.bundles[i];



            GameObject bundle = (GameObject)Instantiate(bundlePrefab, Vector3.zero, Quaternion.Euler(Vector3.forward));

            bundle.name = bundleName;

            Transform bundleChild = bundle.transform.GetChild(0);
            bundleChild.transform.localScale = new Vector3(currentBundle.size.x, currentBundle.size.y, currentBundle.size.z);
            bundleChild.transform.localPosition = new Vector3(0, currentBundle.size.y/2 + 0.2f, currentBundle.size.z/2);

            Transform bundleFoot1 = bundle.transform.GetChild(1);
            bundleFoot1.transform.localPosition = new Vector3( 0f,  0.1f, currentBundle.size.z *  0.2f);

            Transform bundleFoot2 = bundle.transform.GetChild(2);
            bundleFoot2.transform.localPosition = new Vector3( 0f,  0.1f, currentBundle.size.z *  0.8f);


            Transform band1 = bundleFoot1.transform.GetChild(0);
            //band1.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].currentBundle.size.z *  0.8f);
            band1.transform.localScale = new Vector3(1f, 1f,  (band1.transform.localScale.z - 0.03f) * currentBundle.size.y);


            Transform band2 = bundleFoot2.transform.GetChild(0);
            //band2.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].currentBundle.size.z *  0.8f);
            band2.transform.localScale = new Vector3(1f, 1f,  (band2.transform.localScale.z - 0.03f) * currentBundle.size.y);

    }



    // Update is called once per frame
    void Update()
    {
    }
}
