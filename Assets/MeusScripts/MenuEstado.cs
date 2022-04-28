using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEstado : MonoBehaviour
{
    public GameObject workspace;
    public GameObject menuEstadoObj;
    public GameObject enunciado;
    public GameObject menuDeletarEstado;

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
        FecharMenuDeletarEstado();
    }
    public void AbrirMenuDeletarEstado()
    {
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        menuDeletarEstado.SetActive(true);
        workspace.GetComponent<Workspace>().EsconderQuadro();
    }
    public void FecharMenuDeletarEstado()
    {
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        menuDeletarEstado.SetActive(false);
        workspace.GetComponent<Workspace>().MostrarQuadro();
    }
    public void NovaTrans()
    {
        if (!(workspace.GetComponent<Workspace>().GetAlfabeto() == null)) // Se tiver alfabeto
        {
            //workspace.GetComponent<Workspace>().SetNovaTransFlag(true);
            workspace.GetComponent<Workspace>().FecharMenuEstado();
            workspace.GetComponent<Workspace>().AbrirMenuNovaTrans();
        }
        else
        {
            SSTools.ShowMessage("Informe o alfabeto primeiro", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
    public void SetFinal()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(2).gameObject.activeInHierarchy)
        {
            //estado.transform.GetChild(2).gameObject.SetActive(true);
            estado.GetComponent<Estado>().SetFinal(true);
            workspace.GetComponent<Workspace>().SetQuantosEstadosFinais(workspace.GetComponent<Workspace>().GetQuantosEstadosFinais() + 1);
            workspace.GetComponent<Workspace>().AddEstadoFinal(estado);
        }
        else
        {

            //estado.transform.GetChild(2).gameObject.SetActive(false);
            estado.GetComponent<Estado>().SetFinal(false);
            workspace.GetComponent<Workspace>().SetQuantosEstadosFinais(workspace.GetComponent<Workspace>().GetQuantosEstadosFinais() - 1);
            workspace.GetComponent<Workspace>().RemoverEstadoFinal(estado);

        }
        enunciado.GetComponent<Enunciado>().AtulizarEstadosFinais();
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        

    }
    public void SetInicial()
    {
        GameObject estado = workspace.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(3).gameObject.activeSelf)
        {
            //estado.transform.GetChild(3).gameObject.SetActive(true);
            estado.GetComponent<Estado>().SetInicial(true);
            workspace.GetComponent<Workspace>().SetPossuiEstadoInicial(true);
            workspace.GetComponent<Workspace>().SetEstadoInicial(estado);
         }
        else
        {
            //estado.transform.GetChild(3).gameObject.SetActive(false);
            estado.GetComponent<Estado>().SetInicial(false);
            workspace.GetComponent<Workspace>().SetPossuiEstadoInicial(false);
            workspace.GetComponent<Workspace>().RemoverEstadoInicial();
        }
        workspace.GetComponent<Workspace>().FecharMenuEstado();
        enunciado.GetComponent<Enunciado>().AtulizarEstadoInicial();
    }
    public void SetButtonEstado(bool status)
    {
        buttonEstadoInicial.interactable = status;
    }

}
