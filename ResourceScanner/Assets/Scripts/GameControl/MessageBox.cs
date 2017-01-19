using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : Singleton<MessageBox> {

    public struct Message
    {
        public string m_timeStamp;
        public string m_text;
        public Color m_colour;
    }

    [SerializeField]
    private int m_maxMessages = 5;
    [SerializeField]
    private Text[] m_timeStamps;
    [SerializeField]
    private Text[] m_messageBoxes;

    private Queue<Message> m_messageQueue = new Queue<Message>();
    private string m_lastMessage = "";
    private string m_lastTimeStamp = "";

    void Update()
    {
        // Set the queue to an array
        Message[] messages = m_messageQueue.ToArray();

        // Reverse message array
        for (int i = 0; i < messages.Length / 2; i++)
        {
            Message tmp = messages[i];
            messages[i] = messages[messages.Length - i - 1];
            messages[messages.Length - i - 1] = tmp;
        }

        // Display the message queue
        for (int i = 0; i < messages.Length; i++)
        {
            m_timeStamps[i].text = messages[i].m_timeStamp;
            m_messageBoxes[i].text = messages[i].m_text;
            m_messageBoxes[i].color = messages[i].m_colour;
        }
    }

    public void QueueUpMessage(string newMessage, Color messageColour, bool displayTimeStamp = true)
    {
        if (newMessage != m_lastMessage || System.DateTime.Now.ToString() != m_lastTimeStamp)
        {
            // Set date/time, message, and colour
            Message message = new Message();
            if(displayTimeStamp)
                message.m_timeStamp = "[" + System.DateTime.Now + "] ";
            else
                message.m_timeStamp = "";
            message.m_text = newMessage;
            message.m_colour = messageColour;

            // Save for future comparison
            m_lastTimeStamp = System.DateTime.Now.ToString();
            m_lastMessage = newMessage;

            // Queue up message
            m_messageQueue.Enqueue(message);

            if (m_messageQueue.Count > m_maxMessages)
            {
                m_messageQueue.Dequeue();
            }
        }
    }

}
