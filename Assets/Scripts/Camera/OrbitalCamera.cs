using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OrbitalCamera : MonoBehaviour
{
    private CinemachineOrbitalTransposer cinemachineOrbitalTransposer;
    private bool isRotating;
    [SerializeField]private float rotateSpeed;
    private void Start()
    {
        cinemachineOrbitalTransposer = GetComponent<CinemachineOrbitalTransposer>();
    }
    public void Enable()
    {
        if (isRotating) return;
        isRotating = true;
    }

    public void Disable()
    {
        if (!isRotating) return;
        isRotating = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(isRotating)
        {
            cinemachineOrbitalTransposer.m_XAxis.Value += Time.deltaTime * rotateSpeed;
        }
        
    }
}
