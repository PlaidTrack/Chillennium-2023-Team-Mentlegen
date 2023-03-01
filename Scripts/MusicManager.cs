using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource nonPossessed;
    public AudioSource possessed;
    private PlayerController player;

    // Start is called before the first frame update

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        nonPossessed.Play();
        possessed.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentForm == PlayerController.form.parasite && !nonPossessed.isPlaying)
        {
            possessed.Pause();
            nonPossessed.Play();
        }

        if (player.currentForm == PlayerController.form.gunner && !possessed.isPlaying)
        {
            nonPossessed.Pause();
            possessed.Play();
        }
    }
}
