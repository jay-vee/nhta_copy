Imports Adjudication.DataAccess

Public Class AdminScoringRange
    Inherits System.Web.UI.Page

    Dim sScoringRangeID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        If Not (Master.AccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================

        If Not IsPostBack Then
            ' Find if a Production needs to be EDITed
            If Request.QueryString("ScoringRangeID") <> "" Then
                sScoringRangeID = Request.QueryString("ScoringRangeID")
                If sScoringRangeID.ToUpper = "ADD" Then
                    'nothign for this page
                Else
                    Me.txtPK_ScoringRangeID.Text = sScoringRangeID
                    Populate_Data()
                End If
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: No Scoring Range Selected for Edit"
                Me.btnUpdate.Enabled = False
                Exit Sub
            End If
        End If
    End Sub





    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_ScoringRange(sScoringRangeID)

        If dt.Rows.Count > 0 Then
            Me.txtScoringRangeMax.Text = dt.Rows(0)("ScoringRangeMax").ToString
            Me.txtScoringRangeMin.Text = dt.Rows(0)("ScoringRangeMin").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
        End If
    End Sub





    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("AdminScoringRangeList.aspx")
    End Sub

    Public Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(30) As String, NumberTesterMax As Integer, NumberTesterMin As Integer
        '====================================================================================================
        Me.lblErrors.Text = ""
        Me.lblErrors.Visible = False

        Try
            NumberTesterMax = CInt(Me.txtScoringRangeMax.Text)
            If NumberTesterMax <= 0 Then Me.lblErrors.Text = "Error: You must Enter in a valid MAXIMUM Range value greater than Zero."
        Catch ex As Exception
            Me.lblErrors.Text = "Error: You must MAXIMUM Range Value"
        End Try

        Try
            NumberTesterMin = CInt(Me.txtScoringRangeMin.Text)
            If NumberTesterMin <= 0 Then Me.lblErrors.Text = "Error: You must Enter in a valid MINIMUM Range value greater than Zero."
        Catch ex As Exception
            Me.lblErrors.Text = "Error: You must MINIMUM Range Value"
        End Try

        If NumberTesterMax <= NumberTesterMin Then
            Me.lblErrors.Text = "Error: The Maximum Range value must be greater than the Minimum Range value."
        End If

        If Me.lblErrors.Text.Length > 1 Then
            Me.lblErrors.Visible = True
            Exit Sub
        End If

        sDataValues(1) = Me.txtPK_ScoringRangeID.Text
        sDataValues(2) = Me.txtScoringRangeMax.Text
        sDataValues(3) = Me.txtScoringRangeMin.Text
        sDataValues(4) = Master.UserLoginID

        If Save_ScoringRange(sDataValues) = True Then
            Response.Redirect("AdminScoringRangeList.aspx")
        Else
            Me.lblErrors.Visible = True
            Me.lblSucessfulUpdate.Visible = False
        End If

    End Sub



End Class
