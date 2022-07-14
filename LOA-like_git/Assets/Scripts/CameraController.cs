using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [Required] public Camera cam;
    public static CameraController instance;
    [Required] public Transform target;
    public Vector3 chaseOffset;
    public float smoothnessTime;

    Vector3 targetY0 = Vector3.zero;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        SetPosition();
    }

    [Button]
    public void DebugPosition()
    {
        targetY0 = target.position;
        targetY0.y = 0;
        transform.position = targetY0 + chaseOffset;
        transform.LookAt(target);

    }
    public void SetPosition()
    {
        targetY0 = target.position;
        targetY0.y = 0;
        transform.DOMove(targetY0 + chaseOffset, smoothnessTime).Restart();
    }
}
