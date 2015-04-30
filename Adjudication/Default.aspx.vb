Imports Adjudication.DataAccess

Partial Public Class DefaultPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("login.aspx", True)

        'If Not IsPostBack Then
        '    Call Display_Messages()
        '    Call Get_ProductionInfo()
        'End If
    End Sub

    'Private Sub Get_ProductionInfo()
    '    '====================================================================================================
    '    '=== Get all productions opening in the next 100 days
    '    '====================================================================================================
    '    Me.gridCommunity.DataSource = DataAccess.Get_Upcoming_Productions(100)
    '    Me.gridCommunity.DataBind()

    'End Sub

    'Private Sub Display_Messages()
    '    '====================================================================================================
    '    Try
    '        Dim dt As DataTable = DataAccess.Get_ApplicationDefaults

    '        If dt.Rows.Count > 0 Then
    '            Me.lblMainpageApplicationDesc.Text = dt.Rows(0)("MainpageApplicationDesc").ToString
    '            'Me.lblAdminContactName.Text = dt.Rows(0)("AdminContactName").ToString
    '            'Me.lblAdminContactPhoneNum.Text = dt.Rows(0)("AdminContactPhoneNum").ToString
    '            'If dt.Rows(0)("AdminContactEmail").ToString.Length > 5 Then
    '            '    Me.lnkAdminContactEmail.Text = dt.Rows(0)("AdminContactEmail").ToString
    '            '    Me.lnkAdminContactEmail.NavigateUrl = "MailTo:" & dt.Rows(0)("AdminContactEmail").ToString
    '            'End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    'Sub gridCommunity_Edit(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
    '    '====================================================================================================
    '    gridCommunity.EditItemIndex = CInt(E.Item.ItemIndex)
    '    Get_ProductionInfo()
    'End Sub


    'Sub gridCommunity_Cancel(ByVal Sender As Object, ByVal E As DataListCommandEventArgs)
    '    '====================================================================================================
    '    gridCommunity.EditItemIndex = -1
    '    Get_ProductionInfo()
    'End Sub


End Class