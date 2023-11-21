using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    [SerializeField] float timeDisable = .25f;
    private void OnEnable()
    {
        Invoke("DisableObject", timeDisable);
    }
    void DisableObject() {
        gameObject.SetActive(false);
    }
}
