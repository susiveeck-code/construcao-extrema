using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControladorRanking : MonoBehaviour
{
    [SerializeField] private CloudServices cloudServices;
    [SerializeField] private CardRanking cardRankingPrefab;
    [SerializeField] private Transform rankingContent;
    public async void CarregarRanking()
    {
        foreach(Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        List<JogadorRanking> jogadores = await cloudServices.GetPontuacoes();

        foreach(JogadorRanking jogadorRanking in jogadores)
        {
            CardRanking card = Instantiate(cardRankingPrefab, rankingContent);
            card.IniciarCard(jogadorRanking.posicao + 1, jogadorRanking.username, jogadorRanking.pontuacao);
        }
    }
}
 