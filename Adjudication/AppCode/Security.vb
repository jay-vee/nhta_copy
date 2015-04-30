Imports System.Security.Principal

Public Class Security

    '    Dim user As IIdentity = New GenericIdentity("Admin", "CustomAuthentication")

    '    'authenticate the user, you can collect the appropriate list of roles and create a new GenericPrincipal for the identity:

    '    Dim principal As GenericPrincipal
    '    Dim roles() As String = GetUserRoles(user)

    'principal = New GenericPrincipal(user, roles)
    'Thread.CurrentPrincipal = principal

    Public Shared Sub Expire_All_Cookies()
        '==========================================================================================================================================================================
        '=== Created By: Joe Vago, August 2014
        '==========================================================================================================================================================================
        'Deleting a cookie—physically removing it from the user's hard disk—is a variation on modifying it. 
        'You cannot directly remove a cookie because the cookie is on the user's computer. 
        'However, you can have the browser delete the cookie for you. The technique is to create a new cookie 
        'with the same name as the cookie to be deleted, but to set the cookie's expiration to a date earlier
        'than today. When the browser checks the cookie's expiration, the browser will discard the now-outdated 
        'cookie. The following code is one way to delete all the cookies available to the application
        '==========================================================================================================================================================================
        Dim oCookie As HttpCookie = Nothing
        Dim i As Integer = 0
        Dim cookieName As String = Nothing
        Dim limit As Integer = 0
        '==========================================================================================================================================================================
        Try
            Dim CurrContext = HttpContext.Current
            'Request Cookies = cookies that are sent BACK to the browser
            limit = CurrContext.Request.Cookies.Count - 1
            For i = 0 To limit
                cookieName = CurrContext.Request.Cookies(i).Name
                oCookie = New HttpCookie(cookieName)
                oCookie.Expires = DateTime.Now.AddDays(-10)
                CurrContext.Request.Cookies.Add(oCookie)
            Next

            'Response Cookies = cookies that are sent TO to the browser
            limit = CurrContext.Response.Cookies.Count - 1
            For i = 0 To limit
                cookieName = CurrContext.Response.Cookies(i).Name
                oCookie = New HttpCookie(cookieName)
                oCookie.Expires = DateTime.Now.AddDays(-10)
                CurrContext.Response.Cookies.Add(oCookie)
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub Expire_FormsAuthentication_Cookie()
        '==========================================================================================================================================================================
        '=== Created By: Joe Vago, August 2014
        '==========================================================================================================================================================================
        'Deleting a cookie—physically removing it from the user's hard disk—is a variation on modifying it. 
        'You cannot directly remove a cookie because the cookie is on the user's computer. 
        'However, you can have the browser delete the cookie for you. The technique is to create a new cookie 
        'with the same name as the cookie to be deleted, but to set the cookie's expiration to a date earlier
        'than today. When the browser checks the cookie's expiration, the browser will discard the now-outdated 
        'cookie. The following code is one way to delete all the cookies available to the application
        '==========================================================================================================================================================================
        Try
            Dim oCookie As New HttpCookie(FormsAuthentication.FormsCookieName)
            oCookie.Expires = DateTime.Now.AddDays(-10)
            'Set to 10 days to force cookie expiration
            'Request Cookies = cookies that are sent BACK to the browser
            If HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName) IsNot Nothing Then
                HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName)
                HttpContext.Current.Request.Cookies.Add(oCookie)
                HttpContext.Current.Request.Cookies(FormsAuthentication.FormsCookieName).Expires = DateTime.Now.AddDays(-10)
            End If

            'Response Cookies = cookies that are sent TO to the browser
            If HttpContext.Current.Response.Cookies(FormsAuthentication.FormsCookieName) IsNot Nothing Then
                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName)
                HttpContext.Current.Response.Cookies.Add(oCookie)
                HttpContext.Current.Response.Cookies(FormsAuthentication.FormsCookieName).Expires = DateTime.Now.AddDays(-10)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub Logout_Remove_All_LoginCustomer_Data()
        Expire_All_Cookies()

        Dim _with1 = HttpContext.Current
        'Clear the SESSION - oddly this does NOT clear the cookies
        _with1.Session.Clear()
        _with1.Session.Abandon()
        _with1.Session.RemoveAll()

        Dim CookieForceNewSessionID As New HttpCookie("ASP.NET_SessionId", Guid.NewGuid().ToString())
        CookieForceNewSessionID.Expires = DateTime.Now.AddDays(-10)
        _with1.Response.Cookies.Add(CookieForceNewSessionID)

        _with1.Response.Buffer = True
        _with1.Response.Expires = -1
        _with1.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1)
        _with1.User = Nothing

        'Invalidate the Cache on the Client Side
        _with1.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        _with1.Response.Cache.SetNoStore()
    End Sub

    Public Shared Sub Logout()
        '==========================================================================================================================================================================
        '=== Updated By:           Joe Vago, August 2014
        '=== Updated Description:  Added SignOut via Forms Auth and then force custom Cookie to expire ===
        '==========================================================================================================================================================================
        If (HttpContext.Current.User IsNot Nothing) Then
            If HttpContext.Current.User.Identity.IsAuthenticated = False Then
                'redirect user to Login Page
                FormsAuthentication.RedirectToLoginPage()
            Else
                '=== SignOut via Forms Auth and then force custom Cookie to expire ===
                FormsAuthentication.SignOut()
            End If
        End If
        '=== SignOut via Forms Auth and then force custom Cookie to expire ===
        Logout_Remove_All_LoginCustomer_Data()
    End Sub


End Class
