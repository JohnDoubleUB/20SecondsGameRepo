using UnityEngine;

public class PlayerKillArea : MonoBehaviour
{

    [SerializeField]
    private AudioSource AudioTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (AudioTrigger != null) 
        {
            AudioTrigger.Play();
        }

        GameManager.current.EndGamePrematurely();
    }
}
