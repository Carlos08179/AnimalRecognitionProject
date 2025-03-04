using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class AnimalRecognition : MonoBehaviour
{
    public TextMeshProUGUI animalNameText;
    public TextMeshProUGUI animalInfoText;
    public RawImage cameraFeed;
    private FirebaseFirestore firestore;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                firestore = FirebaseFirestore.DefaultInstance;
                Debug.Log("Firebase listo.");

                // Llamada directa con el valor "tigre"
                SearchAnimalInfo("tigre");
            }
            else
            {
                Debug.LogError("Firebase no disponible");
            }
        });
    }

    public void SearchAnimalInfo(string animalId)
    {
        // Verifica si el ID está vacío.
        if (string.IsNullOrEmpty(animalId))
        {
            animalNameText.text = "Error";
            animalInfoText.text = "Por favor, ingresa un ID válido.";
            Debug.Log("ID vacío.");
            return;
        }

        Debug.Log("Buscando animal con ID: " + animalId);

        // Verifica si dbReference está inicializado.
        if (firestore  == null)
        {
            Debug.LogError("dbReference no está inicializado.");
            return;
        }

        firestore.Collection("animales").Document(animalId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                animalNameText.text = "Error";
                animalInfoText.text = "No se pudo conectar a la base de datos.";
                Debug.LogError("Error al buscar información: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Respuesta de Firebase recibida.");

                if (snapshot.Exists)
                {
                    Debug.Log("Datos recibidos: " + snapshot.GetRawJsonValue());

                    string name = snapshot.GetValue<string>("nombre");
                    string info = snapshot.GetValue<string>("descripcion");
                    
                    // Verifica si los campos de texto están asignados.
                    if (animalNameText != null && animalInfoText != null)
                    {
                        animalNameText.text = name;
                        animalInfoText.text = info;
                        Debug.Log("Animal encontrado: " + name);
                    }
                    else
                    {
                        Debug.LogError("animalNameText o animalInfoText no están asignados en el Inspector.");
                    }
                }
                else
                {
                    animalNameText.text = "Animal no encontrado";
                    animalInfoText.text = "Intenta con otra imagen.";
                    Debug.Log("Animal no encontrado.");
                }
            }
        });
    }
}
