Imports Adjudication.DataAccess

Public Class AdminErrorLog
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================
        If Not IsPostBack Then
            Call Populate_DropDownLists()
            Call Populate_Data()
        End If

    End Sub

    Private Sub Populate_DropDownLists()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Users()

        If dt.Rows.Count > 0 Then
            Me.ddlPK_UserID.DataSource = dt
            ddlPK_UserID.DataValueField = "PK_UserID"
            ddlPK_UserID.DataTextField = "FullName"
            ddlPK_UserID.DataBind()
        End If

    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT top 10 PK_ErrorID, ErrorMessage, LastUpdateByName, LastUpdateByDate FROM ErrorLog ORDER BY LastUpdateByDate DESC "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If
    End Sub

    Private Sub btnImpersonate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImpersonate.Click
        Dim dt As DataTable

        dt = DataAccess.Get_Users(Me.ddlPK_UserID.SelectedValue.ToString)

        If dt.Rows.Count > 0 Then
            Session("AccessLevel") = dt.Rows(0)("FK_AccessLevelID")
            Session("LoginID") = dt.Rows(0)("UserLoginID")
            Session.Item("PK_UserID") = dt.Rows(0)("PK_UserID")
            Session.Item("FirstName") = dt.Rows(0)("FirstName")
            Session.Item("LastName") = dt.Rows(0)("LastName")
            Session.Item("FullName") = dt.Rows(0)("FirstName") & " " & dt.Rows(0)("LastName")
            Session.Item("EmailPrimary") = dt.Rows(0)("EmailPrimary")
            ' not in returned dataset: Session.Item("EmailSecondary") = dt.Rows(0)("EmailSecondary")
            Session.Item("PhonePrimary") = dt.Rows(0)("PhonePrimary")
            Response.Redirect("MainPage.aspx")
        End If

    End Sub
End Class
