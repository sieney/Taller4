using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.Networking;
//using System.IO.Ports; --> LIBRERIA PARA ARDUINO

namespace Mirror.Scenes
{
    public class jugador : NetworkBehaviour
    {
    
    // CONTROL
    
    public float vel = 1.5f;
    public float vel2 = 1.5f;

    private int dir;
    private int dir2;
    private int dir3;

    SerialPort puerto = new SerialPort("COM3", 9600);
    
    // JUGADOR
    
        public float rotationSpeed = 100;
        
        public TextMesh nameText;

        // These are set in OnStartServer and used in OnStartClient
        [SyncVar]
        int playerNo;

        [SyncVar]
        public bool isReady;

        // This fires on server when this player object is network-ready
		//el cliente se conecta al servidor
        public override void OnStartServer()
        {
            base.OnStartServer();

            // Set SyncVar values
            // this Id is unique for every connection on the server
            playerNo = connectionToClient.connectionId;

            isReady = true;
        }

        // This fires on all clients when this player object is network-ready
		//arranca el cliente
        public override void OnStartClient()
        {
            base.OnStartClient();
            //coloca como texto el número que le envío el servidor
            nameText.text = string.Format("Player " + playerNo);
        }
     
     // CONTROL
     
       void Start()   
        {
            if (isLocalPlayer)
            {
                this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = true;

            }
            else
            {
                this.transform.GetChild(0).gameObject.GetComponent<Camera>().enabled = false;
            }
        
        puerto.Open();
        puerto.ReadTimeout = 1;
    }

// jugador


        // Update is called once per frame
        void Update()
        {
			// permite el movimiento del jugador de forma independiente para cada de cada cliente, de esa forma cada cliente controla su propio jugador 
            if (!isLocalPlayer)
                return;


            if (Input.GetKey(KeyCode.Space))
            {
            transform.Translate(Vector3.up * vel, Space.Self);
            transform.Translate(Vector3.forward * vel, Space.Self);
            }
            
            if (Input.GetKey(KeyCode.W))
            {
            transform.Translate(Vector3.forward * vel, Space.Self);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
            transform.Rotate(Vector3.down * vel2, Space.Self);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
            transform.Rotate(Vector3.up * vel2, Space.Self);
            }
            
            // CONTROL
            
        if (puerto.IsOpen == true)
        {
            try
            {
                mover(puerto.ReadLine());
               print(puerto.ReadLine());
           }
           catch (System.Exception)
           {
            }
        }

        }
        
        void mover(string datoArduino)
    {
        string[] datosArray = datoArduino.Split(char.Parse(","));

       if (datosArray.Length == 3)
       {
            if(datosArray[0] == "P")
            {
               dir = 1;
            }
            else if (datosArray[0] == "O")
            {
               dir = 0;
            }
            
            if(datosArray[1] == "P")
            {
                dir2 = 1;
            }
            else if (datosArray[1] == "O")
            {
                dir2 = 0;
            }

            if (datosArray[2] == "A")
            {
                dir3 = 1;
            }
            else if (datosArray[2] == "B")
            {
                dir3 = 2;
            }
                else if (datosArray[2] == "C")
            {
               dir3 = 3;
           }


            print(dir + "   " + dir2 + "   " + dir3);
        }

        if (dir == 1)
        {
            transform.Translate(Vector3.forward * vel, Space.Self);
       }
        
        if (dir2 == 1)
        {
            transform.Translate(Vector3.up * vel, Space.Self);
            transform.Translate(Vector3.forward * vel, Space.Self);
       }

        if (dir3 == 1)
        {
            transform.Rotate(Vector3.up * vel2, Space.Self);
        }
        else if (dir3 == 3)
        {
            transform.Rotate(Vector3.down * vel2, Space.Self);
        }


   }

    }
}

