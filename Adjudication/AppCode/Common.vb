Imports System.Data.SqlClient
Imports System.Data
Imports Adjudication.DataAccess


Public Class Common
    '====================================================================================================
    Public Const EMAIL_CUSTOM_MESSAGE As String = "1"
    Public Const EMAIL_LATE_BALLOTS As String = "2"
    Public Const EMAIL_LATE_ADJUDICATOR_CONFIRMATION As String = "3"
    Public Const EMAIL_LATE_LIAISON_CONFIRMATION As String = "4"
    Public Const EMAIL_LATE_NOMINATIONS As String = "5"
    Public Const EMAIL_REMINDER_NOMINATIONS As String = "6"
    Public Const EMAIL_REMINDER_ASSIGNMENTS As String = "7"
    Public Const EMAIL_NOMINATION_SET As String = "8"
    Public Const EMAIL_ADJUDICATOR_ASSIGNED As String = "9"
    Public Const EMAIL_BALLOT_SUBMITTED As String = "10"
    Public Const EMAIL_ADJUDICATOR_REASSIGNED As String = "11"
    '====================================================================================================

    Public Shared Function FormatEmailHTML_AutomatedEmailDisclaimer() As String
        '====================================================================================================
        Return "<hr noshade><B><I>ATTENTION:</b> This is email was automatically generated from the NHTA Adjudication Website.  Please let us know if you received this email in error.</i><br />"
    End Function

    Public Shared Function FormatEmailHTML_AssignedAdjudicator(ByVal dtAdj As DataTable) As String
        '====================================================================================================
        Dim sProductionDateAdjudicated_Planned As String, HTMLString As String = ""
        '====================================================================================================

        Try
            If dtAdj.Rows.Count > 0 Then
                If dtAdj.Rows(0).Item("ProductionDateAdjudicated_Planned").ToString.Length > 6 Then
                    Try
                        sProductionDateAdjudicated_Planned = CDate(dtAdj.Rows(0).Item("ProductionDateAdjudicated_Planned").ToString).ToLongDateString
                    Catch ex As Exception
                        sProductionDateAdjudicated_Planned = "<FONT Color=""RED"">Please Confirm with Producing Company</span>"
                    End Try
                Else
                    sProductionDateAdjudicated_Planned = "<FONT Color=""RED"">Please Confirm with Producing Company</span>"
                End If

                HTMLString = "<ul>" & _
                                "<LI>Adjudicator Assigned: <B>" & dtAdj.Rows(0)("FirstName").ToString & " " & dtAdj.Rows(0)("LastName").ToString & "</B>" & _
                                "<LI>Adjudicator Email Address: <B>" & Create_HTML_MailToLink(dtAdj.Rows(0)("EmailPrimary").ToString) & "</B>" & _
                                "<LI>Adjudicator Confirmation Date to Attend: <B>" & sProductionDateAdjudicated_Planned & "</B>" & _
                                "<LI>Adjudicator Represented Company: <B>" & dtAdj.Rows(0)("CompanyName").ToString & "</B>" & _
                                "<LI>Assignment Set by: <B>" & dtAdj.Rows(0)("LastUpdateByName").ToString & " on " & dtAdj.Rows(0)("LastUpdateByDate").ToString & "</B>" & _
                            "</UL>"
            Else
                HTMLString = "<FONT Color=""RED"">ERROR: No Assigned Adjudicator Information Found.</span><br />"
            End If
        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR:</span> " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function

    Public Shared Function FormatEmailHTML_AssignedAdjudicator(ByVal UserLoginID As String, ByVal ScoringID As String) As String
        '====================================================================================================
        Dim dtAdj As DataTable, sProductionDateAdjudicated_Planned As String, HTMLString As String = ""
        '====================================================================================================
        Try
            Try
                Dim IsThisTheID As Integer = CInt(UserLoginID)
                dtAdj = Find_AdjudicationsForUserLoginID(Get_LoginID(UserLoginID), ScoringID)
            Catch ex As Exception
                dtAdj = Find_AdjudicationsForUserLoginID(UserLoginID, ScoringID)
            End Try

            If dtAdj.Rows.Count > 0 Then
                If dtAdj.Rows(0).Item("ProductionDateAdjudicated_Planned").ToString.Length > 6 Then
                    Try
                        sProductionDateAdjudicated_Planned = CDate(dtAdj.Rows(0).Item("ProductionDateAdjudicated_Planned").ToString).ToLongDateString
                    Catch ex As Exception
                        sProductionDateAdjudicated_Planned = "<FONT Color=""RED"">Please Confirm with Producing Company</span>"
                    End Try
                Else
                    sProductionDateAdjudicated_Planned = "<FONT Color=""RED"">Please Confirm with Producing Company</span>"
                End If

                HTMLString = "<ul>" & _
                                "<LI>Adjudicator Assigned: <B>" & dtAdj.Rows(0)("FirstName").ToString & " " & dtAdj.Rows(0)("LastName").ToString & "</B>" & _
                                "<LI>Adjudicator Email Address: <B>" & Create_HTML_MailToLink(dtAdj.Rows(0)("EmailPrimary").ToString) & "</B>" & _
                                "<LI>Adjudicator Confirmation Date to Attend: <B>" & sProductionDateAdjudicated_Planned & "</B>" & _
                                "<LI>Adjudicator Represented Company: <B>" & dtAdj.Rows(0)("ScoringCompanyName").ToString & "</B>" & _
                                "<LI>Assignment Set by: <B>" & dtAdj.Rows(0)("LastUpdateByName").ToString & " on " & dtAdj.Rows(0)("LastUpdateByDate").ToString & "</B>" & _
                            "</UL>"
            Else
                HTMLString = "<FONT Color=""RED"">ERROR: No Assigned Adjudicator Information Found.</span><br />"
            End If
        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR: " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function


    Public Shared Function FormatEmailHTML_Nomination(ByVal ProductionID As String) As String
        '====================================================================================================
        Dim dtNom As DataTable, HTMLString As String = ""
        '====================================================================================================
        Try
            dtNom = Get_Nomination(ProductionID)       'Get the data for this PRoductionID

            HTMLString = FormatEmailHTML_Nomination(dtNom)

        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR: " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function

    Public Shared Function FormatEmailHTML_Nomination(ByVal dtNom As DataTable) As String
        '====================================================================================================
        Dim HTMLString As String = ""
        '====================================================================================================
        Try
            If dtNom.Rows.Count > 0 Then
                HTMLString = "<i><FONT size=""1"">Blank/empty Categories will not be adjudicated</span></i>" & _
                             "<UL>" & _
                                "<LI>Director: <b> " & dtNom.Rows(0)("Director").ToString & "</b>" & "</b>" & _
                                "<LI>Musical Director: <b> " & dtNom.Rows(0)("MusicalDirector").ToString & "</b>" & _
                                "<LI>Choreographer: <b> " & dtNom.Rows(0)("Choreographer").ToString & "</b>" & _
                                "<LI>Scenic Designer: <b> " & dtNom.Rows(0)("ScenicDesigner").ToString & "</b>" & _
                                "<LI>Lighting Designer: <b> " & dtNom.Rows(0)("LightingDesigner").ToString & "</b>" & _
                                "<LI>Sound Designer: <b> " & dtNom.Rows(0)("SoundDesigner").ToString & "</b>" & _
                                "<LI>Costume Designer: <b> " & dtNom.Rows(0)("CostumeDesigner").ToString & "</b>" & _
                                "<LI>Original Playwright: <b> " & dtNom.Rows(0)("OriginalPlaywright").ToString & "</b>" & _
                                "<LI>Best " & dtNom.Rows(0)("BestPerformer1Gender").ToString & " #1:<b> " & dtNom.Rows(0)("BestPerformer1Name").ToString & "</b>" & " in the Role of: <b> " & dtNom.Rows(0)("BestPerformer1Role").ToString & "</b>" & _
                                "<LI>Best " & dtNom.Rows(0)("BestPerformer2Gender").ToString & " #1:<b> " & dtNom.Rows(0)("BestPerformer2Name").ToString & "</b>" & " in the Role of: <b> " & dtNom.Rows(0)("BestPerformer2Role").ToString & "</b>" & _
                                "<LI>Best Supporting Actor #1: <b> " & dtNom.Rows(0)("BestSupportingActor1Name").ToString & "</b>" & " in the Role of: <b> " & dtNom.Rows(0)("BestSupportingActor1Role").ToString & "</b>" & _
                                "<LI>Best Supporting Actor #2: <b> " & dtNom.Rows(0)("BestSupportingActor2Name").ToString & "</b>" & " in the Role of :<b> " & dtNom.Rows(0)("BestSupportingActor2Role").ToString & "</b>" & _
                                "<LI>Best Supporting Actress #1: <b> " & dtNom.Rows(0)("BestSupportingActress1Name").ToString & "</b>" & " in the Role of: <b> " & dtNom.Rows(0)("BestSupportingActress1Role").ToString & "</b>" & _
                                "<LI>Best Supporting Actress #2: <b> " & dtNom.Rows(0)("BestSupportingActress2Name").ToString & "</b>" & " in the Role of: <b> " & dtNom.Rows(0)("BestSupportingActress2Role").ToString & "</b>" & _
                            "</UL>"
            Else
                HTMLString = "<FONT Color=""RED"">ERROR: No Production Information Found.</span><br />"
            End If
        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR: " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function

    Public Shared Function FormatEmailHTML_Production(ByVal dtProd As DataTable) As String
        '====================================================================================================
        Dim HTMLString As String = ""
        '====================================================================================================
        Try
            If dtProd.Rows.Count > 0 Then
                HTMLString = "<UL TYPE=CIRCLE>" & _
                                "<LI>Production Title:          <B>" & dtProd.Rows(0)("Title").ToString & "</B> (" & dtProd.Rows(0)("ProductionType").ToString & ") " & _
                                "<LI>Producing Theatre Company: <B>" & dtProd.Rows(0)("CompanyName").ToString & "</B>" & _
                                "<LI>Producing Theatre Email: <B>" & Create_HTML_MailToLink(dtProd.Rows(0)("CompanyEmailAddress").ToString) & "</B>" & _
                                "<LI>Producing Theatre Website: <B>" & Create_HTML_WebsiteHyperLink(dtProd.Rows(0)("CompanyWebsite").ToString) & "</B>" & _
                                "<LI>Production Dates: <B>" & dtProd.Rows(0)("FirstPerformanceDateTime") & "</B> thru <B>" & dtProd.Rows(0)("LastPerformanceDateTime") & "</B>" & _
                                "<LI>Performance Details: <B>" & dtProd.Rows(0)("AllPerformanceDatesTimes").ToString & "</B>" & _
                                "<LI>Production Comments: <B>" & dtProd.Rows(0)("Comments").ToString & "</B>" & _
                            "</UL><UL TYPE=DISC>" & _
                                "<LI>Venue Name: <B>" & dtProd.Rows(0)("VenueName").ToString & "</B>" & _
                                "<LI>Venue Location: <B>" & dtProd.Rows(0)("Address") & "  " & dtProd.Rows(0)("City") & ", " & dtProd.Rows(0)("State").ToString & "</B>" & _
                                "<LI>Venue Website: <B>" & Create_HTML_WebsiteHyperLink(dtProd.Rows(0)("Website").ToString) & "</B>" & _
                            "</UL><UL TYPE=SQUARE>" & _
                                "<LI>Tickets Contact: <B>" & dtProd.Rows(0)("TicketContactName").ToString & "</B>" & _
                                "<LI>Tickets Phone: <B>" & dtProd.Rows(0)("TicketContactPhone").ToString & "</B>" & _
                                "<LI>Tickets Email: <B>" & Create_HTML_MailToLink(dtProd.Rows(0)("TicketContactEmail").ToString) & "</B>" & _
                                "<LI>Tickets Details: <B>" & dtProd.Rows(0)("TicketPurchaseDetails").ToString & "</B>" & _
                            "</UL>"
            Else
                HTMLString = "<FONT Color=""RED"">ERROR: No Production Information Found.</span><br />"""
            End If
        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR: " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function

    Public Shared Function FormatEmailHTML_Production(ByVal ProductionID As String) As String
        '====================================================================================================
        Dim dtProd As DataTable, HTMLString As String = ""
        '====================================================================================================
        Try
            dtProd = Get_Production(ProductionID)       'Get the data for this PRoductionID

            HTMLString = FormatEmailHTML_Production(dtProd)

        Catch ex As Exception
            HTMLString = "<FONT Color=""RED"">ERROR: " & ex.Message & "</span><br />"
        End Try

        '====================================================================================================
        Return HTMLString
        '====================================================================================================

    End Function

    Public Shared Function Create_HTML_WebsiteHyperLink(ByVal Link As String) As String

        Dim HTMLLink As String = FormatHyperLink(Link)
        If HTMLLink.Length > 6 Then
            Return "<a href=""" & HTMLLink & """ target=""_blank"">" & UnFormatHyperLink(HTMLLink) & "</a>"
        Else
            Return ""
        End If

    End Function

    Public Shared Function Create_HTML_MailToLink(ByVal Link As String) As String

        If CustomMail.ValidateEmailAddress(Link) = True Then
            Dim HTMLLink As String = FormatHyperLinkEmail(Link)
            If HTMLLink.Length > 6 Then
                Return "<a href=""" & HTMLLink & """>" & Link & "</a>"
            Else
                Return ""
            End If
        Else
            Return ""
        End If

    End Function

    Public Shared Function UnFormatHyperLink(ByVal Link As String) As String

        If Link.Length > 6 Then
            If Link.StartsWith("HTTP://") Then
                Link = Mid(Link, 8, Link.Length)
            End If
            Return Link.ToUpper
        Else
            Return ""
        End If

    End Function
    Public Shared Function FormatHyperLink(ByVal Link As String) As String

        If Link.Length > 6 Then
            If Not Link.StartsWith("HTTP://") Then
                Link = "HTTP://" & Link
            End If
            Return Link.ToUpper
        Else
            Return ""
        End If

    End Function

    Public Shared Function FormatHyperLinkEmail(ByVal Link As String) As String

        If Link.Length > 6 Then
            If Not Link.StartsWith("MAILTO:") Then
                Link = "MAILTO:" & Link
            End If
            Return Link
        Else
            Return ""
        End If

    End Function

    Public Shared Function Get_EmailFooter() As String
        Dim sFooter As String = ""

        sFooter = sFooter & "<br /><br /><hr noshade>To Login to the NH Theatre Awards website goto <A HREF=""" & ConfigurationManager.AppSettings("PasswordResetEmailReLoginURL").ToString & """>" & ConfigurationManager.AppSettings("PasswordResetEmailReLoginURL").ToString & "</a>"
        sFooter = sFooter & "<br /><hr noshade>To request a new Password, follow the simple instructions at <A HREF=""http://www.NHTheatreAwards.org/Adjudication/ForgotPassword.aspx"">http://www.NHTheatreAwards.org/Adjudication/ForgotPassword.aspx </a>"
        sFooter = sFooter & "<br /><hr noshade>For NHTA Questions or Issues please Email <A HREF=""mailto:" & ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & """>" & ConfigurationManager.AppSettings("AdminMessageEmailFrom").ToString & "</a><br />"

        Return sFooter

    End Function

    Public Shared Function Get_RandomPassword(ByVal length As Integer) As String
        'Get the GUID
        Dim guidResult As String = System.Guid.NewGuid().ToString()

        'Remove the hyphens
        guidResult = guidResult.Replace("-", String.Empty)

        'Make sure length is valid
        If length <= 0 OrElse length > guidResult.Length Then
            Throw New ArgumentException("Length must be between 1 and " & guidResult.Length)
        End If

        'Return the first length bytes
        Return guidResult.Substring(0, length)
    End Function

    Public Shared Function RemoveInvalidSQLCharacters(ByVal sReviewText As String) As String
        '====================================================================================================
        ' Function is mainly used to stop SQL Injection attacks
        ' Removes the Following Character sets from the passed text
        '      '
        '      /*
        '      */
        '      --
        ' Also added code to remove astericks * used as wildcards in searches.
        ' Code will ignore an asterick anywhere in string.
        '====================================================================================================
        Dim iCount As Int16, sReturnText As String = String.Empty

        sReviewText = sReviewText.Trim                                      'remove leading and trailing spaces

        For iCount = 0 To sReviewText.Length - 1
            Select Case sReviewText.Substring(iCount, 1)
                Case "'"
                    ' Exclude this character
                Case "*"
                    If Right(sReviewText, 1) <> "*" Then
                        If sReviewText.Substring(iCount + 1, 1) = "/" Then
                            iCount = iCount + 1     'Skip this character  and the next char: /
                        ElseIf sReviewText.Substring(iCount, 1) = "*" Then
                            iCount = iCount     'Skip this character: *
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    Else
                        If sReviewText.Substring(iCount, 1) = "*" Then
                            iCount = iCount    'Skip this character: *
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    End If
                Case "/"
                    If sReviewText.Substring(iCount + 1, 1) = "*" Then
                        iCount = iCount + 1     'Skip this character and the next char: *
                    Else
                        sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                    End If
                Case "-"
                    If Right(sReviewText, 1) <> "-" Then
                        If sReviewText.Substring(iCount + 1, 1) = "-" Then
                            iCount = iCount + 1     'Skip this character  and the next char: -
                        Else
                            sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                        End If
                    End If
                Case " "
                    If sReviewText.Substring(iCount + 1, 1) = " " Then
                        iCount = iCount + 1     'Skip this character  and the next char: ' '
                    Else
                        sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
                    End If
                Case Else
                    sReturnText = sReturnText & sReviewText.Substring(iCount, 1)
            End Select
        Next

        RemoveInvalidSQLCharacters = sReturnText

    End Function

End Class
