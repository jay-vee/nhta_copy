Imports Adjudication.DataAccess

Public Class AdjReassignRequest_Web
    Inherits System.Web.UI.Page



    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents gridMain As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtSortOrder As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSortColumnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblLoginID As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotalNumberOfRecords As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub




    Dim iAccessLevel As Int16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        If Not (Session.Item("AccessLevel") = 1) Then Response.Redirect("../UnAuthorized.aspx")
        Me.lblLoginID.Text = Session("LoginID")
        iAccessLevel = CInt(Session.Item("AccessLevel"))
        '============================================================================================

        If Not IsPostBack Then
            Call Populate_DataGrid()
        End If
    End Sub
    Private Sub Populate_DataGrid()
        Dim dt As DataTable, sSQL As String

        sSQL = "SELECT Scoring.PK_ScoringID, Scoring.FK_UserID_Adjudicator, Scoring.FK_CompanyID_Adjudicator, Nominations.PK_NominationsID, " & _
               "       Production.PK_ProductionID, Company.PK_CompanyID, Production.FK_VenueID, " & _
               "       Scoring.AdjudicatorRequestsReassignment, " & _
               "       Production.Title, " & _
               "       Production.FirstPerformanceDateTime, Production.LastPerformanceDateTime, " & _
               "       Company.CompanyName, " & _
               "       ProductionCategory.ProductionCategory, " & _
               "       Users.LastName + ', ' + Users.FirstName AS FullName, " & _
               "       Users.EmailPrimary, " & _
               "       Users.PhonePrimary, " & _
               "       Scoring.ProductionDateAdjudicated_Planned, " & _
               "       Scoring.LastUpdateByName, Scoring.LastUpdateByDate, " & _
               "       Scoring.AdjudicatorRequestsReassignment, " & _
               "       (SELECT TOP 1 ApplicationName FROM ApplicationDefaults) AS ApplicationName " & _
               " FROM  " & _
               "       Production " & _
               "       INNER JOIN ProductionCategory ON Production.FK_ProductionCategoryID = ProductionCategory.PK_ProductionCategoryID " & _
               "       INNER JOIN Company ON Production.FK_CompanyID = Company.PK_CompanyID " & _
               "       INNER JOIN Nominations ON Production.PK_ProductionID = Nominations.FK_ProductionID " & _
               "       INNER JOIN Scoring ON Nominations.PK_NominationsID = Scoring.FK_NominationsID " & _
               "       LEFT OUTER JOIN Users ON Scoring.FK_UserID_Adjudicator = Users.PK_UserID " & _
               " WHERE " & _
               "       Scoring.AdjudicatorRequestsReassignment = 1 "

        sSQL = sSQL + " ORDER BY " + txtSortColumnName.Text + " " + txtSortOrder.Text

        Try
            dt = DataAccess.Run_SQL_Query(sSQL)
            Me.gridMain.DataSource = dt
            Me.gridMain.DataBind()
            Me.lblTotalNumberOfRecords.Text = "Number of Requested Reassignments: <B>" & dt.Rows.Count & "</B>"

        Catch ex As Exception
            lblTotalNumberOfRecords.Text = "ERROR: " & ex.Message
            lblTotalNumberOfRecords.ForeColor = System.Drawing.Color.Red
        End Try
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


End Class
