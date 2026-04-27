using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScript : MonoBehaviour
{
    public SoundData sound;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEventsManager.Instance.audioEvents.PlaySound(sound, this.transform.position);
            VictoryMenu.Instance.VictoryStart();
        }
    }
}
