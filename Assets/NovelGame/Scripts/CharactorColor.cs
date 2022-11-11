using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorColor : MonoBehaviour
{
    private Image _image = null;

    [SerializeField]
    private bool fadeComplate = false;

    public bool FadeComplate { get { return fadeComplate; } }

    private void Update()
    {
        Debug.Log(_image.color);
    }

    public void AlphaZero(float fadeInterbal)
    {
        if (_image == null) { _image = GetComponent<Image>(); }
        fadeComplate = false;

        var c = _image.color;
        c = new Color(c.r, c.g, c.b, 0);

        DOTween.To(() => _image.color,
            x => _image.color = x,
            c, fadeInterbal).OnComplete(() => fadeComplate = true);
    }

    public void AlphaMax(float fadeInterbal)
    {
        if (_image == null) { _image = GetComponent<Image>(); }

        var c = _image.color;
        c = new Color(c.r, c.g, c.b, 1f);

        Debug.Log(c); 

        fadeComplate = false;

        DOTween.To(() => _image.color,
            x => _image.color = x,
            c, fadeInterbal).OnComplete(() => fadeComplate = true);
    }
}
