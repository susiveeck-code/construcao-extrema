using UnityEngine;
using Unity.Services.LevelPlay;

public class InterstitialAd : MonoBehaviour
{
    [Header("Configurações do LevelPlay")]
    [Tooltip("App Key da sua conta no LevelPlay (disponível no painel do LevelPlay).")]
    [SerializeField] private string appKey = "<Seu AppKey aqui>";

    [Tooltip("ID do Ad Unit de interstitial configurado no painel do LevelPlay.")]
    [SerializeField] private string interstitialAdUnitId = "<Ad Unit aqui>";

    /// <summary>
    /// Referência para o objeto de anúncio intersticial depois que o SDK é inicializado.
    /// É através dele que carregamos e exibimos os anúncios.
    /// </summary>
    private LevelPlayInterstitialAd interstitialAd;

    /// <summary>
    /// Chamado quando o objeto é inicializado pela primeira vez na cena.
    /// Aqui configuramos os callbacks do SDK e iniciamos a inicialização do LevelPlay.
    /// </summary>
    private void Start()
    {
        // Opcional: checa se a integração está correta (útil em builds de teste).
        // LevelPlay.ValidateIntegration();

        // Inscreve nos eventos globais de inicialização do LevelPlay.
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        // Inicia o SDK do LevelPlay usando a App Key configurada.
        Debug.Log("[LevelPlay] Inicializando SDK...");
        LevelPlay.Init(appKey);
    }

    /// <summary>
    /// Chamado quando o objeto é destruído (por exemplo, ao trocar de cena).
    /// Aqui removemos a inscrição dos eventos para evitar leaks e referências penduradas.
    /// </summary>
    private void OnDestroy()
    {
        LevelPlay.OnInitSuccess -= SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed -= SdkInitializationFailedEvent;

        if (interstitialAd != null)
        {
            // Remove handlers dos eventos do interstitial ao destruir o objeto.
            interstitialAd.OnAdLoaded -= InterstitialOnAdLoadedEvent;
            interstitialAd.OnAdLoadFailed -= InterstitialOnAdLoadFailedEvent;
            interstitialAd.OnAdDisplayed -= InterstitialOnAdDisplayedEvent;
            interstitialAd.OnAdDisplayFailed -= InterstitialOnAdDisplayFailedEvent;
            interstitialAd.OnAdClicked -= InterstitialOnAdClickedEvent;
            interstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
            interstitialAd.OnAdInfoChanged -= InterstitialOnAdInfoChangedEvent;
        }
    }

    /// <summary>
    /// Disparado quando o SDK do LevelPlay foi inicializado com sucesso.
    /// Aqui criamos a instância do interstitial e registramos os eventos específicos dela.
    /// </summary>
    /// <param name="config">Configuração retornada pelo SDK após a inicialização.</param>
    private void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log($"[LevelPlay] SDK inicializado com sucesso. Config: {config}");

        // Cria o objeto de interstitial usando o Ad Unit ID configurado.
        interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);

        // Registra os handlers dos eventos do interstitial.
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        Debug.Log("[LevelPlay] Interstitial configurado. Pronto para carregar anúncios.");

        // Já solicita o carregamento do primeiro anúncio assim que o SDK estiver pronto.
        LoadAd();
    }

    /// <summary>
    /// Disparado caso a inicialização do SDK falhe.
    /// Use este método para debug e, se fizer sentido, para tentar uma nova inicialização depois.
    /// </summary>
    /// <param name="error">Informações sobre o erro de inicialização.</param>
    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogError($"[LevelPlay] Falha ao inicializar o SDK. Erro: {error}");
    }

    /// <summary>
    /// Solicita o carregamento de um anúncio intersticial.
    /// Pode ser chamado automaticamente após a inicialização ou em momentos específicos do gameplay.
    /// </summary>
    public void LoadAd()
    {
        if (interstitialAd == null)
        {
            Debug.LogWarning("[LevelPlay] Tentando carregar interstitial antes da inicialização do SDK ou criação do Ad Unit.");
            return;
        }

        Debug.Log("[LevelPlay] Solicitando carregamento de interstitial...");
        interstitialAd.LoadAd();
    }

    /// <summary>
    /// Tenta exibir o anúncio intersticial carregado.
    /// Ideal para ser chamado em momentos chave do jogo (ex: game over, troca de fase).
    /// Verifica antes se o anúncio está pronto usando IsAdReady().
    /// </summary>
    public void ShowAd()
    {
        if (interstitialAd == null)
        {
            Debug.LogWarning("[LevelPlay] Tentando exibir interstitial antes da inicialização do SDK.");
            return;
        }

        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("[LevelPlay] ShowAd chamado, mas o interstitial ainda não está pronto. Considere chamar LoadAd novamente.");
        }
    }

    // ==========================
    // Eventos do Interstitial
    // ==========================

    /// <summary>
    /// Disparado quando o anúncio intersticial é carregado com sucesso.
    ///
    /// Neste momento, o anúncio está pronto para ser exibido.
    /// Você pode escolher:
    /// - Exibir imediatamente (chamando ShowAd() aqui), ou
    /// - Guardar para um momento específico do gameplay e chamar ShowAd() depois.
    /// </summary>
    private void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlay] Interstitial carregado com sucesso. AdInfo: {adInfo}");

        // IsAdReady:
        // Retorna true se o anúncio foi carregado com sucesso e está elegível para exibição
        // (não bloqueado por limites de frequência, capping, etc.), ou false caso contrário.
        //
        // Exemplo de uso:
        // if (interstitialAd.IsAdReady())
        // {
        //     ShowAd();
        // }
    }

    /// <summary>
    /// Disparado quando o carregamento do interstitial falha.
    /// Utilize este evento para logar o erro e, se quiser, agendar uma nova tentativa de LoadAd().
    /// </summary>
    private void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.LogError($"[LevelPlay] Falha ao carregar interstitial. Erro: {error}");
    }

    /// <summary>
    /// Disparado quando o anúncio intersticial é efetivamente exibido na tela.
    /// Bom momento para pausar o jogo, mutar sons ou bloquear entrada do jogador, se necessário.
    /// </summary>
    private void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlay] Interstitial exibido. AdInfo: {adInfo}");
    }

    /// <summary>
    /// Disparado quando ocorre uma falha ao tentar exibir o anúncio (por exemplo, não está mais pronto).
    /// </summary>
    private void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.LogError($"[LevelPlay] Falha ao exibir interstitial. Erro: {error}, AdInfo: {adInfo}");
    }

    /// <summary>
    /// Disparado quando o usuário clica no anúncio intersticial.
    /// Útil para métricas de engajamento ou analytics.
    /// </summary>
    private void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlay] Interstitial clicado. AdInfo: {adInfo}");
    }

    /// <summary>
    /// Disparado quando o anúncio intersticial é fechado pelo usuário.
    /// Geralmente é um bom lugar para:
    /// - Retomar o jogo, e
    /// - Solicitar o carregamento de um novo anúncio (LoadAd()).
    /// </summary>
    private void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlay] Interstitial fechado. AdInfo: {adInfo}");
        Debug.Log("[LevelPlay] Carregando próximo anúncio...");

        // Opcional: já carregar o próximo anúncio depois que o atual for fechado.
        LoadAd();
    }

    /// <summary>
    /// Disparado quando alguma informação do anúncio muda (por exemplo, dados de revenue).
    /// Pode ser útil para tracking, monetização avançada e analytics.
    /// </summary>
    private void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlay] Informações do Interstitial atualizadas. AdInfo: {adInfo}");
    }
}
