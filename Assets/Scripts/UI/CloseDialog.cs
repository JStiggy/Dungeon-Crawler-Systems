using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialog : MonoBehaviour
{
    public void Close() {
        gameObject.SetActive(false);
    }
}
