using UnityEngine;
using Kataru;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Collections;
using JnA.Core.ScriptableObjects;

public class KataruVCamLocal : Handler
{
    [SerializeField] VCamEvent vCamEvent;
    #region KATARU FIELDS
    [SerializeField] [Dropdown("CharacterList")] string reference;
    protected List<string> CharacterList() => Characters.All();
    protected override string Name
    {
        get => reference.ToString();
    }
    #endregion

    /// <summary>
    /// Follow this character for duration
    /// If duration is -1, then revert vcam follow manually
    /// </summary>
    /// <param name="duration"></param>
    [CommandHandler(local: true, autoNext: false)]
    void VCamFollow(double duration, bool wait)
    {
        vCamEvent.Follow(transform);
        if (duration == -1)
        {
            Runner.Next();
            return;
        }
        StartCoroutine(RunVCamFollow(duration, wait));
    }

    IEnumerator RunVCamFollow(double duration, bool wait)
    {
        if (!wait) Runner.Next();
        yield return new WaitForSeconds((float)duration);
        StopFollow();
        if (wait) Runner.Next();
    }

    // must call this if called VCamFollow(duration: -1)
    [CommandHandler(local: true)]
    void StopFollow()
    {
        vCamEvent.StopFollow();
    }
}
