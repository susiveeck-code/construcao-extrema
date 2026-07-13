//using UnityEngine;
//using UnityEngine.Events;

//public class GameOver : MonoBehaviour
//{
//    [SerializeField] private UnityEvent OnGameOver;

//    // Quando um cubo qualquer atravessar/tocar o Box Collider da água, este método é invocado.
//    private void OnTriggerEnter(Collider other)
//    {
//        // Algum objeto entrou no Trigger da água, invoca GameOver
//        OnGameOver.Invoke();
//        Time.timeScale = 0; // Tudo para
//    }
//}

using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGameOver;

    private bool jogoTerminou;

    private void OnTriggerEnter(Collider other)
    {
        // Procura o CuboColisao no objeto ou no objeto pai.
        CuboColisao cubo =
            other.GetComponentInParent<CuboColisao>();

        // Impede que outros objetos ativem o Game Over.
        if (cubo == null)
            return;

        // Se o cubo já havia recebido ponto, ele será descontado.
        cubo.CaiuNaAgua();

        // O desconto é tratado acima.
        // Mas a tela de Game Over deve ser aberta apenas uma vez.
        if (jogoTerminou)
            return;

        jogoTerminou = true;

        OnGameOver.Invoke();
        Time.timeScale = 0;
    }
}