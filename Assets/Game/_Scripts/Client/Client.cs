using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour {

    public GameObject[] PlayersPrefabs;
    public GameObject[] EnemysPrefabs;

    private GameObject PlayerSelection, LevelSelection, RoomSelection,  Loading, PlayerMesh, mainMenu;
    private GameObject Notification;
    private GameObject CantConnect;

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
    private List<bool> BoolPlayer;
    private Queue<Enemy> QueueEnemy;
    //private List<GameObject> GameObjectPlayer;
    //private List<GameObject> GameObjectEnemy;
    private Queue<string> DataReaded;
    private Thread ReadThread;
    private bool flags;


    private void OnEnable()
    {
        TcpReady = false;
        isPlaying = false;
        isReady = false;
        guard = false;
        //Debug.Log("start tcp");
        DontDestroyOnLoad(gameObject);
        TcpConnectToServer(hostToConnect, TcpPortToConnect);
        PlayerData = new Hashtable();
        ListPlayer = new List<Player>();
        BoolPlayer = new List<bool>();
        QueueEnemy = new Queue<Enemy>();
        //GameObjectPlayer = new List<GameObject>();
        //GameObjectEnemy = new List<GameObject>();
        DataReaded = new Queue<string>();
        ReadThread = new Thread(ReadData);
        ReadThread.Start();
        flags = true;
        
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
        for(int i = 0; i < Mathf.Min(ListPlayer.Count, 3); i++)
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

    public void remove(object key)
    {
        PlayerData.Remove(key);
    }


    private bool TcpConnectToServer(string host, int port)
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
            
            CantConnect = GameObject.Find("MainMenu_UI").transform.Find("CantConnect").gameObject;
            CantConnect.SetActive(true);
            //Debug.Log("Socket error " + e.Message);
        }

        return TcpReady;
    }

    public void TcpConnectToServer()
    {
        CantConnect = GameObject.Find("MainMenu_UI").transform.Find("CantConnect").gameObject;
        CantConnect.SetActive(false);
        try
        {
            TcpSocket = new TcpClient(hostToConnect, TcpPortToConnect);
            TcpStream = TcpSocket.GetStream();
            TcpWriter = new StreamWriter(TcpStream);
            TcpReader = new StreamReader(TcpStream);
            TcpReady = true;
        }
        catch (Exception e)
        {
            CantConnect.SetActive(true);
            //Debug.Log("Socket error " + e.Message);
        }

        //return TcpReady;
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
                TcpSend("QUITGAME|" + getString("PlayerName"));
                //Debug.Log("quit game");
            }

            //if (ReadThread.IsAlive)
            //{
            //    ReadThread.Abort();
            //}
            
            TcpWriter.Close();
            TcpReader.Close();
            TcpSocket.Close();
            TcpReady = false;
            
        }
    }
    // login execute
    public void login(string name)
    {
        
        string data;
        if (name.Equals(""))
            return;
        clientName = name;
        TcpSend("LOGIN|" + name);

    }

    public void closeNotification()
    {
        Notification = GameObject.Find("MainMenu_UI").transform.Find("Notification").gameObject;
        Notification.SetActive(false);
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
    public void joinRoom(int RoomIndex)
    {
        string[] data;
        string name = getString("PlayerName");
        int LevelSelect = getInt("LevelSelect");
        int NumberOfRoom = getInt("Level" + LevelSelect);

        //Debug.Log("JOIN_ROOM|" + name + "|" + (LevelSelect * NumberOfRoom + RoomIndex) + "|" + getInt("PlayerIndex"));
        TcpSend("JOIN_ROOM|" + name + "|" + (LevelSelect * NumberOfRoom + RoomIndex) + "|" + getInt("PlayerIndex"));
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
                //if (data == null) continue;
                string[] splited = data.Split('|');

                switch (splited[0])
                {
                    case "STATE":

                        List<Player> raw = new List<Player>();

                        for (int i = 1; i < 4; i++)
                        {
                            if (!(splited[i].Equals("null")))
                            {
                                Player a = Player.CreateFromJSON(splited[i]);
                                raw.Add(a);
                            }
                            
                        }

                        // for each player in state message
                        for (int i = 0; i < raw.Count; i++)
                        {
                            bool isIn = false;
                            for (int j = 0; j < ListPlayer.Count; j++)
                            {
                                if (ListPlayer[j].name.Equals(raw[i].name))
                                {
                                    ListPlayer[j] = raw[i];
                                    isIn = true;
                                    break;
                                }
                            }
                            //Debug.Log("isin " + isIn );

                            if (!isIn)
                            {
                                //Debug.Log("add " + a.name);
                                //raw[i].enable = true;
                                ListPlayer.Add(raw[i]);
                          
                            }
                        }

                        //Debug.Log("raw " + raw.Count + " list player " + ListPlayer.Count);


                        for (int i = 0; i < ListPlayer.Count; i++)
                        {
                            bool isin = false;
                            for(int j = 0; j < raw.Count; j++)
                            {
                                if (ListPlayer[i].name.Equals(raw[j].name))
                                {
                                    isin = true;
                                    break;
                                }
                            }
                            if (!isin)
                            {
                                ListPlayer.Remove(ListPlayer[i]);
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
                        //Debug.Log("SHOTTTTTTTT");
                        DataReaded.Enqueue(data);
                        // to do
                        break;
                    case "START":
                        // to do
                        OnStart();
                        break;
                    default:
                        // push data to stack for main thread and use guard
                       
                        DataReaded.Enqueue(data);
                        // to do
                        break;
                }

            }
        }catch(Exception e){
            Debug.Log(e);
            return;
         
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
                            //Debug.Log("firebullet " + data[1]);
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
                        //Debug.Log("decre health " + data[1] + " " + Mathf.Max(0, enemyHealth.HealthCount - 20));
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
                    case "JOIN_ROOM":
                        //Debug.Log(data[1]);
                        if (int.Parse(data[1]) == 1)
                        {
                            //Debug.Log("joinroom");
                            Loading = GameObject.Find("MainMenu_UI").transform.Find("Loading").gameObject;
                            RoomSelection = GameObject.Find("MainMenu_UI").transform.Find("RoomSelection").gameObject;
                            Loading.SetActive(true);
                            RoomSelection.SetActive(false);
                        }
                        else
                        {
                            Notification = GameObject.Find("MainMenu_UI").transform.Find("Notification").gameObject;
                            Text text = Notification.GetComponentInChildren<Text>();
                            text.text = "Room full, try another";
                            Notification.SetActive(true);
                        }
                        break;
                    case "LOGIN":
                        //Debug.Log(data[1]);
                        if (int.Parse(data[1]) == 1)
                        {
                            setString("PlayerName", clientName);
                            setRoomAndLevel(data);
                        }
                        else
                        {
                            Notification = GameObject.Find("MainMenu_UI").transform.Find("Notification").gameObject;
                            Text text = Notification.GetComponentInChildren<Text>();
                            text.text = "Login fail, try another name";
                            Notification.SetActive(true);
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
            GameObject p;

            if (GameObject.Find(player.name) == null && !isPlaying)
            {
                //Debug.Log("create:" + player.name);
                p = GameObject.Instantiate(PlayersPrefabs[player.plane - 1], player.position, Quaternion.identity) as GameObject;
                p.name = player.name;
                Text playerName = p.GetComponentInChildren<Text>();
                playerName.text = p.name;
                //GameObjectPlayer.Add(p);
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
    public void QuitRoom()
    {
        isPlaying = false;
        ListPlayer.Clear();
        BoolPlayer.Clear();
        //GameObjectPlayer.Clear();
        //Debug.Log("QUITROOM|" + getString("PlayerName"));
        TcpSend("QUITROOM|" + getString("PlayerName"));
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
        //Debug.Log("DESTROY|" + id);
    }

    //call when item time out
    public void DestroyItemTimeOut(string id)
    {
        TcpSend("DESTROYITEM|" + id);
        //Debug.Log("DESTROYITEM|" + id);
    }

    // call when enemy get shot
    public void EnemyGetShot(string id)
    {
        TcpSend("SHOTED|" + id);
        //Debug.Log("SHOTED|" + id);
    }

    // call when player get shot
    public void PlayerGetShot()
    {
        TcpSend("GETSHOTED|" + getString("PlayerName"));
        //Debug.Log("GETSHOTED|" + getString("PlayerName"));
    }

    // send item was picked by player
    public void Item(string item, string id)
    {
        TcpSend("ITEM|" + item + "|" + id + "|" + getString("PlayerName"));
        //Debug.Log("ITEM|" + item + "|" + id + "|" + getString("PlayerName"));
    }

    // send ready
    public void Ready()
    {
        isReady = true;
        TcpSend("READY|" + getString("PlayerName"));
    }

    public void Shot()
    {
        //Debug.Log("SHOT");
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
            bool isin = false;
            for (int i = 0; i < ListPlayer.Count; i++)
            {
                if (ListPlayer[i].name.Equals(player.name))
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, ListPlayer[i].position, Time.deltaTime * 50.1f);
                    HealthController playerHealth = player.GetComponent<HealthController>();
                    playerHealth.HealthCount = ListPlayer[i].health;
                    //Debug.Log("health check: " + ListPlayer[i].health + " name :" + ListPlayer[i].name);
                    playerHealth.UpdateHealthProgress(playerHealth.HealthCount);
                    playerHealth.CheckDeadOrAlive();
                    isin = true;
                    break;
                }
            }

            if (!isin)
            {
                HealthController playerHealth = player.GetComponent<HealthController>();
                playerHealth.HealthCount = 0;
                //Debug.Log("health check: " + ListPlayer[i].health + " name :" + ListPlayer[i].name);
                playerHealth.CheckDeadOrAlive();
            }
        }
        else
        {
            bool isin = false;
            for (int i = 0; i < ListPlayer.Count; i++)
            {
                if (ListPlayer[i].name.Equals(player.name))
                {

                    HealthController playerHealth = player.GetComponent<HealthController>();
                    playerHealth.HealthCount = ListPlayer[i].health;
                    //Debug.Log("health check: " + ListPlayer[i].health + " name :" + ListPlayer[i].name);
                    playerHealth.UpdateHealthProgress(playerHealth.HealthCount);
                    playerHealth.CheckDeadOrAlive();
                    isin = true;
                    break;
                }
            }

            if (!isin)
            {
                HealthController playerHealth = player.GetComponent<HealthController>();
                playerHealth.HealthCount = 0;
                //Debug.Log("health check: " + ListPlayer[i].health + " name :" + ListPlayer[i].name);
                playerHealth.CheckDeadOrAlive();
            }
        }
    }

    public bool checkEndGame()
    {
        int endGame = 0;
        if (IsPlaying())
        {
            for (int i = 0; i < ListPlayer.Count; i++)
            {
                if (ListPlayer[i].health <= 0)
                {
                    endGame++;
                }

            }

        }

        Debug.Log(endGame + " " + ListPlayer.Count);
        if (endGame == ListPlayer.Count)
        {
            //Debug.Log(endGame);
            //Debug.Log(ListPlayer.Count);
            return true;
        }
        else
        {
            return false;
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
        if (isPlaying)
        {
            UpdateEnemyOnScence();
        }
        
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
