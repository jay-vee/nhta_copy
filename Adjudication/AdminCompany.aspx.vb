Imports Adjudication.DataAccess
Imports Adjudication.CustomMail

Public Class AdminCompany
    Inherits System.Web.UI.Page

    Protected WithEvents lblErrors As System.Web.UI.WebControls.Label
    Protected WithEvents lblSucessfulUpdate As System.Web.UI.WebControls.Label
    Protected WithEvents txtAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtState As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZIP As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtWebsite As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLastUpdateByName As System.Web.UI.WebControls.Label
    Protected WithEvents lblLastUpdateByDate As System.Web.UI.WebControls.Label
    Protected WithEvents btnUpdate As System.Web.UI.WebControls.Button
    Protected WithEvents txtLastTrainingDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents pnlUserData As System.Web.UI.WebControls.Panel
    Protected WithEvents txtPK_CompanyID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNumOfProductions As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComments As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCompanyName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEmailAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlActiveCompany As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iCompanyID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
            'Session.Item("CompanyID") = "3"         ' FOR TESTING ONLY
        End If
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If Not (iAccessLevel = 1 Or iAccessLevel = 2 Or iAccessLevel = 3) Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Session("LoginID")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Request.QueryString("CompanyID") <> "" And iAccessLevel = 1 Then
            iCompanyID = Request.QueryString("CompanyID")
        Else
            iCompanyID = DataAccess.Find_CompanyForUserLoginID(sLoginID) ' Find the Company For this Liaison
            'if not a Liaison or a system Admin deny access to this page
            If iCompanyID = 0 And Not iAccessLevel = 1 Then
                Me.pnlUserData.Visible = False
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: You are not a Admin or a Liaison for any Company."
                Exit Sub
            End If
        End If

        If Not IsPostBack Then
            Call Populate_DropDowns()

            If Request.QueryString("Add") = "True" And iAccessLevel = 1 Then
                txtNumOfProductions.Enabled = True
                ddlActiveCompany.Enabled = True
            Else
                Call Populate_Data()
            End If
        End If

    End Sub

    Private Sub Populate_DropDowns()


    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_CompanyID, NumOfProductions, CompanyName, " & _
                "   Address, City, State, ZIP, Phone, EmailAddress, Website, ActiveCompany, Comments, " & _
                "   Company.LastUpdateByName, Company.LastUpdateByDate " & _
                " FROM Company " & _
                " WHERE PK_CompanyID=" & iCompanyID

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtPK_CompanyID.Text = dt.Rows(0)("PK_CompanyID").ToString
            Me.txtCompanyName.Text = dt.Rows(0)("CompanyName").ToString
            Me.txtAddress.Text = dt.Rows(0)("Address").ToString
            Me.txtCity.Text = dt.Rows(0)("City").ToString
            Me.txtState.Text = dt.Rows(0)("State").ToString
            Me.txtZIP.Text = dt.Rows(0)("ZIP").ToString
            Me.txtPhone.Text = dt.Rows(0)("Phone").ToString
            Me.txtEmailAddress.Text = dt.Rows(0)("EmailAddress").ToString
            Me.txtWebsite.Text = dt.Rows(0)("Website").ToString
            Me.ddlActiveCompany.SelectedValue = dt.Rows(0)("ActiveCompany")
            Me.txtNumOfProductions.Text = dt.Rows(0)("NumOfProductions").ToString
            Me.txtComments.Text = dt.Rows(0)("Comments").ToString
            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString

            If iAccessLevel = 1 Then
                txtNumOfProductions.Enabled = True
                ddlActiveCompany.Enabled = True
            End If

        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim sDataValues(20) As String
        '====================================================================================================

        If Me.txtCompanyName.Text = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide Company Name."
            Exit Sub
        End If

        If Me.txtEmailAddress.Text.Length > 0 Then
            If ValidateEmailAddress(Me.txtEmailAddress.Text) = False Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR MESSAGE: Invalid email address in for Company Email Address: [" & Me.txtEmailAddress.Text & "]"
                Exit Sub
            End If
        End If

        sDataValues(1) = Me.txtPK_CompanyID.Text
        sDataValues(3) = Me.txtNumOfProductions.Text
        sDataValues(4) = Me.txtCompanyName.Text
        sDataValues(5) = Me.txtAddress.Text
        sDataValues(6) = Me.txtCity.Text
        sDataValues(7) = Me.txtState.Text
        sDataValues(8) = Me.txtZIP.Text
        sDataValues(9) = Me.txtPhone.Text
        sDataValues(10) = Me.txtEmailAddress.Text
        sDataValues(11) = Me.txtWebsite.Text
        sDataValues(12) = ddlActiveCompany.SelectedValue
        sDataValues(13) = Me.txtComments.Text
        sDataValues(14) = sLoginID

        If Save_Company(sDataValues) = True Then
            If Request.QueryString("CompanyID") <> "" Then
                Response.Redirect("AdminCompanyList.aspx?CompanyID=" & Me.txtPK_CompanyID.Text)
            Else
                Me.lblSucessfulUpdate.Visible = True
                Me.lblSucessfulUpdate.Text = "Successfully save of Company: <B>" & Me.txtCompanyName.Text & "</B>"
            End If
        Else
            Me.lblErrors.Text = "ERROR: Saving Company Data"
            Me.lblErrors.Visible = True
        End If

    End Sub



End Class
