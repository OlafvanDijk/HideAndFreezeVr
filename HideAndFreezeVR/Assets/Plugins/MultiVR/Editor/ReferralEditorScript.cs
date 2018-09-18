using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Referral))]
public class ReferralEditorScript : Editor
{
    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        GameObject obj = Selection.activeGameObject;

        if (obj != null)
        {
            Referral referral = obj.GetComponent<Referral>();

            if (referral != null)
            {
                referral.StartCoroutine(_changeSelection(referral));
            }
        }
    }

    private IEnumerator _changeSelection(Referral referral, bool manual = false)
    {
        if (!manual)
            yield return new WaitForEndOfFrame();

        if (!manual && !Application.isPlaying)
            yield break; // We only refer while playing.

        Debug.Log(referral);

        GameObject target = referral.refersTo;

        Selection.activeGameObject = target; // Change the selection.
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Referral referral = target as Referral;

        if (GUILayout.Button("Go to referred object"))
        {
            referral.StartCoroutine(_changeSelection(referral, true));
        }
    }
}
