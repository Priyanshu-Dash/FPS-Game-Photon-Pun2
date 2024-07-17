using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AutoSelectInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button buttonToPress;
    public bool onStart = false;
    private void Start()
    {
        if(onStart)
        {
            SelectInputField(inputField);
        }
    }
    private void Update()
    {
        PressButton(buttonToPress);
    }

    public void PressButton(Button button)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(inputField != null)
            {
                button.onClick.Invoke();
            }
            else
            {
                Debug.Log("Please fill input field");
            }
        }
    }

    public void SelectInputField(TMP_InputField inputField)
    {
        inputField.Select();
        inputField.ActivateInputField();
    }
}
