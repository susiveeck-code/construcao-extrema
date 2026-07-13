using UnityEngine;

public class BotaoInterstitial : MonoBehaviour
{
    public void MostrarAnuncio()
    {
        Debug.Log("TESTE: o botão chamou MostrarAnuncio.");

        InterstitialAd interstitialAd =
            FindFirstObjectByType<InterstitialAd>();

        if (interstitialAd == null)
        {
            Debug.LogWarning(
                "Não foi encontrado nenhum objeto com o componente InterstitialAd. " +
                "Inicie o jogo pela cena MenuInicial."
            );

            return;
        }

        interstitialAd.ShowAd();
    }
}