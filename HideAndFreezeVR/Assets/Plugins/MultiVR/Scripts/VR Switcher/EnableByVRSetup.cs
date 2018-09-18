using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableByVRSetup : WaitForPlayAreaAwake
{
    [SerializeField]
    [Tooltip("If inclusive, all types inside the array will cause this to enable, if not all setups except those in there will enable this.")]
    private bool _inclusive = true;

    [SerializeField]
    private PlayAreaType[] _types;

    private void Awake()
    {
        if (PlayArea.wasInitialized)
            OnPlayAreaAwake(PlayArea.instance);
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        // Inclusive starts at false since it will turn true when it finds one of the play areas.
        // Exclusive starts at true because it will turn false if it fins one of the play areas.
        bool doEnable = !_inclusive;

        foreach (PlayAreaType type in _types)
        {
            if (playArea.type == type) // Found the play area type.
            {
                doEnable = !doEnable; // Flip the value, so that inclusive will enable, and exclusive will not
                break;
            }
        }

        gameObject.SetActive(doEnable);
    }
}
