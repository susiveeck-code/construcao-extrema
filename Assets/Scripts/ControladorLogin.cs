using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ControladorLogin : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private GameObject erroLoginPopup;
    [SerializeField] private TMP_Text recordeText;
    [SerializeField] private RemovedorDeAnuncios removedorDeAnuncios;

    private async void Start()
    {
        try
        {
            // Usa a instância permanente que está em DontDestroyOnLoad.
            await CloudServices.Instance.SignInAnonymouslyAsync();

            removedorDeAnuncios.LoadCloudData();

            AtualizarUserNameUI();

            // Aguarda a consulta do recorde terminar.
            await AtualizarRecordeUI();
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            if (erroLoginPopup != null)
            {
                erroLoginPopup.SetActive(true);
            }
        }
    }

    private void AtualizarUserNameUI()
    {
        string username = CloudServices.Instance.GetUserName();

        usernameText.text = username;

        int posicaoCerquilha = username.IndexOf("#");

        if (posicaoCerquilha >= 0)
        {
            usernameInputField.text =
                username.Substring(0, posicaoCerquilha);
        }
        else
        {
            usernameInputField.text = username;
        }
    }

    public async void SalvarNovoUsername()
    {
        try
        {
            await CloudServices.Instance.AtualizarUserName(
                usernameInputField.text
            );

            AtualizarUserNameUI();
        }
        catch (Exception e)
        {
            Debug.LogException(e);

            if (erroLoginPopup != null)
            {
                erroLoginPopup.SetActive(true);
            }
        }
    }

    public async Task AtualizarRecordeUI()
    {
        int recorde =
            await CloudServices.Instance.GetPontuacaoJogador();

        recordeText.text = "MEU RECORDE: " + recorde;
    }
}