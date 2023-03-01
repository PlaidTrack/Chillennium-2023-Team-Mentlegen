using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    private PlayerController player;
    public Image black;

    public bool isFinal;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == player.tag)
        {
            if (isFinal)
                StartCoroutine(End());
            else
                StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        black.color = new Color32(0, 0, 0, 63);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 95);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 127);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 159);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 191);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 223);
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 255);

        SceneManager.LoadScene("Final Scene");
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(0.1f);
        black.color = new Color32(0, 0, 0, 255);
    }
}
