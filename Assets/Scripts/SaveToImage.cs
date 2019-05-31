using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveToImage : MonoBehaviour

{
    Camera cam;
    public GameObject mapMarker;
    PlayerPosition pPos;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        pPos = mapMarker.GetComponent<PlayerPosition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            print("saving a picture of the map");
            SaveImage(RTImage(cam), Data.PathScreenShot);

            print("saving visited map");
            SaveImage(pPos.VisitedTex, Data.PathVisited);

            print("overriding visited information");
            SaveImage(OverrideVisited(Data.PathScreenShot, Data.PathVisited), Data.PathOverriden);

            print("adding height map");
            SaveImage(Steganography.HideImage(Data.PathScreenShot, Data.ScreenShotWidth, Data.ScreenShotHeight, Data.PathHeightMap, Data.HeightMapWidth, Data.HeightMapHeight, Data.BitsHidden), Data.PathCombined);
        }
    }

    Texture2D OverrideVisited(string pathScreenshot, string pathVisited) {

        // THIS NEEDS T GO FROM A 512 MAP TO A 1024 MAP


        Texture2D texMerged = new Texture2D(Data.ScreenShotWidth, Data.ScreenShotHeight);
        Color32[] pixMerged = texMerged.GetPixels32();

        byte[] bytesOriginal = File.ReadAllBytes(Application.dataPath + pathScreenshot);
        Texture2D texOriginal = new Texture2D(Data.ScreenShotWidth, Data.ScreenShotHeight);
        texOriginal.LoadImage(bytesOriginal);
        Color32[] pixOriginal = texOriginal.GetPixels32();

        byte[] bytesVisited = File.ReadAllBytes(Application.dataPath + pathVisited);
        Texture2D texVisited = new Texture2D(Data.ScreenShotWidth, Data.ScreenShotHeight);
        texVisited.LoadImage(bytesVisited);
        Color32[] pixVisited = texVisited.GetPixels32();

        for (int i = 0; i < pixVisited.Length; i++) {
            if (pixVisited[i] == Data.ColorUnexplored) {
                pixMerged[i] = Data.ColorUnexplored;
            }
            else {
                pixMerged[i] = pixOriginal[i];
            }
        }

        texMerged.SetPixels32(pixMerged);
        return texMerged;
    }

    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RTImage (Camera cam) {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        // Render the camera's view.
        cam.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        print(image);
        return image;
    }

    public static void SaveImage(Texture2D tex, string path) {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + path, bytes);
    }
}
