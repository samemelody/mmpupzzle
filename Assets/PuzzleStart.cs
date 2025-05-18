using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleStart : MonoBehaviour
{
    // 场景状态存储
    public static string activeCanvas;
    // 背景图控制
    public Image backgroundImage;
    public Sprite backgroundSprite;
    [Range(0,1)]
    public float backgroundAlpha = 1f;
    // 按钮和画布引用
    public Button gameSceneButton;
    public Button listCanvasButton;
    public Button galleryCanvasButton;
    
    public GameObject listCanvas;
    public GameObject galleryCanvas;

    void Awake()
    {
        // 恢复之前保存的界面状态
        if (!string.IsNullOrEmpty(activeCanvas))
        {
            listCanvas.SetActive(activeCanvas == "list");
            galleryCanvas.SetActive(activeCanvas == "gallery");
        }
        // 初始按钮绑定保持不变
        listCanvas.SetActive(false);
        galleryCanvas.SetActive(false);
        
        // 绑定按钮点击事件

        gameSceneButton.onClick.AddListener(LoadGameScene);
        listCanvasButton.onClick.AddListener(SwitchToListCanvas);
        galleryCanvasButton.onClick.AddListener(SwitchToGalleryCanvas);
    }

    // 场景跳转方法
    void LoadGameScene()
    {
        // 保存当前激活的Canvas状态
        activeCanvas = listCanvas.activeSelf ? "list" : 
                      galleryCanvas.activeSelf ? "gallery" : "";
        int randomSeed;
        var listGenerator = FindObjectOfType<ImageListGenerator>();
        
        if (listGenerator?.images != null && listGenerator.images.Length > 0)
        {
            randomSeed = Random.Range(1, listGenerator.images.Length);
        }
        else
        {
            randomSeed = Random.Range(1, 5);
            Debug.LogWarning("使用默认随机数范围");
        }
        GameData.SelectedLevel = randomSeed;
        SceneManager.LoadScene("GameScene");

    }

    // 界面切换方法
    void SwitchToListCanvas()
    {
        listCanvas.SetActive(true);
        galleryCanvas.SetActive(false);
    }

    void SwitchToGalleryCanvas()
    {
        galleryCanvas.SetActive(true);
        listCanvas.SetActive(false);
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
