using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Master/Question", order = 0)]
public class QuestionSo : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] private string question = "Enter new question text here";
    [SerializeField] private string[] answers = new string[4];

    [Range(0, 3)]
    [SerializeField] private int correctAnswerIndex = 0;

    public string Question { get { return question; } }
    public int CorrectAnswerIndex { get { return correctAnswerIndex; } }

    public string GetAnswer(int index) { return index >= 0 && index < answers.Length ? answers[index] : ""; }
    
}
