using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Attaches all scripts on this fake camera to the actual camera used by the current SDK when the play area awakes.
/// It also takes care of references so that it will not stay connected to the previous camera.
/// If your script still has errors, you can make it implement <see cref="WaitForAttachToCamera"/> to manually transfer any information.
/// OR you can use the events exposed in the instance of this class.
/// </summary>
public class AttachToCameraOnPlayAreaAwake : WaitForPlayAreaAwake
{
    /// <summary>
    /// Called when the attachment of the camera happens.
    /// </summary>
    public static event Action<CameraAttachArgs> AttachCameraAwake;

    /// <summary>
    /// Called when the attachment of the camera has happened, and after all awakes have been called.
    /// </summary>
    public static event Action<CameraAttachArgs> AttachCameraStart;

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        Referral referral = MultiVRUtil.MakeReferral(gameObject);

        Camera old = GetComponent<Camera>();
        Camera main = Camera.main;

        // Copy camera specifics
        main.clearFlags = old.clearFlags;
        main.backgroundColor = old.backgroundColor;
        main.cullingMask = old.cullingMask;

        // If we disable the new camera, Awake() and Start() will not be called on any new objects just yet.
        main.gameObject.SetActive(false);

        referral.refersTo = main.gameObject;

        // A list of all components, and the component it was replaced with.
        Dictionary<Component, Component> replaced = new Dictionary<Component, Component>();

        // A list of field reference to any of the existing components, to be replaced once all transfers are done.
        Dictionary<FieldAndComponent, Component> toBeReplacedFields = new Dictionary<FieldAndComponent, Component>();

        foreach (Component comp in GetComponents<Component>())
        {
            bool nonTransferrable = comp is Transform || comp is Camera; // Do not transfer transform and camera.
            bool isDisabled = comp is Behaviour && !(comp as Behaviour).enabled; // Do not transfer disabled scripts.

            if (nonTransferrable || isDisabled)
            {
                //Debug.Log("Not transferring " + comp + " because it is " + (nonTransferrable ? "not transferrable" : "disabled") + ".");
                continue;
            }

            //Debug.Log("Transferring " + comp);
            _transferNormalComponent(comp, old, main, ref replaced, ref toBeReplacedFields);
        }

        // Since we can not replace references to components we just transferred, we must first record all places where we need to
        // replace something, and record what components got replaced by what new instance.
        // Then afterwards, we can go over all the places that need replacing, and replace it with the new instance of whatever component
        // was previously there, that is what we're doing here.
        foreach (KeyValuePair<FieldAndComponent, Component> set in toBeReplacedFields)
        {
            Component replacedInstance = replaced[set.Value]; // Get the new instance of the previous component, this should not fail, ever.

            set.Key.SetValue(replacedInstance); // Replace the value of the field in the new instance, with the replaced instance of the other component.

            //Debug.Log("Field " + set.Key._field.Name + " in " + set.Key._comp + " was replaced with new instance " + set.Value);
        }

        Destroy(gameObject);

        WaitForAttachToCamera[] listeners = GameObject.FindObjectsOfType<WaitForAttachToCamera>();

        CameraAttachArgs args = new CameraAttachArgs() { oldCamera = old, newCamera = main };

        foreach (WaitForAttachToCamera attach in listeners)
        {
            attach.OnAttachToCameraAwake(args);
        }

        if (AttachCameraAwake != null)
            AttachCameraAwake(args);

        foreach (WaitForAttachToCamera attach in listeners)
        {
            attach.OnAttachToCameraStart(args);
        }

        if (AttachCameraStart != null)
            AttachCameraStart(args);

        main.gameObject.SetActive(true); // Re-enable the main camera.
    }

    /// <summary>
    /// Transfer a normal component from this object to the target.
    /// </summary>
    private void _transferNormalComponent(Component fromComponent, Camera fromCamera, Camera toCamera, ref Dictionary<Component, Component> replaced, ref Dictionary<FieldAndComponent, Component> toBeReplaced)
    {
        Component copy = MultiVRUtil.CloneComponent<Component>(fromComponent, toCamera.gameObject);

        replaced.Add(fromComponent, copy);

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        Type type = fromComponent.GetType();

        FieldInfo[] fields = type.GetFields(flags);

        foreach (System.Reflection.FieldInfo field in fields) // Go over all fields
        {
            Type fieldType = field.FieldType;

            if (fieldType.Equals(typeof(Camera))) // Find any camera's
            {
                Camera cameraField = (Camera)field.GetValue(copy);

                if (cameraField != null && cameraField.Equals(fromCamera)) // Check if it was our camera
                {
                    field.SetValue(copy, toCamera); // Replace any references to our old camera with the new one.
                }
            }
            else if (fieldType.IsSubclassOf(typeof(Component)))
            {
                Component compField = (Component)field.GetValue(copy);

                if (compField != null)
                {
                    // Go over existing components to see if it refers to one of these.
                    foreach (Component comp in fromCamera.GetComponents<Component>())
                    {
                        if (compField.Equals(comp)) // If it does
                        {
                            // Record the fact that we have a reference so we can replace it later.
                            toBeReplaced.Add(new FieldAndComponent() { _field = field, _comp = copy }, comp);
                            break;
                        }
                    }
                }
            }
        }
    }

    protected struct FieldAndComponent
    {
        internal FieldInfo _field;
        internal Component _comp;

        /// <summary>
        /// Set the value of the field in the supplied component to a new value using reflection.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(Component value)
        {
            _field.SetValue(_comp, value);
        }
    }
}

/// <summary>
/// Camera attachment arguments.
/// </summary>
public struct CameraAttachArgs
{
    /// <summary>
    /// The camera that was attached to the original gameobject.
    /// </summary>
    public Camera oldCamera;

    /// <summary>
    /// The camera that the new script was attached to.
    /// Note that the gameobject this camera is attached to will be inactive when you receive this.
    /// That is because we're avoiding it calling Awake() and Start() on any components until the attach awake and start happened.
    /// However, the camera was guaranteed active before you received it here, and will be active again after attach awake and start have happened.
    /// </summary>
    public Camera newCamera;
}

/// <summary>
/// Abstract class for receiving camera attach messages by default.
/// You can also use the events in AttachToCameraOnPlayAreaAwake
/// </summary>
public abstract class WaitForAttachToCamera : MonoBehaviour
{
    /// <summary>
    /// Called when the attachment of the camera happens.
    /// </summary>
    public virtual void OnAttachToCameraAwake(CameraAttachArgs args)
    {

    }

    /// <summary>
    /// Called when the attachment of the camera has happened, and after all awakes have been called.
    /// </summary>
    public virtual void OnAttachToCameraStart(CameraAttachArgs args)
    {

    }
}
