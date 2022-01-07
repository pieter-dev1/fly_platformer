using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KnockoutZone : MonoBehaviour
{
    public bool onlyKoWhenGrounded = false;
    [SerializeField]
    private GameObject allowedGround; //only used when onlyKoWhenGrounded = true

    public static int timesKod = 0;
    public static bool skipAvailable;
    private static bool wasSkipAvailableBefore = false;
    [SerializeField] private GameObject skipIcon;

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
            timesKod++;
            if (timesKod == 10 && Challenge.lastReachedCheckpoint < Challenge.checkpoints.Count)
            {
                skipIcon.SetActive(true);
                skipAvailable = true;
                var nextCheckpoint = Challenge.checkpoints[Challenge.lastReachedCheckpoint + 1].GetComponent<CheckpointDetector>();
                other.GetComponent<PlayerPause>().AddTpButton(nextCheckpoint.tpButton, false);
                //if (!wasSkipAvailableBefore)
                //{
                //    GameObject.Find("SkipDialogue").GetComponent<DialogueDetector>().TriggerDialogue();
                //    wasSkipAvailableBefore = true;
                //}
            }

            var playerPos = other.transform.position;
            if (playerPos.z >= Challenge.respawnPoint.z) {
                other.GetComponent<PlayerInput>().ToLastCheckpoint();
            }
            else
            {
                var nearestCheckpoint = Challenge.checkpoints.OrderBy(x => Vector3.Distance(playerPos, x.position)).First();
                var checkpointIndex = Challenge.checkpoints.IndexOf(nearestCheckpoint);
                var allowedIndex = checkpointIndex < Challenge.lastReachedCheckpoint ? checkpointIndex : Challenge.lastReachedCheckpoint;
                other.GetComponent<PlayerInput>().ToCheckpoint(allowedIndex);
            }
               
            //Resets rotation properly
            other.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
