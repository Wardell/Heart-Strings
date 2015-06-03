using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Viral;
using UnityEditor;

   public class LevelManager : MonoBehaviour
    {
        public GameObject goal;
        public List<Enemy> enemies = new List<Enemy>();
        public Text timer;
        private List<EnemyMode> attackPattern = new List<EnemyMode>();
        private List<MovingPlatform> platforms = new List<MovingPlatform>();
        private List<Transform> spawnPoints = new List<Transform>();
        private List <float> tempo;
        [SerializeField]
        private float tempoLevel;
        public float timeElaps = 0;
        private bool StopTimer = true;
        private int SpawnRate = 1;
        private int SpawnTime = 5; //seconds
        private bool hasSpawned = false;
        public bool SlowDownSpawn = false;
        private float SlowDownTimer = 0.0f;
        private float chillOutFactor = 10;
        float mins;
        float oldMin = 0;
        public string levelName;
        private string Song;
        string battleSheetName;
        private int beatCounter = 0;
        public BattleSheet sheet;
        [SerializeField]
        int maxEnemies = 20;
        public float averageSignal = 0;
        public int enemiesOnBoard = 0;
        public bool canSpawn = false;
        CameraFollow camera;
        public string OpenFile()
        {

       
          string path = EditorUtility.OpenFilePanel("Select a music file", @".\", "mp3");
          GetComponent<AudioPlayer>().filepath = path;
          return @""+path;
        }

        void Awake()
        {


            Song = OpenFile();
            levelName = "Gardeia";
            string destination = @".\assets\BattlePatterns\" + levelName + ".xml";
            PrepBattleScore(Song, destination);
            camera = GameObject.FindObjectOfType<CameraFollow>();
            camera.tempo = sheet.BPM;
            Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            p.hypertimes = sheet.BeatTimeStamp;
            p.tempo = tempo;
            for (int i = 0; i < sheet.SignalChain.Count; i++)
            {
                averageSignal += sheet.SignalChain[i];
            }

            averageSignal = averageSignal / sheet.SignalChain.Count;
            UnityEngine.Debug.Log("Average Signal : " + averageSignal);
             //foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy Spawn"))
             //{
             //    spawnPoints.Add(obj.transform);
             //}
             //foreach(MovingPlatform m in GameObject.FindObjectsOfType<MovingPlatform>())
             //{
             //    m.platformSpeed = tempoLevel;
             //    m.sheet = sheet;
             //    m.aveSignal = averageSignal;
             //    platforms.Add(m);
             //} 
          
             foreach (Spawner s in GameObject.FindObjectsOfType<Spawner>())
             {
                 s.aveSignal = averageSignal;
                 s.sheet = sheet;
             }

             EntitySpawner es = GameObject.FindObjectOfType<EntitySpawner>();
             es.aveSignal = averageSignal;
             es.sheet = sheet;
             es.Moves = attackPattern;
             //for (int i = 0; i < enemies.Count; i++)
             //{
             //    enemies[i].Moves = attackPattern;
             //    enemies[i].sheet = sheet;
             //}
        }

        void FindTempoLevel(float tempo)
        {
            if (tempo < 90)
            {
                tempoLevel = 1f;
            }
            else if (tempo >= 90 && tempo < 140)
            {
                tempoLevel = 3.0f;
            }
            else if (tempo >= 140 && tempo < 180)
            {
                tempoLevel = 5f;
            }
            else if (tempo >= 180 && tempo < 250)
            {
                tempoLevel = 7f;
            }
            else
            { 
                tempoLevel = 10f;
            }
        }

        void oldSignalFinder()
        {
            for (int i = 0; i < sheet.SignalChain.Count; i++)
            {
                int next;
                if ((next = i + 1) < sheet.SignalChain.Count)
                {
                    float max = Mathf.Max(sheet.SignalChain[i], sheet.SignalChain[next]);
                    float min = Mathf.Min(sheet.SignalChain[i], sheet.SignalChain[next]);
                    //float max = sheet.SignalChain[i];
                    //float min = sheet.SignalChain[next];
                    float percentage = max  != 0 ? (min / max) * 100 : 0;
                    attackPattern.Add(FindMode(percentage));
                }
            }
        }
        void PrepBattleScore(string fileSelected, string battleSheetLocation)
        {
            Process startup = new Process();
            UnityEngine.Debug.Log("File Selected: "+ fileSelected);
            UnityEngine.Debug.Log("Destination of battlesheet: " + battleSheetLocation);
            battleSheetName = Path.GetFileNameWithoutExtension(fileSelected) + ".xml";
            startup.StartInfo.FileName = @"G:\Game projects\Heart Strings\ViralChords\Beat_Detector\bin\Release\Beat_Detector.exe";
            startup.StartInfo.Arguments = "\"" + fileSelected + "\" \"" + battleSheetLocation + "\"";
            startup.Start();
            startup.WaitForExit();
            XmlSerializer serializer = new XmlSerializer(typeof(BattleSheet));
            FileStream stream = new FileStream(battleSheetLocation, FileMode.Open);
            sheet = (BattleSheet)serializer.Deserialize(stream);           
            tempo = sheet.BPM;
            oldSignalFinder();
            for (int i = 0; i < tempo.Count; i++)
            {
                if (tempo[i] > 1000)
                {
                    tempo[i] = tempo[i] / chillOutFactor;
                }
            }
            FindTempoLevel(tempo[0]);        
        }
        void Start()
        {
            StopTimer = false;

        }

        EnemyMode FindMode(float f)
        {
            if (f < 10)
            {
                return EnemyMode.Repeat;
            }
            else if (f >= 10 && f < 40)
            {
                return EnemyMode.Minor;
            }
            else if (f >= 40 && f < 70)
            {
                return EnemyMode.Moderate;
            }
            else if (f >= 70)
            {
                return EnemyMode.Drastic;
            }
            else return EnemyMode.Repeat;
        }

        public void PauseTimer()
        {
            StopTimer = true;
        }

        public void UnPauseTimer()
        {
            StopTimer = false;
        }
        private void UpdateTimer()
        {
            timeElaps += Time.deltaTime;
            mins = Mathf.FloorToInt(timeElaps / 60);
            float secs = Mathf.FloorToInt(timeElaps  - mins *60.0f);
            float milisecs = (timeElaps * 1000) % 100;
            timer.text = string.Format("{0:00} : {1:00} : {2:000}",mins,secs,milisecs);
        }
     
       
        void Update()
        {

            if (!StopTimer)
            {
                timeElaps += Time.deltaTime;
                UpdateTimer();

                if (Mathf.Abs(sheet.BeatTimeStamp[beatCounter] - timeElaps) <= .1f)
                {
                    beatCounter++;
                }

               
                if(Mathf.FloorToInt(timeElaps /60.0f) <tempo.Count)
                FindTempoLevel(tempo[Mathf.FloorToInt(timeElaps /60.0f)]);
                               
            }

            
         
            
        }


        void FixedUpdate()
        {
               
        }

        int FindIntensity(EnemyMode e)
        {
            switch(e)
            {
                case EnemyMode.Minor:
                    return 1;
                case EnemyMode.Moderate:
                    return 2;
                case EnemyMode.Drastic:
                    return 3;
                default:
                    return 0;
            }
        }
    }

