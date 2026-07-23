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
        
        // Faz o movimento seguir a direção para onde a câmera está olhando        
        Vector3 direcaoCamera = myCamera.TransformDirection(entradasJogador);
        direcaoCamera.y = 0;

        // Acessando a propriedade de posicao do Cubo instanciado e incrementando ela com este novo vetor 
        ultimoCuboGerado.transform.position += direcaoCamera.normalized * Time.deltaTime * 3;
       
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
        if (ultimoCuboGerado == null)
            return;

        GameObject cuboQueFoiSolto = ultimoCuboGerado;
        ultimoCuboGerado = null;

        Rigidbody rigidbodyCubo = cuboQueFoiSolto.GetComponent<Rigidbody>();

        if (rigidbodyCubo != null)
        {
            rigidbodyCubo.useGravity = true;
        }

        if (cuboQueFoiSolto.transform.childCount > 0)
        {
            cuboQueFoiSolto.transform.GetChild(0).gameObject.SetActive(false);
        }

        OnSoltarCubo.Invoke();
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
