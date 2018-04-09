using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drop : MonoBehaviour {
	

	public GameObject[] drops;
	public GUIText introText; //Displays Instructions
	public GUIText finalScoreText;// Displays Final Score
	public GUIText dropCount; //Displays Drop Count on HUD
	public GUIText timerGUI;// Displays Timer HUD
	public GUIText score;// Displays Score HUD

	private Vector3 mousePosition;

	private GameObject BlueDrop;
	private GameObject RedDrop;
	private GameObject GreenDrop;
	private GameObject YellowDrop;

	private RaycastHit2D disableDropRay;
	private RaycastHit2D destroyGameDrop;

	private Color c;
	private int count;
	private int reverseCount;
	private int instantiationCount;
	private int menuCount;

	private float timer;
	private float average;

	private bool startGame;
	private bool altbool;


	// Use this for initialization

	void Awake(){
	
		/*Instructions to Play*/
		introText.text = "Hello \n\n Welcome To Drops \n\n Disable as much as drops by tapping them \n\n" +
			"Once done Tap on the Screen to Quit \n\n Tap to Continue";
	
	}

	void Start () {

		startGame = false; //Game is yet to start for the introduction
		altbool = false;

		//timerGUI.gameObject.transform.position = Camera.main.transform.TransformDirection (Camera.main.transform.position);

		BlueDrop = drops[0];
		RedDrop = drops[1];
		GreenDrop = drops[2];
		YellowDrop = drops[3];

		dropCount.enabled = false;
		timerGUI.enabled = false;
		score.enabled = false;
		finalScoreText.enabled = false;


		//reverseCount = -1; //Keep this negative to avoid generating drops on start

 
	}

		// Update is called once per frame


	void Update () {

		if (menuCount == 2) {
			Time.timeScale = 1;
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

		/*Display Instructions abnd Guidelines*/
	/*	if (instantiationCount == 1) {
			introText.text = "These are called Drops \n\n Tap on them to disable ";
		}
		if(instantiationCount == 2){
			introText.text = "More Drops \n\n Tap on them to disable ";
		}
		if(instantiationCount == 3){
			introText.text = "You can quit anytime \n\n By tapping on the screen ";
		}

		if (reverseCount <= 4 && startGame == true) {
			introText.text = "";
		}*/

		/* Displays the Final score*/
		average = count / timer;


		/*Assign the time vaklue to the Time in System*/
		timer = Time.time;

		/*Display the time*/
		timerGUI.text = "Timer: " +Mathf.Round(timer);

		/*Detect the position of the mouse (Used for Raycasting) */
		mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		/*Calculate Final Score(Average)*/
		//average = count / timer;

		/*Displays the dropCount*/
		dropCount.text = "Count: " +count; 

		/*Diojsplays the Score*/
		score.text = "Score: " + average.ToString("F2");

		if (reverseCount == 0 && startGame == true) {
			Invoke ("InstantiateDrops",0f);
		}




	//Raycast to make the drop colour black
		disableDropRay = Physics2D.Raycast (new Vector2(mousePosition.x,mousePosition.y),Vector2.zero,Mathf.Infinity);

		//Another Raycast that could be used later
		destroyGameDrop = Physics2D.Raycast (drops [1].gameObject.transform.position, drops [1].gameObject.transform.position, Mathf.Infinity);



		if (Input.GetMouseButtonUp (0) && startGame == true  ) {
		menuCount++;
		altbool = true;	
			Invoke ("CalculateFinalScore", 0f);
			finalScoreText.enabled = true;
			Time.timeScale = 0;
		}

		if (Input.GetMouseButtonUp (0) && startGame == false){  
			Invoke ("InstantiateDrops", 0f);
			startGame = true;
				finalScoreText.enabled = false;
				dropCount.enabled = true;
				timerGUI.enabled = true;
			score.enabled = true;
				

		}

	//	Debug.Log ("StartGame:" +startGame);
	//	Debug.Log ("AltBool:" +altbool);


	
		if (startGame == true && altbool == false) {
				if (disableDropRay.collider != null) {
				Invoke ("DetectDrop", 0f);
				}
		}
	

		//Restart - Use only for testing purposes
		if(Input.GetKey(KeyCode.S)){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}


	
	}




	/*Method used to Instantiate Drops*/
	void InstantiateDrop(){
		if(startGame = true){
		introText.text = "";
			Instantiate (drops [UnityEngine.Random.Range (0, 4)], Vector2.zero, Quaternion.identity);
		reverseCount++;
		}

	
	}

	void InstantiateDrops(){
		instantiationCount++;
		Invoke ("InstantiateDrop",0f);
		Invoke ("InstantiateDrop",0.1f);
		Invoke ("InstantiateDrop",0.12f);
		Invoke ("InstantiateDrop",0.2f);
		Invoke ("InstantiateDrop",0.3f);
		if(altbool == true && startGame == false){
			CancelInvoke ();
		}
	
	}




	void CalculateFinalScore(){

		finalScoreText.text = "Number of Drops: " + count.ToString () +
			"\n\nTime Taken: " + Math.Round (timer, 2) +
			"\n\nScore: " + Math.Round (average, 2)+
			"\n\nTap on The Screen to go the Main Menu" ;
		CancelInvoke ();

		if (startGame == false) {
			CancelInvoke ();
		}
	
	}

	void DetectDrop(){
	
		if (disableDropRay.collider.tag == BlueDrop.gameObject.tag || disableDropRay.collider.tag == RedDrop.gameObject.tag
		   || disableDropRay.collider.tag == GreenDrop.gameObject.tag || disableDropRay.collider.tag == YellowDrop.gameObject.tag) {
			c = disableDropRay.collider.GetComponent<SpriteRenderer> ().color;
			disableDropRay.collider.GetComponent<SpriteRenderer> ().color = Color.black;
			reverseCount--;
			count++;
			disableDropRay.collider.tag = "BlackDrop";
		}
	}




}