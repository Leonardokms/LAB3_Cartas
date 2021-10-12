using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostraUltimoENovoRecord : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        // Define o texto de recorde utilizando as PlayerPrefs de recorde antigo e recorde novo
        GameObject.Find("txtRecorde").GetComponent<Text>().text = "Recorde antigo: " + PlayerPrefs.GetInt("RecordeAntigo") + "\nRecorde novo: " + PlayerPrefs.GetInt("Recorde");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
