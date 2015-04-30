Imports Adjudication.DataAccess
Imports Adjudication.Common

Public Class ConfirmAdjudicator
    Inherits System.Web.UI.Page
    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iCompanyID As Integer
    Dim iUserLoginID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        Master.PageTitleLabel = Page.Title
        sLoginID = Master.UserLoginID

        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If (iAccessLevel < 1 Or iAccessLevel > 3) Then Response.Redirect("UnAuthorized.aspx")

        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================
        iCompanyID = DataAccess.Find_CompanyForLiaison(sLoginID) ' Find the Company For this Liaison

        If Not IsPostBack Then
            If iCompanyID = 0 Then
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: You must be an Liaison to Confirm Adjudications."
            Else
                Call Populate_Data()
            End If
        End If
    End Sub


    Private Sub Populate_ProductionDetails(ByVal ProductionID As String)
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT Production.PK_ProductionID, Production.FK_CompanyID, Production.FK_VenueID, Production.FK_AgeApproriateID, Production.FK_ProductionTypeID,  " & _
                "	    Company.CompanyName, Venue.VenueName, Venue.City, Venue.State, " & _
                "       AgeAppropriate.AgeAppropriateName, ProductionType.ProductionType,  " & _
                "	    Production.Title, Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, Production.AllPerformanceDatesTimes,  " & _
                "	    Production.TicketContactName, Production.TicketContactPhone, Production.TicketContactEmail, Production.TicketPurchaseDetails " & _
                "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                "  FROM Production " & _
                "	    INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                "       INNER JOIN Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID " & _
                "	    INNER JOIN AgeAppropriate ON Production.FK_AgeApproriateID = AgeAppropriate.PK_AgeAppropriateID " & _
                "	    INNER JOIN ProductionType ON Production.FK_ProductionTypeID = ProductionType.PK_ProductionTypeID " & _
                " WHERE PK_ProductionID = " & ProductionID

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.lblTitle.Text = dt.Rows(0)("Title").ToString & " - " & dt.Rows(0)("ProductionType").ToString
            Me.lblCompanyName.Text = dt.Rows(0)("CompanyName").ToString & " - " & dt.Rows(0)("ProductionCategory").ToString
            Me.lblFirstPerformanceDateTime.Text = dt.Rows(0)("FirstPerformanceDateTime").ToString
            Me.lblLastPerformanceDateTime.Text = dt.Rows(0)("LastPerformanceDateTime").ToString
            Me.lblVenueName.Text = dt.Rows(0)("VenueName").ToString & " in " & dt.Rows(0)("City").ToString & ", " & dt.Rows(0)("State").ToString
            Me.lblAllPerformanceDatesTimes.Text = dt.Rows(0)("AllPerformanceDatesTimes").ToString
            Me.lblAgeAppropriateName.Text = dt.Rows(0)("AgeAppropriateName").ToString
            Me.lblTicketContactName.Text = dt.Rows(0)("TicketContactName").ToString
            Me.lblTicketContactPhone.Text = dt.Rows(0)("TicketContactPhone").ToString
            Me.lblTicketContactEmail.Text = dt.Rows(0)("TicketContactEmail").ToString
            Me.lblTicketPurchaseDetails.Text = dt.Rows(0)("TicketPurchaseDetails").ToString
            Me.txtFK_VenueID.Text = dt.Rows(0)("FK_VenueID").ToString
        End If
    End Sub

    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = " SELECT PK_NominationsID, PK_ScoringID, PK_ProductionID, " & _
                "       Users.LastName + ', ' + Users.FirstName as Fullname, Company.CompanyName, " & _
                "		Scoring.AdjudicatorRequestsReassignment, " & _
                "		Scoring.ProductionDateAdjudicated_Planned, Scoring.ProductionDateAdjudicated_Actual, " & _
                "       Production.Title, Venue.VenueName, FirstPerformanceDateTime, LastPerformanceDateTime " & _
                "       , ProductionCategory.PK_ProductionCategoryID, ProductionCategory.ProductionCategory " & _
                " FROM  Users INNER JOIN" & _
                "		Scoring ON Users.PK_UserID = Scoring.FK_UserID_Adjudicator INNER JOIN" & _
                "		Company ON Users.FK_CompanyID = Company.PK_CompanyID INNER JOIN" & _
                "       Nominations ON Scoring.FK_NominationsID = Nominations.PK_NominationsID INNER JOIN " & _
                "       Production ON Nominations.FK_ProductionID = Production.PK_ProductionID INNER JOIN " & _
                "       Venue ON Production.FK_VenueID = Venue.PK_VenueID AND Production.FK_VenueID = Venue.PK_VenueID " & _
                "		INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
                " WHERE  Production.FK_CompanyID = " & iCompanyID.ToString & _
                " ORDER BY Production.Title, Users.LastName "

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            gridMain.DataSource = dt
            gridMain.DataBind()
            lblTotalNumberOfRecords.Text = "Number of Adjudicators Assigned: " & dt.Rows.Count.ToString
        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "WARNING: No Company Productions have been Submitted for Adjudication - or have been assigned Adjudicators"
        End If
    End Sub

    Public Sub gridMain_ItemSelect(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Me.txtPK_ScoringID.Text = e.Item.Cells(0).Text

        Select Case CType(e.CommandSource, LinkButton).CommandName
            Case "Confirm_Command"
                Me.pnlGrid.Visible = False
                Me.pnlAddEdit.Visible = True
                Me.pnlSelectedProductionDetail.Visible = True
                Me.lblSucessfulUpdate.Visible = False
                Me.lblErrors.Visible = False
                If e.Item.Cells(3).Text = "&nbsp;" Then
                    Me.txtProductionDateAdjudicated_Actual.Text = ""
                Else
                    Me.txtProductionDateAdjudicated_Actual.Text = CDate(e.Item.Cells(3).Text).ToShortDateString
                End If
                Me.txtFullName.Text = e.Item.Cells(5).Text
                Call Populate_ProductionDetails(e.Item.Cells(2).Text)

                'Case "Title_Command"
                '    Me.pnlGrid.Visible = True
                '    Me.pnlAddEdit.Visible = False
                '    Me.pnlSelectedProductionDetail.Visible = True

                '    Call Populate_ProductionDetails(e.Item.Cells(2).Text)

                'Case Else
                '    ' break 
        End Select
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sSQL As String
        Dim DateTester As Date
        '====================================================================================================
        Me.lblErrors.Visible = False        ' reset error msg
        Me.lblSucessfulUpdate.Visible = False

        If Me.txtProductionDateAdjudicated_Actual.Text <> "" Then
            Try
                DateTester = (CDate(txtProductionDateAdjudicated_Actual.Text))
            Catch ex As Exception
                Me.lblErrors.Visible = True
                Me.lblErrors.Text = "ERROR: Please provide a VALID Actual Adjudication Date value."
                Exit Sub
            End Try
        Else
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide a VALID Actual Adjudication Date value."
            Exit Sub
        End If

        sSQL = "UPDATE Scoring SET " & _
                "   ProductionDateAdjudicated_Actual=CAST('" & Me.txtProductionDateAdjudicated_Actual.Text & "' as SMALLDATETIME), " & _
                "   LastUpdateByName='" & sLoginID & "', LastUpdateByDate= GetDate() " & _
                " WHERE PK_ScoringID=" & Me.txtPK_ScoringID.Text

        Call SQLUpdate(sSQL)

        Me.pnlGrid.Visible = True
        Me.pnlAddEdit.Visible = False
        Me.pnlSelectedProductionDetail.Visible = False

        Me.lblSucessfulUpdate.Visible = True

        Call Populate_Data()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.pnlGrid.Visible = True
        Me.pnlAddEdit.Visible = False
        Me.pnlSelectedProductionDetail.Visible = False
        Me.lblErrors.Visible = False
        Me.lblSucessfulUpdate.Visible = False
    End Sub



End Class
