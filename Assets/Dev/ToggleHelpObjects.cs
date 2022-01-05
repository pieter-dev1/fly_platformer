using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHelpObjects : MonoBehaviour
{
    [SerializeField] private Controls controls;
    [SerializeField] private GameObject helpObjects;

    private void Start()
    {
        controls.toggleHelpObjects.started += _ =>
        {
            helpObjects.SetActive(!helpObjects.activeInHierarchy);
        };
    }

    private void OnEnable()
    {
        controls.toggleHelpObjects.Enable();
    }

    private void OnDisable()
    {
        controls.toggleHelpObjects.Disable();
    }
}
