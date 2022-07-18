using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    string protocol = "http://";
    public void Open(string path)
    {
        Application.OpenURL(protocol + path);
    }
}
