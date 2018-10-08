using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAvatar : MonoBehaviour {

    [SerializeField]
    private VRAvatarController avatarController;
    [SerializeField]
    private int timesToScale;

    private GameObject eye;
    private List<float> scales;
    private GameObject avatar;

    private int t;
    private int countScale = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Test();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.GetInstanceID());
        Test();
    }

    private void Test()
    {
        scales = new List<float>();
        if (avatarController != null)
        {
            avatar = avatarController.actualAvatarVRIK.gameObject;
            VRReferences references = avatar.GetComponent<VRReferences>();

            eye = references.eyeObject;
            if (eye != null)
            {
                //int time = timesToScale;
                //t = timesToScale;
                //while (time > 0)
                //{
                //    Debug.Log("inwhile");
                //    if (time != 0 && t == time)
                //    {
                //        Debug.Log("inwhileif");
                //        time--;
                //        Invoke("Test2", 1f);
                //    }
                //}
                //ApplyScale(CalculateMedian(avatar.transform.localScale.x));
                StartCoroutine(StartCalculation());
            }
        }
    }

    private void Test2()
    {
        scales.Add(Calculate(avatarController.gameObject, eye));
        t--;
        Debug.Log("hoi" + t);
    }

    private IEnumerator StartCalculation()
    {
        int time = timesToScale;
        Debug.Log("Time: " + time);
        for (int x = 0; x<time; x++)
        {
            scales.Add(Calculate(avatarController.gameObject, eye));
            //time--;
            Debug.Log("1a");
            yield return new WaitForSeconds(0.2f);
            Debug.Log("2a");
        }
        Debug.Log("3a");
        ApplyScale(CalculateMedian());
    }

    private float Calculate(GameObject firstObject, GameObject secondObject)
    {
        Ray firstRay = new Ray(firstObject.transform.position, Vector3.down);
        //Debug.Log(firstObject.transform.position);
        //Debug.DrawRay(firstObject.transform.position, Vector3.down * 1000, Color.red,2);
        Ray secondRay = new Ray(secondObject.transform.position, Vector3.down);
        //Debug.DrawRay(secondObject.transform.position, Vector3.down * 1000, Color.blue, 2);
        RaycastHit firstRayHit, secondRayHit;
        float heightFirst = -1, heightSecond = -1;
        if (Physics.Raycast(firstRay, out firstRayHit, 5))//, LayerMask.NameToLayer("Player")))
        {
            //Debug.Log("Hello");
            if (firstRayHit.collider.gameObject.layer != LayerMask.NameToLayer("IgnoreRaycast"))
            {
                Debug.Log(firstRayHit.collider.gameObject);
                //Debug.Log(firstRayHit.collider.gameObject.layer);
                //Debug.Log(firstRayHit.collider.gameObject);

                heightFirst = firstRayHit.distance;
                //Debug.Log("world!");
            }
        }

        if (Physics.Raycast(secondRay, out secondRayHit, 5, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore)) //&& secondRayHit.collider.gameObject.layer != LayerMask.NameToLayer("IgnoreRaycast"))
        {
            //Debug.Log(secondRayHit.collider.gameObject.layer);
            //Debug.Log(secondRayHit.collider.gameObject);
            heightSecond = secondRayHit.distance;
        }

        //heightFirst = GetHeight(firstRay, firstRayHit, "Player", "IgnoreRaycast");
        //heightSecond = GetHeight(secondRay, secondRayHit, "Player", "IgnoreRaycast");

        //Debug.Log("First heigt: " + heightFirst);
        //Debug.Log("Second heigt: " + heightSecond);

        //float ratio = heightFirst / heightSecond;
        //Debug.Log(ratio);
        //return (heightFirst == -1 || heightSecond == -1) ? 1 : ratio;
        return heightFirst;
    }

    private float GetHeight(Ray ray, RaycastHit hit, string layerName, string layerToExclude)
    {
        if (Physics.Raycast(ray, out hit, 5, LayerMask.NameToLayer(layerName)) && hit.collider.gameObject.layer != LayerMask.NameToLayer(layerToExclude))
        {
            //Debug.Log(hit.collider.gameObject.layer);
            //Debug.Log(hit.collider.gameObject);
            return hit.distance;
        }
        return 0;
    }

    private float CalculateMedian()
    {
        float median;
        int scalesCount = scales.Count;
        scales.Sort();
        foreach (float item in scales)
        {
            Debug.Log("median: " + item);
        }
        // Even amount of elements
        if (scales.Count % 2 == 0)
        {
            median = 0.5f * (scales[scalesCount / 2 - 1] + scales[scalesCount / 2]);
        }
        // Odd amount of elements
        else
        {
            median = scales[(scalesCount + 1) / 2 - 1];
        }
        Debug.Log(median + " * " + 1);
        return median * 1;
    }

    private void ApplyScale(float scale)
    {
        //Debug.Log(scale);
        scale *= 0.8f;
        avatar.transform.localScale = new Vector3(scale, scale, scale);
        countScale++;
        if (countScale < 2)
        {
            Test();
        }
    }
}
