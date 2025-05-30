' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim addRecoveryRecordCommand As New RarAddRecoveryRecordCommand("C:\Directory\*.rar", recoveryRecordPercentage:=3) With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = False,
'     .Password = Nothing
' }
' 
' Using rarExecutor As New RarCommandExecutor(addRecoveryRecordCommand)
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
'     Console.WriteLine($"Command-line to execute: {addRecoveryRecordCommand}")
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

#Region " RarAddRecoveryRecordCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for adding a recovery record to a RAR archive.
    ''' <para></para>
    ''' Recovery record is the data area, optionally added to archive and containing error correction codes, 
    ''' namely Reed-Solomon codes for RAR 5.0 archive format. 
    ''' While it increases the archive size, it helps to recover archived files in case of
    ''' disk failure or data loss of other kind, provided that damage is not too severe. 
    ''' Such recovery can be done with <see cref="RarRepairCommand"/> repair command.
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' The recovery record size if defined as a percentage of archive size. If it is omitted, 3% is assumed. 
    ''' Maximum allowed recovery record size is 1000%. 
    ''' <para></para>
    ''' Larger recovery records are processed slower both when creating and repairing. 
    ''' Due to service data overhead, the actual resulting recovery record size only approximately matches the 
    ''' user defined percent and difference is larger for smaller archives.
    ''' <para></para>
    ''' In case of a single continuous damage, typically it is possible to restore slightly less data than recovery record size. 
    ''' Recoverable data size can be lower for multiple damages.
    ''' <para></para>
    ''' If a recovery record is partially broken, its remaining valid data still can be utilized to repair files. 
    ''' Repair command does not fix broken blocks in recovery record itself, only file data is corrected. 
    ''' After successful archive repair, you may need to create a new recovery record for rescued files.
    ''' <para></para>
    ''' While the recovery record improves chances to repair damaged archives, it does not guarantee the successful recovery. 
    ''' Consider combining the recovery record feature with making multiple archive copies to different media for important data.
    ''' </remarks>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim addRecoveryRecordCommand As New RarAddRecoveryRecordCommand("C:\Directory\*.rar", recoveryRecordPercentage:=3) With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = False,
    '''     .Password = Nothing
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(addRecoveryRecordCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {addRecoveryRecordCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarAddRecoveryRecordCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive(s) for which to add a recovery record.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive(s) for which to add a recovery record.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive(s) for which to add a recovery record.")>
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
        ''' Gets or sets the percentage of recovery record to add to the RAR archive.
        ''' <para></para>
        ''' Valid value is from 0% to 1000%. If value exceeds 1000, it gets automatically adjusted by <c>rar.exe</c> process.
        ''' <para></para>
        ''' A high recovery record percentage increase redundancy and archive size, 
        ''' but improves the chance of repairing a damaged archive.
        ''' <para></para>
        ''' If this value is 0% (zero), existing recovery record in the archive is removed.
        ''' <para></para>
        ''' Suggested percentage value by RAR is 3%.
        ''' <para></para>
        ''' Default value is 0% (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The percentage of recovery record to add to the RAR archive.
        ''' </value>
        <DisplayName("Recovery Record Percentage")>
        <Description("The percentage of recovery record to add to the RAR archive. Valid value is from 0 to 1000.")>
        <DefaultValue(0)>
        Public Property RecoveryRecordPercentage As Short = 0

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarAddRecoveryRecordCommand"/> class 
        ''' with empty value for <see cref="RarAddRecoveryRecordCommand.Archive"/> property and 
        ''' 0 (zero) for <see cref="RarAddRecoveryRecordCommand.RecoveryRecordPercentage"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarAddRecoveryRecordCommand"/> class with 
        ''' the specified archive file path and recovery record percentage.
        ''' </summary>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the RAR archive(s) for which to add a recovery record.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarAddRecoveryRecordCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="recoveryRecordPercentage">
        ''' The percentage of recovery record to add to the RAR archive.
        ''' <para></para>
        ''' Valid value is from 0% to 1000%. If value exceeds 1000, it gets automatically adjusted by <c>rar.exe</c> process.
        ''' <para></para>
        ''' A high recovery record percentage increase redundancy and archive size, 
        ''' but improves the chance of repairing a damaged archive.
        ''' <para></para>
        ''' If this value is 0% (zero), existing recovery record in the archive is removed.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarAddRecoveryRecordCommand.RecoveryRecordPercentage"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' <para></para>
        ''' Also thrown when <paramref name="recoveryRecordPercentage"/> is less than zero.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(archivePath As String, recoveryRecordPercentage As Short)

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            If recoveryRecordPercentage < 0 Then
                Throw New ArgumentException("The recovery record percentage must be zero or greater.", NameOf(recoveryRecordPercentage))
            End If

            Me.Archive = archivePath
            Me.RecoveryRecordPercentage = recoveryRecordPercentage
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarAddRecoveryRecordCommand"/>
        ''' to add a recovery record to a RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Add Recovery Record
            sb.Append($"rr{Me.RecoveryRecordPercentage}")

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarAddRecoveryRecordCommand arguments "

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
        ''' Returns a string that represents the current <see cref="RarAddRecoveryRecordCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarAddRecoveryRecordCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
