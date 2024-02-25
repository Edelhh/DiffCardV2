using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite bgImage;

    [SerializeField]
    private GameObject puzzleField;

    [SerializeField]
    private GameObject btn;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<GameObject> btns = new List<GameObject>();

    private bool firstGuess, secondGuess, thirdGuess, fourthGuess;

    private int totalcountguesses;
    private int countGuesses;
    private int countCorrectGuesses;
    private int totalCountCorrectGuesses;
    private int gameGuesses;
    public int colums, totalelements;
    private int equalcard;
    private int countmodegame;
    private int countgame;
    private string IDusuario;
    private int countArray;

    private DateTime hora_inicio;
    private DateTime hora_fin;

    private GridLayoutGroup gridLayout;

    private int firstGuessIndex, secondGuessIndex, thirdGuessIndex, fourthGuessIndex;
    
    private string firstGuessPuzzle, secondGuessPuzzle, thirdGuessPuzzle, fourthGuessPuzzle;


    public Image timbeBar;
    private int maxTime = 300;
    float timeLeft;
    public Text TimerText;
    public Text ScoreText;



    private void Awake()
    {

        IDusuario = PlayerPrefs.GetString("IDUsuario");
        countmodegame = PlayerPrefs.GetInt("ModeGame");
        countgame = PlayerPrefs.GetInt("CountGame");

        if (countmodegame == 2)
        {
            equalcard = 2;
            colums = 5;
            totalelements = 20;
            maxTime = 300;
        } else if (countmodegame == 3) 
        {
            equalcard = 3;
            colums = 7;
            totalelements = 30;
            maxTime = 300;
        } else if (countmodegame == 4) 
        {
            equalcard = 4;
            colums = 10;
            totalelements = 40;
            maxTime = 300;
        } else if (countmodegame == 1)
        {
            equalcard = 2;
            colums = 2;
            totalelements = 6;
            maxTime = 30;
        }


AddButton(totalelements);
        puzzles = Resources.LoadAll<Sprite>("SpritesHisaGames");
        gridLayout = puzzleField.AddComponent<GridLayoutGroup>();

    }

    private void Start()
    {
        hora_inicio = DateTime.Now;
        CreateGrid(colums);
        GetButton();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / equalcard;
        countArray++;
        timeLeft = maxTime;
    }


    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            TimerText.text = "Tiempo: " + Math.Round(timeLeft, 2).ToString();
            timbeBar.fillAmount = timeLeft / maxTime;
        }
        else if (timeLeft <= 0)
        {
            timeLeft = 0;
            Debug.Log("Tiempo terminado");

            hora_fin = DateTime.Now;

            string texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + totalCountCorrectGuesses.ToString() + ";" + totalcountguesses.ToString() + ";" + countArray.ToString() + ";" + hora_inicio.ToString() + ";" + hora_fin.ToString() ;
            WritePuzzle(texto);
            texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + countArray.ToString() + ";" + timeLeft.ToString() + ";" + countGuesses.ToString() + ";" + countCorrectGuesses.ToString();
            WriteArray(texto);
            SceneManager.LoadScene("InicialScene");

        }
        
    }


    void CreateGrid(int columns) 
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        gridLayout.cellSize = new Vector2(100f, 100f);
        gridLayout.spacing = new Vector2(12f, 15f);
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
    }


    void AddButton(int totalElements) 
    {

        for (int i = 0; i < totalElements; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;

            // Establece el contenedor como padre de la carta
            button.transform.SetParent(puzzleField.transform, false);
        }

    }

    void GetButton() 
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++) 
        {
            btns.Add(objects[i]);
            btns[i].GetComponent<Button>().image.sprite = bgImage;
        }
    }

    void AddGamePuzzles() 
    {
        int looper = btns.Count;
        int index = 0;

        for (int i = 0; i < looper;i++) 
        {
            if (index == looper / equalcard)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners() 
    {
        foreach (GameObject btn in btns) 
        {
            btn.GetComponent<Button>().onClick.AddListener(()=> PickAPuzzle(equalcard));    
        }
    }

    public void PickAPuzzle(int modegame) 
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].GetComponent<Button>().image.sprite = gamePuzzles[firstGuessIndex];
            btns[firstGuessIndex].GetComponent<Button>().interactable = false;
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].GetComponent<Button>().image.sprite = gamePuzzles[secondGuessIndex];
            btns[secondGuessIndex].GetComponent<Button>().interactable = false;

            if (modegame == 2) 
            {
                countGuesses++;
                totalcountguesses++;
                StartCoroutine(CheckIfThePuzzlesMatchTwo());
            }
        }
        else if (!thirdGuess && modegame > 2 ) 
        {
            thirdGuess = true;
            thirdGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            thirdGuessPuzzle = gamePuzzles[thirdGuessIndex].name;
            btns[thirdGuessIndex].GetComponent<Button>().image.sprite = gamePuzzles[thirdGuessIndex];
            btns[thirdGuessIndex].GetComponent<Button>().interactable = false;

            if (modegame == 3)
            {
                countGuesses++;
                totalcountguesses++;
                StartCoroutine(CheckIfThePuzzlesMatchThird());
            }
        }
        else if (!fourthGuess && modegame > 3)
        {
            fourthGuess = true;
            fourthGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            fourthGuessPuzzle = gamePuzzles[fourthGuessIndex].name;
            btns[fourthGuessIndex].GetComponent<Button>().image.sprite = gamePuzzles[fourthGuessIndex];
            btns[fourthGuessIndex].GetComponent<Button>().interactable = false;

            if (modegame == 4)
            {
                countGuesses++;
                totalcountguesses++;
                StartCoroutine(CheckIfThePuzzlesMatchFourth());
            }
        }

    }

    IEnumerator CheckIfThePuzzlesMatchTwo() 
    {
        yield return new WaitForSeconds(.5f);
        bool escorrecto = false;

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.1f);
            btns[firstGuessIndex].GetComponent<Button>().interactable = false;
            btns[secondGuessIndex].GetComponent<Button>().interactable= false;


            Destroy(btns[firstGuessIndex]);
            Destroy(btns[secondGuessIndex]);

            CheckIfTheGamesFinished();

            ScoreText.text = "Puntaje: " + totalCountCorrectGuesses.ToString();
            escorrecto = true;

        }
        else
        {
            btns[firstGuessIndex].GetComponent<Button>().interactable = true;
            btns[secondGuessIndex].GetComponent<Button>().interactable = true;
            btns[firstGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[secondGuessIndex].GetComponent<Button>().image.sprite = bgImage;
        }

        string texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + countArray.ToString() + ";" + countGuesses.ToString() + ";" + Math.Round(timeLeft, 2).ToString() + ";" + escorrecto.ToString();
        WriteMovimiento(texto);
        yield return new WaitForSeconds(.1f);
        firstGuess = secondGuess = false;
    }

    IEnumerator CheckIfThePuzzlesMatchThird()
    {
        yield return new WaitForSeconds(.5f);
        bool escorrecto = false;

        if ((firstGuessPuzzle == secondGuessPuzzle) && (secondGuessPuzzle == thirdGuessPuzzle))
        {
            yield return new WaitForSeconds(.1f);
            btns[firstGuessIndex].GetComponent<Button>().interactable = false;
            btns[secondGuessIndex].GetComponent<Button>().interactable = false;
            btns[thirdGuessIndex].GetComponent<Button>().interactable = false;


            Destroy(btns[firstGuessIndex]);
            Destroy(btns[secondGuessIndex]);
            Destroy(btns[thirdGuessIndex]);

            CheckIfTheGamesFinished();

            ScoreText.text = "Puntaje: " + totalCountCorrectGuesses.ToString();
            escorrecto = true;

        }
        else
        {
            btns[firstGuessIndex].GetComponent<Button>().interactable = true;
            btns[secondGuessIndex].GetComponent<Button>().interactable = true;
            btns[thirdGuessIndex].GetComponent<Button>().interactable = true;
            btns[firstGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[secondGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[thirdGuessIndex].GetComponent<Button>().image.sprite = bgImage;
        }

        string texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + countArray.ToString() + ";" + countGuesses.ToString() + ";" + Math.Round(timeLeft, 2).ToString() + ";" + escorrecto.ToString();
        WriteMovimiento(texto);
        yield return new WaitForSeconds(.1f);
        firstGuess = secondGuess = thirdGuess = false;
    }

    IEnumerator CheckIfThePuzzlesMatchFourth()
    {
        yield return new WaitForSeconds(.5f);
        bool escorrecto = false;
        if ((firstGuessPuzzle == secondGuessPuzzle) && (secondGuessPuzzle == thirdGuessPuzzle) && (thirdGuessPuzzle == fourthGuessPuzzle))
        {
            yield return new WaitForSeconds(.1f);
            btns[firstGuessIndex].GetComponent<Button>().interactable = false;
            btns[secondGuessIndex].GetComponent<Button>().interactable = false;
            btns[thirdGuessIndex].GetComponent<Button>().interactable = false;
            btns[fourthGuessIndex].GetComponent<Button>().interactable = false;


            Destroy(btns[firstGuessIndex]);
            Destroy(btns[secondGuessIndex]);
            Destroy(btns[thirdGuessIndex]);
            Destroy(btns[fourthGuessIndex]);

            CheckIfTheGamesFinished();

            ScoreText.text = "Puntaje: " + totalCountCorrectGuesses.ToString();
            escorrecto = true;

        }
        else
        {
            btns[firstGuessIndex].GetComponent<Button>().interactable = true;
            btns[secondGuessIndex].GetComponent<Button>().interactable = true;
            btns[thirdGuessIndex].GetComponent<Button>().interactable = true;
            btns[fourthGuessIndex].GetComponent<Button>().interactable = true;

            btns[firstGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[secondGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[thirdGuessIndex].GetComponent<Button>().image.sprite = bgImage;
            btns[fourthGuessIndex].GetComponent<Button>().image.sprite = bgImage;
        }

        string texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + countArray.ToString() + ";" + countGuesses.ToString() + ";" + Math.Round(timeLeft, 2).ToString() + ";" + escorrecto.ToString();
        WriteMovimiento(texto);
        yield return new WaitForSeconds(.1f);
        firstGuess = secondGuess = thirdGuess = fourthGuess = false;
    }


    void CheckIfTheGamesFinished() 
    {
        countCorrectGuesses++;
        totalCountCorrectGuesses += 1;

        if (countCorrectGuesses == gameGuesses) 
        {
            string texto = IDusuario + ";" + countgame.ToString() + ";" + countmodegame.ToString() + ";" + countArray.ToString() + ";" + Math.Round(timeLeft, 2).ToString() + ";" +  countGuesses.ToString() + ";" + countCorrectGuesses.ToString();
            WriteArray(texto);
            countArray++;
            Debug.Log("Game Finished");
            Debug.Log("It took you " + countGuesses + " many guess(es) to finish the game");
            countGuesses = 0;
            countCorrectGuesses = 0;
            StartCoroutine(WaitAndRestartGame(2f));
        }
    }

    IEnumerator WaitAndRestartGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Reinicia las variables y configura el juego nuevamente
        btns = new List<GameObject>();
        gamePuzzles = new List<Sprite>();

        AddButton(totalelements);
        CreateGrid(colums);
        GetButton();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / equalcard;

        // Restablece variables de seguimiento del juego
        countGuesses = 0;
        countCorrectGuesses = 0;
        firstGuess = secondGuess = false;
    }

    void Shuffle(List<Sprite> list) 
    {
        for (int i = 0; i < list.Count; i++) 
        {
            Sprite temp = list[i];
            int randomindex = UnityEngine.Random.Range(0,list.Count);
            list[i] = list[randomindex];
            list[randomindex] = temp;
        }
    }

    public void WritePuzzle(string datastring)
    {
        string filename = Application.dataPath + "/DatosPuzzle.csv";

        if (File.Exists(filename))
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine(datastring);
            tw.Close();
        }
        else
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine("IDUser;numero_juego;modo_juego;parejas_encontradas;movimientos_totales;array_alcanzado;hora_inicio;hora_fin");
            tw.WriteLine(datastring);
            tw.Close();
        }
    }


    public void WriteArray(string datastring)
    {
        string filename = Application.dataPath + "/DatosArray.csv";

        if (File.Exists(filename))
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine(datastring);
            tw.Close();
        }
        else
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine("IDUser" + ";" + "numero_juego;" + "modo_juego" + ";" + "numero_array" + ";" +"tiempo_usado" + ";" + "movimientos_totales_array"+";"+"movimientos_correctos_array");
            tw.WriteLine(datastring);
            tw.Close();
        }
    }

    public void WriteMovimiento(string datastring)
    {
        string filename = Application.dataPath + "/DatosMovimiento.csv";

        if (File.Exists(filename))
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine(datastring);
            tw.Close();
        }
        else
        {
            TextWriter tw = new StreamWriter(filename, true);
            tw.WriteLine("IDUser" + ";" + "numero_juego;" + "modo_juego" + ";" + "numero_array" + ";" + "numero_movimiento" + ";" + "tiempo_movimiento" + ";" + "es_correcto");
            tw.WriteLine(datastring);
            tw.Close();
        }
    }

}
