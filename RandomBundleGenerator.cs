using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBundleGenerator : MonoBehaviour
{
  public TextAsset textJSON;
  public ShelfHandler shelfHandler;
  public GameObject shelfPrefab;


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
    GameObject REC_Shelf = (GameObject)Instantiate(shelfPrefab, transform.position, Quaternion.Euler(Vector3.forward));
    REC_Shelf.GetComponent<shelfBehavior>().height= 10;
    REC_Shelf.name = "REC3";
    for(int i = 0; i < 20; i++){
      REC_Shelf.GetComponent<shelfBehavior>().bundles.Add(new shelfBehavior.Bundle() {partNumber = 12,
        location = new shelfBehavior.Location(){aisle = "REC", column = 3, shelf = "C"},
        position = new Vector3(26f * i, 0f, 0f),
        quantity = 200,
        size = new Vector3(3f, Random.Range(0.2f, 2.5f), Random.Range(2.5f, 3.2f))});
    }

    for(int i = 0; i < 10; i++){
      REC_Shelf.GetComponent<shelfBehavior>().bundles.Add(new shelfBehavior.Bundle() {partNumber = 12,
        //location = new shelfBehavior.Location(){aisle = currentAisle.aisle, column = currentColumn.column, shelf = currentShelf.shelf},
        position = new Vector3(26f * i, 0f, 0f),
        quantity = 200,
        size = new Vector3(3f, Random.Range(0.2f, 2.5f), Random.Range(4f, 6f))});
    }
  }

  // Update is called once per frame
  void Update()
  {
  }
}
