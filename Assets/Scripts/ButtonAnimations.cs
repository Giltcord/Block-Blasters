using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonAnimations : MonoBehaviour
{
  public List<GameObject> items = new List<GameObject>();
  public float fadetime = 1f;

  private void Start()
  {
    StartCoroutine(ItemsAnimation());
  }

  IEnumerator ItemsAnimation()
  {
    foreach (var item in items)
    {
      item.transform.localScale = Vector3.zero;
    }

    foreach (var item in items)
    {
      item.transform.DOScale(1f, fadetime).SetEase(Ease.OutBounce);
      yield return new WaitForSeconds(0.25f);
    }
  }
}
