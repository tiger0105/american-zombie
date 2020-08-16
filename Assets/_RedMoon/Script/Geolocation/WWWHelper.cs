using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;


public class WWWHelper : MonoBehaviour {

	public delegate void HttpRequestDelegate(int id, WWW www);

	public event HttpRequestDelegate OnHttpRequest;

	private int requestId;

	static WWWHelper current = null;

	static GameObject container = null;

	public static WWWHelper Instance {
		get {
			if (current == null) {
				container = new GameObject();
				container.name = "WWWHelper";
				current = container.AddComponent(typeof(WWWHelper)) as WWWHelper;
			}
			return current;
		}
	}

	public void get(int id, string url) {
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(id, www));
	}

	public void post(int id, string url, IDictionary<string, string> data) {
		WWWForm form = new WWWForm();

		foreach (KeyValuePair<string, string> post_arg in data) {
			form.AddField(post_arg.Key, post_arg.Value);
		}

		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(id, www));
	}

	private IEnumerator  WaitForRequest(int id, WWW www) {
		yield return www;

		bool hasCompleteListener = (OnHttpRequest != null);

		if (hasCompleteListener) {
			OnHttpRequest(id, www);
		}

		www.Dispose();
	}

}
