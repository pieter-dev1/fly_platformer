using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public static Vector3 respawnPoint = new Vector3(2.3f, 0.47f, -28f);
    public static List<Collider> checkpointDialogueTriggers;

    [SerializeField] private Transform[] _checkpoints;
    public static Transform[] checkpoints { get; private set; }
    public static int progress = 0;

    private void Awake()
    {
        checkpoints = _checkpoints;
    }
}
