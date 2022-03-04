using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Checkbox: MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ToDo td;
    [SerializeField]
    private int index;
    public void OnPointerClick(PointerEventData eventData)
    {
        td.CompleteTask(index, gameObject.GetComponent<Image>());      
    }
}
