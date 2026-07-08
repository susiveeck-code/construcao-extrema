//using System;
//using System.Threading.Tasks;
//using Unity.Services.Authentication;
//using Unity.Services.Core;
//using UnityEngine;
//using Unity.Services.Leaderboards;
//using Unity.Services.Leaderboards.Models;
//using System.Collections.Generic;



///*
//    Jogo inicia
//      │
//      ▼
//    Inicializa os serviços da Unity
//      │
//      ▼
//    Conecta o jogador na nuvem
//      │
//      ▼
//    Se der certo:
//        mostra o PlayerID
//    Se der erro:
//        abre um popup
//*/

//public class CloudServices : MonoBehaviour
//{
//    [SerializeField] private GameObject erroLoginPopup;


//    public async Task SignInAnonymouslyAsync()
//    {
//        // O jogador já está logado?
//        if (AuthenticationService.Instance.IsSignedIn) return;

//        try
//        {
//            // Aqui acontece realmente o login. A Unity cria um jogador automaticamente
//            await AuthenticationService.Instance.SignInAnonymouslyAsync();
//            if ((AuthenticationService.Instance.PlayerName == "") || (AuthenticationService.Instance.PlayerName == null)) 
//            {
//                await AtualizarUserName("Player");
//                Debug.Log(AuthenticationService.Instance.PlayerName);
//            }

//            Debug.Log("Sign in anonymously succeeded!");

//            // A Unity cria um identificador único. Esse ID identifica somente aquele jogador
//            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

//        }

//        catch (AuthenticationException ex)
//        {
//            Debug.LogException(ex);
//            erroLoginPopup.SetActive(true);
//        }
//        catch (RequestFailedException ex)
//        {
//            Debug.LogException(ex);
//            erroLoginPopup.SetActive(true);
//        }
//    }

//    public async void TentarLoginNovamente()
//    {
//        erroLoginPopup.SetActive(false);

//        try
//        {
//            if (UnityServices.State != ServicesInitializationState.Initialized)
//            {
//                await UnityServices.InitializeAsync();
//            }

//            await SignInAnonymouslyAsync();
//        }
//        catch (Exception e)
//        {
//            Debug.LogException(e);
//            erroLoginPopup.SetActive(true);
//        }
//    }

//    public async Task AtualizarUserName(string username)
//    {
//        await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
//    }

//    public string GetUserName()
//    {
//        return AuthenticationService.Instance.PlayerName;
//    }

//    public async Task SalvarPontuacao(int pontuacao)
//    {
//        await LeaderboardsService.Instance.AddPlayerScoreAsync("pontuacoes", pontuacao);
//    }

//    public async Task<List<JogadorRanking>> GetPontuacoes()
//    {
//        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("pontuacoes");

//        List<JogadorRanking> jogadoresRanking = new List<JogadorRanking>();

//        foreach (LeaderboardEntry entry in scoresResponse.Results)
//        { 
//            JogadorRanking jogador = new JogadorRanking();
//            jogador.posicao = entry.Rank;
//            jogador.username = entry.PlayerName;
//            jogador.pontuacao = (int) entry.Score;

//            jogadoresRanking.Add(jogador);
//        }
//        return jogadoresRanking;       
//    }

//    public async Task<int> GetPontuacaoJogador()
//    {
//        try
//        {
//            var result = await LeaderboardsService.Instance.GetPlayerScoreAsync("pontuacoes");
//            return (int)result.Score;
//        }
//        catch
//        {
//            return 0;
//        }

//    }
//}

using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

public class CloudServices : MonoBehaviour
{
    [SerializeField] private GameObject erroLoginPopup;

    private const string LeaderboardId = "pontuacoes";

    private async void Start()
    {
        await SignInAnonymouslyAsync();
    }

    private async Task InicializarUnityServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services inicializado com sucesso.");
        }
    }
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("AWAKE - Unity Services inicializado");
    }
    public async Task SignInAnonymouslyAsync()
    {
        await InicializarUnityServices();

        if (AuthenticationService.Instance.IsSignedIn)
            return;

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
            {
                await AtualizarUserName("Player");
            }

            Debug.Log("Sign in anonymously succeeded!");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            erroLoginPopup.SetActive(true);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            erroLoginPopup.SetActive(true);
        }
    }

    public async void TentarLoginNovamente()
    {
        erroLoginPopup.SetActive(false);

        try
        {
            await SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            erroLoginPopup.SetActive(true);
        }
    }

    public async Task AtualizarUserName(string username)
    {
        await InicializarUnityServices();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await SignInAnonymouslyAsync();
        }

        await AuthenticationService.Instance.UpdatePlayerNameAsync(username);
    }

    public string GetUserName()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
            return "Player";

        return AuthenticationService.Instance.PlayerName;
    }

    public async Task SalvarPontuacao(int pontuacao)
    {
        await SignInAnonymouslyAsync();

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, pontuacao);

        Debug.Log($"Pontuação enviada para o Leaderboard: {pontuacao}");
    }

    public async Task<List<JogadorRanking>> GetPontuacoes()
    {
        await SignInAnonymouslyAsync();

        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);

        List<JogadorRanking> jogadoresRanking = new List<JogadorRanking>();

        foreach (LeaderboardEntry entry in scoresResponse.Results)
        {
            JogadorRanking jogador = new JogadorRanking();
            jogador.posicao = entry.Rank;
            jogador.username = entry.PlayerName;
            jogador.pontuacao = (int)entry.Score;

            jogadoresRanking.Add(jogador);
        }

        return jogadoresRanking;
    }

    public async Task<int> GetPontuacaoJogador()
    {
        await SignInAnonymouslyAsync();

        try
        {
            var result = await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
            return (int)result.Score;
        }
        catch
        {
            return 0;
        }
    }
}
