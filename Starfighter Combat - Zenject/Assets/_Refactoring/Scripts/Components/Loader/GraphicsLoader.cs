using UnityEngine;

public enum FPSPreset
{
    FPS_30 = 30,
    FPS_60 = 60,
    FPS_180 = 180,
    FPS_500 = 500,
    FPS_999 = 999,
    FPS_Unlimited = -1
}

public class GraphicsLoader
{
    public static void LoadPreset(int FPSPreset)
    {
        Application.targetFrameRate = FPSPreset;
    }
}
