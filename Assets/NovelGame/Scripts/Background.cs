using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class BackGround : MonoBehaviour
{
    [SerializeField, Tooltip("背景の前に置いてフェード等をする単色背景")]
    private Image _monochromatic;

    [SerializeField, Tooltip("背景1")]
    private Image _BackGround1;

    [SerializeField, Tooltip("背景2")]
    private Image _BackGround2;

    [SerializeField, Tooltip("フェードする時間")]
    private float _fadeInterbal = 1.5f;

    private Tweener _tweener;

    private bool _backGroundChange = false;

    public bool BackGroundChange
    {
        get
        {
            return _backGroundChange;
        }
    }

    private void Start()
    {
        MonochromaticFadeOut();
    }

    private void FadeIn(Image backGround)
    {
        _backGroundChange = true;

        var c = backGround.color;
        c = new Color(c.r, c.g, c.b, 255);

        DOTween.To(() => backGround.color,
            x => backGround.color = x,
            c, _fadeInterbal).OnComplete(() => _backGroundChange = false);
    }

    private void FadeOut(Image backGround)
    {
        _backGroundChange = true;

        var c = backGround.color;
        c = new Color(c.r, c.g, c.b, 0);

        DOTween.To(() => backGround.color,
            x => backGround.color = x,
            c, _fadeInterbal).OnComplete(() => _backGroundChange = false);
    }

    private void MonochromaticFadeOut()
    {
        if (!_backGroundChange)
        {
            FadeOut(_monochromatic);
        }
    }

    private void CrossFade()
    {
        if (!_backGroundChange)
        {
            FadeOut(_BackGround1);
            FadeIn(_BackGround2);
        }
    }
}
