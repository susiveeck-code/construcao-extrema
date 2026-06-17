using UnityEngine;
using UnityEngine.Events;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGameOver;

    // Quando um cubo qualquer atravessar/tocar o Box Collider da água, este método é invocado.
    private void OnTriggerEnter(Collider other)
    {
        // Algum objeto entrou no Trigger da água, invoca GameOver
        OnGameOver.Invoke();
        Time.timeScale = 0; // Tudo para
    }
}
