using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIColorFlicker : MonoBehaviour
{
    #region MEMBERS

    [SerializeField]
    private Graphic targetGraphic;

    [SerializeField]
    private AnimationCurve flickerEaseAnimationCurve;

    [SerializeField]
    private float flickerDuration;

    [SerializeField]
    private Color endColor = Color.white;

    #endregion

    #region PROPERTIES

    private Graphic TargetGraphic => targetGraphic;
    private AnimationCurve FlickerEaseAnimationCurve => flickerEaseAnimationCurve;
    private float FlickerDuration => flickerDuration;
    private Color EndColor => endColor;

    private Tweener FlickerTween { get; set; }

    #endregion

    #region MONOBEHAVIOUR_CALLBACKS

    private void OnEnable()
    {
        StartTween();
    }


    private void OnDisable()
    {
        StopTween();
    }

    private void Reset()
    {
        targetGraphic = GetComponent<Graphic>();
    }

    #endregion

    #region FUNCTIONS

    private void StartTween()
    {
        if (TargetGraphic != null)
        {
            FlickerTween = TargetGraphic.DOColor(EndColor, FlickerDuration);

            FlickerTween.SetLoops(Constants.TWEEN_INFINITE_LOOPS, LoopType.Yoyo);
            FlickerTween.SetEase(FlickerEaseAnimationCurve);
        }
    }

    private void StopTween()
    {
        if (FlickerTween != null)
        {
            FlickerTween.SetAutoKill();
            FlickerTween = null;
        }
    }

    #endregion

    #region CLASS_ENUMS

    #endregion
}
