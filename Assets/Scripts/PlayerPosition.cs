using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject Player;
    Texture2D visitedTex;

    private void Start () {
        visitedTex = new Texture2D((int)Data.HeightMapWidth, (int)Data.HeightMapHeight);

        for (int y = 0; y < visitedTex.height; y++) {
            for (int x = 0; x < visitedTex.width; x++) {
                visitedTex.SetPixel(x, y, Data.ColorUnexplored);
            }
        }
        visitedTex.Apply();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            print("saving visited map");
            SaveToImage.SaveImage(visitedTex, Data.PathVisited);
        }
        MovePlayerMarker();
        UpdateVisitedTexture();
    }

    private void UpdateVisitedTexture () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, 0, Data.HeightMapWidth);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, 0, Data.HeightMapHeight);
        //visitedTex.SetPixel((int)pX, (int)pZ, Data.ColorVisited);

        for (int y = (int)pZ - Data.RangeSight/2 ; y < (int)pZ + Data.RangeSight/2; y++) {
            for (int x = (int)pX - Data.RangeSight/2; x < (int)pX + Data.RangeSight/2; x++) {
                visitedTex.SetPixel(x, y, Data.ColorVisited);
            }
        }

        visitedTex.Apply();
    }

    private void MovePlayerMarker () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, -1f * Data.MiniMapWidth / 2, Data.MiniMapWidth / 2);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, -1f * Data.MiniMapHeight / 2, Data.MiniMapHeight / 2);
        gameObject.transform.localPosition = new Vector3(pX, pZ, 0f);
    }
}
