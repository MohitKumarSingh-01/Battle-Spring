using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class High : MonoBehaviour
{
    [SerializeField] TMP_Text highScore;
    void Update()
    {
		highScore.text = "highscore : " + PlayerPrefs.GetInt("Highscore").ToString();

    }
}
