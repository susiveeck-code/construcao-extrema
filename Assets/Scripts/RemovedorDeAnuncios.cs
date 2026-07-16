using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.Events;

public class RemovedorDeAnuncios : MonoBehaviour
{
    private bool removerAnuncios;
    [SerializeField] private UnityEvent OnRemoverAnuncios;

    public bool GetRemoverAnuncios()
    {
        return removerAnuncios;
    }

    // Salva informacao do jogador diretamente na nuvem
    public async void SaveCloudData()
    {
        var data = new Dictionary<string, object> { { "no_ads", true } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        removerAnuncios = true;
    }

    public async void LoadCloudData()
    {
        try
        {
            var dadosSalvos = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "no_ads" });
            removerAnuncios = dadosSalvos["no_ads"].Value.GetAs<bool>();

            if (removerAnuncios)
            {
                OnRemoverAnuncios.Invoke();
            }
        }
        catch
        {

        }

    }
}