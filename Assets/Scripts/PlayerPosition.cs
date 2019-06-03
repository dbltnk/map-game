using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject Player;

    void Update() {
        MovePlayerMarker();
    }

    void MovePlayerMarker () {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, -1f * Data.MiniMapWidth / 2f, Data.MiniMapWidth / 2f);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, -1f * Data.MiniMapHeight / 2f, Data.MiniMapHeight / 2f);
        gameObject.transform.localPosition = new Vector3(pX, pZ, 0f);
    }
}
