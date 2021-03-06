using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleHandler : MonoBehaviour
{

    public TextAsset textJSON;
    public ShelfHandler shelfHandler;


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

          Vector3 bundlePosition = Vector3.zero;

          for(var j = 0; j < shelfHandler.myShelfList.aisles.Length; j++){
            ShelfHandler.Aisle currentAisle = shelfHandler.myShelfList.aisles[j];
            if(currentAisle.aisle == bundleAisle){

              for(var k = 0; k < shelfHandler.myShelfList.aisles[j].columns.Length; k++){
                ShelfHandler.Column currentColumn = currentAisle.columns[k];
                if(currentColumn.column == bundleColumn){

                  for(var l = 0; l < currentColumn.shelves.Length; l++){
                    ShelfHandler.Shelf currentShelf = currentColumn.shelves[l];
                    if(currentShelf.shelf == bundleShelf){


                      GameObject shelfGameObject = GameObject.Find(currentAisle.aisle + currentColumn.column.ToString() + currentShelf.shelf);


                      shelfGameObject.GetComponent<shelfBehavior>().bundles.Add(new shelfBehavior.Bundle() {partNumber = currentBundle.partNumber,
                        location = new shelfBehavior.Location(){aisle = currentAisle.aisle, column = currentColumn.column, shelf = currentShelf.shelf},
                        position = new Vector3(bundlePosition.x, bundlePosition.y, bundlePosition.z),
                        quantity = currentBundle.quantity,
                        size = currentBundle.size});
                    }
                  }
                }
              }
            }
          }
       }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
