using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagePicker : MonoBehaviour
{
    [SerializeField] private bool displayOnStart = true;
    [SerializeField] private string[] supportedFileTypes = new string[] { "image/*", "public.image"};
    [SerializeField] private Renderer targetRenderer;

    private void Start()
    {
        if (displayOnStart)
            DisplayImagePicker();
    }

    public void DisplayImagePicker()
    {
        NativeFilePicker.PickFile(OnFilePicked, supportedFileTypes);
    }

    private void OnFilePicked(string path)
    {
        Debug.Log($"FOO image at {path}");
        var tex = LoadTexture(path);
        targetRenderer.material.mainTexture = tex; 
    }

    private Texture2D LoadTexture(string imagePath)
    {
        byte[] data = File.ReadAllBytes(imagePath);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(data);
        return tex;
    }
}
