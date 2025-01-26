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
        cinemachineOrbitalTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
        Managers.EventManager.Instance.OnLevelRestart += OnLevelRestart;
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

    private void OnLevelRestart(bool args)
    {
        Disable();
    }
    // On successful level end, orbital camera will rotate around player by rotateSpeed
    void Update()
    {
        if(isRotating)
        {
            cinemachineOrbitalTransposer.m_XAxis.Value += Time.deltaTime * rotateSpeed;
        }
        
    }
}
