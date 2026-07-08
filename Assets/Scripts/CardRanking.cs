using TMPro;
using UnityEngine;

public class CardRanking : MonoBehaviour
{
    [SerializeField] private TMP_Text posicaoText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text pontuacaoText;

    public void IniciarCard(int posicao, string username, int pontuacao)
    {
        posicaoText.text = posicao.ToString();
        usernameText.text = username;
        pontuacaoText.text = pontuacao.ToString();
    }
}
