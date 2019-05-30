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
            SaveImage(RTImage(cam), "map");
        }

        if (Input.GetKeyDown("c")) {
            print("combining images");

            //HideImageAinB(pixA, pixB);
            SaveImage(HideImage("/../map.png", "/../heightmap.png", 4), "combined");
        }
    }

    // Hide bm_hidden inside bm_visible and return the result.
    // Source: http://csharphelper.com/blog/2016/09/use-steganography-to-hide-one-picture-inside-another-in-c/
    Texture2D HideImage (string path_visible, string path_hidden, int hidden_bits) 
    {
        byte[] bytesVisible = File.ReadAllBytes(Application.dataPath + path_visible);
        Texture2D texVisible = new Texture2D(1024, 1024);
        texVisible.LoadImage(bytesVisible);
        Color32[] pixVisible = texVisible.GetPixels32();

        byte[] bytesHidden = File.ReadAllBytes(Application.dataPath + path_hidden);
        Texture2D texHidden = new Texture2D(512, 512);
        texHidden.LoadImage(bytesHidden);
        Color32[] pixHidden = texHidden.GetPixels32();
               
        int shift = (8 - hidden_bits);
        int visible_mask = 0xFF << hidden_bits;
        int hidden_mask = 0xFF >> shift;
        Color32[] pixCombined = new Color32[pixVisible.Length];

        for (int x = 0; x < pixVisible.Length; x++) {

            Color32 clr_visible = pixVisible[x];

            // the hidden image is generally smaller than the visible one so we keep the visible pics color data intact
            if (x >= pixHidden.Length) {
                pixCombined[x] = new Color32(clr_visible.r, clr_visible.g, clr_visible.b, clr_visible.a);
            }
            else {
                Color32 clr_hidden = pixHidden[x];

                int r = (clr_visible.r & visible_mask) +
                    ((clr_hidden.r >> shift) & hidden_mask);
                int g = (clr_visible.g & visible_mask) +
                    ((clr_hidden.g >> shift) & hidden_mask);
                int b = (clr_visible.b & visible_mask) +
                    ((clr_hidden.b >> shift) & hidden_mask);
                int a = (clr_visible.a & visible_mask) +
                    ((clr_hidden.a >> shift) & hidden_mask);

                pixCombined[x] = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
            }
        }

        Texture2D texCombined = new Texture2D(texVisible.width, texVisible.height);
        texCombined.SetPixels32(pixCombined);

        return texCombined;
    }

    Texture2D HideImageAinB (Color32[] A, Color32[] B) {

        print("-----------AAAAAAA-----------------");
        int cA = 0;
        foreach (Color pixel in A) {
            if (cA >= 10) break;
            cA++;
            print(string.Concat(pixel.r, " ", pixel.g, " ", pixel.b, " ", pixel.a, " "));
            print(pixel);
        }
        print("-----------------------------------");
        print("-----------BBBBBBB-----------------");
        int cB = 0;
        foreach (Color pixel in B) {
            if (cB >= 10) break;
            cB++;
            print(string.Concat(pixel.r, " ", pixel.g, " ", pixel.b, " ", pixel.a, " "));
            print(pixel);
        }
        print("-----------------------------------");
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

    void SaveImage(Texture2D tex, string name) {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../" + name + ".png", bytes);
    }
}
