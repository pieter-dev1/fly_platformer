using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public static Vector3 respawnPoint = new Vector3(2.3f, 0.47f, -56f);
    public static List<Collider> checkpointDialogueTriggers;

    [SerializeField] private Transform[] _checkpoints;
    public static List<Transform> checkpoints { get; private set; }
    public static int lastReachedCheckpoint = 0;

    public readonly static List<Vector3> positions = new List<Vector3>(new[] {
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

    private void Awake()
    {
        checkpoints = _checkpoints.ToList();
        if(PlayerPrefs.HasKey("lastReachedCheckpoint"))
            lastReachedCheckpoint = PlayerPrefs.GetInt("lastReachedCheckpoint"); //player gets tped to lastReachedCheckpoint at start in PlayerToNextPoint
    }
}
