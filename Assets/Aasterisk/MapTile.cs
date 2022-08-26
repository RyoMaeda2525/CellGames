using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TileState
{
    None = 0,

    Wall = 1,
    Start = 2,
    Goal = 3,
    Open = 4,
    Close = 5,
}

public class MapTile : MonoBehaviour
{
    [SerializeField]
    TileState _tileState = TileState.None;

    [SerializeField]
    private Text _text = default;

    [SerializeField]
    private Image _image = default;

    int _value = 0;

    public int distance; //ゴールまでの推定コスト

    public int count; //進んで来た実コスト

    public int score; //推定コストと実コストの合計値

    public MapTile openTile; //このタイルを開いたタイル

    public TileState TileState
    {
        get => _tileState;
        set
        {
            _tileState = value;
            OnCellStateChanged();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCellStateChanged();
    }

    private void OnValidate() //Inspectorでの変更が反映される！！！ただ変更する度にEditor実行するので注意が必要
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (!_text) return;

        if (_tileState == TileState.Start)
        {
            _text.text = "S";
        }
        else if (_tileState == TileState.Goal)
        {
            _text.text = "G";
        }
        else if (_tileState == TileState.Wall)
        {
            //_text.text = "W";
            _image.color = Color.black;
        }
        else if (_tileState == TileState.Open)
        {
            _text.text = "*";
            _image.color = Color.red;
        }
        else if (_tileState == TileState.Close)
        {
            _text.text = "*";
            _image.color = Color.blue;
        }
    }

    public int OnOpen(int walkCount  , int disCost , MapTile openMt)
    {
        TileState = TileState.Open;
        count = walkCount;
        distance = disCost;
        score = count + distance;
        openTile = openMt;
        return score;
    }
}
