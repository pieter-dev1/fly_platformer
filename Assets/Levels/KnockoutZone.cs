using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoutZone : MonoBehaviour
{
    public bool onlyKoWhenGrounded = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((!onlyKoWhenGrounded && other.tag == Tags.PLAYER) || (other.tag == Tags.PLAYER && onlyKoWhenGrounded && (!other.GetComponent<FauxAttractor>().enabled || gameObject == other.GetComponent<FauxAttractor>().currentSurface.transform.gameObject)))
        {
            other.GetComponent<PlayerInput>().ToLastCheckpoint();
        }
    }



    
}
