using UnityEngine;

public class FundoMusical : MonoBehaviour
{

    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;
   
    void Start()
    {
        // Acessando um componente do tipo AudioSource
        audioSource = GetComponent<AudioSource>();

        // Seleciona aleatoriamente um elemento da lista (uma música)
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];

        // Toca a música
        audioSource.Play();
    }

  
}
