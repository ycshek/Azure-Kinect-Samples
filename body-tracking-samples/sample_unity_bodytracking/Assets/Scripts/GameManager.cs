using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textQueueOnUI;
    public GameObject txtPrefab;
    public Button randomTxtBtn;
    public List<string> txtList;
    public List<string> textInQueue = new List<string>();
    public float nextTextSec = 1;
    public List<Color> colorList;
    public List<Vector3> spawnPoints;
    public List<AudioSource> createSounds;
    public List<AudioSource> touchSounds;
    public AudioSource destorySound;
    bool autoGen = false;
    int spawnOffset = -1;

    // Start is called before the first frame update
    void Start()
    {
        randomTxtBtn.onClick.AddListener(ToggleGenText);

        // keep checking any text in queue for display
        InvokeRepeating("GetTextInQueue", 1, nextTextSec);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleGenText();
        }
    }

    void GenRandomText()
    {
        // random text for demo
        string randomStr = txtList[Random.Range(0, txtList.Count)];
        AddTextInQueue(randomStr);
        UpdateTextQueueOnUI();
    }

    void GenTextBoxOnScreen(string nextText)
    {
        // gen text box from prefab with random position and angle
        int randomP = Random.Range(0, spawnPoints.Count);
        float randomA = Random.Range(-30, 30);
        Vector3 spawnPosition = Vector3.Scale(spawnPoints[randomP], new Vector3(spawnOffset, 1, 1));
        spawnOffset *= -1;
        GameObject newTxt = Instantiate(txtPrefab, spawnPosition, Quaternion.Euler(0, 0, randomA));
        TextBoxHandler txtBox = newTxt.GetComponent<TextBoxHandler>();

        // random color for init
        Color randomColor = colorList[Random.Range(0, colorList.Count)];
        txtBox.init(nextText, randomColor);

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
            randomTxtBtn.GetComponent<Image>().color = new Color(0, 0.7843f, 0.3921f);
            InvokeRepeating("GenRandomText", 0.1f, 0.1f);
        }
        else
        {
            randomTxtBtn.GetComponent<Image>().color = Color.white;
            CancelInvoke("GenRandomText");
        }
    }

    // if the queue is not empty, fade in the text in queue
    void GetTextInQueue()
    {
        if (textInQueue.Count == 0)
            return;

        string nextTxt = textInQueue[0];
        textInQueue.RemoveAt(0);
        GenTextBoxOnScreen(nextTxt);

        UpdateTextQueueOnUI();
    }

    // display next 10 texts in queue on UI
    void UpdateTextQueueOnUI()
    {
        int count = textInQueue.Count >= 10 ? 10 : textInQueue.Count;
        string displayTxt = "";

        if (count > 0)
        {
            for (var i = 0; i < count; i++)
            {
                displayTxt += textInQueue[i] + "\n";
            }
        }

        textQueueOnUI.text = displayTxt;
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

    public void AddTextInQueue(string text)
    {
        textInQueue.Add(text);
    }
}
