using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    PlayerController pController;
    GameObject npcLayoutGroup;
    public string dialogKey = "";
    public GameObject[] npcData;

    public void OnTriggerEnter(Collider collision) {
        if (pController == null) pController = GameObject.FindObjectOfType<PlayerController>();
        pController.StopPlayer();

        if (npcLayoutGroup == null) npcLayoutGroup = GameObject.Find("NPCLayoutGroup");
        
        foreach(GameObject npcChar in npcData) {
            Instantiate(npcChar, npcLayoutGroup.transform);
        }
        DialogManager.instance.dialogKey = dialogKey;
    }

    public void OnTriggerExit(Collider collision) {
        foreach (Transform npcChar in npcLayoutGroup.transform) {
            npcChar.GetComponent<NpcDeleter>().DespawnNpc();
        }
        DialogManager.instance.dialogKey = "";
    }
}
