<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdjReassignRequest.aspx.vb" Inherits="Adjudication.AdjReassignRequest_Web"%>
<!DOCTYPE html>
<HTML>
	<HEAD>
		<title>REPORT: Adjudicator Late Confirmations</title>
		
		
		
		
		<LINK href="../Styles.css" type="text/css" rel="STYLESHEET" />
	</HEAD>
	<body >
		<form class="container" id="Form1" method="post" runat="server">
			<table   id="tblTitle" cellSpacing="0" cellPadding="0" width="1240" border="0">
				<tr>
					<td class="ReportHeader1240" align="center" colSpan="3" height="30">Late Production 
						Scoring
					</td>
				</tr>
				<tr>
					<td colSpan="3" class="separatorBlack"></td>
				</tr>
				<tr>
					<td colSpan="3"><asp:datagrid id="gridMain" runat="server" Width="1240px" BorderStyle="None" BorderColor="Gainsboro"
							GridLines="Horizontal" AllowSorting="True" BorderWidth="1px" DataKeyField="PK_ScoringID" AutoGenerateColumns="False"
							CellPadding="2" HorizontalAlign="Left"  >
							<AlternatingItemStyle BackColor="White"></AlternatingItemStyle>
							<ItemStyle BackColor="WhiteSmoke"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="Black" BorderColor="Black" BackColor="White"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Title" SortExpression="Title" HeaderText="Production">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="CompanyName" SortExpression="CompanyName" HeaderText="Producing Company">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="FirstPerformanceDateTime" HeaderText="Open Date">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "FirstPerformanceDateTime","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="LastPerformanceDateTime" HeaderText="Close Date">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "LastPerformanceDateTime","{0:MM/dd/yy}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Requests Reassignment">
									<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center" Width="80px" ForeColor="red"></ItemStyle>
									<ItemTemplate>
										YES
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="FullName" SortExpression="FullName" HeaderText="Assigned Adjudicator">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="EmailPrimary" HeaderText="Email Address">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
									<ItemTemplate>
										<a class=fontDataHyperlink href='mailto:<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>'>
											<%# DataBinder.Eval(Container.DataItem, "EmailPrimary") %>
										</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="PhonePrimary" SortExpression="PhonePrimary" HeaderText="Phone #">
									<HeaderStyle HorizontalAlign="left"></HeaderStyle>
									<ItemStyle HorizontalAlign="left"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td colSpan="3" class="separatorBlack"></td>
				</tr>
				<tr>
					<td><asp:label id="lblTotalNumberOfRecords" runat="server" ForeColor="Black">Number of Requested Reassignments: 0</asp:label></td>
					<td><asp:textbox id="txtSortOrder" runat="server" Width="64px" BorderStyle="Dotted"  
							  Visible="False">LastPerformanceDateTime</asp:textbox><asp:textbox id="txtSortColumnName" runat="server" Width="64px" BorderStyle="Dotted"  
							Visible="False"></asp:textbox></td>
					<td   align="right">Logged in as
						<asp:label id="lblLoginID" runat="server"  ></asp:label></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
