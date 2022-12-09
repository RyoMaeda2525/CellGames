using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundColor : MonoBehaviour
{
    private Image _image;

    private BackGround _bg;

    private void Start()
    {
        _bg = GetComponentInParent<BackGround>();
    }

    public void FadeIn(float fadeInterbal) 
    {
        if (_image == null) 
        {
            _image = GetComponent<Image>();
        }

        var c = _image.color;
        c = new Color(c.r, c.g, c.b, 255);

        DOTween.To(() => _image.color,
            x => _image.color = x,
            c, fadeInterbal).OnComplete(() => FadeComplate());
    }

    public void FadeOut(float fadeInterbal)
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        var c = _image.color;
        c = new Color(c.r, c.g, c.b, 0);

        DOTween.To(() => _image.color,
            x => _image.color = x,
            c, fadeInterbal).OnComplete(() => FadeComplate());
    }

    private void FadeComplate()
    {
        _bg.BackGroundChange = false;
    }
}
