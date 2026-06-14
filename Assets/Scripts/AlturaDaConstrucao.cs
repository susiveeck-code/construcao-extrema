using UnityEngine;
using UnityEngine.UIElements;

public class AlturaDaConstrucao : MonoBehaviour
{
    public float AlturaAtual()
    {
        bool alturaEncontrada = false;

        while (!alturaEncontrada)
        {
            if (Physics.CheckBox(transform.position, new Vector3(15, 1, 15)))
            {
                transform.position += Vector3.up;
            }
            else
            {
                alturaEncontrada = true;
            }
        }
        return transform.position.y;
    }
}
