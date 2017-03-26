using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;

public class MovieSelection : MonoBehaviour {

	private int count = 16;

	public int state;
	public Vector3 startPos;
	public Vector3 startScale;
	public bool animating;
	private GameObject back;
	private GameObject caption;

	// Use this for initialization
	void Start () {
		//Renderer r = GetComponent<Renderer>();
		//MovieTexture movie = (MovieTexture)r.material.mainTexture;
		//movie.Play ();
		state = 0;		
		startPos = this.transform.position;
		startScale = this.transform.localScale;
		animating = false;
		back = GameObject.Find ("Back");
		caption = GameObject.Find (gameObject.name + "_text");
	}

	void changeAnimationStatus(bool anim) {
		for (int i = 1; i < count+1; i++) {
			GameObject.Find (i.ToString ()).GetComponent<MovieSelection> ().animating = anim;
		}
		back.GetComponent<BackButton> ().animating = anim;
		Debug.Log (Time.time + "-- animation status changed to " + animating);
	}

	void changeAnimationStatusFalse() {
		changeAnimationStatus (false);
	}

	void playVideo() {
		//changeAnimationStatus (false);
		//play video
		MediaPlayer mp = GameObject.Find ("MediaPlayer-Selection").GetComponent<MediaPlayer> ();
		mp.Events.AddListener (OnVideoEvent);
		mp.OpenVideoFromFile(
			MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, gameObject.name + ".mp4", true);
	}

	void removeListeners() {
		GameObject.Find ("MediaPlayer-Selection").GetComponent<MediaPlayer> ().Events.RemoveAllListeners ();
	}

	void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode) {
		switch (et) {
		case MediaPlayerEvent.EventType.ReadyToPlay:
			gameObject.SetActive (false);
			back.SetActive (false); //NEW LINE NEW LINE NEW LINE NEW LINE for video to play without zoom animation
			caption.SetActive(false);
			mp.Control.Play ();
			break;
		case MediaPlayerEvent.EventType.FinishedPlaying:
			gameObject.SetActive (true);
			back.SetActive (true); //NEW LINE NEW LINE NEW LINE NEW LINE for video to play without zoom animation
			caption.SetActive (true);
			mp.CloseVideo ();
			//changeAnimationStatus (true);
			state = 1;
			Camera.main.GetComponent<IdleStuff> ().inStateOne = true;
			Camera.main.GetComponent<IdleStuff> ().timeOfLastInteraction = Time.time;
			back.GetComponent<BackButton> ().current_selection = int.Parse (this.name);
			//iTween.MoveTo (gameObject, iTween.Hash ("x", 21.3, "y", -5.24, "time", 1f, "oncomplete", "changeAnimationStatusFalse"));
			//iTween.ScaleTo (gameObject, iTween.Hash ("x", 3.2, "z", 1.8, "time", 1f));
			//iTween.MoveTo (back, iTween.Hash ("x", -0.7, "y", 2.35, "z", 0, "time", 1f));
			removeListeners ();
			break;
		}
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) && !animating) {
			if (state == 0) {
				changeAnimationStatus(true);
				state = 1;
				Camera.main.GetComponent<IdleStuff> ().inStateZero = false;
				Camera.main.GetComponent<IdleStuff> ().inStateOne = true;
				Camera.main.GetComponent<IdleStuff> ().timeOfLastInteraction = Time.time;
				back.GetComponent<BackButton> ().current_selection = int.Parse(this.name);
				gameObject.GetComponent<Renderer> ().material = Resources.Load ("Mat" + gameObject.name + "_Play") as Material;
				iTween.MoveTo (gameObject, iTween.Hash ("x", 21.3, "y", -5.24, "time", 1f, "oncomplete", "changeAnimationStatusFalse"));
				iTween.ScaleTo (gameObject, iTween.Hash ("x", 3.2, "z", 1.8, "time", 1f));
				iTween.MoveTo (back, iTween.Hash ("x", -0.7, "y", 2.35, "z", 0, "time", 1f));
				iTween.ScaleTo (caption, iTween.Hash ("x", 2.771219, "y", 2.771219, "time", 1f));
				for (int i = 1; i < count+1; i++) {
					if (i.ToString ().Equals (this.name) == false) {
						//Debug.Log (i.ToString () + "," + this.name);
						GameObject.Find (i.ToString ()).GetComponent<MovieSelection> ().regress ();
					}
				}
			} 
			else if (state == 1) {
				//changeAnimationStatus(true);
				state = 2;
				Camera.main.GetComponent<IdleStuff> ().inStateOne = false;
				//iTween.MoveTo (gameObject, iTween.Hash ("x", 21.4, "y", -10, "time", 1f, "oncomplete", "playVideo"));
				//iTween.ScaleTo (gameObject, iTween.Hash ("x", 5.343862, "y", 5.343862, "z", 3.005919, "time", 1f));
				//iTween.MoveTo (GameObject.Find ("Back"), iTween.Hash ("x", -0.7, "y", 10, "z", 0, "time", 1f));
				playVideo();
			}
			else if (state == 2) {
				//do nothing because we are watching video
			}
		}
	}

	/*IEnumerator wait() {
		yield return new WaitForSeconds(1);
	}*/

	public void regress() {
		iTween.MoveTo(gameObject, iTween.Hash("y", -32, "time", 1f));
	}

	public void progress() {
		iTween.MoveTo(gameObject, iTween.Hash("x", startPos.x, "y", startPos.y, "z", startPos.z, "time", 1f));
	}

	// Update is called once per frame
	void Update () {
	}
}
