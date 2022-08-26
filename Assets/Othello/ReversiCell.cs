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
        _cellAni.Play(_reversiCellState.ToString());
    }

    /// <summary>
    /// 同時にめくりたくないのでpublicでタイミングをと調整する
    /// </summary>
    public void OnReversiState()
    {
        if (_reversiAni == null) { _reversiAni = transform.Find("Reversi").GetComponent<Animator>(); }
        _reversiAni.Play(_reversiState.ToString());
    }

    //---------ここからstate以外の処理------------------------------------------------------

    Animator _cellAni = default;

    Animator _reversiAni = null;

    private void Start()
    {
        _cellAni = transform.Find("Cell").GetComponent<Animator>();
        //_reversiAni = transform.Find("Reversi").GetComponent<Animator>();
    }


    public void CellChack(int Original_x, int Original_z, int me_x, int me_z, ReversiState reversi)
    {
        if (_reversiState == ReversiState.None || _reversiState == reversi)
        {
            //int z = me_z - Original_z;
            //int x = me_x - Original_x;

            //if (z > 1 || z < -1 && x > 1 || x < -1)
            //{
                Reversi.Instance.ReversalBool(me_x, me_z, (int)_reversiState);
            //}
        }
        else if (_reversiState != reversi)
        {
            Reversi.Instance.NextCheck(Original_x, Original_z, me_x, me_z, reversi);
        }
    }

    //public void Reversal(int Original_x, int Original_z, int me_z, int me_x, ReversiState reversi)
    //{
    //    if (_reversiState == reversi)
    //    {
    //        Reversi.Instance.ReversalNext(Original_x, Original_z, me_x, me_z, reversi);
    //    }
    //}

    public void CellReset() { _reversiCellState = ReversiCellState.None; OnCellState(); }
}
