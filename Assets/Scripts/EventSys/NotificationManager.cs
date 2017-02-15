using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLQJ
{
    /// <summary>
    /// 消息分发，解耦
    /// </summary>
    public class NotificationManager
    {
        public static NotificationManager Instance { get { return SingletonProvider<NotificationManager>.Instance; } }

        public delegate void MsgCallback(MessageObject eb);
        /// <summary>
        /// 回调队列
        /// </summary>
        private Dictionary<string, List<MsgCallback>> registedCallbacks = new Dictionary<string, List<MsgCallback>>();
        /// <summary>
        /// 延迟消息队列
        /// </summary>
        private readonly List<MessageObject> delayedNotifyMsgs = new List<MessageObject>();
        /// <summary>
        /// 主消息队列
        /// </summary>
        private readonly List<MessageObject> realCallbacks = new List<MessageObject>();
        private static bool isInCalling = false;

        public  void Init()
        {

        }

        public void Update()
        {
            lock (this)
            {
                if (realCallbacks.Count == 0)
                {
                    //主消息隊列處理完時,加入延時消息到主消息列表 
                    foreach (MessageObject eb in delayedNotifyMsgs)
                    {
                        realCallbacks.Add(eb);
                    }
                    delayedNotifyMsgs.Clear();
                    return;
                }
                //調用主消息處理隊列
                isInCalling = true;
                foreach (MessageObject eb in realCallbacks)
                {
                    if (registedCallbacks.ContainsKey(eb.MsgName))
                    {
                        for (int i = 0; i < registedCallbacks[eb.MsgName].Count; i++)
                        {
                            MsgCallback ecb = registedCallbacks[eb.MsgName][i];
                            if (ecb == null)
                            {
                                continue;
                            }
#if UNITY_EDITOR
                            ecb(eb);
#else
                            try
                            {
                                 ecb(eb);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("CallbackError:" + eb.MsgName + " : " + e.ToString());
                            }    
#endif
                        }
                    }
                    else
                    {
                        Debug.Log("MSG_ALREADY_DELETED:" + eb.MsgName);
                    }

                }
                realCallbacks.Clear();
            }
            isInCalling = false;
        }

        public void Reset()
        {
            Dictionary<string, List<MsgCallback>> systemMsg = new Dictionary<string, List<MsgCallback>>();
            foreach (KeyValuePair<string, List<MsgCallback>> item in this.registedCallbacks)
            {
                if (item.Key.StartsWith("_"))
                {
                    systemMsg.Add(item.Key, item.Value);
                }
            }
            this.registedCallbacks = systemMsg;
        }

        public void Destroy()
        {
            Reset();
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="msgCallback"></param>
        public void Subscribe(string msgName, MsgCallback msgCallback)
        {
            lock (this)
            {
                if (!registedCallbacks.ContainsKey(msgName))
                {
                    registedCallbacks.Add(msgName, new List<MsgCallback>());
                }
                {
                    //防止重复订阅消息回调
                    List<MsgCallback> list = registedCallbacks[msgName];
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Equals(msgCallback))
                        {
                            return;
                        }
                    }
                    list.Add(msgCallback);
                }

            }
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="msgName"></param>
        /// <param name="msgCallback"></param>
        public void UnSubscribe(string msgName, MsgCallback msgCallback)
        {
            lock (this)
            {
                if (!registedCallbacks.ContainsKey(msgName))
                {
                    return;
                }
                //Debug.Log(msgName + ":-s-" + registedCallbacks[msgName].Count);
                registedCallbacks[msgName].Remove(msgCallback);
                //Debug.Log(msgName + ":-e-" + registedCallbacks[msgName].Count);
            }
        }

        public void PrintMsg()
        {
            string content = "";
            foreach (KeyValuePair<string, List<MsgCallback>> registedCallback in registedCallbacks)
            {
                int total = registedCallback.Value.Count;
                if (total > 0)
                {
                    content += registedCallback.Key + ":" + total + "\n";
                    for (int i = 0; i < total; i++)
                    {
                        content += "\t" + registedCallback.Value[i].Method.Name + "--" + registedCallback.Value[i].Target + "\n";
                    }
                }
            }
        }

        /// <summary>
        /// 派发消息
        /// </summary>
        /// <param name="MsgName"></param>
        /// <param name="MsgParam"></param>
        public void Notify(string MsgName, params object[] MsgParam)
        {

            object msgValueParam = null;
            if (MsgParam != null)
            {
                if (MsgParam.Length == 1)
                {
                    msgValueParam = MsgParam[0];
                }
                else
                {
                    msgValueParam = MsgParam;
                }
            }


            lock (this)
            {
                if (!registedCallbacks.ContainsKey(MsgName))
                {
                    return;
                }
                if (isInCalling)
                {
                    delayedNotifyMsgs.Add(new MessageObject(MsgName, msgValueParam));
                }
                else
                {
                    realCallbacks.Add(new MessageObject(MsgName, msgValueParam));
                }
            }
        }
    }

    public class MessageObject
    {
        public object MsgValue;
        public string MsgName;

        public MessageObject()
        {
            MsgName = this.GetType().FullName;
        }

        public MessageObject(string msgName, object ev)
        {
            MsgValue = ev;
            MsgName = msgName;
        }
    }
}
