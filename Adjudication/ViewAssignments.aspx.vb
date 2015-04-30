Imports Adjudication.DataAccess

Public Class ViewAssignments
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Integer = 0
    Dim sLoginID As String = ""
    Dim sProductionName As String = ""
    Dim cBackColor As System.Drawing.Color = System.Drawing.Color.White
    Dim sLastAdjudicatorName As String = ""
    Dim GridBackColor As System.Drawing.Color = System.Drawing.Color.WhiteSmoke

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If Not iAccessLevel > 0 Then Response.Redirect("Login.aspx")
        If Not (iAccessLevel = 1 Or iAccessLevel = 2 Or iAccessLevel = 3 Or iAccessLevel = 4) Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Session("LoginID")
        '============================================================================================
        'PK_AccessLevelID	AccessLevelName
        '       1	    Administrator
        '       2       Liaison & Adjudicator
        '       3	    Liaison
        '       4	    Adjudicator
        '       5	    Backup Adjudicator
        '============================================================================================
        If Not IsPostBack Then
            Call Populate_Data()
        End If
    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As New DataTable, dtUserInfo As DataTable
        '====================================================================================================
        dtUserInfo = DataAccess.Get_UserRecord(Session("PK_UserID"))
        If dtUserInfo.Rows.Count > 0 Then Me.lblCompanyName.Text = dtUserInfo.Rows(0)("CompanyName").ToString

        dt = DataAccess.Find_AdjudicationsForCompanyFromUserLoginID(sLoginID) ' Find all Adjudication assignments for this User's Company
        gridMain.DataSource = dt
        gridMain.DataBind()

        If dt.Rows.Count = 0 Then
            Me.lblAdjudicationMessage.Visible = True
            Me.lblAdjudicationMessage.Text = "No Assignments found for your theatre company " & dtUserInfo.Rows(0)("CompanyName").ToString
        End If

    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        Dim iDaysToConfirmAttendance As Int16, iDaysToWaitForScoring As Int16
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" And Not e.Item.ItemType.ToString = "Footer" Then
            '=== Did Adjudicator Confirm Attendance 
            If e.Item.Cells(3).Text.ToUpper = "NO" Then
                iDaysToConfirmAttendance = CInt(e.Item.Cells(10).Text)

                If Today.Date > CDate(e.Item.Cells(9).Text).AddDays(iDaysToConfirmAttendance) Then
                    e.Item.Cells(3).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(3).Font.Bold = True
                End If

            End If

            '=== Did Adjudicator Submit a required Ballot
            If e.Item.Cells(4).Text = "0" Then
                iDaysToWaitForScoring = CInt(e.Item.Cells(11).Text)
                If Today.Date > CDate(e.Item.Cells(9).Text).AddDays(iDaysToWaitForScoring) Then
                    e.Item.Cells(4).ForeColor = System.Drawing.Color.Red
                    e.Item.Cells(4).Font.Bold = True
                    e.Item.Cells(4).Text = "LATE"
                Else
                    If Today.Date > CDate(e.Item.Cells(9).Text) Then
                        e.Item.Cells(4).Text = "Required"
                        e.Item.Cells(4).Font.Bold = True
                    Else
                        e.Item.Cells(4).Text = ""
                    End If
                End If

            Else
                e.Item.Cells(4).Text = "Submitted"
            End If

            '=== Change Backcolor for each Adjudicator =============================
            If e.Item.Cells(2).Text.Trim <> sLastAdjudicatorName Then
                If GridBackColor.ToString = System.Drawing.Color.White.ToString Then
                    GridBackColor = System.Drawing.Color.WhiteSmoke
                Else
                    GridBackColor = System.Drawing.Color.White
                End If
            End If

            e.Item.BackColor = GridBackColor
            sLastAdjudicatorName = e.Item.Cells(2).Text.Trim

        End If

    End Sub



End Class
