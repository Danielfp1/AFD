using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class Menu_Workspace : MonoBehaviour
{
    public GameObject menuWorkspaceUI;
    public GameObject menuNovoWorkspaceObj;
    public GameObject workspace;

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

    public void AbrirMenuNovoWorkspace()
    {
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        workspace.GetComponent<Workspace>().EsconderQuadro();
        menuWorkspaceUI.SetActive(false);
        menuNovoWorkspaceObj.SetActive(true);
    }
    public void FecharMenuNovoWorkspace()
    {
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        workspace.GetComponent<Workspace>().MostrarQuadro();
        menuWorkspaceUI.SetActive(true);
        menuNovoWorkspaceObj.SetActive(false);
    }
    public void OkMenuNovoWorkspace()
    {
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        workspace.GetComponent<Workspace>().MostrarQuadro();
        menuNovoWorkspaceObj.SetActive(false);
        workspace.GetComponent<Workspace>().ApagarWorkspace();
    }
}
