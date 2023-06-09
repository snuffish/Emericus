using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Animator _animatorLeft;
    [SerializeField] private Animator _animatorRight;

    public IEnumerator LoadLevel(int levelIndex, float loadTime) {
        
        //  Start the transition
        _animatorLeft.SetTrigger("Start");
        _animatorRight.SetTrigger("Start");
        
        //  Wait a little
        yield return new WaitForSeconds(loadTime);
        
        //  Save Speedruntime
        GetComponent<SpeedrunTimerController>().SaveTime();

        //  Change Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
        
    }

    public void RestartGameAfterCredits() {
        StartCoroutine(LoadLevel(0, 2.5f));
    }
    
}