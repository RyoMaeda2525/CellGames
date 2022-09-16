using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum ReversiCellState
{
    None = 0,
    White = 1,
    Black = 2,
}

public enum ReversiState
{
    None = 0,
    White = 1,
    Black = 2,
}

public class ReversiCell : MonoBehaviour
{
    ReversiCellState _reversiCellState = ReversiCellState.None;

    ReversiState _reversiState = ReversiState.None;

    public ReversiCellState ReversiCellState
    {
        get => _reversiCellState;
        set
        {
            _reversiCellState = value;
            OnCellState();
        }
    }

    public ReversiState ReversiState
    {
        get => _reversiState;
        set
        {
            _reversiState = value;
        }
    }

    public void OnCellState()
    {
        if (_cellAni == null) { _cellAni = transform.Find("Cell").GetComponent<Animator>(); }

        _cellAni.Play(_reversiCellState.ToString());
    }

    /// <summary>
    /// 同時にめくりたくないのでpublicでタイミングをと調整する
    /// </summary>
    public void OnReversiState()
    {
        if (_reversiAni == null) { _reversiAni = transform.Find("Reversi").GetComponent<Animator>(); }

        if (_reversiState == ReversiState.None) { _reversiAni.Play("None"); return; }

        _reversiAni.SetBool(_reversiState.ToString() , true);
        if (_reversiState.ToString() == "White")
        {
            _reversiAni.SetBool("Black", false);
        }
        else { _reversiAni.SetBool("White", false); }
    }

    //---------ここからstate以外の処理------------------------------------------------------

    Animator _cellAni = null;

    Animator _reversiAni = null;

    public void CellChack(int original_x, int original_z, int me_x, int me_z, ReversiState reversi)
    {
        if(_reversiState == ReversiState.None) { return; }

        if (_reversiState == reversi)
        {
            Reversi.Instance.ReversalBool(original_x, original_z, me_x, me_z);
        }
        else if (_reversiState != reversi)
        {
            Reversi.Instance.NextCheck(original_x, original_z, me_x, me_z, reversi);
        }
    }

    public void CellReset() { _reversiCellState = ReversiCellState.None; OnCellState(); }

}
