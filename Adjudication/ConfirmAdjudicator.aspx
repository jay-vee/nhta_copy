<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConfirmAdjudicator.aspx.vb" Inherits="Adjudication.ConfirmAdjudicator" Title="Confirm Adjudicator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphhead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
	<style>
		.form-control {
			display: inline-block;
		}

		td {
			vertical-align: middle;
		}

		.listItemSpace td label {
			margin-right: 2em;
			margin-left: 0.5em;
		}
	</style>
	<asp:Panel ID="pnlGrid" Visible="True" runat="server">
		<div class="TextCenter">
			<asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Adjudications: 0</asp:Label>
		</div>
		<asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="Double"
			BorderColor="#000000" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_ScoringID"
			AutoGenerateColumns="False" AllowSorting="false" CellPadding="2" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
			<FooterStyle ForeColor="#000000"></FooterStyle>
			<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
			<AlternatingItemStyle BackColor="LightGoldenrodYellow"></AlternatingItemStyle>
			<ItemStyle ForeColor="#333333" Height="50px"></ItemStyle>
			<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
			<Columns>
				<asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ProductionID"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
				<asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual" HeaderText="ProductionDateAdjudicated_Actual"></asp:BoundColumn>
				<asp:TemplateColumn>
					<ItemTemplate>
						<span style="padding-left: 0.4em;">
							<asp:LinkButton ID="lbtnConfirm" Text="Confirm Attendance" CommandName="Confirm_Command" CssClass="btn btn-gold" runat="server" /></span>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Assigned Adjudicator">
					<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
				</asp:BoundColumn>
				<asp:TemplateColumn SortExpression="ProductionDateAdjudicated_Planned" HeaderText="Planned Date">
					<HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" Width="90"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Planned","{0:MM/dd/yy}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn SortExpression="ProductionDateAdjudicated_Actual" HeaderText="Confirm<br>Date">
					<HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" Width="90"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Actual","{0:MM/dd/yy}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn SortExpression="Title" HeaderText="Production" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "Title") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="First Show">
					<HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" Width="90"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Last Show">
					<HeaderStyle HorizontalAlign="Center" Width="90"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center" Width="90"></ItemStyle>
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
					</ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
		<div class="TextCenter">
			<asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False">ERROR</asp:Label>
			<asp:Label ID="lblSucessfulUpdate" runat="server" ForeColor="SeaGreen" Visible="False" CssClass="alert alert-success" role="alert">Update Successful!</asp:Label>
		</div>


	</asp:Panel>

	<asp:Panel ID="pnlSelectedProductionDetail" Visible="False" runat="server">
		<asp:TextBox ID="txtPK_NominationID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
		<asp:TextBox ID="txtFK_CompanyID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
		<asp:TextBox ID="txtFK_VenueID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted"></asp:TextBox>

		<div class="panel panel-dark">
			<div class="panel-heading">Production Information</div>
			<div class="panel-body">
				<table class="TableSpacing">
					<tr>
						<td style="width: 40%; text-align: right">Production Name:</td>
						<td style="width: 60%;">
							<asp:Label ID="lblTitle" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Company:</td>
						<td style="width: 60%;">
							<asp:Label ID="lblCompanyName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">
							<asp:LinkButton ID="lbtnViewVenue" runat="server">View Venue Details</asp:LinkButton>&nbsp; 
										Venue:</td>
						<td style="width: 60%;">
							<asp:Label ID="lblVenueName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td align="right">Performance Dates:</td>
						<td align="left">
							<asp:Label ID="lblFirstPerformanceDateTime" runat="server" ForeColor="DimGray"></asp:Label>&nbsp;thru
							<asp:Label ID="lblLastPerformanceDateTime" runat="server" ForeColor="DimGray"></asp:Label>
						</td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Performance Dates Detail:</td>
						<td>
							<asp:Label ID="lblAllPerformanceDatesTimes" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Age Appropriate:</td>
						<td>
							<asp:Label ID="lblAgeAppropriateName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Ticket Contact Name:</td>
						<td>
							<asp:Label ID="lblTicketContactName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Ticket Contact Phone:</td>
						<td>
							<asp:Label ID="lblTicketContactPhone" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Ticket Contact Email:</td>
						<td>
							<asp:Label ID="lblTicketContactEmail" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Ticket Purchase Details:</td>
						<td>
							<asp:Label ID="lblTicketPurchaseDetails" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
				</table>
			</div>
		</div>
	</asp:Panel>
	<asp:Panel ID="pnlAddEdit" Visible="False" runat="server">
		<asp:TextBox ID="txtPK_ScoringID" runat="server" Visible="False" Width="64px" BorderStyle="Dotted">0</asp:TextBox>
		<div class="panel panel-dark">
			<div class="panel-heading">Adjudication Information</div>
			<div class="panel-body">
				<table class="TableSpacing">
					<tr>
						<td style="width: 40%; text-align: right">Adjudicator Assigned:</td>
						<td style="width: 60%;">
							<asp:Label ID="txtFullName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right">Date Adjudicated:</td>
						<td style="width: 60%;">
							<asp:TextBox ID="txtProductionDateAdjudicated_Actual" runat="server" Width="120px" CssClass="form-control"></asp:TextBox><span class="FontSmaller">&nbsp;[mm/dd/yy]</span></td>
					</tr>
					<tr>
						<td style="width: 40%; text-align: right"></td>
						<td style="width: 60%;">
							<hr />
							<asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Update"></asp:Button>
							<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
						</td>
					</tr>
				</table>
			</div>
		</div>
	</asp:Panel>
</asp:Content>
