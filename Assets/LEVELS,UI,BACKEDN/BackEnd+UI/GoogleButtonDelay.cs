using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonWithDelay : MonoBehaviour
{
    public Button button;
    public float delay=2;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick); 
    }

    public void OnButtonClick()
    {

        GoogleAuth.instance.SignInWithGoogle();

        Debug.Log("First Event Triggered!");

        StartCoroutine(DelayedEvent());
    }

    IEnumerator DelayedEvent()
    {
        yield return new WaitForSeconds(delay);

        Landing_UIManager.instance.Menupage();

        Debug.Log("Second Event Triggered after " + delay + " seconds!");
    }
}
