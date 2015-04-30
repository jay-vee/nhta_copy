Imports System.Net.Mail
Imports Adjudication.DataAccess

Public Class CustomMail

    'Public Shared SMTPOutServer As String = ConfigurationManager.AppSettings("SMTPOutServer").ToString

    Public Shared Sub SendCDOEmail(ByVal FromAddress As String, _
                                    ByVal ToAddress As String, _
                                    ByVal BCC As Boolean, _
                                    ByVal Subject As String, _
                                    ByVal BodyText As String, _
                                    Optional ByVal HighPriority As Boolean = False, _
                                    Optional ByVal HTMLFormat As Boolean = True, _
                                    Optional ByVal AdminLoginID As String = "", _
                                    Optional ByVal FK_EmailMessageTypesID As String = "1", _
                                    Optional ByVal SendIndividualEmails As Boolean = False)
        '==================================================================================================
        '   Excellent info for .NET 3.5 updates at http://www.systemwebmail.com/
        '==================================================================================================
        Dim SendEmails As Boolean
        Dim sToEmailAddresses() As String, SplitChars() As Char = {",", ";"}
        Dim oMail As New MailMessage
        Dim SmtpMail = New SmtpClient
        Dim TechNotes As String = ""
        '==================================================================================================

        Try
            '==================================================================================================
            Try
                SendEmails = CBool(ConfigurationManager.AppSettings("SendEmails").ToString)
            Catch ex As Exception
                SendEmails = True
            End Try

            'remove trailing comma if found
            If ToAddress.Length > 6 Then
                ToAddress = ToAddress.Trim
                If Mid(ToAddress, Len(ToAddress), 1) = "," Then ToAddress = Mid(ToAddress, 1, Len(ToAddress) - 1)
            End If


            '==================================================================================================
            '=== Create the email with recipients ===
            '==================================================================================================
            If SendEmails = True Then
                sToEmailAddresses = ToAddress.Split(SplitChars)
                oMail.From = New MailAddress(FromAddress)           'Assumes the FROM email address is only 1 address
                oMail.Subject = Subject
                oMail.Body = BodyText    '"Sent at: " + DateTime.Now
                oMail.IsBodyHtml = IIf(HTMLFormat = True, True, False)
                If HighPriority = True Then oMail.Priority = MailPriority.High

                SmtpMail = New SmtpClient
                If SendIndividualEmails = False Then                    'Send 1 Email for all recipients
                    If BCC = True Then
                        For Each s As String In sToEmailAddresses
                            If s.Trim.Length > 0 Then oMail.Bcc.Add(New MailAddress(s.Trim))
                        Next
                    Else
                        For Each s As String In sToEmailAddresses
                            If s.Trim.Length > 0 Then oMail.To.Add(New MailAddress(s.Trim))
                        Next
                    End If

                    TechNotes = "1 Email sent for all recipients."      'Note for log
                    SmtpMail.Send(oMail)                                'SEND the EMAIL

                Else                                                    '1 Email sent per recipient. NOTE: Logged as 1 email for all.
                    'If SendIndividualEmails = True Then   
                    For Each s As String In sToEmailAddresses
                        '=== if sending 1 email per recipient, do not use BCC ===
                        If s.Trim.Length > 0 Then                            'make sure there is an address to send
                            oMail.To.Add(New MailAddress(s.Trim))

                            SmtpMail.Send(oMail)                            'SEND the EMAIL

                            oMail.To.Clear()                                'clear previously added recipient email addresses
                        End If
                    Next
                    TechNotes = "1 Email sent per recipient as TO (not BCC). NOTE: Logged as 1 email for all."  'Note for log
                End If

                oMail = Nothing     ' free up resources

            End If

            '==================================================================================================
            'Log the Email that was sent AFTER the email is successfuly sent
            '==================================================================================================
            If BCC = True Then TechNotes = TechNotes & "Blind Copy; "
            If HighPriority = True Then TechNotes = TechNotes & "Sent with High Priority; "
            If HTMLFormat = True Then TechNotes = TechNotes & "Sent in HTML Format; "

            If AdminLoginID Is Nothing Then AdminLoginID = ""
            If AdminLoginID.Length = 0 Then AdminLoginID = FromAddress

            '=== Log the Email ===
            Save_EmailLog(AdminLoginID, FromAddress, ToAddress, Subject, BodyText, TechNotes, FK_EmailMessageTypesID)

            GC.Collect()                                                'Perform garbage collection of resources

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Function ValidateEmailAddress(ByVal EmailAddress As String) As Boolean
        '==================================================================================================
        Dim bReturnValue As Boolean = True
        '==================================================================================================

        If EmailAddress.IndexOf("@") < 2 Then bReturnValue = False
        'If EmailAddress.IndexOf(".") < 2 Then bReturnValue = False
        If EmailAddress.Length < 9 Then bReturnValue = False

        'If EmailAddress.IndexOf(".") >= 3 Then
        '    If EmailAddress.Substring(EmailAddress.IndexOf(".") + 1).Length < 2 Then
        '        bReturnValue = False
        '    End If
        'End If

        Return bReturnValue

    End Function

End Class
