using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Leaderboard  {

	public static int getCurrentHighScore(){
		
		string leaderboardScores = PlayerPrefs.GetString("leaderboard");
		string[] leaderData = leaderboardScores.Split (',');
		
		return Convert.ToInt32(leaderData[0].Split(':')[2]);
	}
	
  public static ArrayList getLeaderBoardScores(){
		
		ArrayList scores = new ArrayList();
		
		string leaderboardScores = PlayerPrefs.GetString("leaderboard");
		string[] leaderData = leaderboardScores.Split (',');
		
		foreach(string data in leaderData){
			string[] currentData = data.Split(':');
			scores.Add(Convert.ToInt32(currentData[currentData.Length-1]));
		}
		
		return scores;
	}
	
  public static ArrayList getLeaderBoardNames(){
		
		ArrayList names = new ArrayList();
		
		string leaderboardScores = PlayerPrefs.GetString("leaderboard");
		string[] leaderData = leaderboardScores.Split (',');
		
		foreach(string data in leaderData){
			string[] currentData = data.Split(':');
			names.Add(currentData[currentData.Length-2]);
		}
		
		return names;
		
	}

  public static int checkScoreAgainstLeaderboard(int score){
		
		int isFound = -1;
		var i = 0;
		foreach(int leaderScore in GameState.leaderboardScores){
			
			if(score > leaderScore)
				isFound = i;
		}

		return isFound;
	}
	
   public static void insertLeaderboardScore(int index,string name,int score){
		
		for (int i = 0; i < GameState.leaderboardScores.Count; i++) {
			if(i == index){
				GameState.leaderboardScores[index] = score;
				GameState.leaderboardNames[index] = name;
			}
		}

		UpdateHighScores ();
	}

  private static void UpdateHighScores(){

		string leaderboardText = "";
		for (int i=0; i < GameState.leaderboardScores.Count; i++) {
		    
			leaderboardText+=(i+1)+":"+GameState.leaderboardNames+":"+GameState.leaderboardScores+",";
		
		}

		PlayerPrefs.SetString ("leaderboard",leaderboardText);

	}

}
