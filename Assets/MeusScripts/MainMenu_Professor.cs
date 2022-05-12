using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class MainMenu_Professor : MonoBehaviour
{
    public void MeusExercicios()
    {
        SceneManager.LoadScene("MeusExercicios");
    }
    public void MeusProjetos()
    {
        SceneManager.LoadScene("MeusProjetos_Professor");
    }
    public void Sair()
    {
        Debug.Log("Saindo");
        SceneManager.LoadScene("LoginECadastro");
    }
    public void CadastrarExercicios()
    {
        StateNameController.IdProject = "";
        SceneManager.LoadScene("Workspace_Professor");
    }
}
