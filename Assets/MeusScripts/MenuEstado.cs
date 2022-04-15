using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstado : MonoBehaviour
{
    public GameObject workspace;
    public GameObject menuEstadoObj;
    public GameObject enunciado;

    public Button buttonEstadoInicial;


    public void OnMouseOver()
    {
        //Debug.Log("O mouse esta em cima do menu");
    }

    public void OnMouseExit()
    {
        menuEstadoObj.SetActive(false);
    }

    //Botões
    public void DeletarEstado()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        workspace.GetComponent<Workspace>().RemoverEstado(estado);
        menuEstadoObj.SetActive(false);
    }
    public void NovaTrans()
    {
        workspace.GetComponent<Workspace>().SetNovaTransFlag(true);
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        workspace.GetComponent<Workspace>().AbrirMenuNovaTrans();
    }
    public void SetFinal()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(2).gameObject.activeSelf)
        {
            estado.transform.GetChild(2).gameObject.SetActive(true);
            estado.GetComponent<Estado>().SetFinal(true);
            workspace.GetComponent<Workspace>().SetQuantosEstadosFinais(workspace.GetComponent<Workspace>().GetQuantosEstadosFinais() + 1);
            workspace.GetComponent<Workspace>().AddEstadoFinal(estado);
            enunciado.GetComponent<Enunciado>().AtulizarEstadosFinais();
        }
        else
        {

            estado.transform.GetChild(2).gameObject.SetActive(false);
            estado.GetComponent<Estado>().SetFinal(false);
            workspace.GetComponent<Workspace>().SetQuantosEstadosFinais(workspace.GetComponent<Workspace>().GetQuantosEstadosFinais() - 1);
            workspace.GetComponent<Workspace>().RemoverEstadoFinal(estado);
            enunciado.GetComponent<Enunciado>().AtulizarEstadosFinais();
        }
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        

    }
    public void SetInicial()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(3).gameObject.activeSelf)
        {
            estado.transform.GetChild(3).gameObject.SetActive(true);
            estado.GetComponent<Estado>().SetInicial(true);
            workspace.GetComponent<Workspace>().SetPossuiEstadoInicial(true);
            workspace.GetComponent<Workspace>().SetEstadoInicial(estado);
            enunciado.GetComponent<Enunciado>().AtulizarEstadoInicial();

        }
        else
        {
            estado.transform.GetChild(3).gameObject.SetActive(false);
            estado.GetComponent<Estado>().SetInicial(false);
            workspace.GetComponent<Workspace>().SetPossuiEstadoInicial(false);
            workspace.GetComponent<Workspace>().RemoverEstadoInicial();
            enunciado.GetComponent<Enunciado>().AtulizarEstadoInicial();
        }
        workspace.GetComponent<Workspace>().FecharMenuEstado();
    }
    public void SetButtonEstado(bool status)
    {
        buttonEstadoInicial.interactable = status;
    }

}
