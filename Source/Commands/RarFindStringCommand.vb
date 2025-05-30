' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

' This source code is freely distributed as part of the "DevCase Class Library for .NET Developers".
'
' If you find this library useful, consider supporting my work by purchasing the full DevCase suite.
' It includes a powerful set of APIs covering a wide range of features and development topics.
'
' Visit the purchase page here:
' https://github.com/ElektroStudios/DevCase.github.io
'
' Thank you for your support!

#Region " Usage Examples "

' Dim findCommand As New RarFindStringCommand(RarFindStringMode.CaseInsensitive, "C:\Directory\*.rar", "string to find", useCharacterTables:=False) With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = False,
'     .Password = Nothing
' }
' 
' Using rarExecutor As New RarCommandExecutor(findCommand)
' 
'     AddHandler rarExecutor.OutputDataReceived,
'         Sub(sender As Object, e As DataReceivedEventArgs)
'             Console.WriteLine($"[Output] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
'         End Sub
' 
'     AddHandler rarExecutor.ErrorDataReceived,
'         Sub(sender As Object, e As DataReceivedEventArgs)
'             If e.Data IsNot Nothing Then
'                 Console.WriteLine($"[Error] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
'             End If
'         End Sub
' 
'     AddHandler rarExecutor.Exited,
'         Sub(sender As Object, e As EventArgs)
'             Dim pr As Process = DirectCast(sender, Process)
'             Dim rarExitCode As RarExitCode = DirectCast(pr.ExitCode, RarExitCode)
'             Console.WriteLine($"[Exited] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process has terminated with exit code {pr.ExitCode} ({rarExitCode})")
'         End Sub
' 
'     Console.WriteLine($"Command-line to execute: {findCommand}")
'     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
' End Using

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Text

