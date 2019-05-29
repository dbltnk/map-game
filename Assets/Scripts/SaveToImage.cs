using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveToImage : MonoBehaviour

{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            print("taking a screenshot");
            SaveImage(RTImage(cam));
        }

        if (Input.GetKeyDown("c")) {
            print("combining images");

            byte[] bytesA = File.ReadAllBytes(Application.dataPath + "/../heightmap.png");
            Texture2D texA = new Texture2D(512, 512);
            texA.LoadImage(bytesA);
            Color[] pixA = texA.GetPixels();

            byte[] bytesB = File.ReadAllBytes(Application.dataPath + "/../map.png");
            Texture2D texB = new Texture2D(512, 512);
            texB.LoadImage(bytesB);
            Color[] pixB = texB.GetPixels();

            HideImageAinB(pixA, pixB);
        }
    }

    Texture2D HideImageAinB (Color[] A, Color[] B) {

        int counter = 0;
        foreach (Color pixel in A) {
            if (counter >= 10) break;
            counter++;
            print(string.Concat(pixel.r, " ", pixel.g, " ", pixel.b, " ", pixel.a, " "));
            print(pixel);
        }


        Texture2D C = new Texture2D(512, 512);
        return C;
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

    void SaveImage(Texture2D tex) {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../map.png", bytes);
    }
}
