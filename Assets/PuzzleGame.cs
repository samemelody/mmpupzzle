using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
public class PuzzleGame : MonoBehaviour

    
{
    public Sprite[] puzzlePieces; // 拼图块的精灵数组
    public Button[] puzzleButtons; // 拼图块的按钮数组
    public Image completeImage;
    public Image canvasBackground; // 用于引
    public Button returnButton; // 返回按钮
    private int emptyIndex; // 空白块的索引
    int refxpos = 560;
    int refypos = 168;
    public Button restartButton; // 新增：重启按钮
    private int currentLevel; // 新增当前关卡字段
    private float elapsedTime = 0f; // 记录经过的时间
    private bool isTimerRunning = false; // 标记计时器是否正在运行
    public TMP_Text timerText; // 用于显示计时的文本组件

    void Start()
    {
        currentLevel = GameData.SelectedLevel;
        Debug.Log("game start index:" + currentLevel);
        // 初始化拼图块
        InitializePuzzle();
        canvasBackground.sprite = Resources.Load<Sprite>($"source{currentLevel}/finish1");
        Color color = canvasBackground.color;
            color.a = 0.2f; // 0.3 表示 30% 的透明度，你可以根据需求调整
            canvasBackground.color = color;
        completeImage.gameObject.SetActive(false);
        // 打乱拼图
        ShufflePuzzle();
        // 初始化计时器文本
        if (timerText != null)
        {
            timerText.text = "00:00";
        }
        // 为重启按钮添加点击事件
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnToStartScene);
        }
    }

    // 新增：重启游戏方法
    void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    void RestartGame()
    {
        // 停止计时器
        isTimerRunning = false;
        // 重置计时器时间
        elapsedTime = 0f;
        if (timerText != null)
        {
            timerText.text = "00:00";
        }

        // 隐藏完成图片
        completeImage.gameObject.SetActive(false);

        // 重新初始化拼图
        //InitializePuzzle();
        // 重新打乱拼图
        ShufflePuzzle();
    }

    void InitializePuzzle()
    {
        Debug.Log("InitializePuzzle");

        // 为 puzzlePieces 数组赋值
        puzzlePieces = new Sprite[12];
        for (int i = 0; i < 12; i++)
        {
            // 加载 source1 目录下的 01.jpg 到 02.jpg
            puzzlePieces[i] = Resources.Load<Sprite>($"source{currentLevel}/{i + 1}");
            if (puzzlePieces[i] == null)
            {
                Debug.LogError($"无法加载精灵资源 source{currentLevel}/{i + 1}，请检查资源是否存在。");
            }
        }

        // 为每个按钮添加点击事件
        for (int i = 0; i < puzzleButtons.Length; i++)
        {
            int index = i;
            Debug.LogError("lizhuod add button onclick");
            puzzleButtons[i].onClick.AddListener(() => OnPuzzleButtonClick(index));
        }
        // 找到空白块的索引
        emptyIndex = puzzlePieces.Length - 1;
    }

