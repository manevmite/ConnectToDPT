Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim quoteXML = "hey"

        Dim riskDataJson = "bye"


        ConnectionHelper.ConnectToDpt(quoteXML, riskDataJson)



    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click


        Await AuthorizationHelper.GetAccessToken()


    End Sub
End Class
