using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    [SerializeField] private GameObject botoesMenu;
    [SerializeField] private TMP_Text loadingText;

    private bool carregando;

    // Botão JOGAR da cena MenuInicial
    public void Jogar()
    {
        if (carregando)
            return;

        carregando = true;
        Time.timeScale = 1;

        if (botoesMenu != null)
            botoesMenu.SetActive(false);

        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(true);
            loadingText.text = "CARREGANDO...";
        }

        StartCoroutine(IniciarJogo());
    }

    private IEnumerator IniciarJogo()
    {
        // Permite que a Unity desenhe o texto antes de carregar.
        yield return null;

        AsyncOperation carregamento = SceneManager.LoadSceneAsync(1);

        while (!carregamento.isDone)
        {
            yield return null;
        }
    }

    // Botão JOGAR da tela Score
    public void ReiniciarJogo()
    {
        Time.timeScale = 1;

        // Recarrega a própria cena Game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void AbrirRanking()
    {
    }

    public void RemoverAnuncios()
    {
    }
}