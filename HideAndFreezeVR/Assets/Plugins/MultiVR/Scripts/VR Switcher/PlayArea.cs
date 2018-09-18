using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using HutongGames.PlayMaker;
using VRTK;
using UnityEngine.SceneManagement;

public class PlayArea : MonoBehaviour
{
    private static PlayArea _instance;

    public static PlayArea instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// Was the play area initialized?
    /// </summary>
    public static bool wasInitialized
    {
        get
        {
            return _instance != null;
        }
    }

    [SerializeField]
    private PlayAreaType _type;

    public PlayAreaType type
    {
        get
        {
            return _type;
        }
    }

    [SerializeField]
    private VRTK_SDKSetup _setup;

    public VRTK_SDKSetup SDK
    {
        get
        {
            return _setup;
        }
    }

    private void Awake()
    {
        _instance = this;

        SDK_Setup setup = GetComponentInParent<SDK_Setup>();

        MultiVRUtil.MakeReferral(setup.playAreaAttacher.gameObject);

        // Make the play area attacher our parent.
        Transform parent = setup.playAreaAttacher;

        parent.SetParent(setup.playAreaRoot, true);
        parent.position = transform.position;
        parent.rotation = transform.rotation;
        parent.localScale = transform.localScale;

        transform.SetParent(setup.playAreaAttacher, true);

        //FsmVariables globals = FsmVariables.GlobalVariables;

        //globals.GetFsmObject("PlayArea").Value = this;
        //globals.GetFsmGameObject("PlayAreaGameObject").Value = parent.gameObject;

        // Call setup specific initializers.
        switch (type)
        {
            case PlayAreaType.STEAM:
                _awakeSteam();
                break;
            case PlayAreaType.OCULUS:
                _awakeOculus();
                break;
            case PlayAreaType.GEAR:
                _awakeGear();
                break;
        }

        // Awake the scene
        foreach (GameObject rootObj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (Component comp in rootObj.GetComponentsInChildren<Component>(true))
            {
                if (comp is WaitForPlayAreaAwake)
                {
                    WaitForPlayAreaAwake awake = comp as WaitForPlayAreaAwake;

                    if (!awake.gameObject.activeInHierarchy && !awake._callAwakeEvenWhenDisabled)
                        continue;

                    //Debug.Log("Awakening " + awake, awake);
                    awake.OnPlayAreaAwake(this);
                }
                else if (comp is IWaitForPlayAreaAwakeAlways)
                {
                    (comp as IWaitForPlayAreaAwakeAlways).OnPlayAreaAwake(this);
                }
                else if (comp is IWaitForPlayAreaAwake)
                {
                    if (!comp.gameObject.activeInHierarchy)
                        continue;

                    (comp as IWaitForPlayAreaAwake).OnPlayAreaAwake(this);
                }
            }
        }
    }

    /// <summary>
    /// Broadcast the play area type to PlayMaker so we can switch depending on the setup.
    /// </summary>
    private void FixedUpdate()
    {
        string playAreaTypeName = System.Enum.GetName(typeof(PlayAreaType), type);

        //PlayMakerFSM.BroadcastEvent(playAreaTypeName); // Let every SFM know what type of play area we are
    }

    ///// <summary>
    ///// Specific initialization for Simulated
    ///// </summary>
    //protected virtual void _awakeSimulated()
    //{
    //    // Disable the simulated rig if singleplayer
    //    if (VRNetworkLobbyManager.GetInstance().IsSingleplayer)
    //    {
    //        TinkerDisabler disabler = FindObjectOfType<TinkerDisabler>();
            
    //        if (disabler != null)
    //            disabler.HandleDisablingSimulator(gameObject);
    //    }
    //}

    /// <summary>
    /// Specific initialization for SteamVR / HTC Vive
    /// </summary>
    protected virtual void _awakeSteam()
    {

    }

    /// <summary>
    /// Specific initialization for Oculus Rift
    /// </summary>
    protected virtual void _awakeOculus()
    {
        
    }

    /// <summary>
    /// Specific initialization for GearVR
    /// </summary>
    protected virtual void _awakeGear()
    {
        
    }
}

/// <summary>
/// Interface for listening in to the play area awakening.
/// Note that this does not get called even when the object is disabled.
/// </summary>
public interface IWaitForPlayAreaAwake
{
    void OnPlayAreaAwake(PlayArea playArea);
}

/// <summary>
/// Interface for listening in to the play area awakening.
/// This gets called even if the behaviour it is on is disabled.
/// </summary>
public interface IWaitForPlayAreaAwakeAlways : IWaitForPlayAreaAwake
{
    
}

/// <summary>
/// Abstract class for listening in to the play area awakening. (It does so after most other things because of VRTK)
/// </summary>
public abstract class WaitForPlayAreaAwake : MonoBehaviour, IWaitForPlayAreaAwake
{
    [SerializeField]
    [UnityEngine.Tooltip("If this object is disabled, set this to true if you still want it to be notified of the play area awaking.")]
    protected internal bool _callAwakeEvenWhenDisabled = false;

    /// <summary>
    /// Called when the play area awakes
    /// </summary>
    public abstract void OnPlayAreaAwake(PlayArea playArea);
}

/// <summary>
/// The type of play area we are dealing with.
/// NOTE: DO NOT CHANGE, these names are used in global FSM events to change based on play area type.
/// </summary>
public enum PlayAreaType
{
    SIMULATED, STEAM, OCULUS, GEAR
}
