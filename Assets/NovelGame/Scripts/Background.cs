using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class BackGround : MonoBehaviour
{
    [SerializeField, Tooltip("�t�F�[�h���鎞��")]
    private float _fadeInterbal = 1.5f;

    [SerializeField]
    List<BackGroundColor> _backGroundImages = new List<BackGroundColor>();

    [SerializeField, Tooltip("暗転用")]
    BackGroundColor _black;

    [SerializeField]
    GameObject _backGroundCanbas;

    private void Awake()
    {
        for (int i = 0; i < _backGroundImages.Count; i++)
        {
            BackGroundColor imagePrehub = Instantiate(_backGroundImages[i], _backGroundCanbas.transform);

            _backGroundImages[i] = imagePrehub;

            imagePrehub.StartAlpha();
        }

        StartCoroutine(_black.FadeOutNoNext(_fadeInterbal,() => !IsSkipRequested()));
    }

    public void FadeIn(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        backGround.FadeIn(_fadeInterbal, () => !IsSkipRequested());
    }

    public void FadeOut(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeOut(_fadeInterbal, () => !IsSkipRequested()));
    }

    public void FadeInComplete(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeIn(_fadeInterbal, () => !IsSkipRequested()));
    }

    public void FadeOutComplete(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeOut(_fadeInterbal, () => !IsSkipRequested()));
    }

    private BackGroundColor BackGroundSearch(string imageName)
    {
        if (imageName == "Black") { return _black; }

        imageName = imageName + "(Clone)";

        foreach (var image in _backGroundImages)
        {
            if (image.name == imageName)
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
