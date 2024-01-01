using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Hackbox
{
    [RequireComponent(typeof(Host))]
    public class GameHost : MonoBehaviour
    {
        [SerializeField] private GameObject panelConnecting;
        [SerializeField] private GameObject panelLobby;
        [SerializeField] private GameObject panelDisconnected;
        [SerializeField] private GameObject[] waitingOnPlayers;
        [SerializeField] private GameObject minPlayersNotJoined;
        [SerializeField] private GameObject minPlayersJoined;
        [SerializeField] private GameObject[] panelAudience;

        [SerializeField] private TextMeshProUGUI[] roomCodeText;
        [SerializeField] private TextMeshProUGUI[] player1NameText;
        [SerializeField] private TextMeshProUGUI[] player2NameText;
        [SerializeField] private TextMeshProUGUI[] player3NameText;
        [SerializeField] private TextMeshProUGUI[] player4NameText;
        [SerializeField] private TextMeshProUGUI[] player5NameText;
        [SerializeField] private TextMeshProUGUI[] player6NameText;
        [SerializeField] private TextMeshProUGUI[] player7NameText;
        [SerializeField] private TextMeshProUGUI[] player8NameText;
        [SerializeField] private TextMeshProUGUI[] audienceCountText;

        [SerializeField] private State stateLobby;
        [SerializeField] private State stateGame;
        [SerializeField] private State stateEnd;
        [SerializeField] private State stateAudience;
        [SerializeField] private State stateDisconnected;

        private Host host;

        private Member[] players;
        private Member[] audienceMembers;

        private bool hideRoomCode;
        private bool madeFirstConnection;
        private int minPlayers;
        private int maxPlayers;
        private bool enableAudience;
        private bool gameHasStarted;

        void Awake()
        {
            host = GetComponent<Host>();
        }

        // Start is called before the first frame update
        void Start()
        {
            host.Disconnect();
            host.Connect(true);
            players = new Member[0];
            audienceMembers = new Member[0];
            minPlayers = 3;//PlayerPrefs.GetInt("MinPlayers");
            maxPlayers = 8;//PlayerPrefs.GetInt("MaxPlayers");
            enableAudience = true;//PlayerPrefs.GetInt("Audience") > 0;
            if (PlayerPrefs.HasKey("HideRoomCode")) hideRoomCode = PlayerPrefs.GetInt("HideRoomCode") > 0;
            else PlayerPrefs.SetInt("HideRoomCode", 0);
            minPlayersNotJoined.GetComponent<TextMeshProUGUI>().text = "Waiting for " + minPlayers + " more players.";
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            host.OnRoomConnected.AddListener(OnRoomConnected);
            host.OnRoomDisconnected.AddListener(OnRoomDisconnected);
            host.OnMemberJoined.AddListener(OnMemberJoined);
            host.OnMessage.AddListener(OnMessage);
        }

        private void OnDisable()
        {
            host.OnRoomConnected.RemoveListener(OnRoomConnected);
            host.OnRoomDisconnected.AddListener(OnRoomDisconnected);
            host.OnMemberJoined.RemoveListener(OnMemberJoined);
            host.OnMessage.RemoveListener(OnMessage);
        }

        private void OnRoomConnected(string roomCode)
        {
            panelConnecting.SetActive(false);
            panelLobby.SetActive(true);
            foreach (TextMeshProUGUI t in roomCodeText) t.text = roomCode;
            madeFirstConnection = true;
        }

        private void OnRoomDisconnected(string roomCode)
        {
            panelDisconnected.SetActive(true);
        }

        private void OnMemberJoined(Member member)
        {
            if (players.Length < maxPlayers && !gameHasStarted)
            {
                host.UpdateMemberState(member, stateLobby);
                waitingOnPlayers[players.Length].SetActive(false);
                TextMeshProUGUI[] text = new TextMeshProUGUI[0];
                switch (players.Length)
                {
                    case 0:
                        text = player1NameText;
                        break;
                    case 1:
                        text = player2NameText;
                        break;
                    case 2:
                        text = player3NameText;
                        break;
                    case 3:
                        text = player4NameText;
                        break;
                    case 4:
                        text = player5NameText;
                        break;
                    case 5:
                        text = player6NameText;
                        break;
                    case 6:
                        text = player7NameText;
                        break;
                    case 7:
                        text = player8NameText;
                        break;
                }
                foreach (TextMeshProUGUI t in text) t.text = member.Name;
                List<Member> list = new List<Member>(players);
                list.Add(member);
                players = list.ToArray();
                if (players.Length < minPlayers) minPlayersNotJoined.GetComponent<TextMeshProUGUI>().text = "Waiting for " + (minPlayers - players.Length) + " more players.";
                else if (players.Length == minPlayers)
                {
                    minPlayersNotJoined.SetActive(false);
                    minPlayersJoined.SetActive(true);
                }
            }
            else
            {
                host.UpdateMemberState(member, stateAudience);
                if (enableAudience)
                {
                    foreach (GameObject pa in panelAudience) pa.SetActive(true);
                    List<Member> list = new List<Member>(audienceMembers);
                    list.Add(member);
                    audienceMembers = list.ToArray();
                    foreach (TextMeshProUGUI t in audienceCountText) t.text = audienceMembers.Length + "";
                }
            }
            //BzzrPlayer newPlayer = new BzzrPlayer(member);
            //Players[member.UserID] = newPlayer;
            //_hackboxHost.UpdateMemberState(member, WaitingState.State);
            //OnPlayerJoined.Invoke(newPlayer);
        }

        private void OnMessage(Message message)
        {
            switch (message.Event)
            {
                case "buzz":
                    //Players[message.Member.UserID]
                    //OnBuzz.Invoke(newBuzz);
                    //State buzzState = new State(BuzzedState.State);
                    //buzzState.SetComponentText("Text", $"{buzzTime.TotalSeconds:0.0000}s");
                    //host.UpdateMemberState(message.Member, buzzState);
                    break;

                default:
                    break;
            }
        }

        public void SetHideRoomCode(bool h)
        {
            hideRoomCode = h;
            PlayerPrefs.SetInt("HideRoomCode", h ? 1 : 0);
        }

        public void StartGame()
        {
            foreach (Member player in players) host.UpdateMemberState(player, stateGame);
            gameHasStarted = true;
        }

        public void GameOver()
        {
            foreach (Member player in players) host.UpdateMemberState(player, stateEnd);
        }

        public void NewGame()
        {
            host.UpdateAllMemberStates(stateDisconnected);
            host.Disconnect();
            SceneManager.LoadScene("Game");
        }

        public void Quit()
        {
            host.UpdateAllMemberStates(stateDisconnected);
            host.Disconnect();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
