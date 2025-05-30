' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim listingCommand As New RarListingCommand(RarListingMode.Normal, "C:\Directory\*.rar") With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = False,
'     .Password = Nothing
' }
' 
' Using rarExecutor As New RarCommandExecutor(listingCommand)
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
'     Console.WriteLine($"Command-line to execute: {listingCommand}")
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

#Region " RarListingCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for listing the archive contents of a RAR archive.
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim listingCommand As New RarListingCommand(RarListingMode.Normal, "C:\Directory\*.rar") With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = False,
    '''     .Password = Nothing
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(listingCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {listingCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarListingCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive(s) to list the archive content.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive(s) to list the archive content.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive(s) to list the archive content.")>
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
        ''' Gets or sets the mode for listing the contents of a RAR archive.
        ''' <para></para>
        ''' Default value is <see cref="RarListingMode.Normal"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The mode for listing the contents of a RAR archive.
        ''' </value>
        <DisplayName("Listing Mode")>
        <Description("The mode for listing the contents of a RAR archive.")>
        <DefaultValue(GetType(RarListingMode), "Normal")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property ListingMode As RarListingMode = RarListingMode.Normal

        ''' <summary>
        ''' Gets or sets a value indicating whether to list contents of all RAR archive volumes in a volume set. 
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        <DisplayName("List Volumes")>
        <Description("Enables or disables listing contents of all RAR archive volumes in a volume set.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property ListVolumes As Boolean

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarListingCommand"/> class 
        ''' with default value for <see cref="RarListingCommand.ListingMode"/> property,
        ''' and empty value for <see cref="RarListingCommand.Archive"/> property.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarListingCommand"/> class with 
        ''' the specified find mode, archive file path and search string.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The mode for listing the contents of the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarListingCommand.ListingMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the RAR archive(s) to list its contents.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarListingCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="mode"/> is not a valid value of the <see cref="RarListingMode"/> enumeration.
        ''' <para></para>
        ''' This can occurs when an undefined or out-of-range value is passed to the method.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarListingMode, archivePath As String)

            If Not [Enum].IsDefined(GetType(RarListingMode), mode) Then
                Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(RarListingMode))
            End If

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            Me.Archive = archivePath
            Me.ListingMode = mode
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarListingCommand"/>
        ''' to list the contents in a RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Listing Mode
            Select Case Me.ListingMode

                Case RarListingMode.Normal
                    sb.Append("l"c)

                Case RarListingMode.NormalBare
                    sb.Append("lb")

                Case RarListingMode.NormalTechnical
                    sb.Append("lt")

                Case RarListingMode.NormalTechnicalAll
                    sb.Append("lta")

                Case RarListingMode.Verbose
                    sb.Append("v"c)

                Case RarListingMode.VerboseBare
                    sb.Append("vb")

                Case RarListingMode.VerboseTechnical
                    sb.Append("vt")

                Case RarListingMode.VerboseTechnicalAll
                    sb.Append("vta")

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.ListingMode), Me.ListingMode, GetType(RarListingMode))
            End Select

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarListingCommand arguments "

            ' List Volumes
            If Me.ListVolumes Then
                sb.Append(" -v")
            End If

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
        ''' Returns a string that represents the current <see cref="RarListingCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarListingCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
