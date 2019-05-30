using UnityEngine;

public class Data : MonoBehaviour
{
    public static int HeightMapWidth = 512;
    public static int HeightMapHeight = 512;

    public static float ScaleMin = 0.1f;
    public static float ScaleMax = 5f;

    public static int ScreenShotWidth = 1024;
    public static int ScreenShotHeight = 1024;

    public static string PathHeightMap = "/../1_heightmap.png";
    public static string PathScreenShot = "/../2_screenshot.png";
    public static string PathCombined = "/../3_combined.png";
    public static string PathRecovered = "/../4_recovered.png";

    public static int BitsHidden = 5;
}
