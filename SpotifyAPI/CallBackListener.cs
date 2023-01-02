using System.Net;
using System.Text.RegularExpressions;

namespace SpotifyAPI;

public class CallBackListener
{
    private static CallBackListener _callBackListener;
    public static CallBackListener Instance => _callBackListener ??= new CallBackListener();
    
    private HttpListener _httpListener = new HttpListener();

    private CallBackListener()
    {
        _httpListener.Prefixes.Add("http://127.0.0.1:5000/callback/");
        _httpListener.Start();
        var _responseThread = new Thread(ResponseThread);
        _responseThread.Start();
    }

    ~CallBackListener()
    {
        if (_httpListener.IsListening)
        {
            _httpListener.Close();
        }
    }
    
    private void ResponseThread()
    {
        HttpListenerContext context = _httpListener.GetContext();
        var respURL = context.Request.Url.ToString();
        var code = Regex.Match(respURL, "(?<=code=).*").ToString();
        
        byte[] _responseArray;
        if (code != string.Empty)
        {
            _responseArray = 
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
            
            Authentication.GetCallback(code);
        }
        else
        {
            _responseArray = 
                """
                <html>
                    <head>
                        <title>
                            Authentication Failed
                        </title>
                    </head>
                    <body>
                        The Spotify Authentication Failed.
                    </body>
                </html>
            """u8.ToArray();
        }
        context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
        context.Response.KeepAlive = false;
        context.Response.Close();
        _httpListener.Close();
        _callBackListener = null;
    }
}