<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ScoringDetail.aspx.vb" Inherits="Adjudication.ScoringDetail_Web"%>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>Scoring Detail</title>
		
		
		
		
		<LINK href="../Styles.css" type="text/css" rel="STYLESHEET" />
	</HEAD>
	<body >
		<form class="container Text" id="MainMenu" method="post" runat="server">
			<table class="TableSpacing">
				<tr>
					<td class="fontScoringHeader" align="center" colSpan="3" height="30">Production 
						Scoring Detail
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
					<td   vAlign="top" align="left" width="100%" colSpan="2"><asp:panel id="pnlGrid" runat="server">
							<TABLE id="tblMain" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD class="fontDataHeader" colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD class="fontDataHeader" align="left" width="190">Production
									</TD>
									<TD class="fontDataHeader" align="left" width="180">Adjudicator
									</TD>
									<TD class="fontDataHeader" align="left" width="90">Production Category
									</TD>
									<TD class="fontDataHeader" align="center" width="40"># Noms
									</TD>
									<TD class="fontDataHeader" align="center" width="80">Performance Dates
									</TD>
									<TD class="fontDataHeader" align="center" width="80">Attended Adj. Date
									</TD>
									<TD class="fontDataHeader" align="center" width="80">Confirmed Adj. Date
									</TD>
									<TD class="fontDataHeader" align="right" width="60">Score Average
									</TD>
								</TR>
								<TR>
									<TD colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD colSpan="8">
										<asp:datagrid id="gridMain" runat="server" HorizontalAlign="Left" CellSpacing="0" CellPadding="0"
											DataKeyField="PK_ScoringID" BorderWidth="0px" GridLines="Horizontal" BorderColor="Gainsboro"
											BorderStyle="None" width="100%" CssClass="TextSmall" AutoGenerateColumns="False">
											<HeaderStyle></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="PK_ProductionID" HeaderText="PK_ProductionID"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="TotalScore" HeaderText="TotalScore"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="NumberOfNominations" HeaderText="NumberOfNominations"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="Title" HeaderText="Title"></asp:BoundColumn>
												<asp:TemplateColumn>
													<ItemTemplate>
														<table width="100%"   cellpadding="0" cellspacing="0" align="left">
															<tr>
																<td colspan="8" align="right" height="0" class="LabelLarge">
																	<asp:Label Runat="server" ID="ProductionScore" Visible="False" width="100%" BackColor="LightGoldenrodYellow"></asp:Label></td>
															</tr>
															<TR>
																<TD colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
															</TR>
															<tr>
																<td width="190" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "Title") %>
																</td>
																<td width="180" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "FullName") %>
																</td>
																<td width="90" align="left">
																	<%# DataBinder.Eval(Container.DataItem, "ProductionCategory")%>
																</td>
																<td width="40" align="center" rowspan="2" class="TextSmall">
																	<%# DataBinder.Eval(Container.DataItem, "NumberOfNominations","{0:##,##0}") %>
																</td>
																<td width="80" align="center">
																	<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
																</td>
																<td width="80" align="center" rowspan="2">
																	<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Planned","{0:MM/dd/yy}") %>
																</td>
																<td width="80" align="center" rowspan="2">
																	<%# DataBinder.Eval(Container.DataItem, "ProductionDateAdjudicated_Actual","{0:MM/dd/yy}") %>
																</td>
																<td width="60" align="right" rowspan="2">
																	<%# format(DataBinder.Eval(Container.DataItem, "TotalScore") / DataBinder.Eval(Container.DataItem, "NumberOfNominations") ,"##,##0.0") %>
																</td>
															</tr>
															<tr>
																<td width="190" align="left" class="TextSmall"><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></td>
																<td width="180" align="left" class="TextSmall"><%# DataBinder.Eval(Container.DataItem, "AdjudicatorCompanyRepresented") %></td>
																<td width="90" align="left"><%# DataBinder.Eval(Container.DataItem, "ProductionType") %></td>
																<td width="80" align="center"><%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %></td>
															</tr>
															<tr>
																<td colSpan="8" align="right">
																	<table width="520" Class="TextSmall" cellSpacing="0" cellPadding="0">
																		<tr>
																			<td align="Left" width="160" bgcolor="LemonChiffon"><B>Nomination Category</B></td>
																			<td align="Left" width="300" bgcolor="LemonChiffon"><B>Person Nominated</B></td>
																			<td align="right" width="60" bgcolor="LemonChiffon"><B>Score</B></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Director:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "Director") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "DirectorScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Music Director:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "MusicalDirector") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "MusicalDirectorScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Choreographer:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "Choreographer") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "ChoreographerScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Scenic Designer:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "ScenicDesigner") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "ScenicDesignerScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Lighting Designer:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "LightingDesigner") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "LightingDesignerScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Sound Designer:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "SoundDesigner") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "SoundDesignerScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Costume Designer:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "CostumeDesigner") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "CostumeDesignerScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Original Playwright:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "OriginalPlaywright") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "OriginalPlaywrightScore") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Performer #1:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestPerformer1") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestPerformer1Score") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Performer #2:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestPerformer2") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestPerformer2Score") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Supporting Actor #1:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActor1") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActor1Score") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Supporting Actor #2:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActor2") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActor2Score") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Supporting Actress #1:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActress1") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActress1Score") %></td>
																		</tr>
																		<tr>
																			<td align="Left" width="160">Best Supporting Actress #2:</td>
																			<td align="Left" width="300"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActress2") %></td>
																			<td align="right" width="60"><%# DataBinder.Eval(Container.DataItem, "BestSupportingActress2Score") %></td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid></TD>
								</TR>
								<TR>
									<TD class="LabelLarge" align="right" bgColor="lightgoldenrodyellow" colSpan="8">
										<asp:Label id="LastProductionScore"   Runat="server" Visible="False"></asp:Label></TD>
								</TR>
								<TR>
									<TD vAlign="top" colSpan="8" height="12"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD colSpan="8" height="4"><IMG src="../Images/NHTAGradientHorizontal.jpg" /></TD>
								</TR>
								<TR>
									<TD   colSpan="4">
										<asp:Label id="lblTotalNumberOfRecords" runat="server"   ForeColor="Black"> Number of Scores: 0</asp:Label></TD>
									<TD></TD>
									<TD   align="right" colSpan="4">Logged in as
										<asp:label id="lblLoginID" runat="server"   Font-Bold="True"></asp:label></TD>
								</TR>
							</TABLE>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
