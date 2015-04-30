Imports Adjudication.DataAccess
Imports Adjudication.SecureSQL

Partial Public Class Admin_WebsiteNotes
    Inherits System.Web.UI.Page

    Dim SourceTableName As String = "ApplicationDefaults"
    Dim PrimaryKeyFieldName As String = "PK_ApplicationDefaultsID"
    Dim iAccessLevel As Int16
    Dim sLoginID As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID
        iAccessLevel = Master.AccessLevel
        If Not (iAccessLevel = 1) Then Response.Redirect("UnAuthorized.aspx")
        '============================================================================================

        If Not IsPostBack Then
            GetDefaultValues()
        End If

    End Sub

    Private Sub GetDefaultValues()
        '====================================================================================================
        Dim dt As DataTable
        '====================================================================================================
        dt = DataAccess.Get_ApplicationDefaults()

        If dt.Rows.Count > 0 Then
            Me.hiddenPrimaryKeyID.Value = dt.Rows(0)("PK_ApplicationDefaultsID").ToString
            Me.ftbMainpageApplicationDesc.Text = dt.Rows(0)("MainpageApplicationDesc").ToString
            Me.ftbMainpageApplicationNotes.Text = dt.Rows(0)("MainpageApplicationNotes").ToString

            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString

        End If
    End Sub

    Public Sub btnDEFAULTS_Update_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDEFAULTS_Update.Click
        '====================================================================================================
        Dim dtOrig As New DataTable                                                     'datatable for original values for audit check
        Dim strParms As New Dictionary(Of String, String), dateParms As New Dictionary(Of String, String)
        Dim intParms As New Dictionary(Of String, String), boolParms As New Dictionary(Of String, String)
        Dim iInt As Integer = 0, bResult As Integer = 0
        Dim PrimaryKeys As New Dictionary(Of String, Integer)
        '==================================================================================================================================================================================================================================================
        PrimaryKeys.Add(PrimaryKeyFieldName, CInt(Me.hiddenPrimaryKeyID.Value))

        strParms.Add("MainpageApplicationDesc", Me.ftbMainpageApplicationDesc.Text.Trim)
        strParms.Add("MainpageApplicationNotes", Me.ftbMainpageApplicationNotes.Text.Trim)

        Try
            iInt = CInt(Me.hiddenPrimaryKeyID.Value)
        Catch ex As Exception
            iInt = 0
        End Try

        Try
            If iInt > 0 Then    ' UPDATE existing record
                'dtOrig = Get_SourceDataTable(Me.hiddenPrimaryKeyID.Value)
                bResult = SecureSQL.SQLUpdate(SourceTableName, PrimaryKeys, _
                                                                  Master.UserLoginID, Me.Page.Title, _
                                                                  dtOrig, _
                                                                  boolParms, strParms, dateParms, intParms)
            Else                ' INSERT new record
                bResult = SecureSQL.SQLInsert(SourceTableName, PrimaryKeys, _
                                                                  Master.UserLoginID, Me.Page.Title, _
                                                                  boolParms, strParms, dateParms, intParms)
            End If

            If bResult Then
                GetDefaultValues()
                Me.lblErrors.Visible = False
                Me.lblSucessfulUpdate.Visible = True
            Else
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: There was an error SAVING this record.  Please contact the system administrators."
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ibtnMainpageApplicationDesc_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnMainpageApplicationDesc.Click
        If ibtnMainpageApplicationDesc.ImageUrl.ToString = "Images/PlusIcon.jpg" Then
            Me.ftbMainpageApplicationDesc.Height = Unit.Pixel(400)
            ibtnMainpageApplicationDesc.ImageUrl = "Images/MinusIcon.jpg"
        Else
            Me.ftbMainpageApplicationDesc.Height = Unit.Pixel(260)
            ibtnMainpageApplicationDesc.ImageUrl = "Images/PlusIcon.jpg"
        End If
    End Sub

    Private Sub ibtnMainpageApplicationNotes_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnMainpageApplicationNotes.Click
        If ibtnMainpageApplicationNotes.ImageUrl.ToString = "Images/PlusIcon.jpg" Then
            Me.ftbMainpageApplicationNotes.Height = Unit.Pixel(400)
            ibtnMainpageApplicationNotes.ImageUrl = "Images/MinusIcon.jpg"
        Else
            Me.ftbMainpageApplicationNotes.Height = Unit.Pixel(260)
            ibtnMainpageApplicationNotes.ImageUrl = "Images/PlusIcon.jpg"
        End If
    End Sub

End Class