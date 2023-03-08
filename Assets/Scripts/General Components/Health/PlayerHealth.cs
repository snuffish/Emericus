using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : Health
{
    protected override void Die() {
        Debug.Log("Dead");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayGameOver();
    }
}
