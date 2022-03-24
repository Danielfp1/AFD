using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class Menu_Workspace : MonoBehaviour
{
    public GameObject menuWorkspaceUI;

    public void ButtonMenuPressed()
    {
        if (menuWorkspaceUI.activeSelf)
        {
            menuWorkspaceUI.SetActive(false);
        }
        else
        {
            menuWorkspaceUI.SetActive(true);
        }
    }
    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene(1);
    }
}
