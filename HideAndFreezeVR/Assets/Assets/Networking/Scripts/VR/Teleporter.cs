using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public Transform TeleportTo;
    public ParticleSystem particleSystem;

    private void OnTriggerEnter(Collider other)
    {
       LocationDataPlayer player = other.gameObject.GetComponentInParent<LocationDataPlayer>();
        if(player != null)
        {
            StartCoroutine(Teleport(player));
        }
    }

    private IEnumerator Teleport(LocationDataPlayer player)
    {
        yield return new WaitForSeconds(1);
        particleSystem.Play();

        yield return new WaitForSeconds(5);

        player.gameObject.transform.parent.position = Vector3.zero;

        particleSystem.Stop();

    }
}
