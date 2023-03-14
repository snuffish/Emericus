using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : Health
{
    
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private float transistionTime;
    protected override void Die() {
        Debug.Log("Dead");
        StartCoroutine(_sceneManager.LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().handle, transistionTime));
        // GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayGameOver();
    }
}
