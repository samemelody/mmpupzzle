using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageListGenerator : MonoBehaviour
{
    public Sprite[] images;
    public GameObject StartCanvas;
    public GameObject listCanvas;
    public Image listBackground;
    public ScrollRect listScrollView; // 新增ScrollRect引用
    public Button returnButton;
    void Start()
    {
        returnButton.onClick.AddListener(ReturnToMainMenu);
        // 设置背景图片锚点
       /* if (listCanvas != null)
        {
            if (listBackground != null)
            {
                RectTransform rt = listBackground.rectTransform;
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(Screen.width, Screen.height);
            }
        }*/
        // 原有ListCanvas背景设置保持不动
        GenerateImageList();
    }

    void GenerateImageList()
    {

        // 添加网格布局组件
        // 获取Scroll View的Content对象
        if (listScrollView == null)
        {
            Debug.LogError("未找到ListScrollView引用");
            return;
        }
        Transform content = listScrollView.content;
        Debug.Log($"Content对象状态: {content != null}");
        if (content == null)
        {
            Debug.LogError("未找到ScrollView Content，请检查以下路径：Viewport/Content");
            return;
        }

        // 添加网格布局到Content对象
        var grid = content.gameObject.AddComponent<GridLayoutGroup>();
            Debug.Log($"网格布局参数：列数={grid.constraintCount} 单元格尺寸={grid.cellSize} 间距={grid.spacing}");
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 2;
        grid.cellSize = new Vector2(300, 400);
        grid.spacing = new Vector2(150, 75);
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.padding.top = 80;

        // 调整内容区域位置
        content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -40);
        
        // 添加Content Size Fitter确保正确布局
        var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // 检查图片资源加载状态
        Debug.Log($"加载的图片数量：{images.Length}");
        
        // 调整实例化逻辑
        for (int i = 0; i < images.Length; i++)
        {
            // 创建图片按钮
            GameObject button = new GameObject("ImageButton");
            Debug.Log($"创建按钮对象：{button.name}，父对象：{content?.name}");
            
            // 设置父对象到ScrollView Content
            button.transform.SetParent(content);
            Debug.Log($"按钮父对象设置状态：{button.transform.parent != null}");
            
            // 添加Image组件
            var image = button.AddComponent<Image>();
            image.sprite = images[i];
            image.preserveAspect = true;
            
            // 添加按钮组件
            var btn = button.AddComponent<Button>();
            int current = i;
            btn.onClick.AddListener(() => {
                Debug.Log($"点击按钮{current}，加载场景：GameScene{current}");
                LoadGameScene(current);
            });
            
            // 添加自动布局组件
            button.AddComponent<LayoutElement>();
        }
    }

    // 场景跳转方法
    public void LoadGameScene(int levelIndex)
    {
        GameData.SelectedLevel = levelIndex+1;
        SceneManager.LoadScene("GameScene");
    }

    // 返回主菜单场景
    public void ReturnToMainMenu()
    {

        StartCanvas.SetActive(true);
        listCanvas.SetActive(false);
    }

    // 跳转至设置场景
    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings"); // 设置场景名称
    }

    // Canvas切换方法
    public void SwitchToStartCanvas()
    {
        if(StartCanvas != null)
        {
            StartCanvas.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}