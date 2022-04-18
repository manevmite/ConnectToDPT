Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Public Module ConnectionHelper

    Private _gatewayService As GateWayService
    Public Async Function ConnectToDpt(ByVal lstrQuote As String, ByVal riskDataJson As String) As Task(Of Boolean)

        Dim quotationData As New QuotationData With {
            .QuotationXML = lstrQuote,
            .RiskDataDto = riskDataJson
        }

        Dim token = Await AuthorizationHelper.GetAccessToken()

        Dim postData = JsonConvert.SerializeObject(quotationData)

        Dim request = WebRequest.Create("https://dpt-api-sandbox.azurewebsites.net/api/CalculationEngine/Evaluate")

        Try

            request.Timeout = 5000
            request.Headers.Add("Authorization", $"Bearer {token}")
            request.Method = "POST"
            Dim byteArray = Encoding.UTF8.GetBytes(postData)
            request.ContentType = "application/json"
            request.ContentLength = byteArray.Length
            Dim dataStream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader = New StreamReader(dataStream)
            Dim responseFromServer = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()

        Catch ex As Exception
            Dim aa = ex
        End Try

        Return True

    End Function



End Module

Public Class QuotationData
    Public RiskDataDto As String
    Public QuotationXML As String
End Class
