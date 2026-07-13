//using UnityEngine;

//public class CuboColisao : MonoBehaviour
//{
//    [SerializeField] private AudioSource colisaoAudioSource;

//    private bool primeiraColisao;
//    private GerenciadorDePontuacao gerenciadorDePontuacao;

//    private void Start()
//    {
//        gerenciadorDePontuacao =
//            FindFirstObjectByType<GerenciadorDePontuacao>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        colisaoAudioSource.PlayOneShot(colisaoAudioSource.clip);

//        if (primeiraColisao)
//            return;

//        primeiraColisao = true;

//        if (gerenciadorDePontuacao != null)
//        {
//            gerenciadorDePontuacao.AdicionarPontuacao();
//        }
//        else
//        {
//            Debug.LogWarning("GerenciadorDePontuacao não foi encontrado.");
//        }

//        Invoke(nameof(DesativarComponente), 5);
//    }

//    private void DesativarComponente()
//    {
//        enabled = false;
//    }
//}

using UnityEngine;

public class CuboColisao : MonoBehaviour
{
    [SerializeField] private AudioSource colisaoAudioSource;

    private bool primeiraColisao;
    private bool recebeuPonto;

    private GerenciadorDePontuacao gerenciadorDePontuacao;

    private void Start()
    {
        gerenciadorDePontuacao =
            FindFirstObjectByType<GerenciadorDePontuacao>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (colisaoAudioSource != null)
        {
            colisaoAudioSource.PlayOneShot(colisaoAudioSource.clip);
        }

        if (primeiraColisao)
            return;

        primeiraColisao = true;

        if (gerenciadorDePontuacao != null)
        {
            gerenciadorDePontuacao.AdicionarPontuacao();
            recebeuPonto = true;
        }
        else
        {
            Debug.LogWarning(
                "GerenciadorDePontuacao não foi encontrado."
            );
        }

        Invoke(nameof(DesativarComponente), 5);
    }

    public void CaiuNaAgua()
    {
        // Só desconta se este cubo realmente recebeu um ponto.
        if (!recebeuPonto)
            return;

        recebeuPonto = false;

        if (gerenciadorDePontuacao != null)
        {
            gerenciadorDePontuacao.RemoverPontuacao();
        }
    }

    private void DesativarComponente()
    {
        enabled = false;
    }
}