<%@ Page Language="vb" Title="Ballots" MasterPageFile="~/MasterPage.Master" AutoEventWireup="false" CodeBehind="AdminScoring.aspx.vb" Inherits="Adjudication.AdminScoring" %>

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
	<asp:UpdatePanel runat="server" ID="UpdatePanelMain">
		<ContentTemplate>
			<div class="text-center">
				<asp:Label ID="lblSuccessful" runat="server" ForeColor="Green" Visible="False">Update Successful</asp:Label>
			</div>
			<asp:Panel ID="pnlGrid" runat="server">
				<table id="tblMain" class="TableSpacing">
					<tr>
						<td align="center" width="100%" colspan="2">
							<asp:Label ID="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Ballots: 0</asp:Label>&nbsp;
										<asp:TextBox ID="txtSortColumnName" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>
							<asp:TextBox ID="txtSortOrder" runat="server" Visible="False" BorderStyle="Dotted"></asp:TextBox>&nbsp;
						</td>
						<tr>
					<tr>
						<td colspan="2">
							<asp:DataGrid ID="gridMain" runat="server" Width="100%" BorderStyle="Double"
								BorderColor="#000000" GridLines="Horizontal" BorderWidth="1px" AllowPaging="False" DataKeyField="PK_ScoringID"
								AutoGenerateColumns="False" AllowSorting="True" CellPadding="2" CellSpacing="4" HorizontalAlign="Left" OnItemCommand="gridMain_ItemSelect">
								<FooterStyle ForeColor="#000000"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#FFFF99"></SelectedItemStyle>
								<AlternatingItemStyle HorizontalAlign="Left" BackColor="LemonChiffon"></AlternatingItemStyle>
								<ItemStyle ForeColor="#333333"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000000"></HeaderStyle>
								<Columns>
									<asp:BoundColumn Visible="False" DataField="PK_ScoringID" HeaderText="PK_ScoringID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PK_NominationsID" HeaderText="PK_NominationsID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PK_CompanyID" HeaderText="PK_CompanyID"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="PK_UserID" HeaderText="PK_UserID"></asp:BoundColumn>
									<asp:TemplateColumn ItemStyle-Width="50px">
										<ItemTemplate>
											<asp:LinkButton ID="btnEditSummary" Text="Summary" CommandName="Edit_Command"
												ForeColor="blue" runat="server" /><br />
											<asp:LinkButton ID="btnEditDetail" Text="Detail" CommandName="Edit_Ballot"
												ForeColor="blue" runat="server" /><br />
											<asp:LinkButton ID="btnPrintSummary" Text="Print" CommandName="Print"
												ForeColor="blue" runat="server" />
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="TotalScore" SortExpression="TotalScore" HeaderText="Total Score">
										<HeaderStyle HorizontalAlign="Center" Width="60"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="6"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Adjudicator">
										<HeaderStyle HorizontalAlign="left"></HeaderStyle>
										<ItemStyle HorizontalAlign="left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
										<HeaderStyle HorizontalAlign="left"></HeaderStyle>
										<ItemStyle HorizontalAlign="left" Font-Bold="True"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
										<HeaderStyle HorizontalAlign="left"></HeaderStyle>
										<ItemStyle HorizontalAlign="left"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="Show Dates">
										<HeaderStyle HorizontalAlign="Center" Width="50"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="50"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
											<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="ScoringStatus" SortExpression="ScoringStatus" HeaderText="Status">
										<HeaderStyle HorizontalAlign="Center" Width="160"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="160" CssClass="TextSmall"></ItemStyle>
									</asp:BoundColumn>
									<asp:TemplateColumn SortExpression="ProductionDateAdjudicated_Planned" HeaderText="Planned Adjud.">
										<HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Planned","{0:MM/dd/yy}") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn SortExpression="ProductionDateAdjudicated_Actual" HeaderText="Actual Adjud.">
										<HeaderStyle HorizontalAlign="Center" Width="70"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="70"></ItemStyle>
										<ItemTemplate>
											<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Actual","{0:MM/dd/yy}") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn Visible="False" DataField="FirstPerformanceDateTime" HeaderText="FirstPerformanceDateTime"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="LastPerformanceDateTime" HeaderText="LastPerformanceDateTime"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="ProductionType"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Planned" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="ProductionDateAdjudicated_Actual" HeaderText="ProductionDateAdjudicated_Planned"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="AdjudicatorRequestsReassignment" HeaderText="AdjudicatorRequestsReassignment"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="LastUpdateByName" HeaderText="LastUpdateByName"></asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="LastUpdateByDate" HeaderText="LastUpdateByDate"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Center" ForeColor="White" BackColor="#000000" Mode="NumericPages"></PagerStyle>
							</asp:DataGrid></td>
					</tr>
				</table>
			</asp:Panel>


		</ContentTemplate>
	</asp:UpdatePanel>
	<script id="scptDocumentReady" type="text/javascript">
		//=========== document.ready ===========
		$(document).ready(function () {

		});
	</script>

</asp:Content>
