using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoutZone : MonoBehaviour
{
    public bool onlyKoWhenGrounded = false;
    [SerializeField]
    private GameObject allowedGround; //only used when onlyKoWhenGrounded = true

    private void OnTriggerEnter(Collider other)
    {
        //print($"{other.name}: {((!onlyKoWhenGrounded && other.tag == Tags.PLAYER) || (other.tag == Tags.PLAYER && onlyKoWhenGrounded && (!other.GetComponent<FauxAttractor>().enabled || allowedGround == other.GetComponent<FauxAttractor>().currentSurface.transform.gameObject)))}");
        //if ((!onlyKoWhenGrounded && other.tag == Tags.PLAYER) || (other.tag == Tags.PLAYER && onlyKoWhenGrounded && (!other.GetComponent<FauxAttractor>().enabled || gameObject == other.GetComponent<FauxAttractor>().currentSurface.transform.gameObject)))
        //{
        //    print("su");
        //    other.GetComponent<PlayerInput>().ToLastCheckpoint();
        //}
        if((!onlyKoWhenGrounded && other.tag == Tags.PLAYER) || (onlyKoWhenGrounded && other.tag == Tags.PLAYER && other.GetComponent<EntityStats>().grounded))
        {
            other.GetComponent<PlayerInput>().ToLastCheckpoint();
            
            //Resets rotation properly
            other.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
