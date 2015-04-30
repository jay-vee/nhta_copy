Imports Adjudication.DataAccess

Public Class AdminVenue
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
    Protected WithEvents txtEmailAddress As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPhone As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSeatingCapacity As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVenueName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPK_VenueID As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDirections As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlHandicappedAccessible As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtParking As System.Web.UI.WebControls.TextBox
    Protected WithEvents ddlAirConditioned As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlOutdoor As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtComments As System.Web.UI.WebControls.TextBox

    Dim iAccessLevel As Int16
    Dim sLoginID As String
    Dim iVenueID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.PageTitleLabel = Page.Title
        '============================================================================================
        If IsTestMode() = True Then
            Session.Item("AccessLevel") = 1         ' FOR TESTING ONLY
            Session.Item("LoginID") = "JVago"       '"JUDGE"      ' FOR TESTING ONLY
        End If
        '============================================================================================
        'If Request.QueryString("VenueID") = "" Then Response.Redirect("AdminVenue.aspx?VenueID=2") ' FOR TESTING ONLY

        iAccessLevel = CInt(Session.Item("AccessLevel"))
        If Not iAccessLevel > 0 Then Response.Redirect("UnAuthorized.aspx")
        sLoginID = Session("LoginID")
        '============================================================================================
        'Redirect the user if the page times out
        Response.AppendHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 10) & "; URL=Timeout.aspx")
        '============================================================================================
        Me.lblErrors.Visible = False

        If Request.QueryString("VenueID") <> "" Then
            iVenueID = Request.QueryString("VenueID")
            If Not IsPostBack Then
                Call Populate_Data()
            End If
        Else
            If Request.QueryString("Add") = "True" Then
                'Me.btnUpdate.Text = "Add"
            Else
                Me.pnlUserData.Visible = False
                Me.lblErrors.Text = "ERROR: No Venue selected to Edit"
                Me.lblErrors.Visible = True
            End If
        End If

        If Request.QueryString("ViewOnly") = "True" Then
            Me.txtVenueName.Enabled = False
            Me.txtAddress.Enabled = False
            Me.txtCity.Enabled = False
            Me.txtState.Enabled = False
            Me.txtZIP.Enabled = False
            Me.txtPhone.Enabled = False
            Me.txtEmailAddress.Enabled = False
            Me.txtWebsite.Enabled = False
            Me.txtDirections.Enabled = False
            Me.txtParking.Enabled = False
            ddlHandicappedAccessible.Enabled = False
            ddlAirConditioned.Enabled = False
            ddlOutdoor.Enabled = False
            Me.txtSeatingCapacity.Enabled = False
            Me.txtComments.Enabled = False
            Me.btnUpdate.Visible = False
        End If

    End Sub





    Private Sub Populate_Data()
        '====================================================================================================
        Dim dt As DataTable, sSQL As String
        '====================================================================================================
        sSQL = "SELECT PK_VenueID, VenueName, Address, City, State, ZIP, Phone, Website, EmailAddress, " & _
                "   Directions, Parking, HandicappedAccessible, AirConditioned, " & _
                "   Outdoor, Directions, SeatingCapacity, Comments, LastUpdateByName, LastUpdateByDate " & _
                " FROM Venue " & _
                " WHERE PK_VenueID=" & iVenueID

        dt = DataAccess.Run_SQL_Query(sSQL)

        If dt.Rows.Count > 0 Then
            Me.txtPK_VenueID.Text = dt.Rows(0)("PK_VenueID").ToString
            Me.txtVenueName.Text = dt.Rows(0)("VenueName").ToString
            Me.txtAddress.Text = dt.Rows(0)("Address").ToString
            Me.txtCity.Text = dt.Rows(0)("City").ToString
            Me.txtState.Text = dt.Rows(0)("State").ToString
            Me.txtZIP.Text = dt.Rows(0)("ZIP").ToString
            Me.txtPhone.Text = dt.Rows(0)("Phone").ToString
            Me.txtWebsite.Text = dt.Rows(0)("Website").ToString
            Me.txtEmailAddress.Text = dt.Rows(0)("EmailAddress").ToString
            Me.txtDirections.Text = dt.Rows(0)("Directions").ToString
            Me.txtParking.Text = dt.Rows(0)("Parking").ToString
            Me.ddlHandicappedAccessible.SelectedValue = dt.Rows(0)("HandicappedAccessible").ToString
            Me.ddlAirConditioned.SelectedValue = dt.Rows(0)("AirConditioned").ToString
            Me.ddlOutdoor.SelectedValue = dt.Rows(0)("Outdoor").ToString
            Me.txtDirections.Text = dt.Rows(0)("Directions").ToString
            Me.txtSeatingCapacity.Text = dt.Rows(0)("SeatingCapacity").ToString
            Me.txtComments.Text = dt.Rows(0)("Comments").ToString
            Me.lblLastUpdateByName.Text = dt.Rows(0)("LastUpdateByName").ToString
            Me.lblLastUpdateByDate.Text = dt.Rows(0)("LastUpdateByDate").ToString
        End If
    End Sub





    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '====================================================================================================
        Dim dc As New Adjudication.DataAccess, sDataValues(20) As String
        '====================================================================================================

        If Me.txtVenueName.Text = "" Then
            Me.lblErrors.Visible = True
            Me.lblErrors.Text = "ERROR: Please provide Venue Name."
            Exit Sub
        End If

        sDataValues(1) = Me.txtPK_VenueID.Text
        sDataValues(2) = Me.txtVenueName.Text
        sDataValues(3) = Me.txtAddress.Text
        sDataValues(4) = Me.txtCity.Text
        sDataValues(5) = Me.txtState.Text
        sDataValues(6) = Me.txtZIP.Text
        sDataValues(7) = Me.txtPhone.Text
        sDataValues(8) = Me.txtEmailAddress.Text
        sDataValues(9) = Me.txtWebsite.Text
        sDataValues(10) = Me.txtDirections.Text
        sDataValues(11) = Me.txtParking.Text
        sDataValues(12) = ddlHandicappedAccessible.SelectedValue
        sDataValues(13) = ddlAirConditioned.SelectedValue
        sDataValues(14) = ddlOutdoor.SelectedValue
        sDataValues(15) = Me.txtSeatingCapacity.Text
        sDataValues(16) = Me.txtComments.Text
        sDataValues(17) = sLoginID

        If Save_Venue(sDataValues) = True Then
            If Request.QueryString("NewVenue") = "True" Then
                ' Assume the call came from the AdminProduction
                Response.Redirect("AdminProduction.aspx")
            Else
                Response.Redirect("AdminVenueList.aspx")
            End If
        Else
            Me.lblErrors.Text = "ERROR: Saving Company Data"
            Me.lblErrors.Visible = True
        End If

    End Sub



End Class
