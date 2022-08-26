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

    public int distance; //�S�[���܂ł̐���R�X�g

    public int count; //�i��ŗ������R�X�g

    public int score; //����R�X�g�Ǝ��R�X�g�̍��v�l

    public MapTile openTile; //���̃^�C�����J�����^�C��

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

    private void OnValidate() //Inspector�ł̕ύX�����f�����I�I�I�����ύX����x��Editor���s����̂Œ��ӂ��K�v
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
