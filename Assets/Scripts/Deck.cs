using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;

    public Button subirApuesta;
    private int banca = 1000;
    public Text dinero;
    private int dineroMesa = 0; 
    public Text apostado;

    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        for(int palo = 0; palo < 4; palo++)
        {
            for (int i = 0; i < 13; i++)
            { 
                if (i < 10)
                {
                    values[13 * palo + i] = i+1;
                }
                else
                {
                    values[13 * palo + i] = 10;
                }
            }
        }
    }

    private void ShuffleCards()
    {
        for (int i = 0; i < 51; i++)
        {
            int aleatorizador = Random.Range(0,52);
            Sprite faceAux = faces[i];
            int valueAux = values[i];

            faces[i] = faces[aleatorizador];
            values[i] = values[aleatorizador];

            faces[aleatorizador] = faceAux;
            values[aleatorizador] = valueAux;
        }
    }

    void StartGame()
    {
        banca -= 10;
        dinero.text = banca + "€";
        dineroMesa += 10;
        apostado.text = dineroMesa + "€";
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            if (dealer.GetComponent<CardHand>().points == 21)
            {
                subirApuesta.interactable = false;
                hitButton.interactable = false;
                stickButton.interactable = false;
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                finalMessage.text = "Has perdido";
                dineroMesa = 0;
                apostado.text = dineroMesa + "€";
            }
            else if (player.GetComponent<CardHand>().points == 21)
            {
                subirApuesta.interactable = false;
                hitButton.interactable = false;
                stickButton.interactable = false;
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                finalMessage.text = "Has ganado";
                banca += dineroMesa * 2;
                dinero.text = banca + "€";
                dineroMesa = 0;
                apostado.text = dineroMesa + "€";
            }
            else if (dealer.GetComponent<CardHand>().points == 21 && player.GetComponent<CardHand>().points == 21)
            {
                subirApuesta.interactable = false;
                hitButton.interactable = false;
                stickButton.interactable = false;
                dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
                finalMessage.text = "Empate";
                banca += dineroMesa;
                dinero.text = banca + "€";
                dineroMesa = 0;
                apostado.text = dineroMesa + "€";
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
        float casosTotales = values.Length - player.GetComponent<CardHand>().cards.Count - dealer.GetComponent<CardHand>().cards.Count;
        float valorAsuperarDealer = player.GetComponent<CardHand>().points - values[1];
        float favorDealerMas = 0;
        float valorAsuperarPlantarse = player.GetComponent<CardHand>().points;
        float favorPlantarse = 0;
        float favorPasarse = 0;

        if (values[1] > valorAsuperarDealer)
        {
            favorDealerMas++;
        }
        if (values[1] + valorAsuperarPlantarse <= 21 && values[1] + valorAsuperarPlantarse >= 17)
        {
            favorPlantarse++;
        }
        if (values[1] + valorAsuperarPlantarse > 21)
        {
            favorPasarse++;
        }

        for (int i = player.GetComponent<CardHand>().cards.Count + dealer.GetComponent<CardHand>().cards.Count + 1; i < values.Length; i++)
        {
            if (values[i] > valorAsuperarDealer)
            {
                favorDealerMas++;
            }
            if (values[i] + valorAsuperarPlantarse <= 21 && values[i] + valorAsuperarPlantarse >= 17)
            {
                favorPlantarse++;
            }
            if(values[i] + valorAsuperarPlantarse > 21)
            {
                favorPasarse++;
            }
        }

        string probabilidades = "Dealer tenga más: " + Mathf.Round(100 * (favorDealerMas / casosTotales)).ToString() + "%" + "\r\n" +
            "Tener entre 17 y 21: " + Mathf.Round(100 * (favorPlantarse / casosTotales)).ToString() + "% \r\n" +
            "Tener más de 21: " + Mathf.Round(100 * (favorPasarse / casosTotales)).ToString() + "%";
        probMessage.text = probabilidades;
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */  
        if(player.GetComponent<CardHand>().points > 21)
        {
            subirApuesta.interactable = false;
            hitButton.interactable = false;
            stickButton.interactable = false;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "Has perdido";
            dineroMesa = 0;
            apostado.text = dineroMesa + "€";
        }

    }

    public void Stand()
    {
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */    
        while(dealer.GetComponent<CardHand>().points < 17)
        {
            PushDealer();
        }
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        if(dealer.GetComponent<CardHand>().points <= 21 && dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            subirApuesta.interactable = false;
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Has perdido";
            dineroMesa = 0;
            apostado.text = dineroMesa + "€";
        }
        else if (player.GetComponent<CardHand>().points <= 21 && player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points || dealer.GetComponent<CardHand>().points > 21)
        {
            subirApuesta.interactable = false;
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Has ganado";
            banca += dineroMesa * 2;
            dinero.text = banca + "€";
            dineroMesa = 0;
            apostado.text = dineroMesa + "€";
        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            subirApuesta.interactable = false;
            hitButton.interactable = false;
            stickButton.interactable = false;
            finalMessage.text = "Empate";
            banca += dineroMesa;
            dinero.text = banca + "€";
            dineroMesa = 0;
            apostado.text = dineroMesa + "€";
        }
    }

    public void PlayAgain()
    {
        subirApuesta.interactable = true;
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public void Apostar()
    {
        banca -= 10;
        dinero.text = banca + "€";
        dineroMesa += 10;
        apostado.text = dineroMesa + "€";
    }
}
