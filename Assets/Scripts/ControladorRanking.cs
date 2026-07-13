//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class ControladorRanking : MonoBehaviour
//{
//    [SerializeField] private CloudServices cloudServices;
//    [SerializeField] private CardRanking cardRankingPrefab;
//    [SerializeField] private Transform rankingContent;

//    public async void CarregarRanking()
//    {     
//        Debug.Log("Scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
//        Debug.Log("Ranking Content: " + rankingContent.name);
//        Debug.Log(rankingContent.gameObject.scene.name);


//        foreach (Transform child in rankingContent)
//        {
//            Destroy(child.gameObject);
//        }
//        await System.Threading.Tasks.Task.Yield();

//        List<JogadorRanking> jogadores = await CloudServices.Instance.GetPontuacoes();
//        Debug.Log($"Quantidade de jogadores: {jogadores.Count}");

//        foreach (JogadorRanking jogadorRanking in jogadores)
//        {
//            CardRanking card = Instantiate(cardRankingPrefab, rankingContent);
//            card.IniciarCard(jogadorRanking.posicao + 1, jogadorRanking.username, jogadorRanking.pontuacao);
//        }
//    }
//}


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
        Debug.Log("Scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log("Ranking Content: " + rankingContent.name);
        Debug.Log(rankingContent.gameObject.scene.name);

        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        await System.Threading.Tasks.Task.Yield();        

        List<JogadorRanking> jogadores = await CloudServices.Instance.GetPontuacoes();
        Debug.Log($"Quantidade de jogadores: {jogadores.Count}");

        foreach (JogadorRanking jogadorRanking in jogadores)
        {
            CardRanking card = Instantiate(cardRankingPrefab, rankingContent);

            card.IniciarCard(
                jogadorRanking.posicao + 1,
                jogadorRanking.username,
                jogadorRanking.pontuacao
            );
        }
    }

}