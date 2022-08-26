using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None = 0, //空

    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,

    Mine = -1,//地雷

}

public class Cell : MonoBehaviour
{
    [SerializeField]
    private Text _text = default;

    [SerializeField]
    CellState _cellState = CellState.None;

    public CellState CellState 
    {
        get => _cellState;
        set 
        {
            _cellState = value;
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

        if (_cellState == CellState.Mine)
        {
            _text.text = "X";
            _text.color = Color.red;
        }
        else if (_cellState == CellState.None)
        {
            _text.text = "";
        }
        else
        {
            _text.text = ((int)_cellState).ToString();
            _text.color = Color.blue;
        }
    }
}
