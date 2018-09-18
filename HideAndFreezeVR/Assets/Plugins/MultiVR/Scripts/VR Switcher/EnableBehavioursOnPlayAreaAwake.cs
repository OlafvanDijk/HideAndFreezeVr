using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBehavioursOnPlayAreaAwake : WaitForPlayAreaAwake
{
    [SerializeField]
    private float _delay;

    [SerializeField]
    private Behaviour[] _toAwake;

    private void Update() // Need this so we can disable the behaviour.
    {
        
    }

    private void Awake()
    {
        if (!enabled)
            return;

        if (PlayArea.wasInitialized)
            OnPlayAreaAwake(PlayArea.instance);
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        if (!enabled)
            return;

        if (_delay > 0)
            StartCoroutine(_awakeDelayed());
        else
            Activate();
    }

    public void Activate()
    {
        foreach (Behaviour behaviour in _toAwake)
        {
            behaviour.enabled = true;
        }
    }

    private IEnumerator _awakeDelayed()
    {
        yield return new WaitForSeconds(_delay);

        Activate();
    }
}
