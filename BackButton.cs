using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

	private int count = 16;

	public int current_selection;
	public bool animating;

	// Use this for initialization
	void Start () {
		
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) && !animating) {
			back ();
		}
	}

	public void back() {
		GameObject obj = GameObject.Find (current_selection.ToString ());
		obj.GetComponent<Renderer> ().material = Resources.Load ("Mat" + obj.name) as Material;
		MovieSelection ms = obj.GetComponent<MovieSelection> ();
		ms.state = 0;
		IdleStuff iS = Camera.main.GetComponent<IdleStuff> ();
		iS.inStateZero = true;
		iS.inStateOne = false;
		iS.timeOfLastInteraction = Time.time;
		iS.timeOfLastShuffle = Time.time;
		iTween.MoveTo (obj, iTween.Hash ("x", ms.startPos.x, "y", ms.startPos.y, "oncomplete", "functionwhenfinished", "time", 1f));
		iTween.ScaleTo (obj, iTween.Hash ("x", ms.startScale.x, "z", ms.startScale.z, "time", 1f));
		iTween.MoveTo(GameObject.Find("Back"),iTween.Hash("x", -0.7, "y", 10, "z", 0, "time", 1f));
		iTween.ScaleTo (GameObject.Find (obj.name + "_text"), iTween.Hash ("x", 0, "y", 0, "time", 0.5f));
		for (int i = 1; i < count+1; i++) {
			if (i.ToString ().Equals (this.name) == false) {
				//Debug.Log (i.ToString () + "," + this.name);
				GameObject.Find (i.ToString ()).GetComponent<MovieSelection> ().progress ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