void ShufflePuzzle()
    {
        // 使用一个可靠的打乱算法，确保可解
        List<int> indices = new List<int>();
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            indices.Add(i);
        }

        // 打乱列表
        indices.Shuffle();

        // 检查逆序数，确保拼图可解
        while (!IsSolvable(indices))
        {
            indices.Shuffle();
        }

        // 根据打乱后的索引重新排列拼图
        for (int i = 0; i < puzzleButtons.Length; i++)
        {
            puzzleButtons[i].image.sprite = puzzlePieces[indices[i]];
            if (indices[i] == puzzlePieces.Length - 1)
            {
                emptyIndex = i;
                // 设置空白块按钮的颜色为黑色且不透明
                Color blackColor = Color.black;
                blackColor.a = 1f; // 不透明
                puzzleButtons[i].image.color = blackColor;
            }
            else
            {
                // 恢复非空白按钮的颜色为白色且完全不透明
                Color normalColor = Color.white;
                normalColor.a = 1f; // 完全不透明
                puzzleButtons[i].image.color = normalColor;
            }
        }
    }

    // 检查拼图是否可解
    bool IsSolvable(List<int> indices)
    {
        int inversions = 0;
        int blankRow = 0;
        int rows = 4; // 4 行
        int cols = 3; // 3 列

        // 计算逆序数
        for (int i = 0; i < indices.Count - 1; i++)
        {
            for (int j = i + 1; j < indices.Count; j++)
            {
                if (indices[i] != puzzlePieces.Length - 1 && indices[j] != puzzlePieces.Length - 1 && indices[i] > indices[j])
                {
                    inversions++;
                }
            }
        }

        // 找到空白块所在的行
        for (int i = 0; i < indices.Count; i++)
        {
            if (indices[i] == puzzlePieces.Length - 1)
            {
                blankRow = i / cols;
                break;
            }
        }

        // 根据列数的奇偶性判断可解性
        if (cols % 2 == 1)
        {
            return inversions % 2 == 0;
        }
        else
        {
            int blankRowFromBottom = rows - blankRow;
            return (blankRowFromBottom % 2 == 1) == (inversions % 2 == 0);
        }
    }

    List<int> GetAdjacentIndices2(int index)
    {
        List<int> adjacentIndices = new List<int>();
        int row = index / 3;
        int col = index % 3;

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newRow = row + dx[i];
            int newCol = col + dy[i];
            int newIndex = newRow * 3 + newCol;

            if (newRow >= 0 && newRow < 3 && newCol >= 0 && newCol < 3)
            {
                adjacentIndices.Add(newIndex);
            }
        }

        return adjacentIndices;
    }

    void OnPuzzleButtonClick(int index)
    {
        Debug.Log("OnPuzzleButtonClick");

        // 首次点击按钮时启动计时器
        if (!isTimerRunning)
        {
            isTimerRunning = true;
        }

        // 获取点击方块所在的行和列，修改为 4 行 3 列逻辑
        int clickRow = index / 3;
        int clickCol = index % 3;

        // 获取空白方块所在的行和列，修改为 4 行 3 列逻辑
        int emptyRow = emptyIndex / 3;
        int emptyCol = emptyIndex % 3;

        // 如果在同一行
        if (clickRow == emptyRow)
        {
            int step = clickCol > emptyCol ? 1 : -1;
            for (int col = emptyCol + step; col != clickCol + step; col += step)
            {
                int currentIndex = clickRow * 3 + col;
                SwapPieces(currentIndex, currentIndex - step);
                if (currentIndex - step == emptyIndex)
                {
                    emptyIndex = currentIndex;
                }
            }
        }
        // 如果在同一列
        else if (clickCol == emptyCol)
        {
            int step = clickRow > emptyRow ? 1 : -1;
            for (int row = emptyRow + step; row != clickRow + step; row += step)
            {
                int currentIndex = row * 3 + clickCol;
                SwapPieces(currentIndex, currentIndex - step * 3);
                if (currentIndex - step * 3 == emptyIndex)
                {
                    emptyIndex = currentIndex;
                }
            }
        }

        // 检查是否完成拼图
        if (IsPuzzleComplete())
        {
            Debug.Log("恭喜你，完成拼图！");
            string spriteName = "finish1";
            if (elapsedTime < 30f)
            {
                spriteName = "finish2";
            }
            completeImage.sprite = Resources.Load<Sprite>($"source{currentLevel}/{spriteName}");
            completeImage.gameObject.SetActive(true);
            // 完成游戏时停止计时器
            isTimerRunning = false;
            
            // 保存最佳记录
            string key = $"BestTime_Level{currentLevel}";
            float bestTime = PlayerPrefs.GetFloat(key, float.MaxValue);
            if (elapsedTime < bestTime) {
                PlayerPrefs.SetFloat(key, elapsedTime);
                PlayerPrefs.Save();
            }
        }
    }

    void Update()
    {
        // 如果计时器正在运行，则更新时间
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // 同时需要修改 GetAdjacentIndices 函数适配 4 行 3 列
    List<int> GetAdjacentIndices(int index)
    {
        List<int> adjacentIndices = new List<int>();
        int row = index / 3;
        int col = index % 3;

        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int newRow = row + dx[i];
            int newCol = col + dy[i];
            int newIndex = newRow * 3 + newCol;

            // 修改边界检查逻辑适配 4 行 3 列
            if (newRow >= 0 && newRow < 4 && newCol >= 0 && newCol < 3)
            {
                adjacentIndices.Add(newIndex);
            }
        }

        return adjacentIndices;
    }

    void SwapPieces(int index1, int index2)
    {
        // 交换两个方块的精灵
        Sprite temp = puzzleButtons[index1].image.sprite;
        puzzleButtons[index1].image.sprite = puzzleButtons[index2].image.sprite;
        puzzleButtons[index2].image.sprite = temp;

        // 恢复交换的两个方块的透明度为正常
        Color normalColor = Color.white;
        normalColor.a = 1f; // 恢复正常透明度
        puzzleButtons[index1].image.color = normalColor;
        puzzleButtons[index2].image.color = normalColor;

        // 找到新的空白方块索引
        if (puzzleButtons[index1].image.sprite == puzzlePieces[puzzlePieces.Length - 1])
        {
            emptyIndex = index1;
        }
        else if (puzzleButtons[index2].image.sprite == puzzlePieces[puzzlePieces.Length - 1])
        {
            emptyIndex = index2;
        }

        // 将新的空白方块设置为黑色并调整透明度
        Color blackTransparentColor = Color.black;
        blackTransparentColor.a = 0.2f; // 20% 透明度
        puzzleButtons[emptyIndex].image.color = blackTransparentColor;
    }

    bool IsPuzzleComplete()
    {
        // 检查拼图是否完成
        for (int i = 0; i < puzzleButtons.Length; i++)
        {
            if (puzzleButtons[i].image.sprite != puzzlePieces[i])
            {
                return false;
            }
        }
        return true;
    }
}

// 扩展方法：打乱列表的顺序
public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}