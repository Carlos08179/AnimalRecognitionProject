using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using TMPro; 
using System.Collections;

public class AnimalQuizBallena : MonoBehaviour
{
    public GameObject quizCanvas; // Asigna el Canvas del quiz desde el Inspector
    public TextMeshProUGUI questionText; // Asigna el TextMeshPro para la pregunta desde el Inspector
    public TextMeshProUGUI scoreText; // Asigna el TextMeshPro para la puntuación desde el Inspector
    public Button[] answerButtons; // Asigna los botones de respuesta desde el Inspector

    private string[] questions = { "¿Dónde vive la ballena?", "¿Qué come la ballena?" };
    private string[] correctAnswers = { "Océanos", "Carnívoro" };

    private int currentQuestionIndex = 0;
    private int score = 0;

    private bool isQuestionAnswered = false; // Variable para rastrear si la pregunta ya fue respondida

    void Start()
    {
        // Oculta el quiz al inicio
        quizCanvas.SetActive(false);

        // Asigna las preguntas y respuestas a los botones
        ShowQuestion();
        UpdateScore();
    }

    void Update()
    {
        // Verifica si el Image Target está siendo detectado
        var observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour != null && 
            (observerBehaviour.TargetStatus.Status == Status.TRACKED || 
             observerBehaviour.TargetStatus.Status == Status.EXTENDED_TRACKED))
        {
            // Muestra el quiz cuando se detecta la imagen
            quizCanvas.SetActive(true);
        }
        else
        {
            // Oculta el quiz si no se detecta la imagen
            quizCanvas.SetActive(false);
        }
    }

    void ShowQuestion()
    {
    // Muestra la pregunta actual
    questionText.text = questions[currentQuestionIndex];

    // Asigna las opciones de respuesta a los botones
    for (int i = 0; i < answerButtons.Length; i++)
    {
        // Cambia esto para asignar las respuestas específicas
        answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = GetAnswerForQuestion(currentQuestionIndex, i);
    }
}
string GetAnswerForQuestion(int questionIndex, int answerIndex)
{
    // Define las respuestas para cada pregunta
    string[,] answers = {
        { "Bosques", "Desiertos", "Océanos" }, // Respuestas para la pregunta 1
        { "Carnívoro", "Herbívoro", "Omnívoro" } // Respuestas para la pregunta 2
    };

    return answers[questionIndex, answerIndex];
}

    void UpdateScore()
    {
        // Actualiza el texto de la puntuación
        scoreText.text = "Puntuación: " + score;
    }

    public void CheckAnswer(int answerIndex)
 {
    // Verifica si la pregunta ya fue respondida
    if (isQuestionAnswered)
    {
        return; // Si ya fue respondida, no hace nada
    }

    // Verifica si la respuesta es correcta
    if (answerButtons[answerIndex].GetComponentInChildren<TextMeshProUGUI>().text == correctAnswers[currentQuestionIndex])
    {
        Debug.Log("¡Respuesta correcta!");
        score++;
        // Cambia el color del botón a verde si la respuesta es correcta
        answerButtons[answerIndex].image.color = Color.green;
    }
    else
    {
        Debug.Log("Respuesta incorrecta.");
        // Cambia el color del botón a rojo si la respuesta es incorrecta
        answerButtons[answerIndex].image.color = Color.red;
    }

    // Marca la pregunta como respondida
    isQuestionAnswered = true;

    // Actualiza la puntuación
    UpdateScore();

    // Pasa a la siguiente pregunta después de un breve retraso
    StartCoroutine(NextQuestionWithDelay());
}

IEnumerator NextQuestionWithDelay()
{
    // Espera 1 segundo antes de pasar a la siguiente pregunta
    yield return new WaitForSeconds(1);

    // Restablece el color de todos los botones
    foreach (var button in answerButtons)
    {
        button.image.color = Color.white; // O el color original de los botones
    }

    // Reinicia la variable de control
    isQuestionAnswered = false;

    // Pasa a la siguiente pregunta
    currentQuestionIndex = (currentQuestionIndex + 1) % questions.Length;
    ShowQuestion();
}
}
