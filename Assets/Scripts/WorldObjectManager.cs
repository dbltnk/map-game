using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class WorldObjectManager : MonoBehaviour
{
    public GameObject PrefWO;
    public GameObject Player;
    List<GameObject> WorldObjects = new List<GameObject>();
    public Texture2D wOTex;

    // Start is called before the first frame update
    void Start () {
        wOTex = new Texture2D((int)Data.HeightMapWidth, (int)Data.HeightMapHeight);
        ClearWOTex();
        StartCoroutine("UpdateWOTex");
    }

    public void ClearWOTex () {
        for (int y = 0; y < wOTex.height; y++) {
            for (int x = 0; x < wOTex.width; x++) {
                wOTex.SetPixel(x, y, Data.ColorWONone);
            }
        }
        wOTex.Apply();
    }

    public void RemoveAllWO () {
        foreach (GameObject wO in WorldObjects) {
            Destroy(wO);
        }
    }

    Tuple<int, int> GetMapPixelForObject (GameObject gO) {
        int x = Mathf.RoundToInt(Data.MapIntoRange(gO.transform.position.x, 0, Data.TerrainWidth, 0, wOTex.width));
        int y = Mathf.RoundToInt(Data.MapIntoRange(gO.transform.position.z, 0, Data.TerrainWidth, 0, wOTex.width));
        return Tuple.Create(x, y);
    }

    Vector3 GetPositionFromMapPixel (int x, int y) {
        int nX = Mathf.RoundToInt(Data.MapIntoRange(x, 0, wOTex.width, 0, Data.TerrainWidth));
        int nZ = Mathf.RoundToInt(Data.MapIntoRange(y, 0, wOTex.width, 0, Data.TerrainWidth));
        return new Vector3(nX, 0f, nZ);
    }

    public void UpdateWOTex () {
        foreach (GameObject wO in WorldObjects) {
            wOTex.SetPixel(GetMapPixelForObject(wO).Item1, GetMapPixelForObject(wO).Item2, Data.ColorWO1);
        }
        wOTex.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            CreateWO();
        }
    }

    private void CreateWO () {
        print("spawning a world object");
        Vector3 spawnPos = new Vector3();
        GameObject cam = Player.transform.GetChild(0).gameObject;

        spawnPos = cam.transform.position + cam.transform.forward * Data.SpawnDistance;

        RaycastHit hitInfo;
        Physics.SphereCast(cam.transform.position, 1f, cam.transform.forward, out hitInfo, Data.SpawnDistance,
                        Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.point != Vector3.zero) spawnPos = hitInfo.point;

        GameObject go = Instantiate(PrefWO, spawnPos, Player.transform.rotation);
        WorldObjects.Add(go);
    }

    public void LoadWOFromFile (string path) {
        byte[] bytesWorldObjects = File.ReadAllBytes(Application.dataPath + path);
        Texture2D texWorldObjects = new Texture2D(Data.HeightMapWidth, Data.HeightMapHeight);
        texWorldObjects.LoadImage(bytesWorldObjects);
        Color32[] pixWorldObjects = texWorldObjects.GetPixels32();

        for (int x = 0; x < texWorldObjects.width; x++) {
            for (int y = 0; y < texWorldObjects.height; y++) {
                if (texWorldObjects.GetPixel(x, y).a == Data.WO1Alpha) {
                    GameObject go = Instantiate(PrefWO, GetPositionFromMapPixel(x, y), Player.transform.rotation);
                    WorldObjects.Add(go);
                }
            }
        }
    }
}
