using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    public void Jogar()
    {
        Time.timeScale = 1;

        // Carrega a cena do jogo
        SceneManager.LoadScene(1);
    }

    public void VoltarHome()
    {
        Time.timeScale = 1;

        // Carrega a cena do Menu
        SceneManager.LoadScene(0);
    }

    public void AbrirRanking()
    {

    }

    public void RemoverAnuncios()
    {

    }
}
