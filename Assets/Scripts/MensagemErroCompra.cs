using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class MensagemErroCompra : MonoBehaviour
{
    [SerializeField] private GameObject painelMensagem;
    [SerializeField] private TMP_Text textoMensagem;
    //private bool jogadorTentouComprar = false;

    public void ProdutoNaoEncontrado(
        ProductDefinition produto,
        string motivo)
    {
        //if (!jogadorTentouComprar)
        //{
        //    Debug.LogWarning("Produto não encontrado durante a inicialização.");
        //    return;
        //}

        Debug.LogError(
            $"Não foi possível carregar o produto: {produto.id}. Motivo: {motivo}"
        );

        if (textoMensagem != null)
        {
            textoMensagem.text =
                "Não foi possível conectar à loja.\n" +
                "Verifique sua internet e tente novamente.";
        }

        if (painelMensagem != null)
        {
            painelMensagem.SetActive(true);
        }
    }

    public void FecharMensagem()
    {
        if (painelMensagem != null)
        {
            painelMensagem.SetActive(false);
        }
    }

    public void CompraFalhou(FailedOrder pedidoComFalha)
    {
        Debug.LogError($"A compra falhou: {pedidoComFalha}");

        if (textoMensagem != null)
        {
            textoMensagem.text =
                "Não foi possível concluir a compra.\n" +
                "Tente novamente mais tarde.";
        }

        if (painelMensagem != null)
        {
            painelMensagem.SetActive(true);
        }
    }
}