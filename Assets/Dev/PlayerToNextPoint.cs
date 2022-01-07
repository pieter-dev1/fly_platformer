using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToNextPoint : MonoBehaviour
{
    [SerializeField]
    private int startPointIndex = 0;
    private List<Vector3> positions;

    private void Start()
    {
        positions = Challenge.positions;
        if (startPointIndex > 0)
            transform.position = positions[startPointIndex];
        else if (Settings.ResetPosOnStart)
            transform.position = positions[0];
        else
            transform.position = Challenge.checkpoints[Challenge.lastReachedCheckpoint].position;

    }

    public void ToPoint(bool nextPoint)
    {
        var safePointIndex = 0;
        var currPosZ = transform.position.z;
        for (int i = 0; i < positions.Count; i++)
        {
            if(positions[i].z > currPosZ)
            {
                safePointIndex = nextPoint ? i : i - 1;
                break;
            }
            else if (i == positions.Count - 1)
            {
                safePointIndex = positions.Count - 2;
            }
        }

        if (safePointIndex < 0 || safePointIndex >= positions.Count)
            return;

        transform.position = positions[safePointIndex];
    }
}
