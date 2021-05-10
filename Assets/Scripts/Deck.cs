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
        int[] listaBarajados = new int[52];

        int aletorio;

        int longitud = 0;

        for (int i = 0; i < 51; i++)
        {
            aletorio = Random.Range(0, 51);

            for (int j = 0; j < 51; j++)
            {
                if (aletorio != listaBarajados[i])
                {
                    listaBarajados[longitud] = aletorio;
                    longitud++;
                }
            }
        }

        Sprite[] facesAux = faces;
        int[] valuesAux = values;
        for(int i = 0; i < 51; i++)
        {
            faces[i] = facesAux[listaBarajados[i]];
            values[i] = valuesAux[listaBarajados[i]];
        }
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
        int puntuacionJugador = comprobarAs(values[0], values[2]);
        int puntuacionDealer = comprobarAs(values[1], values[3]);
        if (puntuacionJugador == 21)
        {
            Debug.Log("Jugador gana");
        }
        else if (puntuacionDealer == 21)
        {
            Debug.Log("Dealer gana");
        }
        else if (puntuacionJugador == 21 && puntuacionDealer == 21)
        {
            Debug.Log("Empate");
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
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public int comprobarAs(int carta1, int carta2)
    {
        if(carta1 == 1 && carta2+11 == 21)
        {
            return 21;
        }
        if(carta2 == 1 && carta1+11==21)
        {
            return 21;
        }
        return carta1 + carta2;
    }
}
