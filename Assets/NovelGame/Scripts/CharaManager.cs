using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using OpenCover.Framework.Model;
using System.Numerics;

public class CharaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _charactorPosition;

    [SerializeField]
    private GameObject _camvas = null;

    [SerializeField]
    private List<CharactorColor> _charactorImages = new List<CharactorColor>();

    private NovelManager NovelManager => NovelManager.Instance;

    public bool _fadeNow = false;

    private void Awake()
    {
        for (int i = 0; i < _charactorImages.Count; i++)
        {
            CharactorColor imagePrehub = Instantiate(_charactorImages[i] , _camvas.transform);

            _charactorImages[i] = imagePrehub;

            StartCoroutine(imagePrehub.FadeOut(0f , false));
        }
    }

    public IEnumerator FadeIn(string charactorName, int positionIndex , bool end)
    {
        CharactorColor fadeImage = CharaSearch(charactorName);

        if (fadeImage == null) { Debug.Log($"{charactorName}ÇÃâÊëúÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB"); yield return null; }

        Coroutine coroutine = StartCoroutine(fadeImage.FadeIn(2f, () => !NovelManager.IsSkipRequested() , end));
        fadeImage.transform.position = _charactorPosition[positionIndex].transform.position;

        yield return coroutine;

        _fadeNow = false;
    }

    public void DontSkipFadeIn(string charactorName, int positionIndex, bool end)
    {
        CharactorColor fadeImage = CharaSearch(charactorName);

        if (fadeImage == null) { Debug.Log($"{charactorName}ÇÃâÊëúÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB"); return; }

        StartCoroutine(fadeImage.FadeIn(2f , end));
        fadeImage.transform.position = _charactorPosition[positionIndex].transform.position;
    }

    private CharactorColor CharaSearch(string charactorName) 
    {
        charactorName = charactorName + "(Clone)";

        foreach (var image in _charactorImages)
        {
            if (image.name == charactorName)
            {
                return image;
            }
        }

        return null;
    }
}
