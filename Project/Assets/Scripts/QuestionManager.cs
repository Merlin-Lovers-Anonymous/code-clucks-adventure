using UnityEngine;
using System.IO;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public TextAsset questions;

    public GameObject questionHUD;
    public TMP_Text optionA;
    public TMP_Text optionB;
    public TMP_Text optionC;
    public TMP_Text optionD;
    public TMP_Text dialogue;

    public BattleSystem battleSystem;

    private string[,] csvData;

    private string questionAnswer;

    void Start()
    {
        LoadCSV();
    }
    void LoadCSV()
    {
        string[] lines = questions.text.Split("\n");

        int rows = lines.Length;
        int columns = lines[0].Split(',').Length;

        csvData = new string[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            string[] values = lines[i].Split(',');

            for (int j = 0; j < columns; j++)
            {
                csvData[i, j] = values[j];
            }
        }
    }

    void RemoveRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= csvData.GetLength(0))
        {
            Debug.LogError("Invalid row index.");
            return;
        }

        string[,] newCsvData = new string[csvData.GetLength(0) - 1, csvData.GetLength(1)];

        for (int i = 0, newRow = 0; i < csvData.GetLength(0); i++)
        {
            if (i != rowIndex)
            {
                for (int j = 0; j < csvData.GetLength(1); j++)
                {
                    newCsvData[newRow, j] = csvData[i, j];
                }
                newRow++;
            }
        }

        csvData = newCsvData;
    }
    private string[] QuestionPicker()
    {
        int rand = UnityEngine.Random.Range(0, csvData.GetLength(0));
        string[] question = {csvData[rand, 0], csvData[rand, 1], csvData[rand, 2], csvData[rand, 3], csvData[rand, 4], csvData[rand, 5]};
        RemoveRow(rand);

        return question;
    }

    public void AskQuestion()
    {
        string[] question = QuestionPicker();
        dialogue.text = question[0];
        optionA.text = question[1];
        optionB.text = question[2];
        optionC.text = question[3];    
        optionD.text = question[4];
        
        questionAnswer = question[5];

        questionHUD.SetActive(true);
    }

    public void OnButtonA()
    {
        CheckAnswer("A");
    }
    public void OnButtonB()
    {
        CheckAnswer("B");
    }
    public void OnButtonC()
    {
        CheckAnswer("C");
    }
    public void OnButtonD()
    {
        CheckAnswer("D");
    }

    private void CheckAnswer(string answer)
    {
        questionHUD.SetActive(false);
        if (answer.Equals(questionAnswer.Trim()))
        {
            battleSystem.StartCoroutine(battleSystem.PlayerAttack());
        }
        else
        {
            dialogue.text = "The attack failed!";
            battleSystem.StartEnemyTurn();
        }
    }
}
