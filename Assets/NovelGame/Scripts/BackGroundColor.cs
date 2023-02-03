using System;
using System.Collections;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundColor : MonoBehaviour
{
    private Image _image;

    private NovelInput NovelInput => NovelManager.Instance.NovelInput;

    private bool _enabled = false;

    public bool Enabled => _enabled;

    public void StartAlpha()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        var color = _image.color;
        color.a = 0;
        _image.color = color;

        _enabled = false;
    }

    public IEnumerator FadeIn(float fadeInterbal, Func<bool> condition , bool end)
    {
        Debug.Log(_image);
        if (_image == null) 
        {
            _image = GetComponent<Image>();
        }

        _enabled = true;

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
        if (end && condition())
        {
            NovelInput.MoveNext();
        }
        yield return null;
    }

    public IEnumerator FadeOut(float fadeInterbal , Func<bool> condition , bool end)
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _enabled = false;

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
        if (end && condition())
        {
            NovelInput.MoveNext();
        }
        yield return null;
    }

    public IEnumerator FadeIn(float fadeInterbal, bool end)
    {
        Debug.Log(_image);
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _enabled = true;

        var color = _image.color;
        // color のアルファ値を徐々に 1 に近づける処理
        var elapsed = 0F;
        while (elapsed < fadeInterbal)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / fadeInterbal;
            _image.color = color;
            yield return null;
        }

        color.a = 1;
        _image.color = color;
        if (end)
        {
            NovelInput.MoveNext();
        }
        yield return null;
    }


    public IEnumerator FadeOut(float fadeInterbal , bool end)
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _enabled = false;

        var color = _image.color;
        float a = color.a;
        // color のアルファ値を徐々に 0 に近づける処理
        var elapsed = 0F;
        while (elapsed < fadeInterbal)
        {
            elapsed += Time.deltaTime;
            color.a = a - elapsed / fadeInterbal;
            _image.color = color;
            yield return null;
        }

        color.a = 0;
        _image.color = color;
        if (end)
        {
            NovelInput.MoveNext();
        }
        yield return null;
    }
}
