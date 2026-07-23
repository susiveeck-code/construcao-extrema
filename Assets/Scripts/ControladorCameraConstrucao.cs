using UnityEngine;
using Unity.Cinemachine;

public class ControladorCameraConstrucao : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform alvoDaCamera;
    [SerializeField] private CinemachinePositionComposer positionComposer;

    [Header("Distância")]
    [SerializeField] private float distanciaInicial = 20f;
    [SerializeField] private float aumentoPorMetro = 0.9f;
    [SerializeField] private float distanciaMaxima = 45f;

    [Header("Suavização")]
    [SerializeField] private float velocidadeAjuste = 3f;

    private float alturaInicial;
    private float distanciaDesejada;

    private void Start()
    {
        if (alvoDaCamera == null || positionComposer == null)
        {
            Debug.LogError(
                "ControladorCameraConstrucao: referências não configuradas."
            );

            enabled = false;
            return;
        }

        alturaInicial = alvoDaCamera.position.y;
        distanciaDesejada = distanciaInicial;

        positionComposer.CameraDistance = distanciaInicial;
    }

    private void LateUpdate()
    {
        float crescimentoDaConstrucao =
            Mathf.Max(0f, alvoDaCamera.position.y - alturaInicial);

        distanciaDesejada =
            distanciaInicial + crescimentoDaConstrucao * aumentoPorMetro;

        distanciaDesejada = Mathf.Min(
            distanciaDesejada,
            distanciaMaxima
        );

        positionComposer.CameraDistance = Mathf.Lerp(
            positionComposer.CameraDistance,
            distanciaDesejada,
            velocidadeAjuste * Time.deltaTime
        );
    }
}