using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public static string CurrentLevelKey = "current_level";
	public static string TotalGoldKey = "totalgold";
	public static string TotalXPKey = "totalxp";
	public static string CurrentXPLevel = "xp_level";
	public static string TotalScoreKey = "totalscore";

	public static string BombPowerUpsKey = "bomb_powerups";
	public static string DoubleBlastRadiusPowerUpsKey = "double_blast_radius_powerups";
	public static string ReverseTimePowerUpsKey = "reverse_time_powerups";
	public static string ChageQuestionCategoryPowerUpsKey = "change_question_category_powerups";

	public float autoSaveTimeIntervalSeconds = 300;

	private float autoSaveTime;

	void Start() {
	}

	void Update() {
		autoSaveTime += Time.deltaTime;
		if (autoSaveTime >= autoSaveTimeIntervalSeconds) {
			autoSaveTime = 0;

			PostPlayerData();
		}
	}

	public void PostPlayerData() {
		int coins = PlayerPrefs.GetInt (PlayerData.TotalGoldKey, 0);
		int totalScore = PlayerPrefs.GetInt (PlayerData.TotalScoreKey, 0);
		ulong totalXP = ulong.Parse (PlayerPrefs.GetString (PlayerData.TotalXPKey, "0"));

		int bombPowerUps = PlayerPrefs.GetInt (PlayerData.BombPowerUpsKey, 0);
		int doubleBlastRediusPowerUps = PlayerPrefs.GetInt (PlayerData.DoubleBlastRadiusPowerUpsKey, 0);
		int reverseTimePowerUps = PlayerPrefs.GetInt (PlayerData.ReverseTimePowerUpsKey, 0);
		int changeQuestionCategoryPowerUpd = PlayerPrefs.GetInt (PlayerData.ChageQuestionCategoryPowerUpsKey, 0);
	}
}
