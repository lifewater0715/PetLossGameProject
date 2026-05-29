using UnityEngine;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.IO;
using System;
public class ImgLoader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer UserspriteRenderer;  //이미지를 표시할 SpriteRenderer
    [SerializeField] private SpriteRenderer UserspriteRendereBg; //이미지를 표시할 SpriteRenderer
    private GameObject UserImg;
    private GameObject UserImgBG;

    [SerializeField] private bool ImgCutteMod = false;

    void Start()
    {
        UserImg = UserspriteRenderer.transform.gameObject;
        UserImgBG = UserspriteRendereBg.transform.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (ImgCutteMod == true)
        {
            ImgMover(UserImg);
            ImgMover(UserImgBG);

            float Scrollpow = Input.mouseScrollDelta.y;

            ImgZoomer(UserImg,Scrollpow);
            ImgZoomer(UserImgBG,Scrollpow);
        }
    }

    public void ImgZoomer(GameObject ZoomTrg, float ScrPow)
    {
        if (ScrPow > 0)
        {
            //Debug.Log("SCR UP");
            ZoomTrg.transform.localScale += new Vector3(0.2f,0.2f,0);
        }
        if (ScrPow < 0)
        {
            //Debug.Log("SCR Dwon");
            ZoomTrg.transform.localScale -= new Vector3(0.2f,0.2f,0);
        }
    }

    public void ImgMover(GameObject moveTrg)
    {
        moveTrg.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        return;
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
        ImgCutteMod = true;
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
