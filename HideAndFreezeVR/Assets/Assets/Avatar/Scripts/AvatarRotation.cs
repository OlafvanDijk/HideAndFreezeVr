using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRotation : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    private GameObject player;
    private Transform transformPlayer;

    private bool Oculus = true;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitForContainer());
    }

    private IEnumerator WaitForContainer()
    {
        yield return new WaitForSeconds(1f);
        player = GetComponent<VRAvatarController>().containerObject;
        yield return new WaitUntil(() => player != null);
        this.transformPlayer = player.transform;
    }

    // Update is called once per frame
    void Update() {
        if (Oculus)
        {
            try
            {
                if (transformPlayer != null)
                {
                    float newXRotation = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
                    if (newXRotation != 0)
                    {
                        Vector3 oldRotation = transformPlayer.rotation.eulerAngles;
                        transformPlayer.eulerAngles = new Vector3(oldRotation.x, oldRotation.y + (rotationSpeed * newXRotation), oldRotation.z);
                    }
                }
            }
            catch (System.Exception)
            {
                Oculus = false;
            }
        }
    }
}
