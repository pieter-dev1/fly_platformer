using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToNextPoint : MonoBehaviour
{
    [SerializeField]
    private int startPointIndex = 0;
    private List<Vector3> positions = new List<Vector3>(new[] {
        //Start
        new Vector3(2.3f, 0.47f, -56),
        //Waffle-Yogurt
        new Vector3(2.3f,0.47f,-28f),
        //Before Books
        new Vector3(2.3f, 0.47f, 10.7f),
        //After Books
        new Vector3(2.3f, 0.47f, 30f),
        //Table
        new Vector3(4.7f, -0.8f, 49.5f),
        //Shelf
        new Vector3(1.3f, 8.5f, 65f),
        //Closet Book
        new Vector3(-3.3f, 6.7f, 78.6f),
        //Cat House
        new Vector3(14f,-2.25f,73.73f),
        //End area
        new Vector3(-8f, 4.7f, 114f)
    });

    private void Start()
    {
        transform.position = positions[startPointIndex];
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
