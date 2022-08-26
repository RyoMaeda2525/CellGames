using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aasterisk : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup = default;

    [SerializeField]
    private int _rows = 5;

    [SerializeField]
    private int _columns = 5;

    [SerializeField]
    private MapTile _tilePrefab = default;

    List<MapTile> _openMapTiles = new List<MapTile>();

    private static MapTile[,] _tiles;

    int _walkCount = 0;

    int _openrow = 0;
    int _opencol = 0;

    int _goalrow = 0;
    int _goalcol = 0;

    bool _goal = false;

    int[,] tileStates = default;

    private void Start()
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = _columns;

        _tiles = new MapTile[_rows, _columns];
        tileStates = new int[5, 5]
        {
            { 2, 0 , 1 , 0 , 0 },
            { 1, 0 , 0 , 1 , 1 },
            { 0, 1 , 0 , 0 , 1 },
            { 0, 1 , 1 , 0 , 1 },
            { 0, 0 , 1 , 0 , 3 },
        };
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                var tile = Instantiate(_tilePrefab, _gridLayoutGroup.transform);
                _tiles[r, c] = tile;
                _tiles[r, c].TileState = (TileState)tileStates[r, c];
                if (_tiles[r, c].TileState == TileState.Goal) (_goalrow, _goalcol) = (r, c);
                if (_tiles[r, c].TileState == TileState.Start) (_openrow, _opencol) = (r, c);
            }
        }

        while (!_goal)
        {
            _walkCount++;
            MapTile[] mapTiles = TilesArray(_openrow, _opencol);

            MapTile shortestTile = null;
            int minCost = 100;

            int row = 0;
            int col = 0;

            foreach (MapTile mt in mapTiles)
            {
                if (mt.TileState == TileState.None)
                {
                    for (int r = 0; r < _rows; r++)
                    {
                        for (int c = 0; c < _columns; c++)
                        {
                            if (_tiles[r, c] == mt)
                            {
                                int cost = mt.OnOpen(_walkCount, GoalDis(r, c), _tiles[_openrow, _opencol]);
                                if (cost < minCost)
                                {
                                    minCost = cost;
                                    shortestTile = mt.openTile;
                                    _openMapTiles.Add(mt);
                                    row = r;
                                    col = c;
                                }
                            }
                        }
                    }
                }
                else if (mt.TileState == TileState.Goal)
                {
                    _tiles[_openrow, _opencol].TileState = TileState.Close;
                    _goal = true;
                    return;
                }
            }
            if (shortestTile != null && shortestTile.TileState != TileState.Start)
            {

                shortestTile.TileState = TileState.Close;
            }
            else
            {
                foreach (MapTile mt in mapTiles)
                {
                    if (mt.TileState == TileState.Open)
                    {
                        for (int r = 0; r < _rows; r++)
                        {
                            for (int c = 0; c < _columns; c++)
                            {
                                if (_tiles[r, c] == mt)
                                {
                                    int cost = mt.OnOpen(_walkCount, GoalDis(r, c), _tiles[_openrow, _opencol]);
                                    if (cost < minCost)
                                    {
                                        minCost = cost;
                                        shortestTile = mt.openTile;
                                        row = r;
                                        col = c;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            _openrow = row;
            _opencol = col;
            minCost = 0;
        }
    }

    private int GoalDis(int r, int c)
    {
        var dis = Mathf.Abs(_goalrow - r) + Mathf.Abs(_goalcol - c);
        return dis;
    }

    private MapTile[] TilesArray(int r, int c)
    {
        List<MapTile> tiles = new List<MapTile>();
        if (r > 0) tiles.Add(_tiles[r - 1, c]);
        if (c > 0) tiles.Add(_tiles[r, c - 1]);
        if (r < _rows - 1) tiles.Add(_tiles[r + 1, c]);
        if (c < _columns - 1) tiles.Add(_tiles[r, c + 1]);
        return tiles.ToArray();
    }
}
