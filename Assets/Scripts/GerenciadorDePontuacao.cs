using UnityEngine;
using TMPro;

public class GerenciadorDePontuacao : MonoBehaviour
{
    [SerializeField] private TMP_Text pontuacaoText;
    [SerializeField] private TMP_Text pontuacaoGameOverText;
    private int pontuacao;

    public void AdicionarPontuacao()
    {
        pontuacao++;
        pontuacaoText.text = pontuacao.ToString();
        pontuacaoGameOverText.text = "SCORE: " + pontuacao.ToString();
    }

}
