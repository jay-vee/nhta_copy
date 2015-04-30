Imports Adjudication.DataAccess
Imports Adjudication.Common

Partial Public Class MainPage
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Integer = 0
    Dim iCompanyID As Integer = 0
    Dim sLoginID As String = ""
    Dim sProductionName As String = ""
    Dim cBackColor As System.Drawing.Color = System.Drawing.Color.White

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not iAccessLevel > 0 Then Response.Redirect("Login.aspx")

        Master.PageTitleLabel = Page.Title

        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        If Not IsPostBack Then
            Call Get_ApplicationDefaults()
            Call Get_CompanyRegisteredUsers()       ' this function also sets the value for iCompanyID
            Select Case iAccessLevel
                Case 1
                    Me.pnlAdminRequests.Visible = True
                    Call Get_AdminCounts()
                    Call Get_MessageInfo()
                    Call Get_AdjudicatorInfo()
                    Call Get_ProductionAdjudicators()
                Case 2
                    Call Get_ProductionAdjudicators()
                    Call Get_AdjudicatorInfo()
                Case 3
                    Call Get_ProductionAdjudicators()
                Case 4, 5
                    Call Get_AdjudicatorInfo()
            End Select

            'Me.lbtnRunLateBallotReport.Attributes.Add("onclick", "popWin_LateBallotReport();return false;")

        End If

    End Sub

    Private Sub Get_AdminCounts()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty

        sSQL = "SELECT COUNT(Scoring.PK_ScoringID) AS CountOfReassignmentRequests " & _
                    "FROM Production  " & _
                    "   INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                    "   INNER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID  " & _
                    "   INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
                    "   LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID" & _
                    "   WHERE Scoring.AdjudicatorRequestsReassignment = 1 "

        dt = DataAccess.Run_SQL_Query(sSQL)

        Me.lblCountReassignmentRequests.Text = CInt(dt.Rows(0)("CountOfReassignmentRequests").ToString)
        If Me.lblCountReassignmentRequests.Text = "0" Then Me.lblCountReassignmentRequests.ForeColor = System.Drawing.Color.Black

        '====================================================================================================
        dt = New DataTable

        dt = DataAccess.Find_Late_Ballots("LastPerformanceDateTime", "")
        Me.lblCountLateBallots.Text = dt.Rows.Count
        If Me.lblCountLateBallots.Text = "0" Then Me.lblCountLateBallots.ForeColor = System.Drawing.Color.Black


    End Sub

    Private Sub Get_CompanyRegisteredUsers()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty
        sSQL = "SELECT Users.FK_CompanyID, Company.CompanyName FROM Users INNER JOIN Company ON Company.PK_CompanyID = Users.FK_CompanyID " & _
                "   WHERE (UserLoginID = '" & sLoginID & "') "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count = 0 Then
            Me.lblCompany.Text = "ERROR: Could not find Theater Company for user " & sLoginID
            Me.lblProductionCompany.Text = "ERROR: Could not find Theater Company for user " & sLoginID
        Else
            iCompanyID = CInt(dt.Rows(0)("FK_CompanyID").ToString)
            Me.lblCompany.Text = dt.Rows(0)("CompanyName").ToString
            Me.lblProductionCompany.Text = dt.Rows(0)("CompanyName").ToString
        End If

        '====================================================================================================
        dt = New DataTable

        sSQL = "SELECT PK_UserID, LastName + ', ' + FirstName as FullName, PhonePrimary, EmailPrimary, LastTrainingDate, LastLoginTime, " & _
                "       CASE WHEN Active = 1 then 'Yes' ELSE 'No' end as ActiveUser, UserAccessLevels.AccessLevelName " & _
                " FROM Users INNER JOIN UserAccessLevels ON Users.FK_AccessLevelID = UserAccessLevels.PK_AccessLevelID " & _
                " WHERE (Users.FK_AccessLevelID <> 6) AND (Users.Active=1) AND FK_CompanyID = " & iCompanyID & _
                " ORDER BY Active DESC, LastName, FirstName"

        dt = DataAccess.Run_SQL_Query(sSQL)

        Me.gridCompanyUsers.DataSource = dt
        Me.gridCompanyUsers.DataBind()

    End Sub

    Private Sub Get_ProductionAdjudicators()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty
        sSQL = "SELECT  Production.PK_ProductionID, PK_ScoringID, Users.PK_UserID, " & _
          "   CASE WHEN Nominations.PK_NominationsID IS NULL THEN 'No' ELSE 'Yes' END as Nominated, " & _
          "   CASE WHEN Users.LastName IS NULL AND Nominations.PK_NominationsID IS NOT NULL THEN '<EM>Adjudicators Not Yet Assigned</EM>' WHEN Users.LastName IS NULL AND Nominations.PK_NominationsID IS NULL THEN '<EM>No Nominations Sumitted</EM>' ELSE Users.LastName + ', ' + Users.FirstName END as FullName, " & _
          "   CASE WHEN Users.LastName IS NULL THEN Null ELSE Users.EmailPrimary END as EmailPrimary," & _
          "   CASE WHEN Users.LastName IS NULL THEN null ELSE Users.PhonePrimary END PhonePrimary, " & _
          "   Company.CompanyName, Company.PK_CompanyID, " & _
          "   Title, FirstPerformanceDateTime, LastPerformanceDateTime, " & _
          "   ProductionDateAdjudicated_Planned, ProductionDateAdjudicated_Actual, AdjudicatorRequestsReassignment, " & _
          "   DirectorScore + MusicalDirectorScore + ChoreographerScore + ScenicDesignerScore + LightingDesignerScore " & _
          "	        + SoundDesignerScore + CostumeDesignerScore + OriginalPlaywrightScore + BestPerformer1Score " & _
          "	        + BestPerformer2Score + BestSupportingActor1Score + BestSupportingActor2Score " & _
          "	        + BestSupportingActress1Score + BestSupportingActress2Score as TotalScore,  " & _
          "   Scoring.FK_NominationsID, Scoring.LastUpdateByName, Scoring.LastUpdateByDate, Scoring.CreateByName, Scoring.CreateByDate, " & _
          "   Scoring.FK_ScoringStatusID, ScoringStatus.ScoringStatus " & _
          " FROM Scoring " & _
          "       INNER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID AND Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
          "       INNER JOIN Company ON Scoring.FK_CompanyID_Adjudicator = Company.PK_CompanyID " & _
          "       RIGHT OUTER JOIN Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID " & _
          "       RIGHT OUTER JOIN Production ON Nominations.FK_ProductionID = Production.PK_ProductionID " & _
          "	      INNER JOIN ScoringStatus ON ScoringStatus.PK_ScoringStatusID = Scoring.FK_ScoringStatusID " & _
          " WHERE (Production.FK_CompanyID = " & iCompanyID & ") " & _
          " ORDER BY FirstPerformanceDateTime, Users.LastName, Users.FirstName "

        dt = New DataTable
        dt = DataAccess.Run_SQL_Query(sSQL)

        Me.gridSub.DataSource = dt
        Me.gridSub.DataBind()

        Me.pnlMyProductions.Visible = True
        Me.gridSub.Visible = True

    End Sub

    Private Sub Get_AdjudicatorInfo()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================

        dt = DataAccess.Find_AdjudicationsForUserLoginID(sLoginID) ' Find the Productions For this User
        gridMain.DataSource = dt
        gridMain.DataBind()

        If dt.Rows.Count = 0 Then
            If iCompanyID = 0 Then
                Me.lblAdjudicationMessage.Visible = True
                Me.lblAdjudicationMessage.Text = "You are not associated with any Theatre Company."
            Else
                Me.lblAdjudicationMessage.Visible = True
                Me.lblAdjudicationMessage.Text = "You have not been Assigned any Productions to Adjudicate"
            End If
        End If

        Me.pnlMyAdjudications.Visible = True
        Me.gridMain.Visible = True

    End Sub

    Private Sub Get_MessageInfo()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty
        sSQL = "SELECT COUNT(PK_AdminMessageID) AS CountOfMessages, COUNT(DISTINCT LastUpdateByName) AS UsersPosting, MAX(LastUpdateByDate) AS LastMessagePostedDate " & _
                " FROM AdminMessage " & _
                " WHERE LastUpdateByDate > GETDATE() - 31 "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows(0)("CountOfMessages").ToString = "0" Then
            Me.lblAdminMessages.Text = "No Messages left in the past 30 Days."
        Else
            Me.lblAdminMessages.Text = "There are <B>" & dt.Rows(0)("CountOfMessages").ToString & "</B> Messages left by <B>" & dt.Rows(0)("UsersPosting").ToString & "</B> Users in the past 30 Days."
        End If
    End Sub

    Private Sub Get_ApplicationDefaults()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        Dim sSQL As String = String.Empty
        sSQL = "SELECT TOP 1 * FROM ApplicationDefaults "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            'Me.lblMainpageApplicationDesc.Text = dt.Rows(0)("MainpageApplicationDesc").ToString
            Me.lblMainpageApplicationNotes.Text = dt.Rows(0)("MainpageApplicationNotes").ToString
            Me.lblAdminContactName.Text = dt.Rows(0)("AdminContactName").ToString
            Me.lblAdminContactPhoneNum.Text = dt.Rows(0)("AdminContactPhoneNum").ToString
            If dt.Rows(0)("AdminContactEmail").ToString.Length > 5 Then
                Me.lnkAdminContactEmail.Text = dt.Rows(0)("AdminContactEmail").ToString
                Me.lnkAdminContactEmail.NavigateUrl = "MailTo:" & dt.Rows(0)("AdminContactEmail").ToString
            End If
        End If
    End Sub

    Public Sub gridCompanyUsers_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridCompanyUsers.ItemDataBound
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then

            If Not e.Item.Cells(1).Text = "Yes" Then
                e.Item.Cells(0).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(1).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(2).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(3).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(4).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(5).ForeColor = System.Drawing.Color.Gray
                e.Item.Cells(6).ForeColor = System.Drawing.Color.Gray
            End If

            If e.Item.Cells(2).Text.ToUpper = "EXPELLED" Or e.Item.Cells(2).Text.ToUpper = "SUSPENDED" Then
                e.Item.Cells(0).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(1).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(2).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(4).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(5).ForeColor = System.Drawing.Color.Red
                e.Item.Cells(6).ForeColor = System.Drawing.Color.Red
            End If
        End If

    End Sub

    Public Sub gridSub_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridSub.ItemDataBound
        '====================================================================================================
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then
            If sProductionName <> e.Item.Cells(0).Text Then
                If cBackColor.ToString = System.Drawing.Color.WhiteSmoke.ToString Then
                    cBackColor = System.Drawing.Color.White
                Else
                    If Not sProductionName = "" Then cBackColor = System.Drawing.Color.WhiteSmoke
                End If
                sProductionName = e.Item.Cells(0).Text
            End If

            e.Item.BackColor = cBackColor

            '====================================================================================================
            If e.Item.Cells(6).Text = "&nbsp;" And e.Item.Cells(7).Text.Length > 0 And (Not e.Item.Cells(8).Text = "&nbsp;") Then
                If Today.Date > CDate(e.Item.Cells(7).Text).AddDays(14) Then
                    e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(3).Font.Bold = True
                    e.Item.Cells(3).Text = "Please Confirm"
                End If
            End If
        End If
    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        Dim iDaysToConfirmAttendance As Int16, iDaysToWaitForScoring As Int16
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then

            If e.Item.Cells(2).Text.ToUpper = "NO" Then
                iDaysToConfirmAttendance = CInt(e.Item.Cells(9).Text)

                If Today.Date > CDate(e.Item.Cells(8).Text).AddDays(iDaysToConfirmAttendance) Then
                    e.Item.Cells(2).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(2).Font.Bold = True
                End If

                e.Item.Cells(2).Text = "Not Done*"
                Me.lblDaysToConfirm.Text = "*Please confirm your attendance at least <b>" & iDaysToConfirmAttendance.ToString & "</b> days before Productions opening date."
                Me.lblDaysToConfirm.Visible = True
            End If

            If e.Item.Cells(3).Text = "0" Then
                iDaysToWaitForScoring = CInt(e.Item.Cells(10).Text)
                If Today.Date > CDate(e.Item.Cells(8).Text).AddDays(iDaysToWaitForScoring) Then
                    e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(3).Font.Bold = True
                    e.Item.Cells(3).Text = "LATE"
                Else
                    If Today.Date > CDate(e.Item.Cells(8).Text) Then
                        e.Item.Cells(3).Text = "Required"
                        e.Item.Cells(3).Font.Bold = True
                    Else
                        e.Item.Cells(3).Text = ""
                    End If
                End If

            Else
                e.Item.Cells(3).Text = "Submitted"
            End If
        End If

    End Sub

End Class