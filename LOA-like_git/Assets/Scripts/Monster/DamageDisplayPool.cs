using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class DamageDisplayPool : MonoBehaviour
{
    public Camera cameraForUIpos;
    // public GameObject prefab;
    // public Stack<GameObject> poolStack = new Stack<GameObject>();
    public DamageDisplay damageDisplay;

    [Button]
    public DamageDisplay Pop()
    {
        if (!damageDisplay.isActiveAndEnabled)
        {
            damageDisplay.gameObject.SetActive(true);
        }
        return damageDisplay;
        // if (poolStack.Count > 0)
        // {
        //     GameObject _go = poolStack.Pop();
        //     _go.SetActive(true);
        //     return _go;
        // }
        // else
        // {
        //     GameObject _go = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        //     _go.transform.SetParent(this.transform);
        //     _go.GetComponent<DamageDisplay>().pool = this;
        //     return _go;
        // }
    }
    public void Push(GameObject _go)
    {
        damageDisplay.gameObject.SetActive(false);

        // poolStack.Push(_go);
        // _go.SetActive(false);
    }
    public Vector3 GetUIPos(Transform _targetTransform)
    {
        return cameraForUIpos.WorldToScreenPoint(_targetTransform.position);
    }
}
