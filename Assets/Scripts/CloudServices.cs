
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

/*
    Jogo inicia
      │
      ▼
    Inicializa os serviços da Unity
      │
      ▼
    Conecta o jogador na nuvem
      │
      ▼
    Se der certo:
        mostra o PlayerID
    Se der erro:
        abre um popup
*/


public class CloudServices : MonoBehaviour
{
    [SerializeField] private GameObject erroLoginPopup;
    public static CloudServices Instance { get; private set; }

    private const string LeaderboardId = "pontuacoes";

    private Task tarefaInicializacao;
    private bool servicosProntos = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        tarefaInicializacao = InicializarELogar();
    }

    private async Task InicializarELogar()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {                
                var options = new InitializationOptions();
                options.SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                Debug.Log("Unity Services inicializado com sucesso.");
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Sign in anonymously succeeded!");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }

            if (string.IsNullOrEmpty(AuthenticationService.Instance.PlayerName))
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync("Player");
            }

            Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");

            servicosProntos = true;
            Debug.Log("CloudServices pronto para usar Leaderboards.");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);

            if (erroLoginPopup != null)
                erroLoginPopup.SetActive(true);
        }
    }

    private async Task GarantirPronto()
    {
        if (tarefaInicializacao != null)
        {
            await tarefaInicializacao;
        }

        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        while (!servicosProntos)
        {
            await Task.Delay(100);
        }

    }

    public async Task SignInAnonymouslyAsync()
    {
        await GarantirPronto();
    }

    public async void TentarLoginNovamente()
    {
        if (erroLoginPopup != null)
            erroLoginPopup.SetActive(false);

        tarefaInicializacao = InicializarELogar();
        await tarefaInicializacao;
    }

    public async Task AtualizarUserName(string username)
    {
        await GarantirPronto();
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
       
        await GarantirPronto();

        Debug.Log("Vou tentar salvar no Leaderboard agora...");

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, pontuacao);

        Debug.Log($"Pontuação enviada para o Leaderboard: {pontuacao}");

    }

    public async Task<List<JogadorRanking>> GetPontuacoes()
    {
        await GarantirPronto();


        Debug.Log($"Unity State: {UnityServices.State}");
        Debug.Log($"Signed In: {AuthenticationService.Instance.IsSignedIn}");



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
        await GarantirPronto();

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