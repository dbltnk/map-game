using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        float pX = Data.MapIntoRange(Player.transform.position.x, 0, Data.TerrainWidth, -1f * Data.MiniMapWidth / 2, Data.MiniMapWidth / 2);
        float pZ = Data.MapIntoRange(Player.transform.position.z, 0, Data.TerrainHeight, -1f * Data.MiniMapWidth / 2, Data.MiniMapWidth / 2);
        gameObject.transform.localPosition = new Vector3(pX, pZ, 0f);
    }
}
