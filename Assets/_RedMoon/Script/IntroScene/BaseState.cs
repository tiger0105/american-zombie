using UnityEngine;
using System.Collections;

public  class BaseState : MonoBehaviour {
   protected enum Event
    {
        None,
        Enter,
        Play,
        Stop,
        Pause,
        Resume,
        Exit
    }
    protected Event curEvent;
    public delegate void Delegate();
    protected Delegate exitDelegate = null;
    protected Delegate pauseDelegate = null;
    protected Delegate resumeDelegate = null;
	protected Delegate hideDelegate = null;
	protected Delegate showDelegate = null;
    public bool isLeafState;
    public BaseState[] childStateList;

    void Start() {
        //StartCoroutine(Observe());
    }
//    public virtual void Register() {
//        if (isLeafState) return;
//        pauseDelegate = null;
//        resumeDelegate = null;
//        for (int i =0; i<childStateList.Length; i++) { 
//            BaseState child = childStateList[i];
//            pauseDelegate += child.Pause;
//            resumeDelegate += child.Resume;
//        }
//    }
	public virtual void Register(BaseState child) {
		showDelegate += child.Show;
		hideDelegate += child.Hide;
	}
	public virtual void Hide(){
		if (hideDelegate != null)
			hideDelegate ();
		gameObject.SetActive (false);
	}
	public virtual void Show(){
		if (showDelegate != null)
			showDelegate ();
		gameObject.SetActive (true);
	}
    public virtual void Enter(Delegate _exitDelegate = null) {
        curEvent = Event.Enter;
        exitDelegate = _exitDelegate;
        Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is entered.");
    }
    public virtual void Stop()
    {
        if (curEvent == Event.Play || curEvent == Event.Resume)
        {
            curEvent = Event.Stop;
        }        
    }
    public virtual void Pause()
    {       
        switch (curEvent)
        {
            case Event.Enter:
                StartCoroutine(WaitAnEventForPause(Event.Play));
                break;
            case Event.Play:
            case Event.Resume:
                curEvent = Event.Pause;
                Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is paused.");
                if (!isLeafState && pauseDelegate != null) pauseDelegate();
                break;
            default:
                Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " cannot be paused since it's state is " + curEvent.ToString());
                break;
        }
    }
    public virtual void Resume()
    {
        if (curEvent != Event.Pause){
            Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " cannot be resumed since it's state is " + curEvent.ToString());
            return;
        }
        else {
            curEvent = Event.Resume;
            Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is resumed.");
            if (!isLeafState && resumeDelegate != null) resumeDelegate();
        } 
    }
    public virtual void Exit()
    {
        switch (curEvent) {
            case Event.Pause:
                StartCoroutine(WaitAnEventForExit(Event.Resume));
                break;
            case Event.Play:
            case Event.Resume:
            default:
                curEvent = Event.Exit;
                Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is Exited!");
                if (exitDelegate != null) exitDelegate();
                break;
        }
         
    }
    public virtual bool IsPlaying() {
        if (curEvent == Event.Play || curEvent == Event.Resume)
        {
            return true;
        }
        else {
            return false;
        }
    }
   IEnumerator Observe() {
        while (true) {
            Debug.Log(gameObject.name + "'s " + this.GetType().ToString() +  " " + curEvent.ToString());
            if (curEvent == Event.Exit)
            {
                yield break;
            }
            else {
                yield return new WaitForFixedUpdate();
            }
        }
    }
    IEnumerator WaitAnEventForExit(Event e)
    {
        while (true)
        {
            if (curEvent == e)
            {
                curEvent = Event.Exit;
                Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is Exited!");
                if (exitDelegate != null) exitDelegate();
                yield break;
            }
            else {
                yield return new WaitForFixedUpdate();
            }
        }
    }
    IEnumerator WaitAnEventForPause(Event e)
    {
        while (true)
        {
            if (curEvent == e)
            {
                curEvent = Event.Pause;
                Debug.Log(gameObject.name + "'s " + this.GetType().ToString() + " is paused.");
                if (!isLeafState && pauseDelegate != null) pauseDelegate();
                yield break;
            }
            else {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
