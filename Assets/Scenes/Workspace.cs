using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workspace : MonoBehaviour
{
    public int quantosEstados = 0;

    public int getQuantosEstados()
    {
        return quantosEstados;
    }

    public void setQuantosEstados(int quantos)
    {
        quantosEstados = quantos;
    }
    public void novoEstadoAdicionado()
    {
        quantosEstados++;
        //SSTools.ShowMessage(getQuantosEstados().ToString(), SSTools.Position.bottom, SSTools.Time.threeSecond);
    }
}
