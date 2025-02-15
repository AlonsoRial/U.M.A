using UnityEngine;
using UnityEngine.InputSystem;

public class ResetearTodosBinding : MonoBehaviour
{

    [SerializeField] private InputActionAsset inputActions;

    public void ResetearBinding() 
    {
        foreach (InputActionMap map in inputActions.actionMaps) 
        {
            map.RemoveAllBindingOverrides();
        }

        PlayerPrefs.DeleteKey("rebinds");

    }

}
