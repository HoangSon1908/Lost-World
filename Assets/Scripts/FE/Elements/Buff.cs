using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RemoveBuff());
    }

    IEnumerator RemoveBuff()
    {
        yield return new WaitForSeconds(5);
        GameManager.Instance.RemoveBuff();
        Destroy(gameObject);
    }
}
