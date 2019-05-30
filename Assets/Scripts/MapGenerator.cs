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
    public RenderTexture RT;

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    private void Start () {
        RT = new RenderTexture(Data.heightMapWidth, Data.heightMapHeight, 16, RenderTextureFormat.ARGB1555);
        RT.Create();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.G)) {
            print("generating a map");
            //GenerateMap();
            Texture2D heightMap = GenerateHeightMap(Data.heightMapWidth, Data.heightMapHeight);
            WriteHeightMapToFile(heightMap);
            LoadHeightmapFromFile(Data.pathHeightMap);
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            print("loading a map");
            SaveToImage.SaveImage(Steganography.RecoverImage(Data.pathCombined, Data.screenShotWidth, Data.screenShotHeight, Data.bitsHidden, Data.heightMapWidth, Data.heightMapHeight), Data.pathRecovered);
            LoadHeightmapFromFile(Data.pathRecovered);
        }
    }

    Texture2D GenerateHeightMap (int width, int height) {
        Texture2D heightmap = new Texture2D(width, height);
        List<Color32> list = new List<Color32>();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float nx = (float)x / (float)width - 0.5f;
                float ny = (float)y / (float)height - 0.5f;
                float v = Mathf.PerlinNoise(nx, ny);
                Color32 c = new Color32((byte)(v*255), (byte)(v * 255), (byte)(v * 255), 255);
                list.Add(c);
            }
        }
        Color32[] colors = list.ToArray();
        heightmap.SetPixels32(colors);
        return heightmap;
    }

    void WriteHeightMapToFile(Texture2D tex) {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + Data.pathHeightMap, bytes);
    }



    void GenerateMap () {
        scale = Random.Range(1f, 3f);

        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;

        CalcNoise();
        byte[] bytes = noiseTex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + Data.pathHeightMap, bytes);

        RenderTexture rt = new RenderTexture(Data.heightMapWidth, Data.heightMapHeight, 16, RenderTextureFormat.ARGB1555);
        rt.Create();
        Graphics.Blit(noiseTex, rt);
        UpdateTerrainData(rt);
    }

    void UpdateTerrainData (RenderTexture rt) {
        RectInt rI = new RectInt(0, 0, Data.heightMapWidth, Data.heightMapHeight);
        Vector2Int v2I = new Vector2Int(0, 0);
        Terrain.terrainData.CopyActiveRenderTextureToHeightmap(rI, v2I, TerrainHeightmapSyncControl.HeightOnly);
    }

    void CalcNoise () {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height) {
            float x = 0.0F;

            while (x < noiseTex.width) {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord) + 0.25f;
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    void LoadHeightmapFromFile(string path) {
        byte[] bytes = File.ReadAllBytes(Application.dataPath + path);
        Texture2D tex = new Texture2D(Data.heightMapWidth, Data.heightMapHeight);
        tex.LoadImage(bytes);
        Graphics.Blit(tex, RT);
        UpdateTerrainData(RT);
    }

}