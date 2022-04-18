Imports Newtonsoft.Json

Public Class GatewayResponse
    <JsonProperty("access_token")>
    Public Property AccessToken As String

    <JsonProperty("expires_in")>
    Public Property ExpiresIn As Decimal

    <JsonProperty("token_type")>
    Public Property TokenType As String

    <JsonProperty("scope")>
    Public Property Scope As String
End Class
