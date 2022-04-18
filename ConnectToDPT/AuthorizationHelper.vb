Imports System.Net.Http
Imports System.Runtime.Caching
Imports Newtonsoft.Json

Public Module AuthorizationHelper
    Async Function GetAccessToken() As Task(Of String)
        'Dim configuration = New ConfigurationBuilder().AddJsonFile("appsettings.json").Build()

        Dim cache As ObjectCache = MemoryCache.Default

        Dim cacheToken As Object = cache.Get("test.account3@zz04.tenant.sandbox.ogi-mobius.com_Qgg75Xw}w6;28=hd")

        Dim token As String = If(cacheToken Is Nothing, "", cacheToken.ToString())

        If String.IsNullOrEmpty(token) Then

            Dim tokenUri = "https://ui.sandbox.ogi-mobius.com/gateway/"
            Dim values = New Dictionary(Of String, String) From {
                {"client_id", "dynamic-pricing-api-sandbox"},
                {"grant_type", "password"},
                {"scope", "Mobius.API"},
                {"client_secret", "secret"},
                {"username", "tgsladmin"},
                {"password", "password1"},
                {"siteCode", "ZZ04"}
            }

            Dim httpClient = New HttpClient()

            Dim content = New FormUrlEncodedContent(values)
            content.Headers.Clear()
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded")
            Dim response = Await httpClient.PostAsync(New Uri($"{tokenUri}connect/token"), content)
            Dim responseResult As String = Await response.Content.ReadAsStringAsync()

            Dim tokenObject As AccessToken = JsonConvert.DeserializeObject(Of AccessToken)(responseResult)

            httpClient.Dispose()
            token = tokenObject.access_token

            Dim cachepolicy As New CacheItemPolicy With {
                .AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(tokenObject.expires_in)
            }
            cache.Add("test.account3@zz04.tenant.sandbox.ogi-mobius.com_Qgg75Xw}w6;28=hd", token, cachepolicy)
        End If
        Return token
    End Function




End Module

Public Class AccessToken
    Public access_token As String
    Public expires_in As Integer
    Public scope As String
    Public token_type As String
End Class

