Imports Adjudication.DataAccess

Partial Public Class AdminCategory
    Inherits System.Web.UI.Page

    Dim sCategoryID As String = ""
    Dim sLoginID As String = ""
    Dim iAccessLevel As Int16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        If Not IsPostBack Then
            ' Find if a Production needs to be EDITed
            If Request.QueryString("CategoryID") <> "" Then
                sCategoryID = Request.QueryString("CategoryID")
                If sCategoryID.ToUpper = "ADD" Then
                    Get_Next_DisplayOrderNumber()
                Else
                    Me.txtPK_CategoryID.Text = sCategoryID
                    Populate_Data()
                End If
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: No Category Selected for Edit"
                Me.btnUpdate.Enabled = False
                Exit Sub
            End If
        End If

    End Sub

    Private Sub Get_Next_DisplayOrderNumber()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Run_SQL_Query("SELECT MAX(DisplayOrder) + 1 as NextDisplayOrderNumber FROM Category")

        If dt.Rows.Count > 0 Then
            Me.txtDisplayOrder.Text = dt.Rows(0)("NextDisplayOrderNumber").ToString
        Else
            Me.txtDisplayOrder.Text = "1"
        End If
    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_Category(sCategoryID)

        If dt.Rows.Count > 0 Then
            Me.txtCategoryName.Text = dt.Rows(0)("CategoryName").ToString
            Me.ddlActiveCategory.SelectedValue = dt.Rows(0)("ActiveCategory").ToString
            Me.txtScoreFieldName.Text = dt.Rows(0)("ScoreFieldName").ToString
            Me.txtCommentFieldName.Text = dt.Rows(0)("CommentFieldName").ToString
            Me.txtNominationFieldName.Text = dt.Rows(0)("NominationFieldName").ToString
            Me.txtRoleFieldName.Text = dt.Rows(0)("RoleFieldName").ToString
            Me.txtGenderFieldName.Text = dt.Rows(0)("GenderFieldName").ToString
            Me.txtDisplayOrder.Text = dt.Rows(0)("DisplayOrder").ToString
            Me.ftbScoringCriteria.Text = dt.Rows(0)("ScoringCriteria").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("AdminCategoryList.aspx")
    End Sub

    Public Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(30) As String, NumberTester As Integer
        '====================================================================================================
        Me.lblErrors.Text = ""
        Me.lblErrors.Visible = False

        If Me.txtCategoryName.Text.Length < 5 Then Me.lblErrors.Text = "Error: You must Enter in a Category Name"
        Try
            NumberTester = CInt(Me.txtDisplayOrder.Text)
            If NumberTester <= 0 Then Me.lblErrors.Text = "Error: You must Enter in a valid Display Order number."
        Catch ex As Exception
            Me.lblErrors.Text = "Error: You must Display Order Value"
        End Try

        If Me.lblErrors.Text.Length > 1 Then
            Me.lblErrors.Visible = True
            Exit Sub
        End If

        sDataValues(1) = Me.txtPK_CategoryID.Text
        sDataValues(2) = Me.txtCategoryName.Text
        sDataValues(3) = Me.ddlActiveCategory.SelectedValue
        sDataValues(4) = Me.txtScoreFieldName.Text
        sDataValues(5) = Me.txtCommentFieldName.Text
        sDataValues(6) = Me.txtNominationFieldName.Text
        sDataValues(7) = Me.txtRoleFieldName.Text
        sDataValues(8) = Me.txtGenderFieldName.Text
        sDataValues(9) = Me.txtDisplayOrder.Text
        sDataValues(10) = Me.ftbScoringCriteria.Text
        sDataValues(11) = sLoginID

        If Save_Category(sDataValues) = True Then
            Response.Redirect("AdminCategoryList.aspx")
        Else
            Me.lblErrors.Visible = True
            Me.lblSucessfulUpdate.Visible = False
        End If

    End Sub

    Private Sub ibtnExpand_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnExpand.Click
        If ibtnExpand.ImageUrl.ToString = "Images/PlusIcon.jpg" Then
            Me.ftbScoringCriteria.Height = Unit.Pixel(600)
            ibtnExpand.ImageUrl = "Images/MinusIcon.jpg"
        Else
            Me.ftbScoringCriteria.Height = Unit.Pixel(300)
            ibtnExpand.ImageUrl = "Images/PlusIcon.jpg"
        End If
    End Sub


End Class