using UnityEngine;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class DevEventManager : MonoBehaviour
{
    private ClientWebSocket webSocket;
    private CancellationTokenSource cancellationTokenSource;
    
    private string webSocketUrl = "wss://barn-countable-shine.ngrok-free.dev/ws/game"; 

    async void Start()
    {
        //로그인할 때 저장했던 이메일 가져오기
        string loggedInEmail = PlayerPrefs.GetString("UserEmail", "unknown");

        // 주소 뒤를 유저 이메일로 리셋
        webSocketUrl = $"wss://barn-countable-shine.ngrok-free.dev/ws/game?email={loggedInEmail}";
        
        if (webSocketUrl.StartsWith("https"))
        {
            webSocketUrl = webSocketUrl.Replace("https", "wss");
        }
        else if (webSocketUrl.StartsWith("http"))
        {
            webSocketUrl = webSocketUrl.Replace("http", "ws");
        }

        cancellationTokenSource = new CancellationTokenSource();
        await ConnectToServer();
    }

    async Task ConnectToServer()
    {
        try
        {
            webSocket = new ClientWebSocket();
            
            //통과용 헤더
            webSocket.Options.SetRequestHeader("ngrok-skip-browser-warning", "true");
            webSocket.Options.SetRequestHeader("User-Agent", "UnityPlayer");

            Debug.Log($"웹소켓 서버 연결 시도 중... ({webSocketUrl})");
            Uri serverUri = new Uri(webSocketUrl);
            
            await webSocket.ConnectAsync(serverUri, cancellationTokenSource.Token);
            Debug.Log("서버와 실시간 억까 통로 연결 완벽 완료!");

            //연결 성공했으니 실시간으로 수신 대기 굴리기
            _ = ReceiveLoop();
        }
        catch (Exception e)
        {
            Debug.LogError($"웹소켓 연결 실패: {e.Message}");
        }
    }

    async Task ReceiveLoop()
    {
        var buffer = new byte[1024 * 4];

        try
        {
            while (webSocket.State == WebSocketState.Open && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    Debug.Log("서버가 소켓 연결을 종료했습니다.");
                    break;
                }

                //서버가 보낸 텍스트 읽기
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim();
                
                if (!string.IsNullOrEmpty(message))
                {
                    //메인 스레드에서 유니티 함수가 안전하게 돌 수 있도록 처리
                    ProcessEvent(message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"실시간 수신 중 에러 발생: {e.Message}");
        }
    }

    void ProcessEvent(string eventType)
    {
        Debug.Log($"웹 패널에서 날아온 실시간 명령: {eventType}");

        switch (eventType)
        {
            case "SPAWN_WALL":
                Debug.Log("[억까] 플레이어 진로 차단용 벽 레이아웃 오픈!");
                break;

            case "DISABLE_JUMP":
                Debug.Log("[억까] 점프 기능을 일시 봉인합니다!");
                break;

            case "DELETE_ITEMS":
                Debug.Log("억까] 인벤토리 아이템을 증발시킵니다!");
                break;

            case "SPAWN_DEATH_MONSTER":
                Debug.Log("[억까] 즉사급 몬스터를 스폰합니다!");
                break;

            case "RANDOM_TELEPORT":
                Debug.Log("[억까] 위치 강제 이동 감행!");
                break;
        }
    }

    private async void OnApplicationQuit()
    {
        if (cancellationTokenSource != null)
            cancellationTokenSource.Cancel();

        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "App Quit", CancellationToken.None);
            webSocket.Dispose();
            Debug.Log("유니티 종료로 인한 소켓 안전 차단.");
        }
    }
}