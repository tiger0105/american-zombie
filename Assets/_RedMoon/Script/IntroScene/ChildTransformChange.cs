using UnityEngine;
using System.Collections;

public class ChildTransformChange : BaseState {        
	public GameObject apply;
	public Transform Trans1;
	public Transform Trans2;
	public float Duration;
    float TimeSpan;
    void Start() {
        isLeafState = true;
		apply = apply;
    }
    public void Enter(Delegate _exitDelegate = null) {
		base.Enter(_exitDelegate);
        StartCoroutine(Play());
    }
    IEnumerator Play() {
        if (curEvent != Event.Enter) yield break;
        while (TimeSpan < Duration) {
            switch (curEvent)
            {
                case Event.Enter:
                    apply.SetActive(false);
                    yield return new WaitForEndOfFrame();
                    apply.transform.position = Trans1.position + (Trans2.position - Trans1.position) * TimeSpan / Duration;
                    apply.transform.localScale = Trans1.localScale + (Trans2.localScale - Trans1.localScale) * TimeSpan / Duration;
                    TimeSpan += Time.fixedDeltaTime;
                    yield return new WaitForFixedUpdate();
                    apply.SetActive(true);
                    curEvent = Event.Play;
                    break;
                case Event.Play:
                case Event.Resume:
                    apply.transform.position = Trans1.position + (Trans2.position - Trans1.position) * TimeSpan / Duration;
                    apply.transform.localScale = Trans1.localScale + (Trans2.localScale - Trans1.localScale) * TimeSpan / Duration;
                    yield return new WaitForFixedUpdate();
                    TimeSpan += Time.fixedDeltaTime;
                    break;
                case Event.Pause:
                    Debug.Log("Trans...Pause...");
                    yield return new WaitForFixedUpdate();
                    break;
            }                        
        }
        Exit();
    }
  
}
