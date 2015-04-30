<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" Title="Nomination Administration" CodeBehind="AdminNominations.aspx.vb" Inherits="Adjudication.AdminNominations" %>

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
	</style>

	<div class="TextCenter">
		<asp:Label ID="lblErrors" runat="server" CssClass="form-control" ForeColor="red" Visible="False"></asp:Label>
		<asp:Label ID="lblSucessfulUpdate" runat="server" CssClass="form-control" Visible="False">Update Successful!</asp:Label>
	</div>

	<asp:Panel ID="pnlAddEditData" runat="server">
		<asp:TextBox ID="txtPK_NominationsID" runat="server" CssClass="form-control" Visible="False" BorderStyle="Dotted" Width="64px">0</asp:TextBox>
		<asp:TextBox ID="txtPK_ProductionID" runat="server" CssClass="form-control" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
		<div class="panel panel-dark">
			<div class="panel-heading">Production Information</div>
			<div class="panel-body">
				<table class="TableSpacing">
					<tr>
						<td style="text-align: right; width: 40%;">Production Name:
						</td>
						<td style="text-align: left; width: 60%;">
							<asp:Label ID="lblTitle" runat="server" Font-Bold="True" ForeColor="DimGray" Width="398px"></asp:Label>
							<asp:TextBox ID="txtPK_CompanyID" runat="server" Visible="False" BorderStyle="Dotted" Width="64px"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td style="text-align: right; width: 40%;">Venue:</td>
						<td style="text-align: left; width: 60%;">
							<asp:Label ID="lblVenueName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="text-align: right; width: 40%;">Production Category:</td>
						<td style="text-align: left; width: 60%;">
							<asp:Label ID="lblProductionCategory" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="text-align: right; width: 40%;">Company:</td>
						<td style="text-align: left; width: 60%;">
							<asp:Label ID="lblCompanyName" runat="server" ForeColor="DimGray"></asp:Label></td>
					</tr>
					<tr>
						<td style="text-align: right; width: 40%;">Requires Adjudication:</td>
						<td style="text-align: left; width: 60%;">
							<asp:CheckBox ID="chkRequiresAdjudication" runat="server" ForeColor="DimGray" Enabled="False"></asp:CheckBox></td>
					</tr>
					<tr>
						<td align="right">Performance Dates:</td>
						<td align="left">
							<asp:Label ID="lblFirstPerformanceDateTime" runat="server" ForeColor="DimGray"></asp:Label>&nbsp;thru
							<asp:Label ID="lblLastPerformanceDateTime" runat="server" ForeColor="DimGray"></asp:Label>
						</td>
					</tr>
				</table>
			</div>
		</div>

		<div class="panel panel-dark">
			<div class="panel-heading">Nomination Information</div>
			<div class="panel-body">
				<table class="TableSpacing">
					<tr>
						<td align="center" colspan="2">
							<asp:Label ID="lblUpdateNotes" runat="server" class="alert alert-warning" role="alert">NOTE: Fields left Blank will *not* be considered for Adudication</asp:Label>
						</td>
					</tr>
					<tr>
						<td align="center" colspan="2">
							<asp:Label ID="lblSuccess" runat="server" CssClass="form-control" Font-Bold="True" Visible="False" ForeColor="Green"></asp:Label></td>
					</tr>
					<tr>
						<td align="right">Director:</td>
						<td align="left">
							<asp:TextBox ID="txtDirector" runat="server" CssClass="form-control" Width="39%">To Be Announced</asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							<asp:Label ID="lblMusicDirector" runat="server" Enabled="False">Musical Director:</asp:Label></td>
						<td align="left">
							<asp:TextBox ID="txtMusicalDirector" runat="server" CssClass="form-control" Width="39%" Enabled="False"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">
							<asp:Label ID="lblChoreographer" runat="server" Enabled="False">Choreographer:</asp:Label></td>
						<td align="left">
							<asp:TextBox ID="txtChoreographer" runat="server" CssClass="form-control" Width="39%" Enabled="False"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Scenic Designer:</td>
						<td align="left">
							<asp:TextBox ID="txtScenicDesigner" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">Lighting Designer:</td>
						<td align="left">
							<asp:TextBox ID="txtLightingDesigner" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">Sound Designer:</td>
						<td align="left">
							<asp:TextBox ID="txtSoundDesigner" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">Costume Designer:</td>
						<td align="left">
							<asp:TextBox ID="txtCostumeDesigner" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">
							<asp:Label ID="lblOriginalPlaywright" runat="server" Enabled="False">Original Playwright:</asp:Label></td>
						<td align="left">
							<asp:TextBox ID="txtOriginalPlaywright" runat="server" CssClass="form-control" Width="39%" Enabled="False"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Best
							<asp:DropDownList ID="ddlBestPerformer1Gender" runat="server" CssClass="form-control" Width="35%">
								<asp:ListItem Selected="True"></asp:ListItem>
								<asp:ListItem Value="Actor">Actor</asp:ListItem>
								<asp:ListItem Value="Actress">Actress</asp:ListItem>
							</asp:DropDownList>&nbsp;#1:</td>
						<td align="left">
							<asp:TextBox ID="txtBestActor1Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestActor1Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="right">Best
							<asp:DropDownList ID="ddlBestPerformer2Gender" runat="server" CssClass="form-control" Width="35%">
								<asp:ListItem Selected="True"></asp:ListItem>
								<asp:ListItem Value="Actor">Actor</asp:ListItem>
								<asp:ListItem Value="Actress">Actress</asp:ListItem>
							</asp:DropDownList>&nbsp;#2:</td>
						<td align="left">
							<asp:TextBox ID="txtBestActor2Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestActor2Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Best Supporting Actor #1:</td>
						<td align="left">
							<asp:TextBox ID="txtBestSupportingActor1Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestSupportingActor1Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Best Supporting Actor #2:</td>
						<td align="left">
							<asp:TextBox ID="txtBestSupportingActor2Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestSupportingActor2Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Best Supporting Actress #1:</td>
						<td align="left">
							<asp:TextBox ID="txtBestSupportingActress1Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestSupportingActress1Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox></td>
					</tr>
					<tr>
						<td align="right">Best Supporting Actress #2:</td>
						<td align="left">
							<asp:TextBox ID="txtBestSupportingActress2Name" runat="server" CssClass="form-control" Width="39%"></asp:TextBox>&nbsp;in role(s) of
							<asp:TextBox ID="txtBestSupportingActress2Role" runat="server" CssClass="form-control" Width="39%"></asp:TextBox></td>
					</tr>
				</table>
			</div>
		</div>

		<div class="panel panel-dark">
			<div class="panel-heading">Administrative Information</div>
			<div class="panel-body">
				<table class="TableSpacing">
					<tr>
						<td style="text-align: right; width: 40%;">Last Updated By:</td>
						<td style="text-align: left; width: 60%;">
							<asp:Label ID="lblLastUpdateByName" runat="server" ForeColor="Gray"></asp:Label>&nbsp;on&nbsp;
							<asp:Label ID="lblLastUpdateByDate" runat="server" ForeColor="Gray"></asp:Label>
						</td>
					</tr>
				</table>

				<table class="TableSpacing">
					<tr>
						<td style="width: 60%">Additional Comments for Producing Company & Assigned Adjudicators:<br />
							<asp:TextBox ID="txtAdminEmailComments" runat="server" Width="100%" TextMode="MultiLine" Rows="5" CssClass="form-control TextSmall"></asp:TextBox>
						</td>
						<td style="width: 40%">
							<asp:RadioButtonList ID="rblEmailInfo" runat="server" Width="100%">
								<asp:ListItem Value="NoAction">&nbsp;Do not Email Comments</asp:ListItem>
								<asp:ListItem Value="EmailAssignmentToAll" Selected="True">&nbsp;Email Comments</asp:ListItem>
							</asp:RadioButtonList>
						</td>
					</tr>
				</table>

			</div>
		</div>

		<div class="TextCenter">
			<asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-gold" Text="Save"></asp:Button>
			<asp:Button ID="btnDelete" runat="server" CssClass="btn btn-gold" Text="Delete"></asp:Button>
			<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button><br />
			Note: Items with a red asterisk (<span style="color: red;">*) indicate Required Fields.
		</div>

	</asp:Panel>

	<asp:Panel ID="pnlDeleteConfirm" runat="server" Visible="False" Width="100%">
		<div class="panel panel-dark">
			<div class="panel-heading">Delete Nomination</div>
			<div class="panel-body">
				<div class="alert alert-danger" role="alert">
					<p style="text-align: center; font-weight: bold;">NOTE: Deletions Cannot be Undone!</p>
				</div>
				<asp:Panel ID="pnlConfirmDeleteInfo" runat="server" Width="100%">
					<asp:Label ID="lblConfirmDelete" runat="server"></asp:Label>
					<ul>
						<li>
							<asp:Label ID="lblScoreProduction" runat="server"></asp:Label></li>
						<li>
							<asp:Label ID="lblScoreProducingCompany" runat="server"></asp:Label></li>
					</ul>
				</asp:Panel>
				<asp:Label ID="lblCannotDelete" Font-Bold="True" ForeColor="Firebrick" runat="server"></asp:Label>
				<div class="panel-body">
					<asp:DataGrid ID="gridSub" runat="server" Visible="False" BorderColor="#000000"
						BorderStyle="Double" Width="100%" HorizontalAlign="Left" CellPadding="2" AutoGenerateColumns="False"
						DataKeyField="FK_NominationsID" AllowPaging="False" BorderWidth="1px" GridLines="Horizontal">
						<FooterStyle ForeColor="Black"></FooterStyle>
						<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
						<AlternatingItemStyle HorizontalAlign="Left"></AlternatingItemStyle>
						<ItemStyle ForeColor="#333333"></ItemStyle>
						<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="Black"></HeaderStyle>
						<Columns>
							<asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="FK_NominationsID" HeaderText="FK_NominationsID"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="PK_UserID" HeaderText="PK_UserID"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
							<asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Adjudicator">
								<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
								<ItemStyle HorizontalAlign="Left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Representing Company">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="AdjudicatorToSeeProduction" SortExpression="AdjudicatorToSeeProduction"
								HeaderText="Confirm Adjud.">
								<HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" Width="60px"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="TotalScore" SortExpression="TotalScore" HeaderText="Scoring">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:BoundColumn>
							<asp:TemplateColumn SortExpression="LastUpdateByDate" HeaderText="Last Updated">
								<HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
								<ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
								<ItemTemplate>
									<%# DataBinder.Eval(Container.DataItem, "LastUpdateByDate","{0:MM/dd/yy}") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual" HeaderText="ProductionDateAdjudicated_Actual"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="AdjudicatorRequestsReassignment"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="CreateByName" HeaderText="CreateByName"></asp:BoundColumn>
							<asp:BoundColumn Visible="False" DataField="CreateByDate" HeaderText="CreateByDate"></asp:BoundColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="Black" Mode="NumericPages"></PagerStyle>
					</asp:DataGrid>
				</div>
				<div class="TextCenter">
					<asp:Button ID="btnDeleteConfirm" runat="server" CssClass="btn btn-gold" Text="Delete"></asp:Button>
					<asp:Button ID="btnDeleteCancel" runat="server" CssClass="btn btn-gold" Text="Cancel"></asp:Button>
				</div>
			</div>
		</div>
	</asp:Panel>
</asp:Content>
