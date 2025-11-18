using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public float fadetime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition =new Vector3(0f, -100f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadetime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1f, fadetime);
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition =new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -100f), fadetime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(1f, fadetime);
    }
}
