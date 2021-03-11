using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfHandler : MonoBehaviour
{

    public TextAsset textJSON;
    public GameObject signPrefab;
    public GameObject supportStructurePrefab;
    public GameObject shelfBasePrefab;
    public AisleList myShelfList = new AisleList();
    public Shelf mybundle = new Shelf();


    [System.Serializable]
    public class AisleList
    {
      public Aisle[] aisles;
    }

    [System.Serializable]
    public class Aisle
    {
      public string aisle;
      public Vector3 position;
      public Column[] columns;
    }

    [System.Serializable]
    public class Column
    {
      public int column;
      public float width;
      public Shelf[] shelves;
    }

    [System.Serializable]
    public class Shelf
    {
      public string shelf;
      public float altitude;
      public float height;
      public List<ShelfColumn> shelfColumns = new List<ShelfColumn>();
    }

    [System.Serializable]
    public class ShelfColumn
    {
      public float availableWidth;
      public float availableheight;
      public float position;
    }

    [System.Serializable]
    public class ShelfList
    {
      public Shelf[] shelves;
    }

    // Start is called before the first frame update
    void Start()
    {
        //this is how I speculate we will save data from Unity to the json file
        //string json = JsonUtility.ToJson(myBundleList);

        myShelfList = JsonUtility.FromJson<AisleList>(textJSON.text);

       for(int i = 0; i < myShelfList.aisles.Length; i++){
         Aisle currentAisle = myShelfList.aisles[i];
         Vector3 signPosition = new Vector3(currentAisle.position.x, 20, currentAisle.position.z);
         GameObject sign = (GameObject)Instantiate(signPrefab, signPosition, Quaternion.Euler(Vector3.forward));
         Vector3 initialSupportStructurePosition = new Vector3(currentAisle.position.x, 0, currentAisle.position.z);
         GameObject initialSupportStructure = (GameObject)Instantiate(supportStructurePrefab, initialSupportStructurePosition, Quaternion.Euler(Vector3.forward));

         for(int j = 0; j < currentAisle.columns.Length; j++){
           Column currentColumn = currentAisle.columns[j];
           Vector3 supportStructurePosition = new Vector3(currentAisle.position.x, 0, currentAisle.position.z + (currentColumn.width * j) + currentColumn.width);
           GameObject supportStructure = (GameObject)Instantiate(supportStructurePrefab, supportStructurePosition, Quaternion.Euler(Vector3.forward));
           supportStructure.name = currentAisle.aisle + (j+2).ToString();


           for(int k = 0; k < currentColumn.shelves.Length; k++){
             Shelf currentShelf = currentColumn.shelves[k];
             Vector3 shelfBasePosition = new Vector3(currentAisle.position.x, currentShelf.altitude, currentAisle.position.z + (currentColumn.width * j) );

             //find out the difference between shelf above and current shelf and set that to shelf verticalSpace
             float newAvailableHeight = 4f;
             if(k > 0){
               newAvailableHeight = Mathf.Abs(currentColumn.shelves[k-1].altitude - currentShelf.altitude) - 1f;
             }

              GameObject shelfBase = (GameObject)Instantiate(shelfBasePrefab, shelfBasePosition, Quaternion.Euler(Vector3.forward));

              string aisle1 = currentAisle.aisle;
              int column1 = currentColumn.column;
              string shelf1 = currentShelf.shelf;
              shelfBase.name = aisle1 + column1.ToString() + shelf1;
              shelfBase.GetComponent<shelfBehavior>().altitude = currentShelf.altitude;
              shelfBase.GetComponent<shelfBehavior>().width = currentColumn.width;
              if(k > 0){
                shelfBase.GetComponent<shelfBehavior>().height = Mathf.Abs(currentShelf.altitude - currentColumn.shelves[k-1].altitude); //AKA the previous shelf
              } else {
                shelfBase.GetComponent<shelfBehavior>().height = 5f;
              }

              //shelfBase.GetComponent<shelfBehavior>().shelfColumns.Add(new shelfBehavior.ShelfColumn(){ availableWidth = currentColumn.width, availableHeight = newAvailableHeight});

              Transform OSBboard = shelfBase.transform.GetChild(0);
              OSBboard.transform.localScale = new Vector3(2.8f, 0.5f, currentColumn.width - 1f);
              OSBboard.transform.localPosition = new Vector3(-0.25f, -0.25f, currentColumn.width/2 + 0.25f);

              Transform orangeBeam1 = shelfBase.transform.GetChild(1);
              orangeBeam1.transform.localPosition = new Vector3(1.025f, -0.44f, currentColumn.width/2 + 0.19f);
              orangeBeam1.transform.localScale = new Vector3(0.5f, 0.68255f, currentColumn.width - 0.1f);

              Transform orangeBeam2 = shelfBase.transform.GetChild(2);
              orangeBeam2.transform.localPosition = new Vector3(-1.525f, -0.44f, currentColumn.width/2 + 0.19f);
              orangeBeam2.transform.localScale = new Vector3(0.5f, 0.68255f, currentColumn.width - 0.1f);

           }
         }
      }


    }

    // Update is called once per frame
    void Update()
    {
    }
}
