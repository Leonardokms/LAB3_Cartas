using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageBotoes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartJogo()
    {
        SceneManager.LoadScene("Lab3");       // Carrega a cena "Lab3"
    }

    public void FimJogo()
    {
        SceneManager.LoadScene("Lab3_Fim");     // Carrega a cena "Lab3_Fim"
    }
    
}