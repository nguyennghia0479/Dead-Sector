using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public enum OutputResolution
{
    Window, HD_720p, FHD_1080p, UHD_4K, Portrait
}

public class CaptureImage : MonoBehaviour
{
    [Header("Camera & Resolution Settings")]
    public Camera targetCamera;
    public OutputResolution resolutionMode = OutputResolution.FHD_1080p;

    [Header("Storage Settings")]
    [Tooltip("Folder inside Assets. Assets/CaptureImages")]
    public string targetFolder = "_Project/CaptureImages";

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
            CaptureTransparent();
    }

    public void CaptureTransparent()
    {
        if (targetCamera == null) targetCamera = Camera.main;

        if (string.IsNullOrEmpty(targetFolder))
            targetFolder = "Screenshots";

        int width = 1920;
        int height = 1080;

        switch (resolutionMode)
        {
            case OutputResolution.Window:
                width = Screen.width;
                height = Screen.height;
                break;
            case OutputResolution.HD_720p:
                width = 1280;
                height = 720;
                break;
            case OutputResolution.FHD_1080p:
                width = 1920;
                height = 1080;
                break;
            case OutputResolution.UHD_4K:
                width = 3840;
                height = 2160;
                break;
            case OutputResolution.Portrait:
                width = 256;
                height = 256;
                break;
        }

        RenderTexture rt = new(width, height, 32, RenderTextureFormat.ARGB32);
        RenderTexture.active = rt;
        targetCamera.targetTexture = rt;
        targetCamera.Render();

        Texture2D screenShot = new(width, height, TextureFormat.ARGB32, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        string folderPath = Path.Combine(Application.dataPath, targetFolder);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Path.Combine(folderPath, $"Screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");

        File.WriteAllBytes(filename, bytes);
        Debug.Log($"Saved at Assets/{targetFolder}: {filename}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Destroy(screenShot);
    }
}
