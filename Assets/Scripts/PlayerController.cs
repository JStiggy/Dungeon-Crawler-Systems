using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int state = 0;
    float actionTime = 0;
    int collision = 0;
    bool stopPlayer = false;
    Transform npcLayoutGroup;
    Collider playerCollider;

    public float moveMod = 1.5f;
    public float bounceMod = 1.5f;
    public float rotateMod = 1.5f;

    Vector3 startPostion = Vector3.zero;
    Quaternion startRotation = Quaternion.identity;
    Quaternion endRotation = Quaternion.identity;

    void Start() {
        npcLayoutGroup = GameObject.Find("NPCLayoutGroup").transform;
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
        MovePlayer();
        HandleDialog();

        if (state == 99) {
            if (actionTime >= .15f) {
                state = 0;
                actionTime = 0;
            }
        }
    }

    void MovePlayer() {
        if (stopPlayer && state == 0) {
            state = 99;
            actionTime = 0;
            stopPlayer = false;
        } else if (Input.GetKey(KeyCode.W) && state == 0) {
            collision = Physics.Raycast(transform.position, transform.forward, 1.1f) ? 1 : 0;
            startPostion = transform.position;
            actionTime = 0;
            state = 1;
        } else if (Input.GetKey(KeyCode.Q) && state == 0) {
            state = 2;
            startRotation = transform.rotation;
            endRotation = Quaternion.LookRotation(transform.right * -1);
            actionTime = 0;
        } else if (Input.GetKey(KeyCode.E) && state == 0) {
            state = -2;
            startRotation = transform.rotation;
            endRotation = Quaternion.LookRotation(transform.right);
            actionTime = 0;
        } else if (Input.GetKey(KeyCode.S) && state == 0) {
            collision = Physics.Raycast(transform.position, -transform.forward, 1.1f) ? 1 : 0;
            startPostion = transform.position;
            actionTime = 0;
            state = -1;
        } else if (Input.GetKey(KeyCode.D) && state == 0) {
            collision = Physics.Raycast(transform.position, transform.right, 1.1f) ? 1 : 0;
            startPostion = transform.position;
            actionTime = 0;
            state = 3;
        } else if (Input.GetKey(KeyCode.A) && state == 0) {
            collision = Physics.Raycast(transform.position, -transform.right, 1.1f) ? 1 : 0;
            startPostion = transform.position;
            actionTime = 0;
            state = -3;
        }

        actionTime += Time.deltaTime;
        if (state == 1 || state == -1) {
            if (collision == 0) {
                transform.position = Vector3.Lerp(startPostion, startPostion + transform.forward * state, actionTime * moveMod);
                if (actionTime * moveMod >= 1) {
                    transform.position = startPostion + transform.forward * state;
                    state = 0;
                    actionTime = 0;
                }
            } else {
                if (collision == 1) transform.position = Vector3.Lerp(startPostion, startPostion + transform.forward * state * .6f, actionTime * bounceMod);
                if (collision == 2) transform.position = Vector3.Lerp(startPostion + transform.forward * state * .6f, startPostion, actionTime * bounceMod);
                if (actionTime * bounceMod >= .5f && collision == 1) {
                    transform.position = startPostion + transform.forward * state * .3f;
                    actionTime = .5f;
                    collision = 2;
                } else if (actionTime * bounceMod >= 1.0f) {
                    transform.position = startPostion;
                    actionTime = 1;
                    state = 0;
                    collision = 0;
                }
            }
        }

        if (state == 3 || state == -3) {
            if (collision == 0) {
                transform.position = Vector3.Lerp(startPostion, startPostion + transform.right * (state / 3.0f), actionTime * moveMod);
                if (actionTime * moveMod >= 1) {
                    transform.position = startPostion + transform.right * (state / 3.0f);
                    state = 0;
                    actionTime = 0;
                }
            } else {
                if (collision == 1) transform.position = Vector3.Lerp(startPostion, startPostion + transform.right * (state / 3.0f) * .6f, actionTime * bounceMod);
                if (collision == 2) transform.position = Vector3.Lerp(startPostion + transform.right * (state / 3.0f) * .6f, startPostion, actionTime * bounceMod);
                if (actionTime * bounceMod >= .5f && collision == 1) {
                    transform.position = startPostion + transform.right * (state / 3.0f) * .3f;
                    actionTime = .5f;
                    collision = 2;
                } else if (actionTime * bounceMod >= 1.0f) {
                    transform.position = startPostion;
                    actionTime = 1;
                    state = 0;
                    collision = 0;
                }
            }
        }

        if (state == 2 || state == -2) {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, actionTime * rotateMod);
            playerCollider.enabled = false;
            if (actionTime * rotateMod >= 1) {
                playerCollider.enabled = true;
                transform.rotation = endRotation;
                state = 0;
                actionTime = 0;
            }

            foreach (Transform npcChar in npcLayoutGroup) {
                npcChar.GetComponent<NpcDeleter>().DespawnNpc();
            }
        }
    }

    void HandleDialog() {
        if(Input.GetKeyDown(KeyCode.I) && state == 98) {
            DialogManager.instance.AdvanceDialog();
        } else if (Input.GetKeyDown(KeyCode.I) && DialogManager.instance.dialogKey != "" && state == 0) {
            state = 98;
            DialogManager.instance.StartDialog();
        }
    }

    public void StopPlayer() {
        stopPlayer = true;
    }
}
