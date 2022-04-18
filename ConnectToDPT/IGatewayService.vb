Public Interface IGatewayService
    Function getAccessToken(url As String, clientId As String, clientSecret As String, tenantId As String, grantType As String, scope As String, subValue As String, B2B As String) As Threading.Tasks.Task(Of String)
End Interface
