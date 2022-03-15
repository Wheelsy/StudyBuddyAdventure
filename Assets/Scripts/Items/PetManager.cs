using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] petPrefabs;
    private int curActivePet = 99;

    public Transform flyingSpawnPos;
    public Transform groundedSpawnPos;

    public int CurActivePet { get => curActivePet; set => curActivePet = value; }

    public void TurnOnPet(int index)
    {
        if(CurActivePet != 99)
        {
            CurActivePet = index;
        }
        else
        {
            TurnOffPet();
        }

        if (petPrefabs[index].GetComponent<Pet>().grounded)
        {
            Instantiate(petPrefabs[index], groundedSpawnPos.position, Quaternion.identity);
        }
        else
        {
            Instantiate(petPrefabs[index], flyingSpawnPos.position, Quaternion.identity);
        }
    }

    public void TurnOffPet()
    {
        Destroy(GameObject.FindGameObjectWithTag("Pet"));
        curActivePet = 99;
    }
}
