using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSo> questions = new List<QuestionSo>();
    private QuestionSo currentQuestion;

    [Header("Answers")]
    [SerializeField] private Button[] answerButtons;
    private int correctAnswerIndex;
    private bool hasAnsweredEarly = true;

    [Header("Buttons")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] private Image timerImage;
    private Timer timer;

    [Header("Scoring")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] private Slider progressBar;

    public bool isComplete;

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        timer = FindObjectOfType<Timer>();
    }

    private void Start()
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
        isComplete = false;
    }

    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;

        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            GetNextQuestion();
            timer.loadNextQuestion = false;
            hasAnsweredEarly = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage = answerButtons[correctAnswerIndex].image;
        buttonImage.sprite = correctAnswerSprite;

        if (index == currentQuestion.CorrectAnswerIndex)
        {
            questionText.text = "Correct!";
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was;\n" + correctAnswer;
        }
    }

    private void GetNextQuestion()
    {
        if (questions.Count == 0) { return; }

        GetRandomQuestion();
        DisplayQuestion();

        SetButtonState(true);
        SetDefaultButtonSprites();

        progressBar.value++;
        scoreKeeper.IncrementQuestionsSeen();
    }

    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestion.Question;
        correctAnswerIndex = currentQuestion.CorrectAnswerIndex;

        for (int i = 0; i < answerButtons.Length; ++i)
        {
            TextMeshProUGUI answerText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            answerText.text = currentQuestion.GetAnswer(i);
        }
    }

    private void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; ++i)
        {
            answerButtons[i].interactable = state;
        }
    }

    private void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; ++i)
        {
            Image buttonImage = answerButtons[i].image;
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
