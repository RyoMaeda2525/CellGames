using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NovelManager : MonoBehaviour
{
    [SerializeField]
    NovelInput _novelInput;

    [SerializeField]
    CharaManager _charaManager;

    [SerializeField]
    BackGround _backGround;

    public NovelInput NovelInput => _novelInput;
    
    public CharaManager CharaManager => _charaManager;

    public BackGround BackGround => _backGround;
    
    private void Awake()
    {
        CheckInstance();
    }

    public static NovelManager instance;
    public static NovelManager Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(NovelManager);

                instance = (NovelManager)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogWarning($"{t}をアタッチしているオブジェクトがありません");
                }
            }

            return instance;
        }
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(gameObject);
        return false;
    }
}
