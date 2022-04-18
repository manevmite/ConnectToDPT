Imports System.Net.Http

Public Interface IApiProxy
    Function PostAsync(url As String, headers As Dictionary(Of String, String), body As Dictionary(Of String, String), Optional isMultiPartForm As Boolean = False) As Task(Of HttpResponseMessage)
    Function GetAsync(url As String, headers As Dictionary(Of String, String)) As Task(Of HttpResponseMessage)
End Interface
