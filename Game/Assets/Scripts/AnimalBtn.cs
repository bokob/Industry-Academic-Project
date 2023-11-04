using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBtn : MonoBehaviour
{
    public EncyclopediaControl encyclopediaControl;
    public GameObject animalInfo;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickAnimalBtn()
    {
        // string animalName = gameObject.name;
        // string tmp = "Btn";

        // animalName = animalName.Replace(tmp,"");

        encyclopediaControl.ShowAnimalInfo(animalInfo);
    }
}
