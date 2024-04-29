using System.Collections;
using UnityEngine;

public class OutlineIndicator : MonoBehaviour, IIndicator
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private float indicatorTime;
    private WaitForSeconds waitForSeconds;
    private Material indicatorMaterial;
    private Coroutine indicatorCoroutine;
    private void Awake()
    {
        indicatorMaterial = indicator.GetComponent<Renderer>().material;
        waitForSeconds = new WaitForSeconds(indicatorTime);
    }
    public void ShowIndicator(Color color)
    {
        if (indicatorCoroutine != null) StopCoroutine(indicatorCoroutine);
        indicatorCoroutine = StartCoroutine(ShowIndicatorCo(color));
    }
    private IEnumerator ShowIndicatorCo(Color color)
    {
        indicatorMaterial.color = color;
        indicator.SetActive(true);
        yield return waitForSeconds;
        indicator.SetActive(false);
    }
}
