Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text
Imports System.Threading.Tasks
Imports System.Collections.Generic

Public Class ApiProxy
    Implements IApiProxy
    Dim httpClient As IHttpClientWrapper

    Public Sub New(httpClient As IHttpClientWrapper)
        Me.httpClient = httpClient
    End Sub


    Private Shared Sub ApplyHeaders(headers As Dictionary(Of String, String), httpClient As IHttpClientWrapper)
        For Each header As KeyValuePair(Of String, String) In headers
            httpClient.DefaultHeaders.Add(header.Key, header.Value)
        Next header
    End Sub

    Private Shared Function GetContent(body As Dictionary(Of String, String), isMultiPartForm As Boolean) As HttpContent
        If (isMultiPartForm) Then
            Dim content As MultipartFormDataContent = New MultipartFormDataContent(Guid.NewGuid().ToString())
            For Each element As KeyValuePair(Of String, String) In body
                content.Add(New StringContent(element.Value), element.Key)
            Next element
            Return content
        Else
            Return New StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
        End If
    End Function

    Public Async Function PostAsync(url As String, headers As Dictionary(Of String, String), body As Dictionary(Of String, String), Optional isMultiPartForm As Boolean = False) As Task(Of HttpResponseMessage) Implements IApiProxy.PostAsync
        Try
            ApplyHeaders(headers, httpClient)
            Using content As HttpContent = GetContent(body, isMultiPartForm)
                Dim responseResult As HttpResponseMessage = Await httpClient.PostAsync(url, content).ConfigureAwait(False)

                If responseResult.IsSuccessStatusCode Then
                    Return responseResult
                Else
                    Throw New Exception($"Failed to execute the api call: {responseResult.RequestMessage.RequestUri} - {responseResult.StatusCode} - {responseResult.ReasonPhrase}")
                End If

            End Using
        Catch ex As Exception
            Trace.TraceError("Error in TCAS_Objetcs.ApiProxy.PostAsync:" & ex.ToString())
            Throw New Exception("Failed to execute the api call", ex)
        End Try
    End Function

    Public Async Function GetAsync(url As String, headers As Dictionary(Of String, String)) As Task(Of HttpResponseMessage) Implements IApiProxy.GetAsync
        Try
            ApplyHeaders(headers, httpClient)
            Dim response As HttpResponseMessage = Await httpClient.GetAsync(url).ConfigureAwait(False)

            If response.IsSuccessStatusCode Then
                Return response
            Else
                Throw New Exception($"Failed to execute the api call: {response.RequestMessage.RequestUri} - {response.StatusCode} - {response.ReasonPhrase}")
            End If
        Catch ex As Exception
            Throw New Exception("Failed to execute the api call", ex)
        End Try
    End Function
End Class
