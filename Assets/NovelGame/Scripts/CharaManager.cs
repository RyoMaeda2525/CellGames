using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharaManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _charactorPosition;

    [SerializeField]
    private string[] _charactorName;

    [SerializeField]
    private GameObject _camvas = null;

    private List<Image> _charactorImages = new List<Image>();

    private void Awake()
    {
        for (int i = 0; i < _charactorName.Length; i++)
        {
            GameObject imagePrehub = Instantiate((GameObject)Resources.Load($"{_charactorName[i]}"), _camvas.transform);
            imagePrehub.AddComponent<CharactorColor>();
            _charactorImages.Add(imagePrehub.GetComponent<Image>());
            _charactorImages[i].GetComponent<CharactorColor>().AlphaZero(0f);
        }

    }

    private void Start()
    {
        CharactorFadeStand("Camepan", 0);
    }

    public void CharactorFadeStand(string charactorName, int positionIndex)
    {
        Image fadeImage = null;
        charactorName = charactorName + "(Clone)";

        foreach (var image in _charactorImages)
        {
            if (image.name == charactorName)
            {
                fadeImage = image;
                break;
            }
        }
        if (fadeImage == null) { Debug.Log("éwíËÇµÇΩâÊëúÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB"); return; }

        fadeImage.GetComponent<CharactorColor>().AlphaMax(2f);
        fadeImage.transform.position = _charactorPosition[positionIndex].transform.position;
    }
}
