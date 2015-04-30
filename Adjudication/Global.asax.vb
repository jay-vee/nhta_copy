Imports System.Security.Principal
Imports System.Web.SessionState
Imports System.Web.Security
Imports System.Threading

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started        
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        '=== Fires upon attempting to authenticate the user ===
        Try
            If DataAccess.IsTestMode() = True Then
                Dim authCookie As HttpCookie = FormsAuthentication.GetAuthCookie(DataAccess.Set_TestSessionValues("PK_UserID"), False)   ' Create new cookie based on the existing forms authentication ticket
                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)                     ' Get the FormsAuthenticationTicket out of the encrypted cookie
                FormsAuthentication.SetAuthCookie(DataAccess.Set_TestSessionValues("PK_UserID"), True)                                                 ' Sets the Request.IsAuthenticated = True
                HttpContext.Current.Response.Cookies.Add(authCookie)
                'DO NOT REDIRECT IF TEST MODE: HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl, False)
            Else
                If Not HttpContext.Current.User Is Nothing Then
                    If HttpContext.Current.User.Identity.IsAuthenticated = False Then
                        FormsAuthentication.RedirectToLoginPage()           'redirect user to Login Page
                    Else
                        'Dim identity As FormsIdentity = DirectCast(HttpContext.Current.User.Identity, FormsIdentity)
                        'Dim authTicket As FormsAuthenticationTicket = identity.Ticket
                        'Dim sCookieData As String() = authTicket.UserData.Split("~")            '===  dummy split for now
                        'HttpContext.Current.User = New System.Security.Principal.GenericPrincipal(identity, sCookieData)
                    End If
                End If
            End If
        Catch ex As Exception
            FormsAuthentication.RedirectToLoginPage()           'redirect user to Login Page
        End Try
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
End Class