using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : SingletonMonoBehaviour<Cursor>
{
    [SerializeField, Tooltip("位置を揃えるための初期位置のX値")]
    float _firstx = 5.1f;
    [SerializeField, Tooltip("cellと位置合わせるためのX値")]
    float _x = 9.9f;

    [SerializeField, Tooltip("cellと位置合わせるためのZ値")]
    float _z = 10f;

    public int _nowX = 0;

    public int _nowZ = 0;

    public bool PotisionChange(int x, int z)
    {
        if (z > 7 || z < 0 || x > 7 || x < 0)
        {
            Debug.Log("CellArrayOver");

            return false;
        }
        transform.position = new Vector3(_firstx + x * _x, transform.position.y, z * _z);
        _nowZ = z; _nowX = x;
        return true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Reversi.Instance.Arrangement(_nowX , _nowZ);
        }
        else
        {
            var z =
           (Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0) +
           (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0);

            var x =
            (Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0) +
            (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

            if (z != 0 || x != 0)
            {
                bool changeSuccess = PotisionChange(_nowX + x, _nowZ + z);
            }
        }
    }
}
