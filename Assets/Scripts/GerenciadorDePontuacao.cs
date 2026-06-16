using UnityEngine;
using TMPro;

public class GerenciadorDePontuacao : MonoBehaviour
{
    [SerializeField] private TMP_Text pontuacaoText;
    private int pontuacao;

    public void AdicionarPontuacao()
    {
        pontuacao++;
        pontuacaoText.text = pontuacao.ToString();
    }

}
