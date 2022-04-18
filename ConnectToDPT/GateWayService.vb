Imports System.Net.Http
Imports Newtonsoft.Json

Public Class GateWayService
    Implements IGatewayService
    Private _apiProxy As IApiProxy

    Public Sub New(apiProxy As IApiProxy)
        _apiProxy = apiProxy
    End Sub

    Public Async Function getAccessToken(baseUrl As String, clientId As String, clientSecret As String, tenantId As String, grantType As String, scope As String, subValue As String, B2B As String) As Task(Of String) Implements IGatewayService.getAccessToken
        Dim body As Dictionary(Of String, String) = New Dictionary(Of String, String)
        body.Add("client_id", clientId)
        body.Add("client_secret", clientSecret)
        body.Add("tenantid", tenantId)
        body.Add("grant_type", grantType)
        body.Add("scope", scope)
        body.Add("sub", subValue)
        body.Add("B2B", B2B)
        Dim TokenResult As HttpResponseMessage
        Dim TokenDes As GatewayResponse

        Try
            TokenResult = Await _apiProxy.PostAsync($"{baseUrl}", New Dictionary(Of String, String), body, True).ConfigureAwait(False)
            Dim TokenStr As String = Await TokenResult.Content.ReadAsStringAsync().ConfigureAwait(False)
            TokenDes = JsonConvert.DeserializeObject(Of GatewayResponse)(TokenStr)
        Catch ex As Exception
            Trace.TraceError(ex.ToString())
            Dim Message As String = $"Unable to get response from the gateway service. Response: {TokenResult}"
            Throw New OperationCanceledException(Message)
        End Try

        Return TokenDes.AccessToken
    End Function
End Class
