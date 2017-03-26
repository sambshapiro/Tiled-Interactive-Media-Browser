using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RenderHeads.Media.AVProVideo;

public class IdleStuff : MonoBehaviour {

	private int count = 16;

	public GameObject[] videos;
	public Vector3[] slots;
	public float timeOfLastInteraction;
	public float timeOfLastShuffle;
	public bool inStateZero;
	public bool inStateOne;
	public bool inScreenSaver;
	public float shuffleTime = 20f;
	public float screenSaverTime = 120f;
	public float stateOneTimeout;
	private GameObject mpss;
	private GameObject bgss;
	private GameObject bgsel;
	private bool firstScreensave;

	// Use this for initialization
	void Start () {
		timeOfLastInteraction = Time.time;
		timeOfLastShuffle = Time.time;
		inStateZero = true;
		inStateOne = false;
		inScreenSaver = false;
		StartCoroutine (wait ());
		mpss = GameObject.Find ("MediaPlayer-Screensaver");
		bgss = GameObject.Find ("Background-Screensaver");
		bgsel = GameObject.Find ("Background-Selection");
		firstScreensave = true;
	}

	IEnumerator wait() {
		yield return new WaitForSeconds(0.1f);
		videos = new GameObject[count];
		slots = new Vector3[count];
		for (int i = 0; i < count; i++) {
			videos [i] = GameObject.Find ((i+1).ToString ());
			slots [i] = videos [i].GetComponent<MovieSelection> ().startPos;
		}
	}

	public void randomize() {
		int j = 100;
		for (int i = count-1; i > 0; i--) {
			j = Random.Range (0, i);
			Vector3 temp = slots [i];
			slots [i] = slots [j];
			slots [j] = temp;
		}
		for (int i = 0; i < count; i++) {
			videos [i].GetComponent<MovieSelection> ().startPos = slots [i];
			iTween.MoveTo(videos[i], iTween.Hash("x", slots[i].x, "y", slots[i].y, "z", slots[i].z, "time", 2f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (inStateZero && !inScreenSaver && Time.time - timeOfLastInteraction > shuffleTime && Time.time - timeOfLastShuffle > shuffleTime) {
			randomize ();
			timeOfLastShuffle = Time.time;
		}
		if (inStateZero && !inScreenSaver && Time.time - timeOfLastInteraction > screenSaverTime) {
			//start screensaver
			inScreenSaver = true;
			for (int i = 0; i < count; i++) {
				videos [i].SetActive (false);
			}
			bgss.SetActive (true);
			bgsel.SetActive (false);
			if (firstScreensave) {
				mpss.GetComponent<MediaPlayer> ().OpenVideoFromFile (
					MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "Screensaver.mp4", true);
				firstScreensave = false;
			}
			mpss.GetComponent<MediaPlayer> ().Control.Play ();
		}
		if (inStateOne && Time.time - timeOfLastInteraction > stateOneTimeout) {
			GameObject.Find ("Back").GetComponent<BackButton> ().back ();
		}
		if (inScreenSaver && Input.GetMouseButtonDown (0)) {
			mpss.GetComponent<MediaPlayer> ().Control.Pause ();
			bgss.SetActive (false);
			for (int i = 0; i < count; i++) {
				videos [i].SetActive (true);
			}
			bgsel.SetActive (true);
			inScreenSaver = false;
			timeOfLastInteraction = Time.time;
			timeOfLastShuffle = Time.time;
		}
	}
}
