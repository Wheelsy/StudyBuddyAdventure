using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void Open(string path)
    {
        Application.OpenURL(path);
    }
}
