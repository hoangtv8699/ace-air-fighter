using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;
using UnityEngine.UI;

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
    private bool guard;
    private NetworkStream TcpStream;
    private StreamWriter TcpWriter;
    private StreamReader TcpReader;
    private Hashtable PlayerData;
    private List<Player> ListPlayer;
    private Queue<Enemy> QueueEnemy;
    private List<GameObject> GameObjectPlayer;
    //private List<GameObject> GameObjectEnemy;
    private Queue<string> DataReaded;
    private Thread ReadThread;


    private void OnEnable()
    {
        TcpReady = false;
        isPlaying = false;
        isReady = false;
        guard = false;
        Debug.Log("start tcp");
        DontDestroyOnLoad(gameObject);
        TcpConnectToServer(hostToConnect, TcpPortToConnect);
        PlayerData = new Hashtable();
        ListPlayer = new List<Player>();
        QueueEnemy = new Queue<Enemy>();
        GameObjectPlayer = new List<GameObject>();
        //GameObjectEnemy = new List<GameObject>();
        DataReaded = new Queue<string>();
    }
    // Use this for initialization
    void Start () {
        
        
    }

    private int sortPlayerScore(Player x, Player y) {
        return (int)(x.score - y.score);
    }

    public void updateScore(Text[] name, Text[] score)
    {
        ListPlayer.Sort(sortPlayerScore);
        for(int i = 0; i < ListPlayer.Count; i++)
        {
            name[i].text = ListPlayer[i].name + " : ";
            score[i].text = ListPlayer[i].score.ToString();
        }
    }

    public void setInt(string key, int value)
    {
        if (!PlayerData.ContainsKey(key))
        {
            PlayerData.Add(key, value);
        }
        else
        {
            PlayerData[key] = value;
        }
        
    }

    public void setString(string key, string value)
    {
        if (!PlayerData.ContainsKey(key))
        {
            PlayerData.Add(key, value);
        }
        else
        {
            PlayerData[key] = value;
        }
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
            //Debug.Log("LOGIN|" + name);
        }
        else
        {
            return false;
        }

        data = TcpRead();

        //TcpReady = false;
        //TcpConnectToServer(hostToConnect, TcpPortToConnect);

        //Debug.Log("test data: " + data);
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
    // set room and level
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

        //Debug.Log("JOIN_ROOM|" + name + "|" + (LevelSelect * NumberOfRoom + RoomIndex) + "|" + getInt("PlayerIndex"));

        TcpSend("JOIN_ROOM|" + name + "|" + (LevelSelect * NumberOfRoom + RoomIndex) + "|" + getInt("PlayerIndex"));
        data = TcpRead().Split('|');

        //Debug.Log(data[0] + data[1]);

        if (data[0].Equals("JOIN_ROOM") && int.Parse(data[1]) == 1)
        {
            temp = true;

            // create a new thread to handle read data
            ReadThread = new Thread(ReadData);
            ReadThread.Start();

        }
        return temp;
    }
    // read data from server message
    void ReadData()
    {
        try
        {
            while (true)
            {
                string data = TcpRead();
                //Debug.Log(data);

                string[] splited = data.Split('|');

                switch (splited[0])
                {
                    case "STATE":
                        
                        // for each player in state message
                        for (int i = 1; i < Mathf.Min(splited.Length, 4); i++)
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
                                        for (int j = 0; j < ListPlayer.Count; j++)
                                        {
                                            if (a.name == ListPlayer[j].name)
                                            {
                                                ListPlayer[j] = a;
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
                                            //Debug.Log("add " + a.name);
                                            ListPlayer.Add(a);
                                        }

                                    }
                                }
                            }

                        }

                        for (int i = 4; i < splited.Length; i++)
                        {
                            if (!splited[i].Equals("null"))
                            {
                                // make Enemy a from json an check
                                Enemy a = Enemy.CreateFromJSON(splited[i]);
                                if (a != null)
                                {
                                    QueueEnemy.Enqueue(a);
                                }
                            }

                        }
                        break;
                    case "SHOT":
                        // push data to stack for main thread and use guard
                        while (guard) ;
                        guard = true;
                        DataReaded.Enqueue(data);
                        guard = false;
                        // to do
                        break;
                    case "START":
                        // to do
                        OnStart();
                        break;
                    default:
                        // push data to stack for main thread and use guard
                        Debug.Log("data readed: " + data);
                        while (guard) ;
                        guard = true;
                        DataReaded.Enqueue(data);
                        guard = false;
                        // to do
                        break;
                }

            }
        }catch(Exception e)
        {
            Debug.Log(e);
        }

    }
    // handle data in DataReaded
    void handle()
    {
        if (!guard)
        {
            guard = true;
            while(DataReaded.Count > 0)
            {
                string[] data = DataReaded.Dequeue().Split('|');
                switch (data[0])
                {
                    case "SHOT":
                        GameObject playerShot = GameObject.Find(data[1]);
                        if (playerShot.active)
                        {
                            GunController gun = playerShot.GetComponent<GunController>();
                            gun.FireBullets();
                            Debug.Log("firebullet " + data[1]);
                        }
                        break;
                    case "ITEM":
                        switch (data[1])
                        {
                            case "SHIELD":
                                GameObject playerShield = GameObject.Find(data[3]);

                                if (playerShield != null)
                                {
                                    PlayerController playerscript = playerShield.GetComponent<PlayerController>();
                                    playerscript.CancelInvoke("SwithOffShield");
                                    playerscript.Invoke("SwithOffShield", 5);
                                    playerscript.shiledRenderObject.SetActive(true);
                                    playerscript.isShieldOn = true;
                                }

                                GameObject Shielditem = GameObject.Find("ShiledPickup" + "|" + data[2]);

                                if (Shielditem != null)
                                {
                                    Destroy(Shielditem);
                                }
                                break;
                                break;
                            case "HEALTH":
                                GameObject Healthitem = GameObject.Find("HealthPickup" + "|" + data[2]);

                                if (Healthitem != null)
                                {
                                    Destroy(Healthitem);
                                }
                                break;
                            case "GUN":
                                GameObject player = GameObject.Find(data[3]);
                                GameObject item = GameObject.Find("GunPickup" + "|" +data[2]);

                                if(player != null)
                                {
                                    PlayerController playerscript = player.GetComponent<PlayerController>();
                                    playerscript.IncreaseGun();
                                }

                                if (item != null)
                                {
                                    Destroy(item);
                                }
                                break;
                        }
                        break;
                    case "SHOTED":
                        HealthController enemyHealth = GameObject.Find(data[1]).GetComponent<HealthController>();
                        Debug.Log("decre health " + data[1] + " " + Mathf.Max(0, enemyHealth.HealthCount - 20));
                        if (enemyHealth != null)
                        {
                            
                            enemyHealth.HealthCount = Mathf.Max(0, enemyHealth.HealthCount - 20);
                            enemyHealth.CheckDeadOrAlive();
                        }
                        break;
                    case "DESTROY":
                        HealthController enemyDestroy = GameObject.Find(data[1]).GetComponent<HealthController>();

                        if (enemyDestroy != null)
                        {
                            enemyDestroy.HealthCount = 0;
                            enemyDestroy.CheckDeadOrAlive();
                        }
                        break;
                }
            }
            guard = false;
        }
    }

    // create player on scence when waiting
    public void CreatePlayerOnScence()
    {
        //Debug.Log("size " + ListPlayer.Count);
        foreach (Player player in ListPlayer)
        {
            //Debug.Log("find");
            GameObject p = GameObject.Find(player.name);

            if (p == null)
            {
                //Debug.Log("create");
                p = GameObject.Instantiate(PlayersPrefabs[player.plane - 1], player.position, Quaternion.identity) as GameObject;
                p.name = player.name;
                Text playerName = p.GetComponentInChildren<Text>();
                playerName.text = p.name;
                GameObjectPlayer.Add(p);
            }
        }
    }

    
    // spawn enemy if not spawned
    public void UpdateEnemyOnScence()
    {
        // if enemy has created so update health, else create enemy
        while(QueueEnemy.Count > 0)
        {
            Enemy enemy = QueueEnemy.Dequeue();
            GameObject enemyObject = GameObject.Find(enemy.id.ToString());
            bool isIn = false;

            if(enemyObject == null && isPlaying)
            {
                GameObject e = GameObject.Instantiate(EnemysPrefabs[enemy.plane - 1]) as GameObject;
                e.transform.position = enemy.position;
                e.name = enemy.id.ToString();
            }
        }
    }
    // call when enemy out of heal
    public void Destroy(string id)
    {
        Debug.Log("DESTROY|" + id + "|" + getString("PlayerName"));
        TcpSend("DESTROY|" + id + "|" + getString("PlayerName"));
    }

    //call when enemy time out
    public void DestroyTimeOut(string id)
    {
        TcpSend("DESTROY|" + id);
        Debug.Log("DESTROY|" + id);
    }

    //call when item time out
    public void DestroyItemTimeOut(string id)
    {
        TcpSend("DESTROYITEM|" + id);
        Debug.Log("DESTROYITEM|" + id);
    }

    // call when enemy get shot
    public void EnemyGetShot(string id)
    {
        TcpSend("SHOTED|" + id);
        Debug.Log("SHOTED|" + id);
    }

    // call when player get shot
    public void PlayerGetShot()
    {
        TcpSend("GETSHOTED|" + getString("PlayerName"));
        Debug.Log("GETSHOTED|" + getString("PlayerName"));
    }

    // send item was picked by player
    public void Item(string item, string id)
    {
        TcpSend("ITEM|" + item + "|" + id + "|" + getString("PlayerName"));
        Debug.Log("ITEM|" + item + "|" + id + "|" + getString("PlayerName"));
    }

    // send ready
    public void Ready()
    {
        isReady = true;
        TcpSend("READY|" + getString("PlayerName"));
    }

    public void Shot()
    {
        TcpSend("SHOT|" + getString("PlayerName"));
    }

    //send move to server
    public void Move(Vector3 position)
    {
        if (!IsPlaying())
            return;

        string move = "MOVE|" + position.x + "|" + position.y + "|" + position.z + "|" + getString("PlayerName");

        TcpSend(move);
    }
    // update move of player
    public void UpdateMove(GameObject player)
    {
        if (IsPlaying())
        {
            for (int i = 0; i < ListPlayer.Count; i++)
            {
                if (ListPlayer[i].enable && ListPlayer[i].name == player.name)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, ListPlayer[i].position, Time.deltaTime * 50.1f);
                    HealthController playerHealth = player.GetComponent<HealthController>();
                    playerHealth.HealthCount = ListPlayer[i].health;
                    playerHealth.UpdateHealthProgress(playerHealth.HealthCount);
                    playerHealth.CheckDeadOrAlive();
                }
                else if(!ListPlayer[i].enable && ListPlayer[i].name == player.name)
                {
                    player.SetActive(false);
                }
            }
        }
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
        // if playing update position
        //UpdateMove();
        handle();
        UpdateEnemyOnScence();
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
        public bool shield;
        public int gun;

        public static Player CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Player>(jsonString);
        }
    }

    [Serializable]
    public class Enemy
    {
        public long id;
        public int plane;
        public Vector3 position;

        public static Enemy CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Enemy>(jsonString);
        }
    }
}
