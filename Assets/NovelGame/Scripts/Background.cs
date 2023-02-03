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

    private NovelManager NovelManager => NovelManager.Instance;

    private void Awake()
    {
        for (int i = 0; i < _backGroundImages.Count; i++)
        {
            BackGroundColor imagePrehub = Instantiate(_backGroundImages[i], _backGroundCanbas.transform);

            _backGroundImages[i] = imagePrehub;

            imagePrehub.StartAlpha();
        }

        StartCoroutine(_black.FadeOut(_fadeInterbal , false));
    }

    public void FadeIn(string imageName , bool end)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        else StartCoroutine(backGround.FadeIn(_fadeInterbal, () => !NovelManager.IsSkipRequested(), end));
    }

    public void FadeOut(string imageName , bool end)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeOut(_fadeInterbal, () => !NovelManager.IsSkipRequested(), end));
    }

    public void DontSkipFadeOut(string imageName , bool end)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeOut(_fadeInterbal, end));
    }

    public void DontSkipFadeIn(string imageName, bool end)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        if (backGround == null) { Debug.Log($"{imageName}の画像が見つかりません。"); return; }

        StartCoroutine(backGround.FadeIn(_fadeInterbal, end));
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
}
