using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static CancellationTokenSource CancellationTokenSource;
    public static CancellationToken Token;
    private SaveData saveData;
    private LevelData levelData;

    [Inject]
    public void Construct(SaveData saveData, LevelData levelData)
    {
        this.saveData = saveData;
        this.levelData = levelData;
    }
    private void Awake()
    {
        CancellationTokenSource = new CancellationTokenSource();
        Token = CancellationTokenSource.Token;
        Application.targetFrameRate = 60;
    }
    public void Restart()
    {
        SceneManager.LoadScene(saveData.CurrentLevelIndex);
    }
    public void LoadNextLevel()
    {
        saveData.CurrentLevelIndex++;
        saveData.CurrentGold += levelData.GoldPrize;
        if (saveData.CurrentLevelIndex > saveData.MaxLevelIndex) saveData.CurrentLevelIndex = 0;

        SceneManager.LoadScene(saveData.CurrentLevelIndex);
    }
    
    private void OnDestroy()
    {
        CancellationTokenSource.Cancel();
        CancellationTokenSource.Dispose();
    }
}
