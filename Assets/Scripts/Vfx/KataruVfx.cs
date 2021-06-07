using System.Collections;
using System.Collections.Generic;
using JnA.Core.ScriptableObjects;
using Kataru;
using UnityEngine;

namespace JnA.Vfx
{
    public class KataruVfx : Handler
    {
        [SerializeField] VfxEvent vfxEvent;

        [CommandHandler]
        void LerpLUTContribution(double target, double duration)
        {
            vfxEvent.LerpLUTContribution?.Invoke((float)target, (float)duration, DG.Tweening.Ease.InOutCubic);
        }
    }
}