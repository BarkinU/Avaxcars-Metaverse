using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System;

namespace FirebaseWebGL.Examples.Database
{
    public class DatabaseHandler : MonoBehaviour
    {
        [Header("                           Singleton Variables")]
        private static DatabaseHandler instance = null;
        public static DatabaseHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ChatManager").AddComponent<DatabaseHandler>();
                }

                return instance;
            }
        }
        private void OnEnable()
        {
            instance = this;
        }
        private string currentPath = Strings.RegionalMessages;
        public TMP_InputField valueInputField;
        public GameObject messagePrefab;
        public GameObject ownerMessagePrefab;
        [SerializeField] private ScrollRect scrollRect;

        public Transform messagesContainer;
        public List<GameObject> allmessages = new List<GameObject>();
        private int pageIndex = 0;
        private int limit = 20;
        private int total = 100;
        private int messageIndex = 0;
        private bool isOwnMessage = false;
        [SerializeField] private Transform redButton;
        private bool isAnimationDone = false;
        public GameObject PanelMenu;
        public enum CountdownFormatting { DaysHoursMinutesSeconds, HoursMinutesSeconds, MinutesSeconds, Seconds };
        public Button EventMessagesButton;
        public bool isWriting = false;


        private void Start()
        {
            // // NetworkManager.Instance.GetUserCountry(FailCallback, UserCountry);
            // if (Application.platform != RuntimePlatform.WebGLPlayer)
            //     DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");

            // DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            SendMessage();
        }

        public void NewMessageSection()
        {
            currentPath = Strings.RegionalMessages + "/" + GameManager.Instance.userCountry;
            scrollRect.onValueChanged.AddListener(OnDragScroll);
            ListenForChildAdded();

        }

        public void GetJSON() =>
            FirebaseDatabase.GetJSON(currentPath, gameObject.name, "ConvertGettingMessageToText", "DisplayErrorObject");

        public void PostJSON() => FirebaseDatabase.PostJSON(currentPath, valueInputField.text, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");

        public void PushJSON(string messageJson)
        {
            FirebaseDatabase.PushJSON(currentPath, messageJson, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");
            isOwnMessage = true;
        }

        public void UpdateJSON() => FirebaseDatabase.UpdateJSON(currentPath, valueInputField.text,
            gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void DeleteJSON() =>
            FirebaseDatabase.DeleteJSON(currentPath, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void ListenForValueChanged() =>
            FirebaseDatabase.ListenForValueChanged(currentPath, gameObject.name, "DisplayData", "DisplayErrorObject");

        public void StopListeningForValueChanged() => FirebaseDatabase.StopListeningForValueChanged(currentPath, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void ListenForChildAdded() =>
            FirebaseDatabase.ListenForChildAdded(currentPath, gameObject.name, "ConvertListeningMessageToText", "DisplayErrorObject");


        public void StopListeningForChildAdded() => FirebaseDatabase.StopListeningForChildAdded(currentPath, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void ListenForChildChanged() =>
            FirebaseDatabase.ListenForChildChanged(currentPath, gameObject.name, "DisplayData", "DisplayErrorObject");

        public void StopListeningForChildChanged() => FirebaseDatabase.StopListeningForChildChanged(currentPath, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void ListenForChildRemoved() =>
            FirebaseDatabase.ListenForChildRemoved(currentPath, gameObject.name, "DisplayData", "DisplayErrorObject");

        public void StopListeningForChildRemoved() => FirebaseDatabase.StopListeningForChildRemoved(currentPath, gameObject.name, "DisplayInfo", "DisplayErrorObject");


        public void ToggleBooleanWithTransaction() =>
            FirebaseDatabase.ToggleBooleanWithTransaction(currentPath, gameObject.name, "DisplayInfo",
                "DisplayErrorObject");

        public void DisplayData(string data)
        {
            Debug.Log(data);
        }

        public void DisplayInfo(string info)
        {
            Debug.Log(info);
        }

        public void DisplayErrorObject(string error)
        {
            var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
            DisplayError(parsedError.message);
        }

        public void DisplayError(string error)
        {
            Debug.LogError(error);
        }

        public void ChangeTablePath(string incomingPath)
        {
            StopListeningForChildAdded();
            CleanAllMessages();
            switch (incomingPath)
            {
                case Strings.RegionalMessages:
                    incomingPath += "/" + GameManager.Instance.userCountry;
                    break;
                case Strings.EventMessages:
                    incomingPath += "/" + EventManager.Instance.selectedRoomID;
                    break;
            }
            currentPath = incomingPath;
            ListenForChildAdded();

        }

        public void ConvertSendedMessageToJson()
        {
            if (valueInputField.text != "")
            {
                Message message = new Message(GameManager.Instance.userInformation.data.user.username, valueInputField.text, "");
                string messageJson = JsonUtility.ToJson(message);
                PushJSON(messageJson);
            }
        }

        public void ConvertListeningMessageToText(string data)
        {
            Debug.Log("Message" + data);
            var json = JsonUtility.FromJson<MessageContent>(data);
            InitiateListenedMessage(json, false);
            Debug.Log("MessageConverted");
        }


        private void InitiateListenedMessage(MessageContent message, bool isOld)
        {
            Debug.Log(message);
            Debug.Log("MessageCreated");
            Debug.Log(allmessages.Count);

            if (isOld == false)
            {
                if (message.sender == GameManager.Instance.userInformation.data.user.username)
                {
                    var newMessage = Instantiate(ownerMessagePrefab, transform.position, Quaternion.identity);
                    newMessage.transform.SetParent(messagesContainer, false);
                    newMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message.sender;
                    newMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message.text;
                    long l = long.Parse(message.date);
                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddMilliseconds(l).ToLocalTime();
                    newMessage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + date;
                    //newMessage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = message.date;
                    allmessages.Add(newMessage);
                }
                else
                {
                    var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
                    newMessage.transform.SetParent(messagesContainer, false);
                    newMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message.sender;
                    newMessage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = message.text;
                    long l = long.Parse(message.date);
                    DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime date = start.AddMilliseconds(l).ToLocalTime();
                    newMessage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "" + date;
                    //newMessage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = message.date;
                    allmessages.Add(newMessage);
                }



                if (allmessages.Count > limit)
                {
                    messagesContainer.transform.GetChild(messageIndex).gameObject.SetActive(false);
                    messageIndex++;
                }


            }
            /*  else
              {
                  if (message.sender == GameManager.Instance.userInformation.data.user.username)
                  {
                      var newMessage = Instantiate(ownerMessagePrefab, transform.position, Quaternion.identity);
                      newMessage.transform.SetParent(messagesContainer, false);
                      newMessage.transform.SetSiblingIndex(0);
                      newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message.sender;
                      newMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message.text;
                  }
                  else
                  {
                      var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
                      newMessage.transform.SetParent(messagesContainer, false);
                      newMessage.transform.SetSiblingIndex(0);
                      newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message.sender;
                      newMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message.text;
                  }
              }*/

        }

        /* public void ConvertListeningMessageToText(string data)
         {
             Debug.Log("Message" + data);
             var json = JsonUtility.FromJson<MessageContent>(data);
             InitiateListenedMessage(json);
             Debug.Log("MessageConverted");
         }


         private void InitiateListenedMessage(MessageContent message)
         {
             Debug.Log(message);
             Debug.Log("MessageCreated");
             allmessages.Add(message);
             var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
             newMessage.transform.SetParent(messagesContainer, false);
             newMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message.sender;
             newMessage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = message.text;
             Debug.Log(message.sender);
             Debug.Log(message.text);

         }*/

        public void OnDragScroll(Vector2 value)
        {

            if (value.y >= 1f)
            {
                int totalPage = allmessages.Count / limit;
                Debug.Log(totalPage + "totalpage");
                pageIndex++;
                int remainNumber = allmessages.Count % limit;
                Debug.Log(remainNumber + "remain");
                int targetIndex = allmessages.Count - ((pageIndex + 1) * limit);
                if (pageIndex <= totalPage)
                {
                    if (pageIndex == totalPage)
                    {
                        for (int i = allmessages.Count - (limit * pageIndex); i >= targetIndex + limit-remainNumber; i--)
                        {
                            if (allmessages.Count > 0)
                            {
                                allmessages[i].SetActive(true);
                                Debug.Log("" + allmessages[i].name);
                            }
                        }
                    }
                    else
                    {
                        for (int i = allmessages.Count - (limit * pageIndex); i >= targetIndex; i--)
                        {
                            if (allmessages.Count > 0)
                            {
                                allmessages[i].SetActive(true);
                                Debug.Log("" + allmessages[i].name);
                            }

                        }
                    }

                    Debug.Log("Top of scroll bar");
                }
            }
        }

        public void CleanAllMessages()
        {
            pageIndex = 0;
            messageIndex = 0;
            allmessages.Clear();
            foreach (Transform child in messagesContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }


        public void ShowHideMenu()
        {
            if (PanelMenu != null)
            {
                Animator animator = PanelMenu.GetComponent<Animator>();
                if (animator != null)
                {
                    bool isOpen = animator.GetBool("Show");
                    animator.SetBool("Show", !isOpen);
                }
            }
        }

        public void SlideRedButton(GameObject button)
        {
            if (isAnimationDone == false)
            {
                isAnimationDone = true;
                StartCoroutine(Slide(new Vector2(button.transform.position.x, button.transform.position.y), 0.3f));
            }
        }
        IEnumerator Slide(Vector2 targetPosition, float duration)
        {
            float time = 0;
            Vector2 startPosition = redButton.transform.position;
            while (time < duration)
            {
                redButton.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            if (time >= duration)
            {
                isAnimationDone = false;
                redButton.transform.position = targetPosition;
            }
        }

        void UserCountry(string data)
        {
            DataGeoIP response = JsonUtility.FromJson<DataGeoIP>(data);
        }

        void FailCallback(string data)
        {
            var response = JsonUtility.FromJson<FailCallbackError>(data);
            print(data);
        }

        public void IFSelect()
        {
            isWriting = true;
        }
        public void IFDeSelect()
        {
            isWriting = false;
        }

        private void SendMessage()
        {
            if (isWriting == true)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ConvertSendedMessageToJson();
                }
            }
        }
    }
}