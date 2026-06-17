using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [Header("입력 필드 연결")] public TMP_InputField emailInput;
    public TMP_InputField pwInput;
    public TMP_InputField usernameInput; //회원가입 시에만 사용

    [Header("안내 텍스트")] public TMP_Text logText;

    [Header("서버 주소")] public string serverBaseUrl = "https://barn-countable-shine.ngrok-free.dev";

    public void OnClickLogin() => StartCoroutine(LoginRequest());
    public void OnClickRegister() => StartCoroutine(RegisterRequest());

    //회원가입
    IEnumerator RegisterRequest()
    {
        string registerUrl = "https://barn-countable-shine.ngrok-free.dev/signup";

        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("email", emailInput.text);
        form.AddField("password", pwInput.text);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(registerUrl, form))
        {
            webRequest.SetRequestHeader("ngrok-skip-browser-warning", "true");
            webRequest.SetRequestHeader("User-Agent", "UnityPlayer");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                logText.text = "회원가입 성공!";
                Debug.Log("서버 응답: " + webRequest.downloadHandler.text);
            }
            else
            {
                logText.text = "회원가입 실패! 코드: " + webRequest.responseCode;
                Debug.LogError("에러 상세: " + webRequest.error);
                Debug.LogError("바디: " + webRequest.downloadHandler.text);
            }
        }
    }

    //로그인 요청
    IEnumerator LoginRequest()
    {
        string loginUrl = "https://barn-countable-shine.ngrok-free.dev/login"; 

        WWWForm form = new WWWForm();
        
        form.AddField("email", emailInput.text);
        form.AddField("password", pwInput.text);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(loginUrl, form))
        {
            webRequest.SetRequestHeader("ngrok-skip-browser-warning", "true");
            webRequest.SetRequestHeader("User-Agent", "UnityPlayer");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                logText.text = "로그인 성공!";
                // 성공 시 씬 이동
            }
            else
            {
                logText.text = "로그인 실패:\n" + webRequest.downloadHandler.text;
            }
        }
    }
}