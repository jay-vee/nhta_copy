Imports Adjudication.DataAccess
Imports Adjudication.Common

Partial Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        'TESTING - TESTING         If HttpContext.Current.User.Identity.IsAuthenticated = False Then FormsAuthentication.RedirectToLoginPage() 'redirect user to Login Page

        Me.lblWebsiteLastBuildDate.Text = System.IO.File.GetLastWriteTime(Server.MapPath("BIN/Adjudication.DLL"))
        '============================================================================================
        Me.lblLoginID.Text = UserLoginID()
        Call Set_Menu_BasedOnRole(AccessLevel())
        '============================================================================================
        'Redirect the user if the page times out
        'TURNED OFF FOR TESTING  Call SetTimeOut()                               'Sets a Timeout based on the "SessionTimeout" value in Web.Config file

    End Sub

    Private Sub Set_Menu_BasedOnRole(ByVal AccessLevel As String)
        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        Try
            Select Case CInt(AccessLevel)
                Case 1
                    Me.mnuAdjudicators.Visible = True
                    Me.mnuLiaisons.Visible = True
                    Me.mnuWebsiteSettings.Visible = True
                    Me.mnuAdministration.Visible = True
                    Me.mnuEmail.Visible = True
                Case 2
                    Me.mnuAdjudicators.Visible = True
                    Me.mnuLiaisons.Visible = True
                    Me.mnuWebsiteSettings.Visible = False
                    Me.mnuAdministration.Visible = False
                    Me.mnuEmail.Visible = False
                Case 3
                    Me.mnuAdjudicators.Visible = False
                    Me.mnuLiaisons.Visible = True
                    Me.mnuWebsiteSettings.Visible = False
                    Me.mnuAdministration.Visible = False
                    Me.mnuEmail.Visible = False
                Case 4
                    Me.mnuAdjudicators.Visible = True
                    Me.mnuLiaisons.Visible = False
                    Me.mnuWebsiteSettings.Visible = False
                    Me.mnuAdministration.Visible = False
                    Me.mnuEmail.Visible = False
                Case 5
                    Me.mnuAdjudicators.Visible = True
                    Me.mnuLiaisons.Visible = False
                    Me.mnuWebsiteSettings.Visible = False
                    Me.mnuAdministration.Visible = False
                    Me.mnuEmail.Visible = False
                Case Else
                    Response.Redirect("Default.aspx")
            End Select

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Property MenuVisible() As Boolean
        Get
            Return Me.navMenu.Visible
        End Get
        Set(value As Boolean)
            Me.navMenu.Visible = value
        End Set
    End Property

    Public Property MenuMainVisible() As Boolean
        Get
            Return Me.divMainMenu.Visible
        End Get
        Set(value As Boolean)
            Me.divMainMenu.Visible = value
        End Set
    End Property
    Public Property MenuBallotVisible() As Boolean
        Get
            Return Me.divBallotMenu.Visible
        End Get
        Set(value As Boolean)
            If value = True Then
            Else

            End If
            Me.divBallotMenu.Visible = value
        End Set
    End Property
    Public Property PageTitleVisible() As Boolean
        Get
            Return Me.divPageTitle.Visible
        End Get
        Set(value As Boolean)
            Me.divPageTitle.Visible = value
        End Set
    End Property
    Public ReadOnly Property AccessLevel() As Integer
        'Need to keep Session Variables until all pages are converted to use MasterPage
        Get
            Try
                If IsTestMode() = True Then
                    Return ConfigurationManager.AppSettings.Get("IsTestMode_AccessLevel").Trim()
                Else
                    Return CInt(Session.Item("AccessLevel"))
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Get
    End Property
    Public ReadOnly Property UserLoginID() As String
        'Need to keep Session Variables until all pages are converted to use MasterPage        
        Get
            If IsTestMode() = True Then
                Return ConfigurationManager.AppSettings.Get("IsTestMode_PK_UserID").Trim()
            Else
                If Session.Item("LoginID") Is Nothing Then
                    FormsAuthentication.RedirectToLoginPage()
                End If
                Return Session.Item("LoginID")
            End If
        End Get
    End Property
    Public ReadOnly Property UserName() As String
        Get
            Try
                If IsTestMode() = True Then
                    Dim firstName = Set_TestSessionValues("FirstName")
                    Dim lastName = Set_TestSessionValues("LastName")
                    Return firstName & " " & lastName
                Else
                    Return Session.Item("FirstName") & " " & Session.Item("LastName")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Get
    End Property
    Public ReadOnly Property PK_UserID() As Integer
        Get
            Try

                If IsTestMode() = True Then
                    Return Set_TestSessionValues("PK_UserID")
                Else
                    Return CInt(Session.Item("PK_UserID").ToString)
                End If
            Catch ex As Exception

            End Try
        End Get
    End Property
    Public ReadOnly Property EmailPrimary() As String
        Get
            If IsTestMode() = True Then
                Return Set_TestSessionValues("EmailPrimary")
            Else
                Return Session.Item("EmailPrimary")
            End If

        End Get
    End Property
    Public ReadOnly Property PhonePrimary() As String
        Get
            If IsTestMode() = True Then
                Return Set_TestSessionValues("PhonePrimary")
            Else
                Return Session.Item("PhonePrimary")
            End If
        End Get
    End Property

    Public Property PageTitleLabel()
        Get
            Return Me.lblPageTitle.Text
        End Get
        Set(ByVal value)
            If IsTestMode() = True Then
                spnOverridesActive.Visible = True
            End If
            Me.lblPageTitle.Text = value
        End Set
    End Property

    Private Sub SetTimeOut()
        Dim timeOutVal As Double = Convert.ToDouble(ConfigurationManager.AppSettings.Get("SessionTimeout").Trim())
        Me.hdnTimeoutValue.Value = DateTime.Now.AddMinutes(timeOutVal).ToString()
        Me.hdnCurrentTime.Value = DateTime.Now.ToString()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Try
            'WILL LET THE SESSION AND FORMS AUTHENTICATION TIMEOUT WORK INSTEAD
            'Dim csm As ClientScriptManager = Page.ClientScript
            'Dim csType As Type = Me.GetType()
            'Dim csCounterJs As String = "/Includes/Dcountup.js"

            'If (Request.ApplicationPath.Trim().Length > 1) Then
            '    csCounterJs = Request.ApplicationPath.Trim() + csCounterJs
            'End If

            'If Not (csm.IsClientScriptIncludeRegistered(csType, "Counter")) Then
            '    csm.RegisterClientScriptInclude(csType, "csCounterJs", ResolveClientUrl(csCounterJs))
            'End If

        Catch ex As Exception
            'do nothing (no error?)
        End Try

    End Sub

    Private Sub btnTimeOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTimeOut.Click
        If Request.Url.Segments.Length = 2 Then
            Response.Redirect("TimeOut.aspx")
        Else
            Response.Redirect("../TimeOut.aspx")
        End If
    End Sub

End Class

