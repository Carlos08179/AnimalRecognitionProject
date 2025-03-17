using UnityEngine;
using UnityEngine.Video;
using Vuforia;

public class CiervoVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el Video Player desde el Inspector

    private ObserverBehaviour observerBehaviour; // Referencia al ObserverBehaviour

    void Start()
    {
        // Obtén el componente ObserverBehaviour del Image Target
        observerBehaviour = GetComponent<ObserverBehaviour>();

        // Detén el video al inicio
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

    void Update()
    {
        // Verifica si el Image Target está siendo detectado
        if (observerBehaviour != null)
        {
            if (observerBehaviour.TargetStatus.Status == Status.TRACKED ||
                observerBehaviour.TargetStatus.Status == Status.EXTENDED_TRACKED)
            {
                // Si el video no se está reproduciendo, inícialo
                if (videoPlayer != null && !videoPlayer.isPlaying)
                {
                    videoPlayer.Play();
                }
            }
            else
            {
                // Si el Image Target no está siendo detectado, detén el video
                if (videoPlayer != null && videoPlayer.isPlaying)
                {
                    videoPlayer.Stop();
                }
            }
        }
    }
}