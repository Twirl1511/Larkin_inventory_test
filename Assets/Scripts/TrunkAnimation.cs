using System;
using DG.Tweening;
using UnityEngine;

public class TrunkAnimation : MonoBehaviour
{
    [Tooltip("Объект, вокруг которого будет вращаться крышка")]
    [SerializeField] private Transform _pivotTransform;

    [Space()]
    [Header("Настройки анимации")]
    [SerializeField] private float _openAngle = -90f;
    [SerializeField] private float _openDuration = 1.5f;
    [SerializeField] private float _closeDuration = 1f;
    [SerializeField] private AnimationCurve _openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _closeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        if (_pivotTransform != null)
        {
            _pivotTransform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        if (_pivotTransform == null)
            throw new NullReferenceException($"_pivotTransform is null!");

        _pivotTransform.DOLocalRotate(new Vector3(_openAngle, 0f, 0f), _openDuration).SetEase(_openCurve);
    }

    [ContextMenu("Close")]
    public void Close()
    {
        if (_pivotTransform == null)
            throw new NullReferenceException($"_pivotTransform is null!");

        _pivotTransform.DOLocalRotate(Vector3.zero, _closeDuration).SetEase(_closeCurve);
    }
}
