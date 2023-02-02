using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundColor : MonoBehaviour
{
    private Image _image;

    private NovelInput NovelInput => NovelManager.Instance.NovelInput;

    public void StartAlpha()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        var color = _image.color;
        color.a = 0;
        _image.color = color;
    }

    public IEnumerator FadeIn(float fadeInterbal, Func<bool> condition)
    {
        Debug.Log(_image);
        if (_image == null) 
        {
            _image = GetComponent<Image>();
        }

        var color = _image.color;
        // color のアルファ値を徐々に 1 に近づける処理
        var elapsed = 0F;
        while (condition() && elapsed < fadeInterbal)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / fadeInterbal;
            _image.color = color;
            yield return null;
        }

        color.a = 1;
        _image.color = color;
        NovelInput.MoveNext();
        yield return null;
    }

    public IEnumerator FadeOut(float fadeInterbal , Func<bool> condition)
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        var color = _image.color;
        float a = color.a;
        // color のアルファ値を徐々に 0 に近づける処理
        var elapsed = 0F;
        while (condition() && elapsed < fadeInterbal)
        {
            elapsed += Time.deltaTime;
            color.a = a - elapsed / fadeInterbal;
            _image.color = color;
            yield return null;
        }

        color.a = 0;
        _image.color = color;
        NovelInput.MoveNext();
        yield return null;
    }

    public IEnumerator FadeOutNoNext(float fadeInterbal, Func<bool> condition) 
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        var color = _image.color;
        float a = color.a;
        // color のアルファ値を徐々に 0 に近づける処理
        var elapsed = 0F;
        while (condition() && elapsed < fadeInterbal)
        {
            elapsed += Time.deltaTime;
            color.a = a - elapsed / fadeInterbal;
            _image.color = color;
            yield return null;
        }

        color.a = 0;
        _image.color = color;
        yield return null;
    }
}
