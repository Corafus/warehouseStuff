using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shelfBehavior : MonoBehaviour
{

    public float altitude;
    public float height;
    public float width;
    public bool fitsHorizontally = true;
    public List<ShelfColumn> shelfColumns = new List<ShelfColumn>();
    public List<Bundle> bundles = new List<Bundle>();
    public GameObject bundlePrefab;






    [System.Serializable]
    public class ShelfColumn
    {
      public float availableWidth;
      public float availableHeight;
      public float position;
      public List<Bundle> bundles = new List<Bundle>();
    }

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
    public class Location
    {
      public string aisle;
      public int column;
      public string shelf;
    }

    [System.Serializable]
    public class BundleList
    {
      public List<Bundle> bundles = new List<Bundle>();
    }

    void Start()
    {
      if(bundles.Count>0){
        shelfColumns.Insert(0, new ShelfColumn(){availableWidth = bundles[0].size.z});
        shelfColumns[0].bundles = bundles;
      }

      if(shelfColumns.Count>0){
          shelfColumns[0].bundles = Sort(shelfColumns[0].bundles);
      }

      //keep cutting the shelf columns down to size to fit the shelf height
      for(var i = 0; i < shelfColumns.Count; i++){
        shelfColumns = DivideStack(shelfColumns, shelfColumns[i].bundles);
      }

      InstantiateBundles();
      repositionBundles();




      //repositionBundles();







      //check if bundles fit into the shelf horizontally once all of the sorting and sizing is complete
      //if it doesn't fit, nothing happens just yet. A simply gets unchecked
      fitsHorizontally = checkFit(shelfColumns);
      repositionBundles2();



    }



    public void repositionBundles(){
        int bundleCount = 0;
        for(var j = 0; j < shelfColumns.Count; j++){
          if(j>0){
            shelfColumns[j].position = shelfColumns[j-1].position + shelfColumns[j-1].availableWidth + 0.25f; //for a gap
          }
        float currentHeight = transform.position.y;
        for(var i = 0; i < shelfColumns[j].bundles.Count; i++){
            //bundle.name = this.name + " Bundle: " + bundleCount.ToString();
            shelfColumns[j].bundles[i].position.x = transform.position.x;
            shelfColumns[j].bundles[i].position.y = transform.position.y;
            shelfColumns[j].bundles[i].position.z = transform.position.z + shelfColumns[j].position + 0.75f; //added .75 for the supportwidth
            shelfColumns[j].bundles[i].position.y = currentHeight;
            currentHeight += shelfColumns[j].bundles[i].size.y + 0.2f; //for the 2x4

            Vector3 bundleGameObjectPosition = new Vector3(shelfColumns[j].bundles[i].position.x, shelfColumns[j].bundles[i].position.y, shelfColumns[j].bundles[i].position.z);
            GameObject bundle = GameObject.Find(this.name + " Bundle: " + bundleCount);
            bundle.transform.position = bundleGameObjectPosition;
            bundleCount++;
        }
      }
    }

    public void repositionBundles2(){
        int bundleCount = 0;
        for(var j = 0; j < shelfColumns.Count; j++){
        for(var i = 0; i < shelfColumns[j].bundles.Count; i++){
            Vector3 bundleGameObjectPosition = new Vector3(shelfColumns[j].bundles[i].position.x, shelfColumns[j].bundles[i].position.y, shelfColumns[j].bundles[i].position.z);
            GameObject bundle = GameObject.Find(this.name + " Bundle: " + bundleCount);
            bundle.transform.position = bundleGameObjectPosition;
            bundleCount++;
        }
      }
    }

    // Update is called once per frame
    void Update()
    {
      //repositionBundles2();
    }

    List<Bundle> Sort(List<Bundle> unsortedBundles){


      //create a new list of bundles that will be returned as our sorted list
      List<Bundle> newList = new List<Bundle>();
      //if the list has 2 or more bundles, there is likely something to be sorted
      if(unsortedBundles.Count>1){
        //insert the first unsorted item into the sorted list
        newList.Insert(0, unsortedBundles[0]);

        for(var i = 1; i < unsortedBundles.Count; i++){
          for(var j = newList.Count -1; j >= 0; j--){
            //if the last item in the new list is larger than the next item in our old List,
            //we can just stop right here and attach the item
            if(newList[j].size.z >= unsortedBundles[i].size.z){
              newList.Insert(j + 1, unsortedBundles[i]);
              break;
            //if EVERY new list is smaller than the next item in our old list,
            //we add the old item to the bottom of the new list, since it's the largest item
            //that's why we need the && j==0
            } else if(newList[j].size.z <= unsortedBundles[i].size.z && j == 0) {
              newList.Insert(0, unsortedBundles[i]);
              break;
            //this is for when the old list item is bigger,
            //but we're not yet to the end of the list, so we just CONTINUE to the next item,
            //until one of the top two if statements has been satisfied
            } else if(newList[j].size.z < unsortedBundles[i].size.z && j > 0){
              continue;
            }
          }
        }
        //return the new, sorted list
        return newList;
      } else {
        //if the original unsorted list only has one bundle, we just return it
        //because there's nothing to sort
        return unsortedBundles;
      }
    }

    List<ShelfColumn> DivideStack(List<ShelfColumn> originalShelfColumnList,List<Bundle> undividedStack){
      List<ShelfColumn> newShelfColumns = new List<ShelfColumn>();
      List<Bundle> newList2 = new List<Bundle>();
      float stackHeight = 0.72f; //fix dis
      int divisionPoint = 0;
      bool shouldWeCut = false;
      for(var i = 0; i < undividedStack.Count; i++){
        stackHeight += (undividedStack[i].size.y + 0.2f); //for the 2x4
        if(stackHeight > height){
          divisionPoint = i;
          shouldWeCut = true;
          break;
        }
      }

      if(shouldWeCut){
        for(var i = undividedStack.Count-1; i >= divisionPoint; i--){
          newList2.Insert(0, undividedStack[i]);
          undividedStack.Remove(undividedStack[i]);
        }
        originalShelfColumnList.RemoveAt(originalShelfColumnList.Count-1);

        originalShelfColumnList.Add(new ShelfColumn(){availableWidth = undividedStack[0].size.z, bundles = undividedStack});
        originalShelfColumnList.Add(new ShelfColumn(){availableWidth = newList2[0].size.z, bundles = newList2});
        return originalShelfColumnList;

      } else{
        //originalShelfColumnList.RemoveAt(0);

        return originalShelfColumnList;
      }
    }

    public void AddStack(List<Bundle> stack1, List<Bundle> stack2){
      for(var i = 0; i < stack2.Count; i++){
        stack1.Add(stack2[i]);
      }
    }

    bool checkFit(List<ShelfColumn> shelfColumns){
      float materialWidth = 0.75f;
      for(var i = 0; i < shelfColumns.Count; i++){
        materialWidth += shelfColumns[i].availableWidth + 0.25f;
      }
      if(materialWidth > width){
        return false;
      } else {
        return true;
      }
    }

    //just combines all of the stacks back into one stack. no sorting. no exceptions
    public void combinesStacks(){
      while(shelfColumns.Count > 1){
        for(var j = 0; j < shelfColumns[1].bundles.Count; j++){
          shelfColumns[0].bundles.Add(shelfColumns[1].bundles[j]);
          shelfColumns[1].bundles.RemoveAt(0);
          }
        if(shelfColumns[1].bundles.Count == 0){
            shelfColumns.RemoveAt(1);
          }
      }
    }

    void InstantiateBundles(){
      //create all of the bundle game objects and adjust their parts accordingly
      int bundleCount = 0;
      for(var j = 0; j < shelfColumns.Count; j++){
        for(var i = 0; i < shelfColumns[j].bundles.Count; i++){

            Vector3 bundleGameObjectPosition = new Vector3(shelfColumns[j].bundles[i].position.x, shelfColumns[j].bundles[i].position.y, shelfColumns[j].bundles[i].position.z);
            GameObject bundle = (GameObject)Instantiate(bundlePrefab, bundleGameObjectPosition, Quaternion.Euler(Vector3.forward));
            bundle.name = this.name + " Bundle: " + bundleCount.ToString();

            Transform bundleChild = bundle.transform.GetChild(0);
            bundleChild.transform.localScale = new Vector3(shelfColumns[j].bundles[i].size.x, shelfColumns[j].bundles[i].size.y, shelfColumns[j].bundles[i].size.z);
            bundleChild.transform.localPosition = new Vector3(0, shelfColumns[j].bundles[i].size.y/2 + 0.2f, shelfColumns[j].bundles[i].size.z/2);

            Transform bundleFoot1 = bundle.transform.GetChild(1);
            bundleFoot1.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].bundles[i].size.z *  0.2f);

            Transform bundleFoot2 = bundle.transform.GetChild(2);
            bundleFoot2.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].bundles[i].size.z *  0.8f);


            Transform band1 = bundleFoot1.transform.GetChild(0);
            //band1.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].bundles[i].size.z *  0.8f);
            band1.transform.localScale = new Vector3(1f, 1f,  (band1.transform.localScale.z - 0.03f) * shelfColumns[j].bundles[i].size.y);


            Transform band2 = bundleFoot2.transform.GetChild(0);
            //band2.transform.localPosition = new Vector3( 0f,  0.1f, shelfColumns[j].bundles[i].size.z *  0.8f);
            band2.transform.localScale = new Vector3(1f, 1f,  (band2.transform.localScale.z - 0.03f) * shelfColumns[j].bundles[i].size.y);
            bundleCount++;
        }
      }
    }
}
