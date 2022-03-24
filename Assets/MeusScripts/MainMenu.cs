using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class MainMenu : MonoBehaviour
{
    public void MenuOptionListaDeExercicios()
    {
        SceneManager.LoadScene(4);
    }

    public void MenuOptionMeusProjetos()
    {
        SceneManager.LoadScene(5);
    }

    public void FecharJogo()
    {
        Debug.Log("Fechando Aplicação");
        Application.Quit();  
    }

    public void Sair()
    {
        Debug.Log("Saindo");
        SceneManager.LoadScene(0);
    }

}
