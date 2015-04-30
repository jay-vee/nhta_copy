Imports System.Web
Imports System.Web.Security
Imports System.Diagnostics
Imports System.Collections
Imports System
Imports System.Collections.Generic

Imports Adjudication.DataAccess

Public Class Login
    Inherits System.Web.UI.Page

    Protected WithEvents btnLogin As System.Web.UI.WebControls.Button
    Protected WithEvents txtLoginID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lbtnRequestAccess As System.Web.UI.WebControls.LinkButton
    Protected WithEvents pnlLogin As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlContactInfo As System.Web.UI.WebControls.Panel
    Protected WithEvents lnkAdminContactEmail As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblMainpageApplicationDesc As System.Web.UI.WebControls.Label

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Item("AccessLevel") = 0         ' Reset login access level
        Session.Item("LoginID") = ""            ' Reset login access level
        Session.Item("PK_UserID") = ""            ' Reset login access level

        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Primary Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        If Not IsPostBack Then
            If Not HttpContext.Current.User Is Nothing Then
                If HttpContext.Current.User.Identity.IsAuthenticated = True Then
                    Adjudication.Security.Logout()                  'logout the user upon page visit
                    Exit Sub
                End If
            End If

            Call Get_ProductionInfo()

            If Request.QueryString("ContactInfo") = "True" Then
                Call ContactInfo("Request")
            Else
                If Not Session.Item("LoginID") = "" Then
                    Me.txtLoginID.Text = Session.Item("LoginID").ToString
                End If
            End If
        End If
    End Sub

    Private Sub Get_ProductionInfo()
        '====================================================================================================
        '=== Get all productions opening in the next 100 days
        '====================================================================================================
        Me.gridCommunity.DataSource = DataAccess.Get_Upcoming_Productions(100)
        Me.gridCommunity.DataBind()
    End Sub

    Sub gridCommunity_Edit(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
        '====================================================================================================
        gridCommunity.EditItemIndex = CInt(E.Item.ItemIndex)
        Get_ProductionInfo()
    End Sub


    Sub gridCommunity_Cancel(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
        '====================================================================================================
        gridCommunity.EditItemIndex = -1
        Get_ProductionInfo()
    End Sub



    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim dt As DataTable

        dt = Adjudication.DataAccess.Secure_UserValidate(Me.txtLoginID.Text, Me.txtPassword.Text)

        Me.lblErrors.Visible = False
        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Primary Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        If dt.Rows(0)("ReturnValue") = "0" Then
            Session.Item("AccessLevel") = dt.Rows(0)("AccessLevel")
            Session.Item("LoginID") = Me.txtLoginID.Text
            Session.Item("PK_UserID") = dt.Rows(0)("PK_UserID")
            Session.Item("FirstName") = dt.Rows(0)("FirstName")
            Session.Item("LastName") = dt.Rows(0)("LastName")
            Session.Item("FullName") = dt.Rows(0)("FirstName") & " " & dt.Rows(0)("LastName")
            Session.Item("EmailPrimary") = dt.Rows(0)("EmailPrimary")
            Session.Item("PhonePrimary") = dt.Rows(0)("PhonePrimary")   ' not in returned dataset: Session.Item("EmailSecondary") = dt.Rows(0)("EmailSecondary")

            If BrowserDetect(dt.Rows(0)) = True Then
                ''==========================================================================================================================================================================
                ''=== Created By: Joe Vago, August 2014
                ''==========================================================================================================================================================================
                ''=== Based on article at http:'www.asp.net/web-forms/tutorials/security/introduction/an-overview-of-forms-authentication-vb#
                ''=== more info at http:'www.experts-exchange.com/Programming/Languages/.NET/ASP.NET/Q_21513331.html
                ''=== and at http:'www.hanselman.com/blog/PermaLink.aspx?guid=859d9aed-3d2d-4064-bf9f-9169f8c17806
                ''=== more: http:'stackoverflow.com/questions/16364858/keeping-user-logged-in-formsauthentication
                ''==========================================================================================================================================================================
                Dim authCookie As HttpCookie = FormsAuthentication.GetAuthCookie(dt.Rows(0)("PK_UserID").ToString, False)   ' Create new cookie based on the existing forms authentication ticket
                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)                     ' Get the FormsAuthenticationTicket out of the encrypted cookie
                FormsAuthentication.SetAuthCookie(Me.txtLoginID.Text, True)                                                 ' Sets the Request.IsAuthenticated = True
                HttpContext.Current.Response.Cookies.Add(authCookie)
                HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl, False)
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR encountered saving Browser Detected information."
            End If

        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = dt.Rows(0)("ReturnUserInformation")
        End If

    End Sub

    Private Function BrowserDetect(ByVal drLoginInfo As DataRow) As Boolean
        Dim ht As New Hashtable

        ht(0) = drLoginInfo("PK_UserID")
        ht(1) = drLoginInfo("FirstName") & " " & drLoginInfo("LastName")
        ht(2) = Request.Browser.Type.ToString
        ht(3) = Request.Browser.Browser.ToString
        ht(4) = Request.Browser.Version.ToString
        ht(5) = Request.Browser.MajorVersion.ToString
        ht(6) = Request.Browser.MinorVersion.ToString
        ht(7) = Request.Browser.Platform.ToString
        ht(8) = Request.Browser.Beta.ToString
        ht(9) = Request.Browser.AOL.ToString
        ht(10) = Request.Browser.Win16.ToString
        ht(11) = Request.Browser.Win32.ToString
        ht(12) = Request.Browser.Frames.ToString
        ht(13) = Request.Browser.Tables.ToString
        ht(14) = Request.Browser.Cookies.ToString
        ht(15) = Request.Browser.VBScript.ToString
        ht(16) = Request.Browser.EcmaScriptVersion.ToString
        ht(17) = Request.Browser.JavaApplets.ToString
        ht(18) = Request.Browser.CDF.ToString

        Return Adjudication.DataAccess.Record_BrowserDetect(ht)

    End Function


    'Private Sub lbtnRequestAccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnRequestAccess.Click
    '    Call ContactInfo("Request")
    'End Sub

    Private Sub ContactInfo(ByVal ContactType As String)
        Me.pnlLogin.Visible = False
        Me.pnlContactInfo.Visible = True
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT TOP 1 * FROM ApplicationDefaults "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("AdminContactEmail").ToString.Length > 5 Then
                Me.lnkAdminContactEmail.Text = dt.Rows(0)("AdminContactEmail").ToString
                Me.lnkAdminContactEmail.NavigateUrl = "MailTo:" & dt.Rows(0)("AdminContactEmail").ToString
            End If
        End If

    End Sub


End Class
