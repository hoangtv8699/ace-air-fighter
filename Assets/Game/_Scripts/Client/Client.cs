using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;

public class Client : MonoBehaviour {

    public GameObject[] PlayersPrefabs;
    public GameObject[] EnemysPrefabs;

    private string clientName;
    private int TcpPortToConnect = 3308;
    private string hostToConnect = "127.0.0.1";
    private TcpClient TcpSocket;
    private bool TcpReady;
    public bool isPlaying;
    private bool isReady;
    private NetworkStream TcpStream;
    private StreamWriter TcpWriter;
    private StreamReader TcpReader;
    private Hashtable PlayerData;
    private List<Player> ListPlayer;
    private List<GameObject> GameObjectPlayer;
    private Thread ReadThread;



    private void OnEnable()
    {
        TcpReady = false;
        isPlaying = false;
        isReady = false;
        Debug.Log("start tcp");
        DontDestroyOnLoad(gameObject);
        TcpConnectToServer(hostToConnect, TcpPortToConnect);
        PlayerData = new Hashtable();
        ListPlayer = new List<Player>();
        GameObjectPlayer = new List<GameObject>();
    }
    // Use this for initialization
    void Start () {
        
        
    }

    public void setInt(string key, int value)
    { 
        PlayerData.Add(key, value);
    }

    public void setString(string key, string value)
    {
        PlayerData.Add(key, value);
    }

    public int getInt(string key)
    {
        return (int)PlayerData[key];
    }

    public string getString(string key)
    {
        return (string)PlayerData[key];
    }

    public bool TcpConnectToServer(string host, int port)
    {
        if (TcpReady)
            return true;

        try
        {
            TcpSocket = new TcpClient(host, port);
            TcpStream = TcpSocket.GetStream();
            TcpWriter = new StreamWriter(TcpStream);
            TcpReader = new StreamReader(TcpStream);

            TcpReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
        }

        return TcpReady;
    }

    public bool TcpSend(string data)
    {
        if (!TcpReady)
            return false;

        TcpWriter.WriteLine(data);
        TcpWriter.Flush();
        return true;
    }

    public string TcpRead()
    {
        if (!TcpReady)
            return null;
        return TcpReader.ReadLine();
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    public void CloseSocket()
    {
        if (!TcpReady)
        {
            return;
        }
        else
        {
            if (getString("PlayerName") != null)
            {
                TcpSend("QUIT|" + getString("PlayerName"));
            }
            TcpWriter.Close();
            TcpReader.Close();
            TcpSocket.Close();
            TcpReady = false;
            ReadThread.Abort();
        }
    }
    // login execute
    public bool login(string name)
    {
        string data;
        if (name.Equals(""))
            return false;
        
        if(TcpSend("LOGIN|" + name))
        {
            Debug.Log("LOGIN|" + name);
        }
        else
        {
            return false;
        }

        data = TcpRead();

        TcpReady = false;
        TcpConnectToServer(hostToConnect, TcpPortToConnect);

        Debug.Log("test data: " + data);
        if (data != null && check(data.Split('|')))
        {
            setString("PlayerName", name);
            return true;
        }
        return false;
    }

    private bool check(string[] data)
    {
        if (data[0].Equals("LOGIN") && int.Parse(data[1]) == 1)
        {
            setRoomAndLevel(data);
            return true;
        }

        return false;
    }

    private void setRoomAndLevel(string[] data)
    {
        int NumOfLevel = int.Parse(data[2]);
        setInt("NumOfLevel", NumOfLevel);

        for (int i = 1; i <= NumOfLevel; i++)
        {
            //setInt("Level" + (i + 1), int.Parse(data[i + 2]));
            setInt("Level" + i, int.Parse(data[3]));
        }
    }
    // join room execute
    public bool joinRoom(int RoomIndex)
    {
        bool temp = false;
        string[] data;
        string name = getString("PlayerName");
        int LevelSelect = getInt("LevelSelect");
        int NumberOfRoom = getInt("Level" + LevelSelect);

        TcpSend("JOIN_ROOM|" + name + "|" + (LevelSelect * NumberOfRoom + RoomIndex) + "|" + getInt("PlayerIndex"));
        data = TcpRead().Split('|');

        Debug.Log(data[0] + data[1]);

        if (data[0].Equals("JOIN_ROOM") && int.Parse(data[1]) == 1)
        {
            temp = true;

            // create a new thread to handle read data
            ReadThread = new Thread(handle);
            ReadThread.Start();

        }
        return temp;
    }

    void handle()
    {
        while (true)
        {
            Debug.Log("handle");
            string data = TcpRead();
            Debug.Log(data);

            string[] splited = data.Split('|');

            switch (splited[0])
            {
                case "STATE":
                    // for each player in state message
                    for(int i = 1; i <= 3; i++)
                    {
                        if (!splited[i].Equals("null"))
                        {
                            // make Player a from json an check
                            Player a = Player.CreateFromJSON(splited[i]);
                            if (a != null)
                            {
                                if (IsPlaying())
                                {
                                    // if playing so update state game
                                    Debug.Log("add " + a.name);
                                    for (int j = 0; j < GameObjectPlayer.Count; j++)
                                    {
                                        if (a.name == GameObjectPlayer[j].name)
                                        {
                                            GameObjectPlayer[j].transform.position = Vector3.MoveTowards(GameObjectPlayer[j].transform.position, a.position, Time.deltaTime * 50.1f);
                                        }
                                    }
                                }
                                else
                                {
                                    //else add if not in
                                    Debug.Log("not playing: " + ListPlayer.Count);
                                    bool isIn = false;
                                    for (int j = 0; j < ListPlayer.Count; j++)
                                    {
                                        if (a.name == ListPlayer[j].name)
                                        {
                                            isIn = true;
                                            break;
                                        }
                                    }

                                    if (!isIn && ListPlayer.Count < 3)
                                    {
                                        Debug.Log("add " + a.name);
                                        ListPlayer.Add(a);
                                    }

                                }
                            }
                        }

                    }
                    // to do
                    break;
                case "SHOT":
                    // to do
                    break;
                case "START":
                    // to do
                    OnStart();
                    break;
            }

        }

    }

    public void CreatePlayerOnScence()
    {
        Debug.Log("size " + ListPlayer.Count);
        foreach (Player player in ListPlayer)
        {
            Debug.Log("find");
            GameObject p = GameObject.Find(player.name);

            if (p == null)
            {
                Debug.Log("create");
                p = GameObject.Instantiate(PlayersPrefabs[player.plane - 1], player.position, Quaternion.identity) as GameObject;
                p.name = player.name;
                GameObjectPlayer.Add(p);
            }
        }
    }

    public void CreateEnemyOnScence()
    {
        // to do
    }

    public void move(Vector3 position)
    {
        if (!IsPlaying())
            return;

        string move = "MOVE|" + position.x + "|" + position.y + "|" + position.z + "|" + getString("PlayerName");

        TcpSend(move);
    }

    public void Ready()
    {
        isReady = true;
        TcpSend("READY|" + getString("PlayerName"));
    }

    //call when recive START message
    public void OnStart()
    {
        isPlaying = true;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    // Update is called once per frame
    void Update () {

    }

    [Serializable]
    public class Player
    {
        public string name;
        public bool enable;
        public long score;
        public int plane;
        public Vector3 position;
        public int health;
        public int shield;
        public int gun;

        public static Player CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Player>(jsonString);
        }
    }

    [Serializable]
    public class Enemy
    {
        public string id;
        public bool enable;
        public int plane;
        public Vector3 position;
        public int health;

        public static Player CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Player>(jsonString);
        }
    }
}
