using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using HostClientCommunication;
using Newtonsoft.Json.Linq;

namespace ChromeControlLibrary
{
    public static class ChromeControl
    {
        public static bool FocusWindow(int windowId)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"focuswindow {windowId}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                return response == $"Window Id: {windowId} focused";
            }

            return false;
        }

        public static bool CloseWindow(int windowId)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"closewindow {windowId}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                return response == $"Window {windowId} removed";
            }

            return false;
        }

        public static bool CloseTab(int tabId)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"closetab {tabId}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                return response == $"Tab {tabId} removed";
            }

            return false;
        }

        public static bool OpenWindow(string url, out int windowId, out int tabId)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"openwindow {url}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();
                
                JObject ro = JObject.Parse(responseObject["text"].ToString());

                var windowIdString = ro["windowId"];
                //string windowIdString = token.Value<string>("windowId");
                var tabIdString = ro["tabId"];

                windowId = int.Parse(windowIdString.ToString());
                tabId = int.Parse(tabIdString.ToString());
                return true;
            }

            windowId = -1;
            tabId = -1;
            return false;
        }

        public static bool OpenTab(int windowId, string url, out int tabId)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"opentab {windowId} {url}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();
                tabId = int.Parse(response);
                return true;
            }
            
            tabId = -1;
            return false;
        }

        public static bool ChangeTabUrl(int tabId, string url)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"changetaburl {tabId} {url}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                return response == "Tab URL updated";
            }
            
            return false;
        }

        public static bool GetWindowIds(out int[] ids)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"getwindows");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                try
                {
                    string[] stringIds = response.Split(',');

                    int[] finalIds = new int[stringIds.Length];
                    for (int i = 0; i < stringIds.Length; i++)
                    {
                        finalIds[i] = int.Parse(stringIds[i]);
                    }

                    ids = finalIds;
                    return true;
                }
                catch (Exception)
                {
                    ids = null;
                    return false;
                }
            }

            ids = null;
            return false;
        }

        public static bool GetTabIds(out int[] ids)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"gettabs");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                try
                {
                    string[] stringIds = response.Split(',');

                    int[] finalIds = new int[stringIds.Length];
                    for (int i = 0; i < stringIds.Length; i++)
                    {
                        finalIds[i] = int.Parse(stringIds[i]);
                    }

                    ids = finalIds;
                    return true;
                }
                catch (Exception)
                {
                    ids = null;
                    return false;
                }
            }

            ids = null;
            return false;
        }

        public static bool GetTabIdsInWindow(int windowId, out int[] ids)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"gettabsinwindow {windowId}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                try
                {
                    string[] stringIds = response.Split(',');

                    int[] finalIds = new int[stringIds.Length];
                    for (int i = 0; i < stringIds.Length; i++)
                    {
                        finalIds[i] = int.Parse(stringIds[i]);
                    }

                    ids = finalIds;
                    return true;
                }
                catch (Exception)
                {
                    ids = null;
                    return false;
                }
            }

            ids = null;
            return false;
        }

        public static bool GetUrl(int tabId, out string url)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"geturl {tabId}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                url = response;
                return !response.Contains("does not exist");
            }

            url = "";
            return false;
        }

        public static bool MoveWindow(int windowId, int xPos, int yPos)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"movewindow {windowId} {xPos} {yPos}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();
                
                return !response.Contains("invalid");
            }
            
            return false;
        }

        public static bool ChangeWindowState(int windowId, ChromeWindowStates windowState)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "dataDyneChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            ServerCommunication serverCommunication = new ServerCommunication(pipeClient);
            if (serverCommunication.ReadMessage() == "dataDyne Chrome Server")
            {
                serverCommunication.SendMessage($"changestate {windowId} {windowState.ToString().ToLower()}");

                JObject responseObject = serverCommunication.ReadMessageAsJObject();

                string response = responseObject["text"].ToString();

                return !response.Contains("invalid");
            }

            return false;
        }
    }

    public enum ChromeWindowStates
    {
        Normal,
        Minimized,
        Maximized,
        Fullscreen,
    }
}
