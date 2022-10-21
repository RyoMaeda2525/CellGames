using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class BackGround : MonoBehaviour
{
    [SerializeField, Tooltip("�w�i�̑O�ɒu���ăt�F�[�h��������P�F�w�i")]
    private Image _monochromatic;

    [SerializeField, Tooltip("�w�i1")]
    private Image _BackGround1;

    [SerializeField, Tooltip("�w�i2")]
    private Image _BackGround2;

    [SerializeField, Tooltip("�t�F�[�h���鎞��")]
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
