using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GeradorDeCubos : MonoBehaviour
{
    [SerializeField] private GameObject cuboPrefab;
    private GameObject ultimoCuboGerado;
    private AlturaDaConstrucao alturaDaConstrucao;
    private Transform myCamera;
    [SerializeField] private UnityEvent OnSoltarCubo;

    private Vector3 entradasJogador;
    void Start()
    {
        myCamera = Camera.main.transform;
        alturaDaConstrucao = GetComponent<AlturaDaConstrucao>();
        GerarCubo();
    }
    void Update()
    {
        if (ultimoCuboGerado == null) { return; }

        // Criado vetor com as setas Horizontal, vertical e as teclas WSAD
        //Vector3 entradasJogador = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Faz o movimento seguir a direção para onde a câmera está olhando        
        Vector3 direcaoCamera = myCamera.TransformDirection(entradasJogador);
        direcaoCamera.y = 0;

        // Acessando a propriedade de posicao do Cubo instanciado e incrementando ela com este novo vetor 
        ultimoCuboGerado.transform.position += direcaoCamera.normalized * Time.deltaTime * 3;

        // Se jogador clicou na barra de espaco, o cubo é solto
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SoltarCubo();

        //    // Invoca o método GerarCubo depois de 3 segundos
        //    Invoke(nameof(GerarCubo), 3);
        //}
    }
    private void GerarCubo()
    {
        // Instancia cubo com altura atual + 2 metros, já no X e Z passa valores aleatórios 
        // para o cubo nao nascer sempre na mesma posicao. Também nao tem rotacao
        ultimoCuboGerado = Instantiate(cuboPrefab, new Vector3(Random.Range(-3, 4), alturaDaConstrucao.AlturaAtual() + 2, Random.Range(-3, 4)), Quaternion.identity);

        // Seleciona tamanho aleatório para o cubo criado        
        int tamanhoX = Random.Range(1, 5);
        int tamanhoY = Random.Range(1, 3);
        int tamanhoZ = Random.Range(1, 5);

        // Redefine o tamanho do cubo gerado
        ultimoCuboGerado.transform.localScale = new Vector3(tamanhoX, tamanhoY, tamanhoZ);

        // Troca o cubo de cor de forma aleatória
        //ultimoCuboGerado.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(); // Cores feias

        Color[] cores =
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.magenta,
            Color.cyan,
            new Color(1f, 0.5f, 0f),   // laranja
            new Color(1f, 0.2f, 0.6f), // rosa
            new Color(0.5f, 0f, 1f)    // roxo
        };

        ultimoCuboGerado.GetComponent<MeshRenderer>().material.color =
            cores[Random.Range(0, cores.Length)];

    }

    public void SoltarCubo()
    {
        // Ativa gravidade do Cubo
        ultimoCuboGerado.GetComponent<Rigidbody>().useGravity = true;

        // Pega o primeiro filho desse objeto (no caso, a linha brilhosa) e o desativa quando soltar o cubo
        ultimoCuboGerado.transform.GetChild(0).gameObject.SetActive(false);

        // Torna a instancia do cubo nula para que o jogador nao a mova mais
        ultimoCuboGerado = null;

        // Toca um som ao soltar o cubo
        OnSoltarCubo.Invoke();

        // Invoca o método GerarCubo depois de 3 segundos
        Invoke(nameof(GerarCubo), 3);
    }

    public void MoverCubo(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        entradasJogador = new Vector3(input.x, 0, input.y);
    }

    public void SoltarCubo(InputAction.CallbackContext value)
    {
        // Chama o Método quando o Evento de click for iniciado
        if (value.started)
        {
            SoltarCubo();
        }
    }
}
