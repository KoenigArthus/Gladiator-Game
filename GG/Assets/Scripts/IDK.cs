using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IDK : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CardLibrary.Setup();
       CardObject c =  CardObject.Instantiate(CardLibrary.Cards.Where(x => x.Set == CardSet.Trident).First(), gameObject.transform.position);
        GameObject.Destroy(c);
    }

}
