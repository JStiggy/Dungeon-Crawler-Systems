using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance = null;
    public string dialogKey = "";
    string currentKey;
    public JSONNode dialog;

    public GameObject dialogPanel;
    public Text dialogDisplay;
    public Button[] choices;
    int choice = 0;

    // Start is called before the first frame update
    void Start() {
        if (instance == null)
            instance = this;
        dialog = JSON.Parse(Resources.Load<TextAsset>("Json/NpcText").text);
    }

    public void StartDialog() {
        //TODO: Animation for opening
        //TODO: Nice scroll effect on text when advancing
        dialogPanel.SetActive(true);
        dialogDisplay.text = dialog[dialogKey]["text"];
        currentKey = dialogKey;
        CheckChoices();
    }

    public void AdvanceDialog() {
        if(dialog[currentKey]["next"] == "") {
            EndDialog();
            return;
        }
        if(dialog[currentKey]["type"] == "Basic") {
            currentKey = dialog[currentKey]["next"];
            
        } else {
            currentKey = dialog[currentKey]["a_" + choice.ToString()];
        }
        dialogDisplay.text = dialog[currentKey]["text"];
        CheckChoices();
    }

    public void EndDialog() {
        GameObject.FindObjectOfType<PlayerController>().state = 0;
        dialogDisplay.text = "";
        foreach (Button button in choices) {
            button.GetComponentInChildren<Text>().text = "";
        }
        dialogPanel.GetComponent<Animator>().SetTrigger("Close");
    }

    void CheckChoices() {
        GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        int i = 1;
        int count = dialog[currentKey]["count"].AsInt;
        foreach (Button button in choices) {
            button.gameObject.SetActive(false);
        }
        bool active = false;
        foreach (Button button in choices) {
            active = ("Choice" == dialog[currentKey]["type"]);
            if (!active || i > count) break;
            print(i + " " + count + " " + dialog[currentKey]["type"]);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<Text>().text = dialog[currentKey]["r_" + i.ToString()];
            i += 1;
        }
        if (active) {
            print("Active");
            GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject(choices[0].gameObject);
        }
    }

    public void MakeChoice(int decision) {
        choice = decision;
    }
}
