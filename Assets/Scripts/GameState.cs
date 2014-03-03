using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class GameState {

    
	public static int gameState = 1;
	public static int gameOn = 1;
	public static int gameOver = 0;
	public static string playerName = "Khule";
	public static float playerLife = 600.0f;
	public static int numberOfZombies = 3;
	public static int numberOfKilledZombies = 0;
	public static int[] levels = {0,1,8};
	public static int currentLevelIndex = 0;
	public static int currentHighScore;
	public static int currentScore = 600;

	public static ArrayList leaderboardScores;
	public static ArrayList leaderboardNames;

	public static bool backgroundMusic = false;
	public static float backgroundVolume;
	
	public static bool gameSound = false;
	public static float soundVolume;

	public static bool forceFeedback = false;

	public static int countDown;
	public static bool isConnectedGame = false;
	public static string count;

	public static bool multiplayerGame = true;

    public static bool sceneStarting = true;


	public static int getGameState(){
		return gameState;
	}

	public static void setGameState(int state){
		gameState = state;
	}

	public static void resetGameState(){

		gameState = 1;
		playerLife = 400.0f;
		numberOfZombies = 3;
		numberOfKilledZombies = 0;
	}


}
