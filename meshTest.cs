using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshTest : MonoBehaviour
{
    // Start is called before the first frame update

    public string partNumber;
    public int quantity;
    bool stackInPairs;
    public int textureWidth;
    public int textureHeight;
    public float maxBundleDepth;
    public float pixelsPerInch;
    public float bundleHeight;
    public float bundleDepth;
    public float bundleLength;
    public float howManyPiecesWide;
    public float howManyPiecesTall;
    public Texture[] textures;
    public Renderer m_Renderer;

    void Start()
    {




      m_Renderer = GetComponent<Renderer> ();
      quantity = 50;
      stackInPairs = true;
      textureWidth = 226;
      textureHeight = 58;
      maxBundleDepth = 3;
      pixelsPerInch = 50;
      m_Renderer.material.SetTexture("_MainTex", textures[2]);
      textureWidth = textures[2].width;
      textureHeight= textures[2].height;

      if(stackInPairs){
        howManyPiecesWide = maxBundleDepth * 12 / (textureWidth/pixelsPerInch);
      } else {
        howManyPiecesWide = maxBundleDepth * 12 / (textureWidth/pixelsPerInch)/2;
      }

      if(stackInPairs){
        howManyPiecesTall = Mathf.Ceil(quantity/howManyPiecesWide)/2;
      } else {
        howManyPiecesTall = Mathf.Ceil(quantity/howManyPiecesWide);
      }


      bundleHeight = (textureHeight/pixelsPerInch) * (howManyPiecesTall/12);
      bundleDepth = (textureWidth/pixelsPerInch) * (howManyPiecesWide/12);





        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8]; //8
        Vector2[] uv = new Vector2[8]; //8
        int[] triangles = new int[36]; //36

        //profile side 1
        vertices[0] = new Vector3(0,0);
        vertices[1] = new Vector3(0, bundleHeight);
        vertices[2] = new Vector3(bundleDepth, bundleHeight);
        vertices[3] = new Vector3(bundleDepth, 0);

        //profile side 2
        vertices[4] = new Vector3(0,0, 3);
        vertices[5] = new Vector3(0, bundleHeight, 3);
        vertices[6] = new Vector3(bundleDepth, bundleHeight, 3);
        vertices[7] = new Vector3(bundleDepth, 0, 3);



        uv[0] = new Vector2(0,0);
        uv[1] = new Vector2(0,howManyPiecesTall);
        uv[2] = new Vector2(howManyPiecesWide, howManyPiecesTall);
        uv[3] = new Vector2(howManyPiecesWide,0);

        uv[4] = new Vector2(0,0);
        uv[5] = new Vector2(0,howManyPiecesTall);
        uv[6] = new Vector2(howManyPiecesWide,howManyPiecesTall);
        uv[7] = new Vector2(howManyPiecesWide,0);


        //the numbers on the right of the equal sign refer to what vertex
        //the side of the triangle will start from
        //triangles need to go clockwise, else the normals will be flipped
        //and it will be invisible

        //profile side 1
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        //profile side 2
        triangles[6] = 6;
        triangles[7] = 5;
        triangles[8] = 4;
        triangles[9] = 7;
        triangles[10] = 6;
        triangles[11] = 4;

        //very top
        triangles[12] = 1;
        triangles[13] = 5;
        triangles[14] = 2;
        triangles[15] = 2;
        triangles[16] = 5;
        triangles[17] = 6;


        //very bottom
        triangles[18] = 3;
        triangles[19] = 4;
        triangles[20] = 0;
        triangles[21] = 3;
        triangles[22] = 7;
        triangles[23] = 4;

        //leftSide
        triangles[24] = 4;
        triangles[25] = 5;
        triangles[26] = 1;
        triangles[27] = 4;
        triangles[28] = 1;
        triangles[29] = 0;


        //rightSide
        triangles[30] = 6;
        triangles[31] = 7;
        triangles[32] = 2;
        triangles[33] = 2;
        triangles[34] = 7;
        triangles[35] = 3;


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight){
      return new Vector2((float)x/textureWidth, (float)y/textureHeight);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
