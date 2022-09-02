using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReversiGameManager : SingletonMonoBehaviour<ReversiGameManager>
{
    [SerializeField , Tooltip("���������̐F��\������e�L�X�g")]
    private Text _winnerText = null;

    // Update is called once per frame
    public void Gameset(ReversiState rs)
    {
        _winnerText.gameObject.SetActive(true);

        if (rs == ReversiState.White)
        {
            _winnerText.text = "White Win";
        }
        else _winnerText.text = "Black Win";
    }
}
