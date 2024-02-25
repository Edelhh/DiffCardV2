using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialController : MonoBehaviour
{
    private int modegame = 1;
    private int countgame = 0;
    public Text viewModeGame; 
    public InputField inputUsuario;


    public void Start()
    {
        print(PlayerPrefs.GetInt("CountGame"));
        //PlayerPrefs.SetInt("CountGame",0);
    }

    public void BotonDerecho() 
    {
        if (modegame < 4)
        {
            modegame++;
        }
        else 
        {
            modegame = 1;
        }

        viewModeGame.text = modegame.ToString();

    }

    public void BotonIzquierdo()
    {
        if (modegame > 1)
        {
            modegame--;
        }
        else
        {
            modegame = 4;
        }
        viewModeGame.text = modegame.ToString();

    }

    public void ExitButton() 
    {
        Application.Quit();
    }


    public void StartGame() 
    {
        if (inputUsuario.text == "") 
        {
            inputUsuario.text = "0";
        } 
        PlayerPrefs.SetString("IDUsuario", inputUsuario.text);
        PlayerPrefs.SetInt("ModeGame", modegame);

        countgame = PlayerPrefs.GetInt("CountGame") + 1 ;
        print(countgame);
        PlayerPrefs.SetInt("CountGame", countgame);


        SceneManager.LoadScene("GameScene");
        


    }

}
