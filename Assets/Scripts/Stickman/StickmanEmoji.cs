using System.Collections;
using UnityEngine;

public class StickmanEmoji : MonoBehaviour, IEmotion
{
    [SerializeField] private GameObject smileEmoji;
    [SerializeField] private GameObject angryEmoji;
    [SerializeField] private float angryFaceActiveTime;
    private GameObject activeEmoji;
    private Coroutine angryCoroutine;

    public void Angry()
    {
        if(angryCoroutine != null) StopCoroutine(angryCoroutine);

        activeEmoji?.SetActive(false);
        angryEmoji.SetActive(true);
        activeEmoji = angryEmoji;
        angryCoroutine = StartCoroutine(DisableTimer());
    }

    public void Happy()
    {
        activeEmoji?.SetActive(false);
        smileEmoji.SetActive(true);
        activeEmoji = smileEmoji;
    }
    public void NoEmotion()
    {
        activeEmoji?.SetActive(false);
    }
    private IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(angryFaceActiveTime);
        activeEmoji?.SetActive(false);
    }
}
