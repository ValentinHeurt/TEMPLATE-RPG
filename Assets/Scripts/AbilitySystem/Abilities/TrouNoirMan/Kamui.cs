using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamui : AbilityBase
{
    [Header("LittleBlackHole")]
    [SerializeField] Transform _kamui;
    [SerializeField] Transform _playerGfx;
    [SerializeField] float _speed;
    [SerializeField] float _delay;
    [SerializeField] float _targetMultiplier;

    private bool _isVisible = true;

    public override void Ability()
    {
        StartCoroutine(ShowKamui());
    }

    IEnumerator ShowKamui()
    {
        Vector3 initialScale = _kamui.localScale;
        Vector3 targetScale = Vector3.one * _targetMultiplier;

        float time = 0;

        while (time != 1)
        {
            time += Time.deltaTime * _speed;
            if (time > 1)
            {
                time = 1;
            }
            _kamui.localScale = Vector3.Lerp(initialScale, targetScale, time);
            yield return new WaitForEndOfFrame();
        }

        if (_isVisible)
        {
            StartCoroutine(HideCharacter());
            yield return new WaitForSeconds(_delay);
            StartCoroutine(HideKamui());
        }
        else
        {
            StartCoroutine(ShowCharacter());
            yield return new WaitForSeconds(_delay);
            StartCoroutine(HideKamui());
        }
        _isVisible = !_isVisible;
    }

    IEnumerator HideCharacter()
    {
        Vector3 initialScale = _playerGfx.localScale;
        Vector3 targetScale = Vector3.zero;

        float time = 0;

        while (time != 1)
        {
            time += Time.deltaTime * _speed;
            if (time > 1)
            {
                time = 1;
            }
            _playerGfx.localScale = Vector3.Lerp(initialScale, targetScale, time);
            _playerGfx.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, time));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ShowCharacter()
    {
        Vector3 initialScale = _playerGfx.localScale;
        Vector3 targetScale = Vector3.one;

        float time = 0;

        while (time != 1)
        {
            time += Time.deltaTime * _speed;
            if (time > 1)
            {
                time = 1;
            }
            _playerGfx.localScale = Vector3.Lerp(initialScale, targetScale, time);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator HideKamui()
    {
        Vector3 initialScale = _kamui.localScale;
        Vector3 targetScale = Vector3.zero;

        float time = 0;

        while (time != 1)
        {
            time += Time.deltaTime * _speed;
            if (time > 1)
            {
                time = 1;
            }
            _kamui.localScale = Vector3.Lerp(initialScale, targetScale, time);
            yield return new WaitForEndOfFrame();
        }
    }
}
