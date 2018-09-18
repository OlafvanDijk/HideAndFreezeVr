using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MultiVRUtil
{
    /// <summary>
    /// A velocity with a magnitude less than this value can be considered as zero.
    /// Values below this will give errors when processed to other forms such as rotations.
    /// </summary>
    public const float ZERO_VELOCITY = 0.001f;

    /// <summary>
    /// Clones a component to a new game object using reflection.
    /// This should effectively be the same as adding a component to an object, then copying all values.
    /// This will however also include private / internal and other values.
    /// </summary>
    /// <param name="comp">The component to clone.</param>
    /// <param name="target">The target game object, the clone component will be added to it.</param>
    /// <returns>The cloned component, added to the target object.</returns>
    public static T CloneComponent<T>(T comp, GameObject target) where T : Component
    {
        System.Type type = comp.GetType();

        T copy = (T) target.AddComponent(type);

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (System.Reflection.FieldInfo field in type.GetFields(flags)) // Copy all fields.
        {
            field.SetValue(copy, field.GetValue(comp));
        }

        // Custom cloning for audio source, since it doesn't support cloning with the method above.
        if (comp is AudioSource)
        {
            AudioSource src = comp as AudioSource;
            AudioSource dst = copy as AudioSource;

            dst.mute = src.mute;
            dst.bypassEffects = src.bypassEffects;
            dst.bypassListenerEffects = src.bypassListenerEffects;
            dst.bypassReverbZones = src.bypassReverbZones;
            dst.playOnAwake = src.playOnAwake;
            dst.loop = src.loop;

            dst.priority = src.priority;
            dst.volume = src.volume;
            dst.pitch = src.pitch;
            dst.panStereo = src.panStereo;
            dst.spatialBlend = src.spatialBlend;
            dst.reverbZoneMix = src.reverbZoneMix;

            dst.dopplerLevel = src.dopplerLevel;
            dst.spread = src.spread;

            dst.rolloffMode = src.rolloffMode;
            dst.minDistance = src.minDistance;
            dst.maxDistance = src.maxDistance;

            foreach (AudioSourceCurveType curve in System.Enum.GetValues(typeof(AudioSourceCurveType)))
            {
                dst.SetCustomCurve(curve, src.GetCustomCurve(curve));
            }
        }

        return copy;
    }

    /// <summary>
    /// Calculate a lerp scalar for this frame based on a total time.
    /// ie. Your animation takes 5 seconds total, this frame takes 0.1s
    /// You will receive back (0.1 / 5) = 0.02
    /// This also handles if the total time is zero or less 
    /// which can cause division by zero problems, so it will always return 1 in that case.
    /// </summary>
    /// <param name="totalTime"></param>
    /// <returns></returns>
    public static float LerpThisFrame(float totalTime)
    {
        return Time.deltaTime / Mathf.Max(totalTime, Time.deltaTime);
    }

    /// <summary>
    /// Makes a referral for this object.
    /// Useful for when objects are moved somewhere completely different, but you still want to go to it quickly.
    /// </summary>
    /// <param name="source"></param>
    public static Referral MakeReferral(GameObject source)
    {
        GameObject clone = new GameObject(source.name + " (Referral)");

        Referral referral = clone.AddComponent<Referral>();

        referral.refersTo = source;

        Transform parent = source.transform.parent;

        if (parent != null)
        {
            clone.transform.SetParent(source.transform.parent);
            clone.transform.SetSiblingIndex(source.transform.GetSiblingIndex()); // Take over the same position
        }

        return referral;
    }
}

public static class Extension
{
    /// <summary>
    /// Returns a vector's raw values as a string. (No rounding applied)
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static string ToRawString(this Vector3 vec)
    {
        return string.Format("({0}, {1}, {2})", vec.x, vec.y, vec.z);
    }

    /// <summary>
    /// Returns a vector's rounded to 4 significant digits.
    /// This is more detailed than the default Vector3.ToString() which only has 1 significant digit.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static string ToDetailedString(this Vector3 vec)
    {
        return vec.ToString("G4");
    }

    /// <summary>
    /// Check if a layer is contained inside this LayerMask.
    /// </summary>
    /// <returns></returns>
    public static bool Contains(this LayerMask layermask, int layer)
    {
        // A layermask is just an array of bools/bits encoded as an integer.
        // (1 << layer) creates a LayerMask with just the input layer. (ie. 00100 if we are the 3rd layer)
        // Then we use inclusive OR on it with the layermask, meaning that it will always return 1 on a bit if either is 1.
        // So in that case, once it comes to the index of the layer, if the layermask has a 0 there, but the layer does, it will not be equal.
        // Eg. 0101 == (0101 | 0010), looking if the 3rd layer is contained inside, 
        // the result is 0111, which is not equal to 0101 because the 3rd layer is not included. 
        return layermask == (layermask | (1 << layer));
    }

    /// <summary>
    /// Copy this transform to the target transform
    /// </summary>
    public static void CopyTo(this Transform from, Transform to, bool position = true, bool rotation = true, bool localScale = true)
    {
        if (position)
            to.position = from.position;

        if (rotation)
            to.rotation = from.rotation;

        if (localScale)
            to.localScale = from.localScale;
    }

    /// <summary>
    /// Copy the local values of this transform to the target transform
    /// </summary>
    public static void CopyLocalTo(this Transform from, Transform to, bool localPosition = true, bool localRotation = true, bool localScale = true)
    {
        if (localPosition)
            to.localPosition = from.localPosition;

        if (localRotation)
            to.localRotation = from.localRotation;

        if (localScale)
            to.localScale = from.localScale;
    }
}
