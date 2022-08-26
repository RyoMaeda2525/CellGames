using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LifeState 
{
    dead = 0,
    alive = 1
}

public class LifeCell : MonoBehaviour
{

    LifeState _lifeState = LifeState.dead;

    Image _image = null;

    public LifeState LifeState
    {
        get => _lifeState;
        set 
        {
            _lifeState = value;
            OnLifeState();
        }
    }

    private void OnLifeState() 
    {
        if(_image == null) _image = GetComponent<Image>();

        if (_lifeState == LifeState.alive) _image.color = Color.black;
        else _image.color = Color.white;
    } 
}
