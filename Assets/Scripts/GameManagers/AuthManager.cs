using UnityEngine;
using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Concurrent;

public class AuthManager : MonoBehaviour
{
    private const int PORT = 34115;
    private HttpListener listener;
    private Thread listenerThread;
    private volatile bool isListening = false;
    private ConcurrentQueue<string> tokenQueue = new ConcurrentQueue<string>();

    public bool IsAuthenticated => !string.IsNullOrEmpty(GetToken());

    private string storedToken = null;

    public void StartDiscordAuth()
    {
        if (!isListening)
        {
            StartLocalServer();
        }
        Application.OpenURL($"https://api.gisketch.com/auth/discord");
    }

    private void StartLocalServer()
    {
        listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{PORT}/");
        listenerThread = new Thread(ListenForToken);
        isListening = true;
        listenerThread.Start();
    }

    private void ListenForToken()
    {
        listener.Start();
        Debug.Log($"Listening on port {PORT}");

        while (isListening)
        {
            try
            {
                HttpListenerContext context = listener.GetContext();
                ThreadPool.QueueUserWorkItem(HandleRequest, context);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in listener thread: {e.Message}");
            }
        }

        listener.Stop();
        Debug.Log("Listener stopped");
    }

    private void HandleRequest(object state)
    {
        var context = (HttpListenerContext)state;
        var request = context.Request;
        var response = context.Response;

        // Add CORS headers
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

        if (request.HttpMethod == "OPTIONS")
        {
            // Preflight request. Reply successfully:
            response.AddHeader("Access-Control-Max-Age", "1728000");
            response.Close();
            return;
        }

        if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/auth")
        {
            using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
            {
                string body = reader.ReadToEnd();
                var data = JsonUtility.FromJson<TokenResponse>(body);
                storedToken = data.token;
                Debug.Log("Received token: " + storedToken);
            }

            string responseString = "Token received successfully";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        response.Close();
    }

    private void OnDestroy()
    {
        isListening = false;
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            listenerThread.Join();
        }
    }

    public string GetToken()
    {
        return storedToken;
    }

    [System.Serializable]
    private class TokenResponse
    {
        public string token;
        public string username;
    }
}