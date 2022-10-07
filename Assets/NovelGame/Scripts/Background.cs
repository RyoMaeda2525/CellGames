using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField, Tooltip("”wŒi‚Ì‘O‚É’u‚¢‚ÄƒtƒF[ƒh“™‚ð‚·‚é’PF”wŒi")]
    private Image _monochromatic;

    [SerializeField, Tooltip("Ø‚è‘Ö‚¦‚é‘O‚Ì”wŒi")]
    private Image _swapBackGround;

    [SerializeField, Tooltip("ƒtƒF[ƒh‚·‚éŽžŠÔ")]
    private float _fadeInterbal = 1.5f;

    private Tweener _tweener;

    private void Start()
    {
        BackGrountChange();
    }

    private void FadeIn() 
    {
        var c = _monochromatic.color;
        c = new Color(c.r, c.g, c.b, 0);

        DOTween.To(() => _monochromatic.color,
            x => _monochromatic.color = x,
            c,
            _fadeInterbal); 
    }

    private void FadeOut() 
    {
    
    }

    private void BackGrountChange() 
    {
        var c = _swapBackGround.color;
        c = new Color(c.r, c.g, c.b, 0);

        DOTween.To(() => _swapBackGround.color,
            x => _swapBackGround.color = x,
            c,_fadeInterbal);
    }
}
