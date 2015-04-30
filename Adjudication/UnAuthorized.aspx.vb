Public Class UnAuthorized
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        'Dim dt As DataTable
        '============================================================================================
        If Not CInt(Session.Item("AccessLevel")) > 0 Then Response.Redirect("Login.aspx")

        If Page.IsPostBack = False Then
            If DataAccess.Save_ErrorLog(Session("LoginID"), "UnAuthorized Access to Requested Webpage") = True Then

            End If
        End If

    End Sub

End Class
