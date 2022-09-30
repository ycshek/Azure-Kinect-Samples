using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject txtPrefab;
    public Button randomTxtBtn;
    public List<string> txtList;
    public List<Color> colorList;
    public List<Vector3> spawnPoints;
    public List<AudioSource> createSounds;
    public List<AudioSource> touchSounds;
    public AudioSource destorySound;
    bool autoGen = false;

    // Start is called before the first frame update
    void Start()
    {
        randomTxtBtn.onClick.AddListener(ToggleGenText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenRandomText()
    {
        // gen text box from prefab with random position and angle
        int randomP = Random.Range(0, spawnPoints.Count);
        float randomA = Random.Range(-30, 30);
        GameObject newTxt = Instantiate(txtPrefab, spawnPoints[randomP], Quaternion.Euler(0, 0, randomA));
        TextBoxHandler txtBox = newTxt.GetComponent<TextBoxHandler>();

        // random text and color for init
        string randomStr = txtList[Random.Range(0, txtList.Count)];
        Color randomColor = colorList[Random.Range(0, colorList.Count)];
        txtBox.init(randomStr, randomColor);

        // play create sound
        PlayCreateSound();

        // add event listener to sound
        txtBox.TouchHandler += PlayTouchSound;
        txtBox.DestroyHandler += PlayDestroySound;
    }

    void ToggleGenText()
    {
        autoGen = !autoGen;

        if (autoGen)
        {
            InvokeRepeating("GenRandomText", 0.5f, 0.5f);
        }
        else
        {
            CancelInvoke();
        }
    }

    void PlayCreateSound()
    {
        int randomP = Random.Range(0, createSounds.Count);
        createSounds[randomP].Play();
    }

    void PlayTouchSound()
    {
        int randomP = Random.Range(0, touchSounds.Count);
        touchSounds[randomP].Play();
    }

    void PlayDestroySound()
    {
        destorySound.Play();
    }
}
