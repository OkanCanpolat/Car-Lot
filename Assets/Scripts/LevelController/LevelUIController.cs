using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelUIController : MonoBehaviour
{
    private SaveData saveData;
    private LevelData LevelData;
    private SignalBus signalBus;
    private GameManager gameManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private TMP_Text totalGoldText;
    [SerializeField] private GameObject celebrationParticle;
    [SerializeField] private GameObject levelFisnishPanel;
    [SerializeField] private GameObject finalGoldAnimImage;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;

    [Inject]
    public void Construct(SignalBus signalBus, SaveData saveData, LevelData LevelData, GameManager gameManager)
    {
        this.signalBus = signalBus;
        this.saveData = saveData;
        this.LevelData = LevelData;
        this.gameManager = gameManager;
        signalBus.Subscribe<LevelFinishSignal>(OnLevelFinished);
    }

    private void Start()
    {
        levelNumberText.text = saveData.CurrentLevelIndex.ToString();
        totalGoldText.text = saveData.CurrentGold.ToString();
    }
    public void ContinueButton()
    {
        StartCoroutine(ContinueButtonC());
    }

    private void OnLevelFinished()
    {
        StartCoroutine(OnLevelFinishedC());
    }
    private IEnumerator ContinueButtonC()
    {
        restartButton.enabled = false;
        continueButton.enabled = false;
        continueButton.GetComponent<Image>().color = Color.grey;
        yield return StartCoroutine(GoldAnimation());
        totalGoldText.text = (saveData.CurrentGold + LevelData.GoldPrize).ToString();
        yield return new WaitForSeconds(1);
        gameManager.LoadNextLevel();
    }
    private IEnumerator OnLevelFinishedC()
    {
        celebrationParticle.SetActive(true);
        yield return new WaitForSeconds(1);
        levelFisnishPanel.SetActive(true);
    }
    private IEnumerator GoldAnimation()
    {
        finalGoldAnimImage.SetActive(true);

        RectTransform imageTransform = finalGoldAnimImage.transform as RectTransform;
        RectTransform destinationTransform = totalGoldText.transform as RectTransform;

        Transform totalGoldCurrentParent = totalGoldText.transform.parent;
        totalGoldText.transform.SetParent(canvas.transform, true);
        Vector2 totalGoldCurrentPosition = destinationTransform.anchoredPosition;
        totalGoldText.transform.SetParent(totalGoldCurrentParent, true);

        finalGoldAnimImage.transform.SetParent(canvas.transform, true);
        
        Vector2 startPos = imageTransform.anchoredPosition;
        Vector2 endPos = totalGoldCurrentPosition;
        
        float t = 0;
        while (t < 1)
        {
            imageTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return null;
        }
        finalGoldAnimImage.SetActive(false);
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<LevelFinishSignal>(OnLevelFinished);
    }
}
