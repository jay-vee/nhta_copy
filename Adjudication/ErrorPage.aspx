<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ErrorPage.aspx.vb" Inherits="Adjudication.ErrorPage"%>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>Error Encountered</title>
		
		
		
		
		<LINK href="Styles.css" type="text/css" rel="STYLESHEET" />
		<script src="Includes/Functions.js" type="text/javascript"></script>
	</HEAD>
	<body  onload='ShowAccessNavs("<%Response.Write(Session.Item("AccessLevel").ToString())%>");'>
		<form class="container" id="MainMenu" method="post" runat="server"  >
			<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td colspan="2" width="100%"> 
					</td>
				</tr>
				<tr>
					<td id="oldLeftNav_DeleteThis_cell"> 
					</td>
					<td width="txt" align="left" valign="top">
						<TABLE id="tblPageTitle" width="txt">
							<TR>
								<TD class="LabelMediumBold" align="left">Error Found
								</TD>
								<td   align="right"></td>
							</TR>
							<tr>
								<td colspan="2"><div class="separatorBlack">&nbsp;</div></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<tr>
					<td align="left" valign="top" width="txt"    colspan="2">
						<table width="txt">
							<tr>
								<td>
									<TABLE class="LabelLarge" id="tblDefaultMessages" borderColor="black" cellSpacing="0"
										cellPadding="4" width="100%" border="2">
										<tr>
										</tr>
										<TR>
											<td class="LabelLarge" align="left">
												<P><STRONG>The application has encountered an Error</STRONG></P>
												<P><FONT color="#ff0000"> To begin to resolve the issue please Email contact the 
														Administrator.
														<br />
														<br />
														Please explain in detail what you were doing just before you were directed to 
														this page</span></P>
												<P><FONT color="#ff0000">Please copy the Error message below into the Email you will be 
														sending.</span>
												</P>
												<p>Thank You</p>
												<P></P>
											</td>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td>
									<TABLE class="LabelLarge" id="tblErrorMessages" borderColor="black" cellSpacing="0"
										cellPadding="4" width="650" border="2">
										<tr>
										</tr>
										<TR>
											<td class="LabelLarge" align="left">
												<asp:Button Runat="server" ID="lbtnViewError" Text="View Error Message" CssClass="LabelLarge"></asp:Button>
												<asp:Panel id="pnlErrorMessage" runat="server" Visible="False" Height="200px" Width="650">
													<TABLE class="LabelLarge" id="tblDisplayErrors">
														<TR>
															<TD vAlign="top"><B>Error Message:</B></TD>
														</TR>
														<TR>
															<TD vAlign="top">
																<asp:TextBox id="txtErrorMessage"   Runat="server" Width="640" Height="300px"
																	ReadOnly="True" TextMode="MultiLine" BorderStyle="None" Font-Names="courier new">Unknown</asp:TextBox></TD>
														</TR>
													</TABLE>
												</asp:Panel>
											</td>
										</TR>
									</TABLE>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
