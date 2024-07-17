using UnityEngine;

public class ToggleMe : MonoBehaviour
{
    public void ToggleObject()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
