using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using OpenCover.Framework.Model;

public class CharaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _charactorPosition;

    [SerializeField]
    private GameObject _camvas = null;

    [SerializeField]
    private List<CharactorColor> _charactorImages = new List<CharactorColor>();

    private void Awake()
    {
        for (int i = 0; i < _charactorImages.Count; i++)
        {
            CharactorColor imagePrehub = Instantiate(_charactorImages[i] , _camvas.transform);

            _charactorImages[i] = imagePrehub;

            StartCoroutine(imagePrehub.AlphaZero(0f , () => !IsSkipRequested()));
        }
    }

    private void Start()
    {
        //CharactorFadeStand("Camepan", 0);
    }

    public void CharactorFadeStand(string charactorName, int positionIndex)
    {
        CharactorColor fadeImage = CharaSearch(charactorName);

        if (fadeImage == null) { Debug.Log($"{charactorName}‚Ì‰æ‘œ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñB"); return; }

        StartCoroutine(fadeImage.AlphaMax(2f, () => !IsSkipRequested()));
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

    private static bool IsSkipRequested()
    {
        return Input.GetMouseButtonDown(0);
    }
}
