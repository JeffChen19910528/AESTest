Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class Form1

    Dim DesKey As String = "abc!@#df", DesIv As String = "+_)xyz*&"

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim s As String = "Test"
        Dim Encrypt As String = DesEncrypt(s)
        Dim Decrypt As String = DesDecrypt(DesEncrypt(s))
        showtxt.Text = "Word:" + s + vbCrLf + "Encrypt:" + Encrypt + vbCrLf + "Decrypt:" +Decrypt
    End Sub


    Function DesEncrypt(ByVal source As String) As String
        Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider()
        Dim key() As Byte = Nothing
        Dim iv() As Byte = Nothing
        Dim dataByteArray() As Byte = Nothing

        key = Encoding.ASCII.GetBytes(DesKey)
        iv = Encoding.ASCII.GetBytes(DesIv)
        dataByteArray = Encoding.UTF8.GetBytes(source)

        des.Key = key
        des.IV = iv

        Dim encrypt As String = String.Empty
        Using ms As MemoryStream = New MemoryStream()
            Try
                Using cs As CryptoStream = New CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(dataByteArray, 0, dataByteArray.Length)
                    cs.FlushFinalBlock()

                    Dim sb As StringBuilder = New StringBuilder()
                    For Each b As Byte In ms.ToArray()
                        sb.AppendFormat("{0:X2}", b)
                    Next
                    encrypt = sb.ToString()
                End Using
            Catch ex As Exception
                encrypt = "error"
            End Try
        End Using

        Return encrypt
    End Function


    Function DesDecrypt(ByVal encrypt As String) As String

        Dim a1 As Integer = ((encrypt.Length) / 2)
        a1 = a1 - 1
        Dim dataByteArray(a1) As Byte

        For x1 As Integer = 0 To a1
            Dim i As Integer = Convert.ToInt32(encrypt.Substring(x1 * 2, 2), 16)
            dataByteArray(x1) = CByte(i)
        Next

        Dim des As DESCryptoServiceProvider = New DESCryptoServiceProvider()
        Dim key() As Byte = Nothing
        Dim iv() As Byte = Nothing

        key = Encoding.ASCII.GetBytes(DesKey)
        iv = Encoding.ASCII.GetBytes(DesIv)

        des.Key = key
        des.IV = iv

        Dim DesString As String = String.Empty
        Using ms As MemoryStream = New MemoryStream()
            Try
                Using cs As CryptoStream = New CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(dataByteArray, 0, dataByteArray.Length)
                    cs.FlushFinalBlock()
                    DesString = Encoding.UTF8.GetString(ms.ToArray())
                End Using
            Catch ex As Exception
                DesString = "error"
            End Try
        End Using
        Return DesString
    End Function
End Class
