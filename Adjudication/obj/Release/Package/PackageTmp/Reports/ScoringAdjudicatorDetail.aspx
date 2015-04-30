<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScoringAdjudicatorDetail.aspx.vb" Inherits="Adjudication.ScoringAdjudicatorDetail" %>

<!DOCTYPE html>
<html>
<head>
	<title>Score Detail by Adjudicator</title>
	
	
	
	
	<link href="../Styles.css" type="text/css" rel="STYLESHEET" />
</head>
<body>
	<form class="container Text" id="MainMenu" method="post" runat="server">
	<table class="TableSpacing">
		<tr>
			<td   align="center">
				<asp:Label ID="lblErrors" runat="server" ForeColor="red" Visible="False"  ></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="fontTitle" align="center" height="30">
				Scoring Adjudicator Detail for<br />
				<asp:Label ID="lblCategoryName" runat="server" CssClass="fontTitle" Font-Bold="true" Text="CATEGORY_NAME"></asp:Label>
			</td>
		</tr>
		<tr>
			<td   valign="top" align="left" width="100%" colspan="2">
				<asp:DataGrid ID="grid_Scoring" runat="server"   AutoGenerateColumns="False" AllowSorting="False" width="100%" AllowPaging="False" ShowFooter="False" BorderStyle="Solid" BorderColor="darkgray" GridLines="Horizontal" BorderWidth="1px" DataKeyField="PK_ScoringID" CellPadding="0" CellSpacing="0" HorizontalAlign="Left" ShowHeader="False">
					<ItemStyle ForeColor="#333333"></ItemStyle>
					<Columns>
						<asp:BoundColumn Visible="False" DataField="PK_ProductionID"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="ProductionCategory"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="ProductionType"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="Title"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="CompanyName"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="BestName"></asp:BoundColumn>
						<asp:BoundColumn Visible="False" DataField="BestRole"></asp:BoundColumn>
						<asp:TemplateColumn>
							<ItemTemplate>
								<asp:Table runat="server" ID="tblHeader" width="100%" CellPadding="0" CellSpacing="0" Visible="False">
									<asp:TableRow CssClass="TextSmall_ReportHeader" BackColor="Cornsilk" Font-Bold="True">
										<asp:TableCell Width="100" HorizontalAlign="Left">Type</asp:TableCell>
										<asp:TableCell Width="150" HorizontalAlign="Left">Production</asp:TableCell>
										<asp:TableCell Width="200" HorizontalAlign="Left">Nominee</asp:TableCell>
										<asp:TableCell Width="160"></asp:TableCell>
										<asp:TableCell Width="40" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													Used<br />Rsrve<br />Ballot
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													Total<br />Score
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													Max<br />Score
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													Min<br />Score
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													# Adj<br />with<br />Ballot
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													#<br />Ballots<br />Used
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center" ForeColor="black" Font-Bold="True">
													Total<br />Score<br />Final
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="right" ForeColor="black" Font-Bold="True">
													Final<br />Avg<br />Score
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<asp:Table runat="server" ID="tblProdData" width="100%" CellPadding="0" CellSpacing="0" Visible="False">
									<asp:TableRow CssClass="fontData_LightBorder" Font-Bold="True" BackColor="WhiteSmoke">
										<asp:TableCell Width="100" HorizontalAlign="Left">
													<%# DataBinder.Eval(Container.DataItem, "ProductionType") %>
										</asp:TableCell>
										<asp:TableCell Width="150" HorizontalAlign="Left">
													<%# DataBinder.Eval(Container.DataItem, "Title") %>
										</asp:TableCell>
										<asp:TableCell Width="200" HorizontalAlign="Left">
													<%# DataBinder.Eval(Container.DataItem, "BestName") %>
										</asp:TableCell>
										<asp:TableCell Width="160"></asp:TableCell>
										<asp:TableCell Width="40" HorizontalAlign="Center">
													<%# DataBinder.Eval(Container.DataItem, "UsingReserveAdjudicatorScore") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"####") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# DataBinder.Eval(Container.DataItem, "MaxScore") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# DataBinder.Eval(Container.DataItem, "MinScore") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# DataBinder.Eval(Container.DataItem, "NumOfAdjudicatorsWithCompletedBallot") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# DataBinder.Eval(Container.DataItem, "NumOfAdjudicatorsForProduction") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="Center">
													<%# format(DataBinder.Eval(Container.DataItem, "TotalScoreFinal") ,"####") %>
										</asp:TableCell>
										<asp:TableCell Width="50" HorizontalAlign="right">
													<%# format(DataBinder.Eval(Container.DataItem, "AverageScoreFinal") ,"####0.00") %>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow CssClass="TextSmall" Font-Bold="True" Height="18" VerticalAlign="Top" BackColor="WhiteSmoke">
										<asp:TableCell Width="100" HorizontalAlign="Left">
													<%# DataBinder.Eval(Container.DataItem, "ProductionCategory") %>
										</asp:TableCell>
										<asp:TableCell Width="150" HorizontalAlign="Left">
													<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>
										</asp:TableCell>
										<asp:TableCell Width="200" HorizontalAlign="Left" ColumnSpan="10" Font-Italic="True">
													<%# DataBinder.Eval(Container.DataItem, "BestRole") %>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<table width="100%"   cellpadding="0" cellspacing="0" align="left">
									<tr>
										<td width="100" align="left">
											<%# DataBinder.Eval(Container.DataItem, "ProductionType") %>
										</td>
										<td width="150" align="left">
											<%# DataBinder.Eval(Container.DataItem, "Title") %>
										</td>
										<td width="200" align="left">
											<%# DataBinder.Eval(Container.DataItem, "BestName") %>
										</td>
										<td width="200" align="left">
											<b>
												<%# DataBinder.Eval(Container.DataItem, "AdjudicatorName") %></b> <font class="TextSmall" style="color: firebrick; font-variant: small-caps">
													<%# IIF(DataBinder.Eval(Container.DataItem, "ReserveAdjudicator")=1,"Reserve","")  %>
												</span>
										</td>
										<td width="50" align="Center">
											<%# DataBinder.Eval(Container.DataItem, "AdjudicatorScore") %>
										</td>
										<td width="300">
										</td>
									</tr>
									<tr>
										<td width="100" align="left" class="TextSmall">
											<%# DataBinder.Eval(Container.DataItem, "ProductionCategory") %>
										</td>
										<td width="150" align="left" class="TextSmall">
											<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>
										</td>
										<td width="200" align="left" class="TextSmall">
											<i>
												<%# DataBinder.Eval(Container.DataItem, "BestRole") %></i>
										</td>
										<td width="550" align="left" class="TextSmall" colspan="8">
											<%# DataBinder.Eval(Container.DataItem, "AdjudicatorCompanyRepresented") %>
										</td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
			</td>
		</tr>
		<tr>
			<td class="fontFooter" valign="middle" align="center" height="24">
				<span style="color: red;">NOTE: For Productions with less than 4 Adjudicators, the High and Low Scores will not be dropped.</span>
			</td>
		</tr>
		<tr>
			<td   align="right" colspan="4">
				Logged in as
				<asp:Label ID="lblLoginID" runat="server"   Font-Bold="True"></asp:Label>
			</td>
		</tr>
	</table>
	</TD></TR></TABLE></form>
</body>
</html>
