//using UnityEngine;
//using TMPro;

//public class GerenciadorDePontuacao : MonoBehaviour
//{
//    [SerializeField] private TMP_Text pontuacaoText;
//    [SerializeField] private TMP_Text pontuacaoGameOverText;
//    private CloudServices cloudServices; 

//    private int pontuacao;


//    private void Awake()
//    {
//        cloudServices = CloudServices.Instance;
//    }
//    public void AdicionarPontuacao()
//    {
//        pontuacao++;
//        pontuacaoText.text = pontuacao.ToString();
//        pontuacaoGameOverText.text = "SCORE: " + pontuacao.ToString();
//    }

//    public async void RegistrarPontuacao()
//    {
//        await cloudServices.SalvarPontuacao(pontuacao);
//    }

//}


using UnityEngine;
using TMPro;

public class GerenciadorDePontuacao : MonoBehaviour
{
    [SerializeField] private TMP_Text pontuacaoText;
    [SerializeField] private TMP_Text pontuacaoGameOverText;

    private CloudServices cloudServices;
    private int pontuacao;

    private void Awake()
    {
        cloudServices = CloudServices.Instance;
        AtualizarTextos();
    }

    public void AdicionarPontuacao()
    {
        pontuacao++;
        AtualizarTextos();
    }

    public void RemoverPontuacao()
    {
        // Impede a pontuação de ficar negativa.
        pontuacao = Mathf.Max(0, pontuacao - 1);
        AtualizarTextos();
    }

    private void AtualizarTextos()
    {
        pontuacaoText.text = pontuacao.ToString();
        pontuacaoGameOverText.text = "SCORE: " + pontuacao;
    }

    public async void RegistrarPontuacao()
    {
        await cloudServices.SalvarPontuacao(pontuacao);
    }
}