Imports Adjudication.DataAccess
Imports Adjudication.Common
Imports Adjudication.CustomMail

Partial Public Class AdminProductionList
    Inherits System.Web.UI.Page

    Dim iAccessLevel As Int16
    Dim sCompanyID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("UnAuthorized.aspx")
        'sLoginID = Session("LoginID")
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If

    End Sub


    Private Sub Populate_DataGrid()
        '====================================================================================================
        Dim dt As New DataTable
        '====================================================================================================
        dt = DataAccess.Get_Productions(txtSortColumnName.Text, txtSortOrder.Text, Me.chkShowOnlyFutureProductions.Checked)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
        End If

        lblTotalNumberOfRecords.Text = dt.Rows.Count.ToString

    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        '====================================================================================================
        Dim dt As DataTable, str As String = ""
        '====================================================================================================

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Edit_Command"
                Response.Redirect("AdminProduction.aspx?Admin=True&ProductionID=" & e.Item.Cells(0).Text)

            Case "Nomination_Command"
                Response.Redirect("AdminNominations.aspx?Admin=True&ProductionID=" & e.Item.Cells(0).Text)

            Case "Remove_Command"
                Me.pnlGrid.Visible = False
                Me.pnlRemoveShow.Visible = True
                Me.lblSuccessful.Visible = False
                '====================================================================================================
                ' List of Assigned Adjudicators for Selected Production
                dt = DataAccess.Set_ProductionAdjudicatorList(e.Item.Cells(11).Text)
                Me.gridSub.DataSource = dt
                Me.gridSub.DataBind()

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        str += dr.Item("PK_UserID").ToString & ", " & dr.Item("PK_CompanyID").ToString & ", "
                    Next
                    ViewState("AssignedAdj") = str.Substring(0, str.Length - 2)
                Else
                    ViewState("AssignedAdj") = String.Empty
                End If

                Me.txtProdName.Text = e.Item.Cells(3).Text
                Me.lblTitle.Text = e.Item.Cells(3).Text
                Me.lblProductionCategory.Text() = e.Item.Cells(6).Text
                Me.lblCompanyName.Text = e.Item.Cells(5).Text
                Me.lblProductionType.Text = e.Item.Cells(4).Text
                Me.lblFirstPerformanceDateTime.Text = CDate(e.Item.Cells(12).Text).ToShortDateString
                Me.lblLastPerformanceDateTime.Text = CDate(e.Item.Cells(13).Text).ToShortDateString
                ViewState("ProductionID") = e.Item.Cells(0).Text
                ViewState("CompanyID") = e.Item.Cells(14).Text
                ViewState("NominationsID") = e.Item.Cells(11).Text

            Case Else
                ' break 
        End Select
    End Sub

    Sub gridMain_SortCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gridMain.SortCommand

        If txtSortColumnName.Text = e.SortExpression Then
            If txtSortOrder.Text = " DESC " Then
                txtSortOrder.Text = ""
            Else
                txtSortOrder.Text = " DESC "
            End If
        Else
            txtSortOrder.Text = ""
        End If

        txtSortColumnName.Text = e.SortExpression

        Populate_DataGrid()

    End Sub

    Public Sub gridMain_DataItemBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles gridMain.ItemDataBound
        '====================================================================================================
        '====================================================================================================
        If Not e.Item.ItemType.ToString = "Header" Then         ' Do not check if row is a HEADER Row
            Select Case e.Item.Cells(7).Text.ToUpper
                Case "COMMUNITY"
                    e.Item.Cells(4).ForeColor = System.Drawing.Color.DarkGreen
                    e.Item.Cells(6).ForeColor = System.Drawing.Color.DarkGreen
                    e.Item.Cells(7).ForeColor = System.Drawing.Color.DarkGreen
                Case "PROFESSIONAL"
                    e.Item.Cells(4).ForeColor = System.Drawing.Color.Firebrick
                    e.Item.Cells(6).ForeColor = System.Drawing.Color.Firebrick
                    e.Item.Cells(7).ForeColor = System.Drawing.Color.Firebrick
                Case "YOUTH"
                    e.Item.Cells(4).ForeColor = System.Drawing.Color.DarkOrange
                    e.Item.Cells(6).ForeColor = System.Drawing.Color.DarkOrange
                    e.Item.Cells(7).ForeColor = System.Drawing.Color.DarkOrange
            End Select
        End If
    End Sub

    Private Sub lbtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdd.Click
        Response.Redirect("AdminProduction.aspx?Add=True&Admin=True")
    End Sub

    Private Sub chkShowOnlyFutureProductions_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOnlyFutureProductions.CheckedChanged
        Call Populate_DataGrid()
    End Sub

    Private Sub btnRemoveDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveDelete.Click
        Me.pnlGrid.Visible = True
        Me.pnlRemoveShow.Visible = False
        Me.txtAdminEmailComments_Assign.Text = String.Empty
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.lblSuccessful.Visible = False
        If rblEmailInfo.SelectedIndex = 1 Then
            If Email_ProductionCancellations(True, Me.txtAdminEmailComments_Assign.Text) = True Then
                Me.lblSuccessful.Visible = True
                Me.lblSuccessful.Text = "Sucessfully Removed the Production from Adjudication.  Emails sent.  Check Email Log to confirm sending of Emails to assigned Adjudicators."

                Me.pnlGrid.Visible = True
                Me.pnlRemoveShow.Visible = False

                Call Populate_DataGrid()
            End If
        Else
            If DeleteProductionAndAssignments() = True Then
                Me.lblCompleteDeleteNotice.Text = "This Production has been Removed/Deleted from Adjudication."
                Me.lblSuccessful.Visible = True
                Me.lblSuccessful.Text = "Sucessfully removed the Production from Adjudication."

                Me.pnlGrid.Visible = True
                Me.pnlRemoveShow.Visible = False

                Call Populate_DataGrid()
            End If
        End If
    End Sub

    Private Function DeleteProductionAndAssignments() As Boolean
        Dim sSQL As String
        Try
            ''=== Delete all assigned Adjudicators
            sSQL = "DELETE FROM SCORING WHERE FK_NominationsID = " & ViewState("NominationsID")
            Call SQLDelete(sSQL)
            '=== Delete the Nominations for the Production
            sSQL = "DELETE FROM [Nominations] WHERE FK_ProductionID= " & ViewState("ProductionID")
            Call SQLDelete(sSQL)
            '=== Delete the Production
            sSQL = "DELETE FROM [Production] WHERE PK_ProductionID= " & ViewState("ProductionID")
            Call SQLDelete(sSQL)

            If Me.lblCompleteDeleteNotice.Visible = False Then
                Me.lblCompleteDeleteNotice.Text = "All Adjudicator Assignments for this Production have been Removed/Deleted, and this Production has also been Removed/Deleted from Adjudication."
                Me.lblCompleteDeleteNotice.Visible = True
            End If

            Return True

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: " & ex.Message
            Return False
        End Try

    End Function

    Private Function Email_ProductionCancellations(Optional ByVal DeleteAdjudicator As Boolean = False, Optional ByVal AdminComments As String = "") As Boolean
        '============================================================================================
        Dim dtUser As New DataTable
        Dim sSubject As String, sBody As String = "", sTo As String = "", sFrom As String = ""
        Dim sToProductingCompany As String = "", sToProductingCompanyNames As String = ""
        Dim sToNames As String = "", sUserCompanyID As String = ""
        Dim bEmailAddressError As Boolean = True
        Dim i As Int16 = 0
        '============================================================================================
        Me.lblSuccessful.Visible = False
        Me.lblErrors.Visible = False

        Try
            sFrom = ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString

            '=== Producing Company Email Reciepients 
            sToProductingCompany = Get_CompanyMemberEmails(ViewState("CompanyID"), , 3, True)
            sToProductingCompanyNames = Get_CompanyMemberEmails(ViewState("CompanyID"), , 3, True, True)

            Dim sAdjToEmail() As String = ViewState("AssignedAdj").ToString.Split(",")                          'Previously Stored IDs as:  Adjudicator UserID, Adjudicator CompanyID,

            Do While i < sAdjToEmail.Length

                If sAdjToEmail.Length > 1 Then                                                                  '=== 1 element means NO Adjudicators assigned - just email the Company
                    sTo = Get_CompanyMemberEmails(sAdjToEmail(i + 1), sAdjToEmail(i), 3, True, False)           '=== Adjudicating Company: Email Reciepients (Liaisons and Company emails addresses)
                    sToNames = Get_CompanyMemberEmails(sAdjToEmail(i + 1), sAdjToEmail(i), 3, True, True)
                End If

                If sToNames.Length > 6 Or sToProductingCompanyNames.Length > 6 Then
                    sToNames = sToNames & sToProductingCompanyNames
                    sToNames = sToNames.Trim.Substring(0, sToNames.Trim.Length - 1)                             'removes last comma
                    sToNames = sToNames.Replace(",", "<li>")
                    sToNames = "<hr noshade><B>NOTE:</b> This email has been sent to the Producing Theatre Company and all Assigned Adjudcators: " & _
                                "<ul><li>" & sToNames & "</ul>"
                End If

                If AdminComments.Length > 1 Then AdminComments = "<br /><p style=""BACKGROUND-COLOR: lemonchiffon; ""><B><FONT COLOR=#404000>NHTA ADMINISTRATOR COMMENTS:</span></B> " & AdminComments & "</p>"

                ' === Create the Text for the Email Message ======================================================================
                sSubject = "NHTA | Production Removed from Adjudication: '" & Me.lblTitle.Text & "'"
                If sAdjToEmail.Length > 1 Then                                                                  '=== 1 element means NO Adjudicators assigned 
                    dtUser = Get_UserRecord(sAdjToEmail(i))
                    sSubject = sSubject & "; removed Adjudication assignment for '" & dtUser.Rows(0)("FullName").ToString & "'"
                End If

                sBody = sBody & AdminComments
                sBody = sBody & "The production of '" & Me.lblTitle.Text & "' has been <i>removed from Adjudication</i> by the Producing Theatre Company. "
                If sAdjToEmail.Length > 1 Then sBody = sBody & " As a result, the NH Theatre Awards Adjudication Assignment for <b>" & dtUser.Rows(0)("FullName").ToString & "</b> has been removed. "
                sBody = sBody & "Please direct questions regarding this removal to the Producing Company Liaison.<br /><br />Production Specifics:<br />"
                sBody = sBody & FormatEmailHTML_Production(ViewState("ProductionID"))

                sBody = sBody & "Thank you.<br /><br />"
                sBody = sBody & FormatEmailHTML_AutomatedEmailDisclaimer()
                sBody = sBody & sToNames
                sBody = sBody & Common.Get_EmailFooter()

                ' === Send the Email ========================================================================================
                sTo = sTo & sToProductingCompany
                SendCDOEmail(sFrom, sTo, False, sSubject, sBody, True, True, Session("LoginID"), EMAIL_ADJUDICATOR_ASSIGNED)
                sBody = String.Empty

                i = i + 2                                                                                   '=== TO Step Properly to next 
            Loop


            If DeleteProductionAndAssignments() = True Then                                                 '=== Perform data Deletions
                Me.lblCompleteDeleteNotice.Text = "An Confirmation Email has been successfully sent <br /> from <b>" & _
                                            ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & _
                                            "</b> to the following email addresses<br /><ul><li>" & sToNames & "</ul><br />"
                Me.lblCompleteDeleteNotice.Visible = True
                '=====> for testing <=====
                'Me.lblCompleteDeleteNotice.Text = Me.lblCompleteDeleteNotice.Text & "<br /><br />" & sBody
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = Me.lblErrors.Text & "<P>ERROR MESSAGE: " & ex.Message.ToString & "</p>"
            Return False
        End Try

    End Function

End Class