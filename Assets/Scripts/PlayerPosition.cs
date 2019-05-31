﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject Player;
    public Texture2D VisitedTex;

    private void Start () {
        VisitedTex = new Texture2D((int)Data.HeightMapWidth, (int)Data.HeightMapHeight);

        for (int y = 0; y < VisitedTex.height; y++) {
            for (int x = 0; x < VisitedTex.width; x++) {
                VisitedTex.SetPixel(x, y, Data.ColorUnexplored);
            }
        }
        VisitedTex.Apply();
    }

    void Update() {
        MovePlayerMarker();
        UpdateVisitedTexture();
    }

    private void UpdateVisitedTexture () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, 0, Data.HeightMapWidth);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, 0, Data.HeightMapHeight);

        for (int y = (int)pZ - Data.RangeSight/2 ; y < (int)pZ + Data.RangeSight/2; y++) {
            for (int x = (int)pX - Data.RangeSight/2; x < (int)pX + Data.RangeSight/2; x++) {
                VisitedTex.SetPixel(x, y, Data.ColorVisited);
            }
        }

        VisitedTex.Apply();
    }

    private void MovePlayerMarker () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, -1f * Data.MiniMapWidth / 2, Data.MiniMapWidth / 2);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, -1f * Data.MiniMapHeight / 2, Data.MiniMapHeight / 2);
        gameObject.transform.localPosition = new Vector3(pX, pZ, 0f);
    }
}
