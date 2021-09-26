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
    [SerializeField] [Dropdown("NamespaceList")] string kataruNamespace = Namespaces.Global;
    [SerializeField] [Dropdown("CharacterList")] string reference = Characters.None;
    protected string[] NamespaceList() => Namespaces.All();
    protected string[] CharacterList() => Characters.InNamespace(kataruNamespace);
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
    [CommandHandler(character: true, autoNext: false)]
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
    [CommandHandler(character: true)]
    void StopFollow()
    {
        vCamEvent.StopFollow();
    }
}
