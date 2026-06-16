using UnityEngine;

public class CuboColisao : MonoBehaviour
{
    [SerializeField] private AudioSource colisaoAudioSource;
    private bool primeiraColisao;

    private void OnCollisionEnter(Collision collision)
    {
        colisaoAudioSource.PlayOneShot(colisaoAudioSource.clip);

        if (!primeiraColisao)
        {
            primeiraColisao = true;
            Invoke(nameof(DesativarComponente), 5);
        }
    }

    private void DesativarComponente()
    {
        enabled = false;
    }
}
