using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Minesweeper _minesweeper = default;

    [SerializeField]
    MineHideController _mineHideController = default;

    [SerializeField]
    Text _timeText = default;

    [SerializeField]
    GameObject _endPanel = default;

    [SerializeField]
    Text _endText = default;

    [SerializeField]
    Text _bombCountText = default;

    DateTime _dateTime = default;
    TimeSpan _timeSpan = new TimeSpan(0 , 0 , 0 , 0 , 0);
    static public int _bombCount = 0;
    static public bool _inGame = false;

    public void GameStart()
    {
        _inGame = true;
        _dateTime = DateTime.Now;
    }

    private void Update()
    {
        _bombCountText.text = _bombCount.ToString();

        if (_inGame) 
        {
            _timeSpan = DateTime.Now - _dateTime;

            if (_timeSpan.Hours * 60 + _timeSpan.Minutes < 999)
                _timeText.text = _timeSpan.Hours * 60 + _timeSpan.Minutes.ToString("0") + " : " + _timeSpan.Seconds.ToString("00");
            else _timeText.text = "999 : 59";
        }
    }

    public void Retry() 
    {
        _inGame = false;
        _timeText.text = "00 : 00";
        SceneManager.LoadScene("Minesweeper");
    }

    public void GameClear(bool clear) 
    {
        _inGame = false;
        _endPanel.SetActive(true);
        if (clear)
            _endText.text = "Game Clear";
        else
        {
            _endText.text = "Game Over";
            _minesweeper.MineDisclosure();
        } 
    }
}
