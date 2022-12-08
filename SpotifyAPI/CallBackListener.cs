using System.Net;
using System.Text.RegularExpressions;

namespace SpotifyAPI;

public class CallBackListener
{
    private HttpListener _httpListener = new HttpListener();

    public void StartListener()
    {
        _httpListener.Prefixes.Add("http://127.0.0.1:5000/callback/");
        _httpListener.Start();
        var _responseThread = new Thread(ResponseThread);
        _responseThread.Start();
    }
    
    private void ResponseThread()
    {
        HttpListenerContext context = _httpListener.GetContext();
        var respURL = context.Request.Url.ToString();

        byte[] _responseArray = 
            """
                <html>
                    <head>
                        <title>
                            Authentication Successful
                        </title>
                    </head>
                    <body>
                        The Spotify Authentication was Successful. You can now close this page.
                    </body>
                </html>
            """u8.ToArray();
        context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
        context.Response.KeepAlive = false;
        context.Response.Close();

        var code = Regex.Match(respURL, "(?<=code=).*").ToString();
        Authentication.GetCallback(code);
    }
}