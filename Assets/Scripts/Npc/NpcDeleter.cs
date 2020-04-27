using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDeleter : MonoBehaviour
{
    public Animator anim;

    public void DespawnNpc() {
        anim.SetTrigger("Despawn");
    }

    // Start is called before the first frame update
    public void DeleteNpc() {
        Destroy(gameObject);
    }
}
