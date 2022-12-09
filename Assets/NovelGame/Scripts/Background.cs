using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackGround : MonoBehaviour
{
    [SerializeField, Tooltip("�t�F�[�h���鎞��")]
    private float _fadeInterbal = 1.5f;

    [SerializeField, Tooltip("暗転用")]
    BackGroundColor _black;

    [SerializeField]
    GameObject _backGroundCanbas;

    [SerializeField]
    public NovelInput _novelInput;

    private bool _backGroundChange = false;

    public bool BackGroundChange
    {
        set
        {
            if (_backGroundChange == true)
            {
                _backGroundChange = value;
                _novelInput.MoveNext();
            }
        }
    }

    public void FadeIn(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        backGround.FadeIn(_fadeInterbal);
    }

    public void FadeOut(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        backGround.FadeOut(_fadeInterbal);
    }

    public void FadeInComplete(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        _backGroundChange = true;

        backGround.FadeIn(_fadeInterbal);
    }

    public void FadeOutComplete(string imageName)
    {
        BackGroundColor backGround = BackGroundSearch(imageName);

        _backGroundChange = true;

        backGround.FadeOut(_fadeInterbal);
    }

    private BackGroundColor BackGroundSearch(string imageName)
    {
        if (imageName == "Black") { return _black; }

        BackGroundColor image = null;

        Transform transform = _backGroundCanbas.transform.Find(imageName);

        //子オブジェクトから画像を出す
        if (transform != null)
        {
            image = transform.GetComponent<BackGroundColor>();
        }
        //Resourcesから画像を出す
        else
        {
            image = Instantiate(Resources.Load<BackGroundColor>($"NovelGame/{imageName}"), _backGroundCanbas.transform);
            image.name = imageName;
        }

        return image;
    }
}
