Imports Adjudication.DataAccess

Partial Class AdminCategoryScoringRange
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageTitleLabel = Page.Title
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")

        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            ' Find if a Production needs to be EDITed
            If Request.QueryString("CategoryID") <> "" Then
                Populate_Dropdownlists()
                If Request.QueryString("CategoryID").ToUpper = "ADD" Then
                    Me.ddlCategoryID.Enabled = True
                    Me.ddlScoringRangeID.Enabled = True
                Else
                    Me.txtPK_ID.Text = Request.QueryString("CategoryID")
                    Me.txtPK_ID_2.Text = Request.QueryString("ScoringRangeID")
                    Populate_Data()
                    Me.ddlCategoryID.Enabled = False
                    Me.ddlScoringRangeID.Enabled = False
                End If
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: No Category Selected for Edit"
                Me.btnUpdate.Enabled = False
                Exit Sub
            End If
        End If

    End Sub

    Private Sub Populate_Dropdownlists()
        '====================================================================================================

        '====================================================================================================
        Me.ddlCategoryID.DataSource = DataAccess.Get_Categories(, , True)
        Me.ddlCategoryID.DataValueField = "PK_CategoryID"
        Me.ddlCategoryID.DataTextField = "CategoryName"
        Me.ddlCategoryID.DataBind()

        Me.ddlScoringRangeID.DataSource = DataAccess.Get_ScoringRanges(, , True)
        Me.ddlScoringRangeID.DataValueField = "PK_ScoringRangeID"
        Me.ddlScoringRangeID.DataTextField = "ScoringRange"
        Me.ddlScoringRangeID.DataBind()

    End Sub


    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Category_ScoringRange(Me.txtPK_ID.Text, Me.txtPK_ID_2.Text)

        If dt.Rows.Count > 0 Then
            Me.ddlCategoryID.SelectedValue = dt.Rows(0)("FK_CategoryID").ToString
            Me.ddlScoringRangeID.SelectedValue = dt.Rows(0)("FK_ScoringRangeID").ToString
            Me.txtMatrixAdjectives.Text = dt.Rows(0)("MatrixAdjectives").ToString
            Me.ftbMatrixDetail.Text = dt.Rows(0)("MatrixDetail").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: No record found!"
            Me.btnUpdate.Enabled = False
            Exit Sub
        End If
    End Sub





    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("AdminCategoryScoringRangeList.aspx")
    End Sub

    Public Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim sDataValues(30) As String
        '====================================================================================================
        Me.lblErrors.Text = ""
        Me.lblErrors.Visible = False

        If Me.ddlCategoryID.SelectedIndex = 0 Then Me.lblErrors.Text = "Error: You must Select a Category."
        If Me.ddlScoringRangeID.SelectedIndex = 0 Then Me.lblErrors.Text = "Error: You must Select a Scoring Range."

        If Me.lblErrors.Text.Length > 1 Then
            Me.lblErrors.Visible = True
            Exit Sub
        End If

        If Me.txtPK_ID.Text = "0" Then
            Dim sSQL As String
            sSQL = "SELECT Count(*) as RecFound FROM Category_ScoringRange WHERE FK_CategoryID = " & Me.ddlCategoryID.SelectedValue.ToString & _
                    "       AND Category_ScoringRange.FK_ScoringRangeID = " & Me.ddlScoringRangeID.SelectedValue.ToString

            Dim dtCheck As DataTable = Run_SQL_Query(sSQL)
            If Not dtCheck.Rows(0)("RecFound").ToString = "0" Then
                Me.lblErrors.Text = "Error: A Matrix Description already exists for the Category <b>'" & Me.ddlCategoryID.SelectedItem.ToString & "'</b> and the Scoring Range <b>" & Me.ddlScoringRangeID.SelectedItem.ToString & "</b>"
                Me.lblErrors.Visible = True
                Exit Sub
            End If
        End If

        sDataValues(1) = Me.ddlCategoryID.SelectedValue.ToString
        sDataValues(2) = Me.ddlScoringRangeID.SelectedValue.ToString
        sDataValues(3) = Me.txtMatrixAdjectives.Text
        sDataValues(4) = Me.ftbMatrixDetail.Text
        sDataValues(5) = Master.UserLoginID

        Try
            If Save_Category_ScoringRange(sDataValues) = True Then Response.Redirect("AdminCategoryScoringRangeList.aspx")
        Catch ex As Exception
            Throw ex
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Me.lblErrors.Visible = True
        End Try


    End Sub

End Class
