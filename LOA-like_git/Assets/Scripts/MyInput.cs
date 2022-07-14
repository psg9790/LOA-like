using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class MyInput : MonoBehaviour
{
    public static MyInput instance;
    public PlayerInput playerInput;
    [BoxGroup("Awake")] public InputActionMap map;
    [BoxGroup("Awake")] public InputAction space;
    [BoxGroup("Awake")] public InputAction mouseDelta;
    [BoxGroup("Awake")] public InputAction mousePosition;
    [BoxGroup("Awake")] public InputAction mouseRight;
    [BoxGroup("Awake")] public InputAction Q;
    [BoxGroup("Awake")] public InputAction W;
    [BoxGroup("Awake")] public InputAction E;
    [BoxGroup("Awake")] public InputAction R;
    [BoxGroup("Awake")] public InputAction A;
    [BoxGroup("Awake")] public InputAction S;
    [BoxGroup("Awake")] public InputAction D;
    [BoxGroup("Awake")] public InputAction F;


    private void Awake()
    {
        instance = this;
        map = playerInput.actions.FindActionMap("Player");
        space = map.FindAction("Space");
        mouseDelta = map.FindAction("MouseDelta");
        mousePosition = map.FindAction("MousePosition");
        mouseRight = map.FindAction("MouseRight");
        Q = map.FindAction("Q");
        W = map.FindAction("W");
        E = map.FindAction("E");
        R = map.FindAction("R");
        A = map.FindAction("A");
        S = map.FindAction("S");
        D = map.FindAction("D");
        F = map.FindAction("F");

    }
}
