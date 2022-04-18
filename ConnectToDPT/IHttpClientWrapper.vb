Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

Public Interface IHttpClientWrapper

    Function GetAsync(url As String) As Task(Of HttpResponseMessage)
    Function PostAsync(url As String, content As HttpContent) As Task(Of HttpResponseMessage)
    ReadOnly Property DefaultHeaders As HttpRequestHeaders

End Interface
