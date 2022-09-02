using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversi : SingletonMonoBehaviour<Reversi>
{
    [SerializeField, Tooltip("セルとリバーシをまとめた親オブジェクト")]
    GameObject _cellsPosition = default;

    private ReversiCell[,] _cells = new ReversiCell[8, 8];

    List<ReversiCell> _ReversiCheckList = new List<ReversiCell>();

    private int _trunCount = 0;

    private int _placementcCellCount = 0;

    private bool _reverseBool = false;

    private bool _afterPlacement = false;

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

        if (_trunCount % 2 == 0)
        {
            rt = ReversiState.White;
        }
        else
        {
            rt = ReversiState.Black;
        }

        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                if (_cells[x, z].ReversiState == ReversiState.None)
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

        _afterPlacement = true;

        _cells[x, z].OnReversiState();

        Reversal(x, z, _cells[x, z].ReversiState);

        _placementcCellCount++;

        if (_placementcCellCount == 60) { GameSet(); }

        CellReset();

        _trunCount++;

        _afterPlacement = false;

        _ReversiCheckList = new List<ReversiCell>();

        AllCheck();

        Debug.Log(_trunCount);

        if(_ReversiCheckList.Count == 0) { _trunCount++; AllCheck(); }
    }

    private void CellCheckFast(int x, int z, ReversiState rt)
    {
        ReversiCell[] rc = CellCheck(x, z, 1);

        for (int i = 0; i < 8; i++)
        {
            if (rc[i] != null)
            {
                if (rc[i].ReversiState != ReversiState.None && rc[i].ReversiState != rt)
                {
                    for (int r = 0; r < 8; r++)
                    {
                        for (int c = 0; c < 8; c++)
                        {
                            if (rc[i] == _cells[r, c])
                                rc[i].CellChack(x, z, r, c, rt);
                        }
                    }
                }
            }
        }
        if (_reverseBool == true)
        {
            _cells[x, z].ReversiCellState = (ReversiCellState)rt;
            _cells[x, z].OnCellState();
        }
        _reverseBool = false;
    }

    public void NextCheck(int Original_x, int Original_z, int me_x, int me_z, ReversiState reversi)
    {
        int x = me_x - Original_x;
        int z = me_z - Original_z;

        if (x < 0) x--;
        else if (x > 0) x++;

        if (z < 0) z--;
        else if (z > 0) z++;

        if (x == 0 && z == 0) return;

        if (Original_z + z < 8 && Original_z + z > -1 && Original_x + x < 8 && Original_x + x > -1)
        {
            _cells[Original_x + x, Original_z + z].CellChack(Original_x, Original_z, Original_x + x, Original_z + z, reversi);
        }
    }

    private void Reversal(int x, int z, ReversiState rs)
    {
        _ReversiCheckList = new List<ReversiCell>();

        CellCheckFast(x, z, rs);

        //この値を加算していくことで周囲の値を探索する
        //for (int i = 1; i < 7; i++)
        //{
        //    foreach (var reversiCell in CellCheck(x, z, i))
        //    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                if (_ReversiCheckList.Find(n => n == _cells[r, c]))
                {
                    _cells[r, c].ReversiState = rs;
                    _cells[r, c].OnReversiState();
                }
            }
        }
        //    }
        //}
    }

    public void ReversalBool(int Original_x, int Original_z, int me_x, int me_z, int value)
    {
        if (!_reverseBool)
        {
            _reverseBool = true;
        }

        //if (_afterPlacement)
        //{

            _ReversiCheckList.Add(_cells[me_x, me_z]);

            int x = me_x - Original_x;
            int z = me_z - Original_z;

            while (x != 0 || z != 0)
            {
                if (x < 0) x++;
                else if (x > 0) x--;

                if (z < 0) z++;
                else if (z > 0) z--;

                if (x == 0 && z == 0) { break; }

                _ReversiCheckList.Add(_cells[Original_x + x, Original_z + z]);
            }
        //}
    }

    private ReversiCell[] CellCheck(int x, int z, int addValue)
    {
        ReversiCell[] _opencellsArray = new ReversiCell[8];

        if (z - addValue >= 0) _opencellsArray[0] = _cells[x, z - addValue];

        if (z + addValue <= 7) _opencellsArray[1] = _cells[x, z + addValue];

        if (x - addValue >= 0) _opencellsArray[2] = _cells[x - addValue, z];

        if (x + addValue <= 7) _opencellsArray[3] = _cells[x + addValue, z];

        if (z + addValue <= 7 && x + addValue <= 7) _opencellsArray[4] = _cells[x + addValue, z + addValue];

        if (z - addValue >= 0 && x - addValue >= 0) _opencellsArray[5] = _cells[x - addValue, z - addValue];

        if (z + addValue <= 7 && x - addValue >= 0) _opencellsArray[6] = _cells[x - addValue, z + addValue];

        if (z - addValue >= 0 && x + addValue <= 7) _opencellsArray[7] = _cells[x + addValue, z - addValue];

        return _opencellsArray;

    }

    private void CellReset()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                    _cells[r, c].CellReset();
            }
        }
    }

    private void GameSet() 
    {
        int black = 0;
        int white = 0;

        for (int r = 0; r < 8; r++) 
        {
            for (int c = 0; c < 8; c++) 
            {
                if (_cells[r, c].ReversiState == ReversiState.White) white++;
                else black++;
            }
        }

        if (white > black)
        {
            ReversiGameManager.Instance.Gameset(ReversiState.White);
        }
        else ReversiGameManager.Instance.Gameset(ReversiState.Black);
        
    }
}
