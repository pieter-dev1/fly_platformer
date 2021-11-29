using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToNextPoint : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>(new[] {
        new Vector3(2.3f,0.47f,-28f),
        new Vector3(2.3f, 0.47f, 10.7f),
        new Vector3(2.3f, 0.47f, 30f),
        new Vector3(4.7f, 0.48f, 49.5f),
        new Vector3(1.3f, 8.5f, 60f),
        new Vector3(-3.3f, 6.7f, 78.6f)
    });

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
