using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Steganography : MonoBehaviour
{
    // Hide bm_hidden inside bm_visible and return the result.
    // Source: http://csharphelper.com/blog/2016/09/use-steganography-to-hide-one-picture-inside-another-in-c/
    public static Texture2D HideImage (string path_visible, int width_visible, int height_visible, string path_hidden, int width_hidden, int height_hidden, int hidden_bits) {
        byte[] bytesVisible = File.ReadAllBytes(Application.dataPath + path_visible);
        Texture2D texVisible = new Texture2D(width_visible, height_visible);
        texVisible.LoadImage(bytesVisible);
        Color32[] pixVisible = texVisible.GetPixels32();

        byte[] bytesHidden = File.ReadAllBytes(Application.dataPath + path_hidden);
        Texture2D texHidden = new Texture2D(width_hidden, height_hidden);
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
            } else {
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

    // Recover a hidden image.
    // Source: http://csharphelper.com/blog/2016/09/use-steganography-to-hide-one-picture-inside-another-in-c/
    public static Texture2D RecoverImage (string path_combined, int width_visible, int height_visible, int hidden_bits, int width_hidden, int height_hidden) {
        byte[] bytesCombined = File.ReadAllBytes(Application.dataPath + path_combined);
        Texture2D texCombined = new Texture2D(width_visible, height_visible);
        texCombined.LoadImage(bytesCombined);
        Color32[] pixCombined = texCombined.GetPixels32();

        int shift = (8 - hidden_bits);
        int hidden_mask = 0xFF >> shift;
        Color32[] pixRecovered = new Color32[width_hidden * height_hidden];

        for (int x = 0; x < pixCombined.Length; x++) {

            if (x >= pixRecovered.Length) {
                break;
            } else {
                Color32 clr_combined = pixCombined[x];

                int r = (clr_combined.r & hidden_mask) << shift;
                int g = (clr_combined.g & hidden_mask) << shift;
                int b = (clr_combined.b & hidden_mask) << shift;
                int a = (clr_combined.a & hidden_mask) << shift;

                pixRecovered[x] = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
            }
        }
        Texture2D texRecovered = new Texture2D(width_hidden, height_hidden);
        texRecovered.SetPixels32(pixRecovered);

        return texRecovered;
    }
}
