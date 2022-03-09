using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] petPrefabs;
    [SerializeField]
    private Transform spawnPos;
    private int curActivePet = 99;

    public int CurActivePet { get => curActivePet; set => curActivePet = value; }

    public void TurnOnPet(int index)
    {
        if(CurActivePet != 99)
        {
            CurActivePet = index;
        }

        Instantiate(petPrefabs[index], spawnPos.position, Quaternion.identity);
    }

    public void TurnOffPet()
    {
        Destroy(GameObject.FindGameObjectWithTag("Pet"));
        curActivePet = 99;
    }
}
