using UnityEngine;

public class Data : MonoBehaviour
{
    public static int HeightMapWidth = 1024;
    public static int HeightMapHeight = 1024;

    public static float ScaleMin = 4f;
    public static float ScaleMax = 7f;

    public static int ScreenShotWidth = 1024;
    public static int ScreenShotHeight = 1024;

    public static float TerrainWidth = 1000;
    public static float TerrainHeight = 1000;

    public static float MiniMapWidth = 250;
    public static float MiniMapHeight = 250;

    public static string PathHeightMap = "/../1_heightmap.png";
    public static string PathScreenShot = "/../2_screenshot.png";
    public static string PathVisited = "/../3_visited.png";
    public static string PathHeightOverriden = "/../4_heightmap_overriden.png";
    public static string PathScreenOverriden = "/../5_screenshot_overriden.png";
    public static string PathCombined = "/../6_combined.png";
    public static string PathRecovered = "/../7_recovered.png";

    public static Color ColorUnexplored = new Color(0, 0, 0, 1);
    public static Color ColorVisited = new Color(0, 0, 0, 0);

    public static int RangeSight = 64;

    public static int BitsHidden = 5;

    public static float MapIntoRange (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
