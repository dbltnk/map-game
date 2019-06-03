using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitedMapManager : MonoBehaviour
{
    public Texture2D VisitedTex;
    public GameObject Player;

    void Start () {
        VisitedTex = new Texture2D((int)Data.HeightMapWidth, (int)Data.HeightMapHeight);
        ClearVisitedTex();
        StartCoroutine("UpdateVisitedTexture");
    }

    public void ClearVisitedTex () {
        for (int y = 0; y < VisitedTex.height; y++) {
            for (int x = 0; x < VisitedTex.width; x++) {
                VisitedTex.SetPixel(x, y, Data.ColorUnexplored);
            }
        }
        VisitedTex.Apply();
    }

    IEnumerator UpdateVisitedTexture () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, 0, VisitedTex.width);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, 0, VisitedTex.height);

        for (int y = (int)pZ - Data.RangeSight / 2; y < (int)pZ + Data.RangeSight / 2; y++) {
            for (int x = (int)pX - Data.RangeSight / 2; x < (int)pX + Data.RangeSight / 2; x++) {
                VisitedTex.SetPixel(x, y, Data.ColorVisited);
            }
        }
        VisitedTex.Apply();
        yield return new WaitForSeconds(.1f);
        StartCoroutine("UpdateVisitedTexture");
    }
}
