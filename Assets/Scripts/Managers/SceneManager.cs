using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public IEnumerator LoadLevel(int levelIndex, float loadTime) {
        
        //  Start the transition
        _animator.SetTrigger("Start");
        
        //  Wait a little
        yield return new WaitForSeconds(loadTime);
        
        //  Change Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        
    }
    
}
