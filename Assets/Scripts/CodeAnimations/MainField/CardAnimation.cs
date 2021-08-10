using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimation : MonoBehaviour
{
    [Header("Dissolve References")]
    [SerializeField] Card card;
    [SerializeField] Material dissolveMaterial;
    [SerializeField] float dissolveAnimSpeed;

    public IEnumerator Dissolve(bool inOut)
    {
        card.design.material = new Material(dissolveMaterial);
        card.backgroundImage.material = new Material(dissolveMaterial);

        card.design.material.mainTexture = card.design.sprite.texture;
        card.backgroundImage.material.mainTexture = card.backgroundImage.sprite.texture;

        float x, y;
        x = inOut ? 0 : 1;

        while (inOut? x < 1 : x > 0)
        {
            x += (inOut ? 1 : -1) * (dissolveAnimSpeed* Time.deltaTime);
            y = (-x * x + 2 * x);

            card.backgroundImage.material.SetFloat("_Fill", y);
            card.design.materialForRendering.SetFloat("_Fill", y);

            SetAlphaUIElements(x);

            yield return null;
        };
        OnEndDissolveAnimation(inOut);
    }
    public void OnEndDissolveAnimation(bool inOut, Action<int> DoAfter = null)
    {
        if(!inOut)
            Destroy(transform.parent.gameObject);
        
    }

    void SetAlpha<T>(T graphic, float alpha)
        where T : Graphic
    {
        var tempColor = graphic.color;
        tempColor.a = alpha;
        graphic.color = tempColor;
    }

    void SetAlphaUIElements(float alpha)
    {
        SetAlpha(card.nameText, alpha);
        SetAlpha(card.descText, alpha);
        SetAlpha(card.lifeText, alpha);
        SetAlpha(card.goldText, alpha);
        SetAlpha(card.powerText, alpha);
        SetAlpha(card.imageShadow, alpha);
        SetAlpha(card.descShadow, alpha);
    }
}
