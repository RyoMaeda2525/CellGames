using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideCell : MonoBehaviour
{
    MineHideController _mhc;

    public bool _open = false;

    private void Start()
    {
        _mhc = FindObjectOfType<MineHideController>();
    }

    public void HideCellDisclosure(int r , int c)
    {
        if (!_open)
        {
            Destroy(GetComponent<Image>());
            Destroy(transform.GetChild(0).gameObject);
            _open = true;
        }

        if (Minesweeper._cells[r ,c].CellState == CellState.None && GameManager._inGame) 
        {
            _mhc.HideOpen(r, c);
        }
    }
}
