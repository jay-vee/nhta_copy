<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ScoringProductionStaff.aspx.vb" Inherits="Adjudication.ScoringProductionStaff" %>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>Scoring Detail</title>
		
		
		
		
		<LINK href="../Styles.css" type="text/css" rel="STYLESHEET" />
	</HEAD>
	<body >
		<form class="container" class="fontData" id="MainMenu" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="800">
				<tr>
					<td class="ReportHeader800" align="center" colSpan="3" height="30">
						Summary Production Staff Scoring
					</td>
				</tr>
				<tr>
					<td colSpan="3"><IMG src="../Images/NHTAGradientHorizontal800.jpg" /></td>
				</tr>
				<tr>
					<td colSpan="3" class="fontFooter" align="center" height="24" valign="middle"><span style="color: red;">NOTE: 
							For Productions with less than 4 Adjudicators, the High and Low Scores will not 
							be dropped.</span></td>
				</tr>
				<tr>
					<td class="fontData" vAlign="top" align="left" width="800" colSpan="2"><asp:panel id="pnlGrid" runat="server">
							<TABLE id="tblMain" cellSpacing="0" cellPadding="0" width="800" border="0">
								<TR>
									<TD class="fontDataHeader" colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD class="fontDataHeader" align="left" width="240">Director</TD>
									<TD class="fontDataHeader" align="left" width="240">Production
									</TD>
									<TD class="fontDataHeader" align="left" width="180">Adjudicator
									</TD>
									<TD class="fontDataHeader" align="center" width="80">
									Performance Dates
									<TD class="fontDataHeader" align="right" width="60">Score
									</TD>
								</TR>
								<TR>
									<TD colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD colSpan="8">
										<asp:datagrid id="gridMain" runat="server" AutoGenerateColumns="False" CssClass="fontDatasmall"
											Width="800px" BorderStyle="None" BorderColor="Gainsboro" GridLines="Horizontal" BorderWidth="0px"
											DataKeyField="PK_ScoringID" CellPadding="0" CellSpacing="0" HorizontalAlign="Left">
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="TotalScore" HeaderText="TotalScore"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="BestName" HeaderText="BestName"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="CompanyName" HeaderText="CompanyName"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="Title" HeaderText="Title"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="AverageScore" HeaderText="AverageScore"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="ProductionCategory" HeaderText="ProductionCategory"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="ProductionType" HeaderText="ProductionType"></asp:BoundColumn>
												<asp:TemplateColumn>
													<ItemTemplate>
														<table width="800" class="fontData" cellpadding="0" cellspacing="0" align="left">
															<tr>
																<td colspan="8" align="right" height="0" class="fontHeaderLight">
																	<asp:Label Runat="server" ID="AverageScore" Visible="False" Width="800" BackColor="LightGoldenrodYellow"></asp:Label></td>
															</tr>
															<tr>
																<td colspan="10" align="left" height="0" class="DefaultGridHeader800">
																	<asp:Label Runat="server" ID="lblCategory" Visible="False"><%# DataBinder.Eval(Container.DataItem, "ProductionCategory")%> - <%# DataBinder.Eval(Container.DataItem, "ProductionType") %></asp:Label></td>
															</tr>
															<tr>
																<td width="240" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "BestName") %>
																</td>
																<td width="240" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "Title") %>
																</td>
																<td width="180" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "FullName") %>
																</td>
																<td width="80" align="center">
																	<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
																</td>
																<td width="60" align="right" rowspan="2">
																	<%# format(DataBinder.Eval(Container.DataItem, "TotalScore") ,"##,##0.0") %>
																</td>
															</tr>
															<tr>
																<td width="240" align="left" class="fontDatasmall"></td>
																<td width="240" align="left" class="fontDatasmall"><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
																<td width="180" align="left" class="fontDatasmall"><%# DataBinder.Eval(Container.DataItem, "AdjudicatorCompanyRepresented") %></td>
																<td width="80" align="center"><%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %></td>
															</tr>
														</table>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD class="fontHeaderLight" align="right" bgColor="lightgoldenrodyellow" colSpan="8">
										<asp:Label id="lblLastScore" CssClass="fontData" Visible="False" Runat="server"></asp:Label></TD>
								</TR>
								<TR>
									<TD vAlign="top" colSpan="8" height="12"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD class="fontData" colSpan="2">
										<asp:Label id="lblTotalNumberOfRecords" runat="server" CssClass="fontData" ForeColor="Black"> Number of Scores: 0</asp:Label></TD>
									<TD></TD>
									<TD class="fontData" align="right" colSpan="4">Logged in as
										<asp:label id="lblLoginID" runat="server" CssClass="fontData" Font-Bold="True"></asp:label></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
