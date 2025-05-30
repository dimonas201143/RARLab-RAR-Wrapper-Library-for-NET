' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim reconstructRecoveryVolumesCommand As New RarReconstructRecoveryVolumesCommand("C:\Directory\Archive.part01.rar") With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)"
' }
' 
' Using rarExecutor As New RarCommandExecutor(reconstructRecoveryVolumesCommand)
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
'     Console.WriteLine($"Command-line to execute: {reconstructRecoveryVolumesCommand}")
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

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarReconstructRecoveryVolumesCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for reconstructing missing or damaged recovery volumes for a multi-volume RAR archive.
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' Original copies of damaged volumes are renamed to *.bad before reconstruction. 
    ''' For example, volname.part03.rar will be renamed to volname.part03.rar.bad.
    ''' </remarks>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim reconstructRecoveryVolumesCommand As New RarReconstructRecoveryVolumesCommand("C:\Directory\Archive.part01.rar") With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)"
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(reconstructRecoveryVolumesCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {reconstructRecoveryVolumesCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarReconstructRecoveryVolumesCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the multi-volume RAR archive to reconstruct its missing or damaged recovery volumes.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the multi-volume RAR archive to reconstruct its missing or damaged recovery volumes.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the multi-volume RAR archive to reconstruct its missing or damaged recovery volumes.")>
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
        ''' Returns <see langword="False"/>. This command does not support file recursivity
        ''' </summary>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        Public Shadows ReadOnly Property RecurseSubdirectories As Boolean = False

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarReconstructRecoveryVolumesCommand"/> class 
        ''' with empty value for <see cref="RarReconstructRecoveryVolumesCommand.Archive"/> property.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarReconstructRecoveryVolumesCommand"/> class with 
        ''' the specified multi-volume RAR archive file path.
        ''' </summary>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the multi-volume RAR archive to reconstruct its missing or damaged recovery volumes.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarReconstructRecoveryVolumesCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(archivePath As String)

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            Me.Archive = archivePath
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarReconstructRecoveryVolumesCommand"/>
        ''' to reconstruct missing or damaged recovery volumes for a multi-volume RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Reconstruct Recovery Volumes
            sb.Append("rc")

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarReconstructRecoveryVolumesCommand arguments "

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

#Region " Ending required argument: Path to the multi-volume RAR archive "

            ' Multi-volume RAR Archive File Path
            If Not String.IsNullOrWhiteSpace(Me.Archive) Then
                sb.Append($" ""{Me.Archive}""")
            End If

#End Region

            Return sb.ToString()
        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Returns a string that represents the current <see cref="RarReconstructRecoveryVolumesCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarReconstructRecoveryVolumesCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
