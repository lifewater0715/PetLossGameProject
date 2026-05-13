using UnityEngine;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.IO;
using System;
public class ImgLoader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer UserspriteRenderer; //이미지를 표시할 SpriteRenderer
    [SerializeField] private SpriteRenderer UserspriteRendereBg; //이미지를 표시할 SpriteRenderer

    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Open File Browser")]
    public void OnenUI()
    {
        FileBrowser.SetFilters(false, ".png", ".jpg", ".jpeg");
        FileBrowser.ShowLoadDialog(onSuccess, onCancel, 0, false, null, null);
    }
    public void pickMode()
    {
        Debug.Log("Single file selection mode.");
    }
    public void onSuccess(string[] paths)
    {
        Debug.Log("Selected: " + string.Join(", ", paths));

        LoadImage(paths[0]);
    }
    public void onCancel()
    {
        Debug.Log("File selection cancelled.");
    }

    public void LoadImage(string path) //이미지 로드 함수
    {
        byte[] bytes = File.ReadAllBytes(path);
        string url = "file:///" + path;
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        UserspriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        UserspriteRendereBg.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

    }
}
