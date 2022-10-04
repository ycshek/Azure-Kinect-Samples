using UnityEngine;

public class main : MonoBehaviour
{
    // Handler for SkeletalTracking thread.
    public GameObject m_tracker;
    private SkeletalTrackingProvider m_skeletalTrackingProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();
    public GameObject playerBody;
    public bool playerActive;

    void Start()
    {
        //tracker ids needed for when there are two trackers
        const int TRACKER_ID = 0;
        m_skeletalTrackingProvider = new SkeletalTrackingProvider(TRACKER_ID);
    }

    void Update()
    {
        if (m_skeletalTrackingProvider.IsRunning)
        {
            if (m_skeletalTrackingProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                if (m_lastFrameData.NumOfBodies != 0)
                {
                    //Debug.Log(JsonUtility.ToJson(m_lastFrameData));
                    m_tracker.GetComponent<TrackerHandler>().updateTracker(m_lastFrameData);
                    if (playerBody != null)
                    {
                        playerBody.SetActive(true);                        
                    }
                    playerActive = true;
                }
                else
                {
                    if (playerBody != null)
                    {
                        playerBody.SetActive(false);                        
                    }
                    playerActive = false;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (m_skeletalTrackingProvider != null)
        {
            m_skeletalTrackingProvider.Dispose();
        }
    }
}