'Imports DevCase.Runtime.Attributes
'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarFindStringCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for finding strings within the files in a RAR archive.
    ''' <para></para>
    ''' This command performs a dummy archive extraction, writing nothing to the output stream, 
    ''' in order to validate the specified file(s).
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim findCommand As New RarFindStringCommand(RarFindStringMode.CaseInsensitive, "C:\Directory\*.rar", "string to find", useCharacterTables:=False) With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = False,
    '''     .Password = Nothing
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(findCommand)
    ''' 
    '''     AddHandler rarExecutor.OutputDataReceived,
    '''         Sub(sender As Object, e As DataReceivedEventArgs)
    '''             Console.WriteLine($"[Output] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
    '''         End Sub
    ''' 
    '''     AddHandler rarExecutor.ErrorDataReceived,
    '''         Sub(sender As Object, e As DataReceivedEventArgs)
    '''             If e.Data IsNot Nothing Then
    '''                 Console.WriteLine($"[Error] {Date.Now:yyyy-MM-dd HH:mm:ss} - {e.Data}")
    '''             End If
    '''         End Sub
    ''' 
    '''     AddHandler rarExecutor.Exited,
    '''         Sub(sender As Object, e As EventArgs)
    '''             Dim pr As Process = DirectCast(sender, Process)
    '''             Dim rarExitCode As RarExitCode = DirectCast(pr.ExitCode, RarExitCode)
    '''             Console.WriteLine($"[Exited] {Date.Now:yyyy-MM-dd HH:mm:ss} - rar.exe process has terminated with exit code {pr.ExitCode} ({rarExitCode})")
    '''         End Sub
    ''' 
    '''     Console.WriteLine($"Command-line to execute: {findCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarFindStringCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive(s) to search for the specified string inside its files.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive(s) to search for the specified string inside its files.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive(s) to search for the specified string inside its files.")>
        Public Overrides Property Archive As String

        ''' <summary>
        ''' Gets or sets the password used to read an encrypted archive.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The password used to read an encrypted archive; or <c>null</c> if no password is specified.
        ''' </value>
        <DisplayName("Password")>
        <Description("The password used to read an encrypted archive.")>
        Public Property Password As RarPassword

        ''' <summary>
        ''' Gets or sets the mode for string search through the contents of a RAR archive.
        ''' <para></para>
        ''' Default value is <see cref="RarFindStringMode.CaseInsensitive"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The mode for string search through the contents of a RAR archive.
        ''' </value>
        <DisplayName("Find Mode")>
        <Description("The mode for string search through the contents of a RAR archive.")>
        <DefaultValue(GetType(RarFindStringMode), "CaseInsensitive")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FindMode As RarFindStringMode = RarFindStringMode.CaseInsensitive

        ''' <summary>
        ''' Gets or sets the string to search inside the files of a RAR archive.
        ''' <para></para>
        ''' If <see cref="RarFindStringCommand.FindMode"/> is <see cref="RarFindStringMode.Hexadecimal"/>, 
        ''' use the following example syntax to specify the string: "f0e0aeaeab2d83e3a9"
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The string to search inside the files of a RAR archive; or <c>null</c> if no string is specified.
        ''' </value>
        <DisplayName("Search String")>
        <Description("The string to search.")>
        Public Property SearchString As String

        ''' <summary>
        ''' Gets or sets a value indicating whether to use ANSI, UTF-8, UTF-16 and OEM character tables. 
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        <DisplayName("Use Character Tables")>
        <Description("Enables or disables using ANSI, UTF-8, UTF-16 and OEM character tables.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property UseCharacterTables As Boolean

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarFindStringCommand"/> class 
        ''' with default values for <see cref="RarFindStringCommand.FindMode"/> and 
        ''' <see cref="RarFindStringCommand.UseCharacterTables"/> properties, 
        ''' and empty values for <see cref="RarFindStringCommand.Archive"/> and 
        ''' <see cref="RarFindStringCommand.SearchString"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarFindStringCommand"/> class with 
        ''' the specified find mode, archive file path and search string.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The search mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarFindStringCommand.FindMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the RAR archive(s) to search for the specified string inside its files.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarFindStringCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="searchString">
        ''' The string to search.
        ''' <para></para>
        ''' If <paramref name="mode"/> is <see cref="RarFindStringMode.Hexadecimal"/>, 
        ''' use the following example syntax to specify the string: "f0e0aeaeab2d83e3a9"
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarFindStringCommand.SearchString"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="mode"/> is not a valid value of the <see cref="RarFindStringMode"/> enumeration.
        ''' <para></para>
        ''' This can occurs when an undefined or out-of-range value is passed to the method.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarFindStringMode, archivePath As String, searchString As String,
                   Optional useCharacterTables As Boolean = False)

            If Not [Enum].IsDefined(GetType(RarFindStringMode), mode) Then
                Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(RarFindStringMode))
            End If

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            If String.IsNullOrEmpty(searchString) Then
                Throw New ArgumentException("The search string cannot be null or empty.", NameOf(searchString))
            End If

            Me.Archive = archivePath
            Me.FindMode = mode
            Me.SearchString = searchString
            Me.UseCharacterTables = useCharacterTables
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarFindStringCommand"/>
        ''' to perform a find string command in a RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Find Mode
            sb.Append(" ""i")
            Select Case Me.FindMode

                Case RarFindStringMode.CaseInsensitive
                    sb.Append("i"c)

                Case RarFindStringMode.CaseInsensitive
                    sb.Append("c"c)

                Case RarFindStringMode.Hexadecimal
                    sb.Append("h"c)

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.FindMode), Me.FindMode, GetType(RarFindStringMode))
            End Select

            ' Use Character Tables
            If Me.UseCharacterTables Then
                sb.Append("t"c)
            End If

            ' Search String
            sb.Append("="c)
            If Not String.IsNullOrEmpty(Me.SearchString) Then
                sb.Append(Me.SearchString)
            End If
            sb.Append(""""c)

            ' e.g. "ict=find string"

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarFindStringCommand arguments "

            ' Password
            If Me.Password IsNot Nothing Then

                Dim unsafePassword As String = Me.Password.GetUnsafePassword()
                sb.Append($" -p""{unsafePassword}""")
            End If

#End Region

#Region " Switch termination arguments "

            ' Disable file lists (all such parameters found after this switch will be considered as file names, not file lists.
            sb.Append(" -@")

            ' Stop switches scanning (e.g. 'rar.exe a -s -- "-StrangeName.rar"').
            sb.Append(" --")

#End Region

#Region " Ending required argument: Path to the RAR archive(s) "

            ' Archive File Path
            If Not String.IsNullOrWhiteSpace(Me.Archive) Then
                sb.Append($" ""{Me.Archive}""")
            End If

#End Region

            Return sb.ToString()
        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Returns a string that represents the current <see cref="RarFindStringCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarFindStringCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
