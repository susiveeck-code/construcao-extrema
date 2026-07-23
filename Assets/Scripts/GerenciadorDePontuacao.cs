using UnityEngine;
using TMPro;

public class GerenciadorDePontuacao : MonoBehaviour
{
    [SerializeField] private TMP_Text pontuacaoText;
    [SerializeField] private TMP_Text pontuacaoGameOverText;

    private CloudServices cloudServices;
    private int pontuacao;

    private void Awake()
    {
        cloudServices = CloudServices.Instance;
        AtualizarTextos();
    }

    public void AdicionarPontuacao()
    {
        pontuacao++;
        AtualizarTextos();
    }

    public void RemoverPontuacao()
    {
        // Impede a pontuação de ficar negativa.
        pontuacao = Mathf.Max(0, pontuacao - 1);
        AtualizarTextos();
    }

    private void AtualizarTextos()
    {
        pontuacaoText.text = pontuacao.ToString();
        pontuacaoGameOverText.text = "SCORE: " + pontuacao;
    }

    public async void RegistrarPontuacao()
    {
        await cloudServices.SalvarPontuacao(pontuacao);
    }
}






//using UnityEngine;
//using TMPro;
//using Unity.Cinemachine;

//public class GerenciadorDePontuacao : MonoBehaviour
//{
//    [Header("Textos da pontuação")]
//    [SerializeField] private TMP_Text pontuacaoText;
//    [SerializeField] private TMP_Text pontuacaoGameOverText;

//    [Header("Ajuste da câmera")]
//    [SerializeField] private CinemachinePositionComposer positionComposer;

//    [Tooltip("Pontuação em que a câmera começa a baixar o ponto de visão.")]
//    [SerializeField] private int pontuacaoInicialCamera = 8;

//    [Tooltip("Pontuação em que a câmera chega ao deslocamento máximo.")]
//    [SerializeField] private int pontuacaoFinalCamera = 14;

//    [Tooltip("Valor inicial do Target Offset Y.")]
//    [SerializeField] private float offsetInicialY = 0f;

//    [Tooltip("Valor final do Target Offset Y.")]
//    [SerializeField] private float offsetFinalY = -3f;

//    private CloudServices cloudServices;
//    private int pontuacao;

//    private void Awake()
//    {
//        cloudServices = CloudServices.Instance;

//        AtualizarTextos();
//        AtualizarCamera();
//    }

//    public void AdicionarPontuacao()
//    {
//        pontuacao++;

//        AtualizarTextos();
//        AtualizarCamera();
//    }

//    public void RemoverPontuacao()
//    {
//        // Impede a pontuação de ficar negativa.
//        pontuacao = Mathf.Max(0, pontuacao - 1);

//        AtualizarTextos();
//        AtualizarCamera();
//    }

//    private void AtualizarTextos()
//    {
//        pontuacaoText.text = pontuacao.ToString();
//        pontuacaoGameOverText.text = "SCORE: " + pontuacao;
//    }

//    private void AtualizarCamera()
//    {
//        if (positionComposer == null)
//            return;

//        float progresso = Mathf.InverseLerp(
//            pontuacaoInicialCamera,
//            pontuacaoFinalCamera,
//            pontuacao
//        );

//        float novoOffsetY = Mathf.Lerp(
//            offsetInicialY,
//            offsetFinalY,
//            progresso
//        );

//        Vector3 targetOffset = positionComposer.TargetOffset;
//        targetOffset.y = novoOffsetY;
//        positionComposer.TargetOffset = targetOffset;
//    }

//    public async void RegistrarPontuacao()
//    {
//        await cloudServices.SalvarPontuacao(pontuacao);
//    }
//}