using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

// Source: https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html

public class MapGenerator : MonoBehaviour {
    public Terrain Terrain;
    private RenderTexture rT;
    public GameObject Player;
    public GameObject mapMarker;

    private void Start () {
        rT = new RenderTexture(Data.HeightMapWidth, Data.HeightMapHeight, 16, RenderTextureFormat.ARGB1555);
        rT.Create();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.G)) {
            print("generating a map");
            Texture2D heightMap = GenerateHeightMap(Data.HeightMapWidth, Data.HeightMapHeight);
            WriteHeightMapToFile(heightMap);
            LoadHeightmapFromFile(Data.PathHeightMap);
            Player.GetComponent<ResetToStart>().Reset();
            mapMarker.GetComponent<PlayerPosition>().ClearVisitedTex();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            print("loading a map");
            SaveToImage.SaveImage(Steganography.RecoverImage(Data.PathCombined, Data.ScreenShotWidth, Data.ScreenShotHeight, Data.BitsHidden, Data.HeightMapWidth, Data.HeightMapHeight), Data.PathRecovered);
            LoadHeightmapFromFile(Data.PathRecovered);
            Player.GetComponent<ResetToStart>().Reset();
        }
    }

    Texture2D GenerateHeightMap (int width, int height) {
        Texture2D heightmap = new Texture2D(width, height);
        List<Color32> list = new List<Color32>();

        float seed = Random.Range(0f, 9999f);
        float scale = Random.Range(Data.ScaleMin, Data.ScaleMax);

        float y = 0.0f;
        while (y < height) {
            float x = 0.0f;
            while (x < width) {
                float xCoord = seed + x / width * scale;
                float yCoord = seed + y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) + 0.25f;
                Color c = new Color(sample, sample, sample);
                list.Add(c);
                x++;
            }
            y++;
        }
        Color32[] colors = list.ToArray();
        heightmap.SetPixels32(colors);
        return heightmap;
    }

    void WriteHeightMapToFile(Texture2D tex) {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + Data.PathHeightMap, bytes);
    }

    void UpdateTerrainData (RenderTexture rt) {
        RectInt rI = new RectInt(0, 0, Data.HeightMapWidth, Data.HeightMapHeight);
        Vector2Int v2I = new Vector2Int(0, 0);
        Terrain.terrainData.CopyActiveRenderTextureToHeightmap(rI, v2I, TerrainHeightmapSyncControl.HeightAndLod);
    }
    
    void LoadHeightmapFromFile(string path) {
        byte[] bytes = File.ReadAllBytes(Application.dataPath + path);
        Texture2D tex = new Texture2D(Data.HeightMapWidth, Data.HeightMapHeight);
        tex.LoadImage(bytes);
        Graphics.Blit(tex, rT);
        UpdateTerrainData(rT);
    }
}