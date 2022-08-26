using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversi : SingletonMonoBehaviour<Reversi>
{
    [SerializeField, Tooltip("セルとリバーシをまとめた親オブジェクト")]
    GameObject _cellsPosition = default;

    [SerializeField, Tooltip("選択しているセルを表すカーソル")]
    GameObject _cursor = default;

    private ReversiCell[,] _cells = new ReversiCell[8, 8];

    List<ReversiCell> _cellsCheckList = new List<ReversiCell>();

    List<ReversiCell> _ReversiCheckList = new List<ReversiCell>();

    List<ReversiCell> _RastCheckList = new List<ReversiCell>();

    private bool _reverseBool = false;

    private int _reverseValue = 0;

    private int _trunCount = 0;

    private void Start()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                _cells[r, c] = _cellsPosition.transform.Find($"CellandReversi {r} {c}").GetComponent<ReversiCell>();
                _cells[r, c].ReversiState = ReversiState.None;
                _cells[r, c].OnReversiState();
            }
        }
        Setup();
    }

    /// <summary>
    /// ゲームの初期化
    /// </summary>
    void Setup()
    {
        _cells[3, 3].ReversiState = ReversiState.White; _cells[3, 3].OnReversiState();
        _cells[4, 4].ReversiState = ReversiState.White; _cells[4, 4].OnReversiState();
        _cells[3, 4].ReversiState = ReversiState.Black; _cells[3, 4].OnReversiState();
        _cells[4, 3].ReversiState = ReversiState.Black; _cells[4, 3].OnReversiState();
        Cursor.Instance.PotisionChange(4, 3);
        AllCheck();
    }

    private void AllCheck()
    {
        ReversiState rt;

        _cellsCheckList = new List<ReversiCell>();

        if (_trunCount % 2 == 0)
        {
            rt = ReversiState.White;
        }
        else
        {
            rt = ReversiState.Black;
        }

        for (int z = 1; z < 7; z++)
        {
            for (int x = 1; x < 7; x++)
            {
                if (_cells[x, z].ReversiState != ReversiState.None)
                    CellCheckFast(x, z, rt);
            }
        }
    }

    public void Arrangement(int x, int z)
    {
        if (_cells[x, z].ReversiState != ReversiState.None
            || _cells[x, z].ReversiCellState == ReversiCellState.None) { return; }

        if (_trunCount % 2 == 0)
        {
            _cells[x, z].ReversiState = ReversiState.White;
        }
        else
        {
            _cells[x, z].ReversiState = ReversiState.Black;
        }

        _cells[x, z].OnReversiState();

        Reversal(x, z, _cells[x, z].ReversiState);

        CellReset();

        _trunCount++;

        AllCheck();
    }

    private void CellCheckFast(int x, int z, ReversiState rt)
    {
        ReversiCell[] rc = CellCheck(x, z, 1);

        for (int i = 0; i < 8; i++)
        {
            if (_ReversiCheckList.Count == 0 && _cells[x, z].ReversiState != rt && _cells[x, z].ReversiState != ReversiState.None)
            { _ReversiCheckList.Add(_cells[x, z]); }

            if (rc[i] == null)
            {
                if (i % 2 == 0)
                {
                    i++;
                }

                _reverseBool = false;
                _cellsCheckList = new List<ReversiCell>();
                _ReversiCheckList = new List<ReversiCell>();

            }
            else if (i % 2 == 0 || i % 2 != 0 && _reverseBool == true)
            {
                for (int r = 0; r < 8; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        if (_cells[r, c] == rc[i] && rc[i] != _cellsCheckList.Find(n => rc[i]))
                        {
                            rc[i].CellChack(x, z, r, c, rt);
                        }
                    }
                }
            }
            else
            {
                _reverseBool = false;
                _cellsCheckList = new List<ReversiCell>();
                _ReversiCheckList = new List<ReversiCell>();
            }


            if (i % 2 != 0 && _reverseBool == true)//反対含め判定が成功したとき
            {
                if (_cellsCheckList.Count < 2)
                {
                    _reverseBool = false;
                    _cellsCheckList = new List<ReversiCell>();
                    _ReversiCheckList = new List<ReversiCell>();
                    return;
                }

                if (_reverseValue == 0 && _cellsCheckList[1].ReversiState != 0)//どちらが空白Cellか
                {
                    _cellsCheckList[0].ReversiCellState = (ReversiCellState)(int)rt;
                }
                else if (_cellsCheckList[1].ReversiState != (ReversiState)_reverseValue)
                {
                    _cellsCheckList[1].ReversiCellState = (ReversiCellState)(int)rt;
                }

                _reverseBool = false;
                _cellsCheckList = new List<ReversiCell>();
                _ReversiCheckList = new List<ReversiCell>();
            }
        }
    }

    public void NextCheck(int Original_x, int Original_z, int me_x, int me_z, ReversiState reversi)
    {
        int x = me_x - Original_x;
        int z = me_z - Original_z;

        if (x < 0) x--;
        else if (x > 0) x++;

        if (z < 0) z--;
        else if (z > 0) z++;

        if (x + z == 0) return;

        if (Original_z + z < 6 && Original_z + z > 1 && Original_x + x < 6 && Original_x + x > 1)
        {
            _ReversiCheckList.Add(_cells[me_x, me_z]);

            if (!_cellsCheckList.Find(n => _cells[Original_x + x, Original_z + z]))
                _cells[Original_x + x, Original_z + z].CellChack(Original_x, Original_z, Original_x + x, Original_z + z, reversi);
        }
    }

    private void Reversal(int x, int z, ReversiState rs)
    {
        //foreach (var reversiCell in CellCheck(x, z, 1))
        //{
        //    for (int r = 0; r < 7; r++)
        //    {
        //        for (int c = 0; c < 7; c++)
        //        {
        //            if (_cells[r, c] == reversiCell)
        //            {
        //                reversiCell.Reversal(x, z, r, c, _cells[x, z].ReversiState);
        //            }
        //        }
        //    }
        //}

        _RastCheckList = new List<ReversiCell>();
        _cellsCheckList = new List<ReversiCell>();
        _ReversiCheckList = new List<ReversiCell>();

        ReversiCheck(x, z, rs);

        //この値を加算していくことで周囲の値を探索する
        for (int i = 1; i < 7; i++)
        {
            foreach (var reversiCell in CellCheck(x, z, i))
            {
                for (int r = 0; r < 8; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        if (_cells[r, c] == reversiCell && _RastCheckList.Find(n => n == reversiCell))
                        {
                            reversiCell.ReversiState = rs;
                            reversiCell.OnReversiState();
                        }
                    }
                }
            }
        }
    }

    public void ReversalBool(int x, int z, int value)
    {
        if (!_reverseBool)
        {
            _reverseBool = true;
            _reverseValue = value;
            _cellsCheckList.Add(_cells[x, z]);
        }
        else
        {
            if (_reverseValue == value || _ReversiCheckList.Count < 1)
            {
                _reverseBool = false;
                _cellsCheckList = new List<ReversiCell>();
                _ReversiCheckList = new List<ReversiCell>();
            }
            else _cellsCheckList.Add(_cells[x, z]);
        }
    }

    public void ReversiCheck(int x, int z, ReversiState rt)
    {
        ReversiCell[] rc = CellCheck(x, z, 1);

        for (int i = 0; i < 8; i++)
        {
            if (rc[i] != null && rc[i].ReversiState != ReversiState.None && rc[i].ReversiState != rt)
            {
                for (int r = 0; r < 8; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        if (_cells[r, c] == rc[i])
                        {
                            NextCheck(x, z, r, c, rt);
                            if (_cellsCheckList.Count > 0)
                            {
                                foreach (ReversiCell a in _ReversiCheckList)
                                {
                                    if (!_RastCheckList.Find(n => n == a))
                                        _RastCheckList.Add(a);
                                }
                            }
                            _reverseBool = false;
                            _cellsCheckList = new List<ReversiCell>();
                            _ReversiCheckList = new List<ReversiCell>();
                        }
                    }
                }

            }
        }
    }

    //public void ReversalNext(int Original_x, int Original_z, int me_x, int me_z, ReversiState reversi)
    //{
    //    int x = me_x - Original_x;
    //    int z = me_z - Original_z;

    //    if (x < 0) x--;
    //    else if (x > 0) x++;

    //    if (z < 0) z--;
    //    else if (z > 0) z++;

    //    if (Original_z + z < 6 && Original_z + z > 1 && Original_x + x < 6 && Original_x + x > 1)
    //        _cells[Original_z + z, Original_x + x].Reversal(Original_x, Original_z, x, z, reversi);
    //}

    private List<ReversiCell> ReversalCellCheck(int x, int z, int addValue)
    {
        List<ReversiCell> _opencellsList = new List<ReversiCell>();

        if (z - addValue > 0) _opencellsList.Add(_cells[x, z - addValue]);

        if (z + addValue < 8) _opencellsList.Add(_cells[x, z + addValue]);

        if (x - addValue > 0) _opencellsList.Add(_cells[x - addValue , z]);

        if (x + addValue < 8) _opencellsList.Add(_cells[x + addValue , z]);

        if (z + addValue < 8 && x + addValue < 8) _opencellsList.Add(_cells[x + addValue , z + addValue]);

        if (z - addValue > 0 && x - addValue > 0) _opencellsList.Add(_cells[x - addValue , z - addValue]);

        if (z + addValue < 8 && x - addValue > 0) _opencellsList.Add(_cells[x - addValue , z + addValue]);

        if (z - addValue > 0 && x + addValue < 8) _opencellsList.Add(_cells[x + addValue , z - addValue]);

        return _opencellsList;

    }

    private ReversiCell[] CellCheck(int x, int z, int addValue)
    {
        ReversiCell[] _opencellsArray = new ReversiCell[8];

        if (z - addValue > 0) _opencellsArray[0] = _cells[x, z - addValue];

        if (z + addValue < 7) _opencellsArray[1] = _cells[x, z + addValue];

        if (x - addValue > 0) _opencellsArray[2] = _cells[x - addValue, z];

        if (x + addValue < 7) _opencellsArray[3] = _cells[x + addValue, z];

        if (z + addValue < 7 && x + addValue < 7) _opencellsArray[4] = _cells[x + addValue, z + addValue];

        if (z - addValue > 0 && x - addValue > 0) _opencellsArray[5] = _cells[x - addValue, z - addValue];

        if (z + addValue < 7 && x - addValue > 0) _opencellsArray[6] = _cells[x - addValue, z + addValue];

        if (z - addValue > 0 && x + addValue < 7) _opencellsArray[7] = _cells[x + addValue, z - addValue];

        return _opencellsArray;

    }

    private void CellReset()
    {
        for (int r = 0; r < 7; r++)
        {
            for (int c = 0; c < 7; c++)
            {
                if (_cells[r, c].ReversiState == ReversiState.None)
                {
                    _cells[r, c].CellReset();
                }
            }
        }
    }
}
