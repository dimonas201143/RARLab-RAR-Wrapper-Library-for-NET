' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim createRecoveryVolumesCommand As New RarCreateRecoveryVolumesCommand(RarCreateRecoveryVolumesMode.NumberOfVolumes, "C:\Directory\Archive.part01.rar", recoveryVolumes:=3) With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .Password = Nothing
' }
' 
' Using rarExecutor As New RarCommandExecutor(createRecoveryVolumesCommand)
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
'     Console.WriteLine($"Command-line to execute: {createRecoveryVolumesCommand}")
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

'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarCreateRecoveryVolumesCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for creating recovery volumes (.rev files) for a multi-volume RAR archive, 
    ''' which can be later used to reconstruct missing and damaged files in a volume set. 
    ''' <para></para>
    ''' This command makes sense only for multivolume archives.
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' This feature may be useful for backups or, for example, when you posted a multivolume archive 
    ''' to a newsgroup and a part of subscribers did not receive some of the files. 
    ''' Reposting recovery volumes instead of usual volumes may reduce the total number of files to repost.
    ''' <para></para>
    ''' Each recovery volume is able to reconstruct one missing or damaged RAR volume. 
    ''' For example, if you have 30 volumes and 3 recovery volumes, you are able to reconstruct any 3 missing volumes. 
    ''' If the number of .rev files is less than the number of missing volumes, reconstructing is impossible. 
    ''' The total number of usual and recovery volumes must not exceed 65535.
    ''' <para></para>
    ''' Original RAR volumes must not be modified after creating recovery volumes. 
    ''' Recovery algorithm uses data stored both in REV files and in RAR volumes to rebuild missing RAR volumes. 
    ''' So if you modify RAR volumes, for example, lock them, after creating REV files, recovery process will fail.
    ''' <para></para>
    ''' Additionally to recovery data, RAR 5.0 recovery volumes also store service information such as checksums of protected RAR files. 
    ''' So they are slightly larger than RAR volumes which they protect. 
    ''' If you plan to copy individual RAR and REV files to some removable media, 
    ''' you need to take it into account and specify RAR volume size by a few kilobytes smaller than media size.
    ''' </remarks>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim createRecoveryVolumesCommand As New RarCreateRecoveryVolumesCommand(RarCreateRecoveryVolumesMode.NumberOfVolumes, "C:\Directory\Archive.part01.rar", recoveryVolumes:=3) With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .Password = Nothing
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(createRecoveryVolumesCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {createRecoveryVolumesCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarCreateRecoveryVolumesCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the multi-volume RAR archive for which to create recovery volumes (.rev files).
        ''' <para></para>
        ''' You need to specify the name of the first volume in the set as the archive name (e.g. "Archive.part01.rar").
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the multi-volume RAR archive for which to create recovery volumes (.rev files).
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the multi-volume RAR archive for which to create recovery volumes (.rev files).")>
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
        ''' Gets or sets the mode for creating recovery volumes (.rev files) for the multi-volume RAR archive.
        ''' <para></para>
        ''' Default value is <see cref="RarCreateRecoveryVolumesMode.NumberOfVolumes"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The mode for creating recovery volumes (.rev files) for the multi-volume RAR archive.
        ''' </value>
        <DisplayName("Recovery Volumes Mode")>
        <Description("The mode for creating recovery volumes (.rev files) for the multi-volume RAR archive.")>
        <DefaultValue(GetType(RarCreateRecoveryVolumesMode), "NumberOfVolumes")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property RecoveryVolumesMode As RarCreateRecoveryVolumesMode = RarCreateRecoveryVolumesMode.NumberOfVolumes

        ''' <summary>
        ''' Gets or sets the recovery volumes (.rev files) to create for the multi-volume RAR archive.
        ''' <para></para>
        ''' The meaning of this value depends on <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumesMode"/> property.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The recovery volumes (.rev files) to create for the multi-volume RAR archive.
        ''' </value>
        <DisplayName("Recovery Volumes")>
        <Description("The recovery volumes (.rev files) to create for the multi-volume RAR archive.")>
        <DefaultValue(0)>
        Public Property RecoveryVolumes As Short = 0

        ''' <summary>
        ''' Returns <see langword="False"/>. This command does not support file recursivity
        ''' </summary>
        <Browsable(False)>
        <EditorBrowsable(EditorBrowsableState.Never)>
        Public Shadows ReadOnly Property RecurseSubdirectories As Boolean = False

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreateRecoveryVolumesCommand"/> class 
        ''' with empty value for <see cref="RarCreateRecoveryVolumesCommand.Archive"/> property, and  
        ''' default values for <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumesMode"/> and
        ''' <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumes"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreateRecoveryVolumesCommand"/> class with 
        ''' the specified archive file path and recovery volumes value.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The mode for creating recovery volumes (.rev files) for the multi-volume RAR archive.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumesMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the multi-volume RAR archive for which to create recovery volumes (.rev files).
        ''' <para></para>
        ''' You need to specify the name of the first volume in the set as the archive name (e.g. "Archive.part01.rar").
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreateRecoveryVolumesCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="recoveryVolumes">
        ''' The recovery volumes (.rev files) to create for the multi-volume RAR archive.
        ''' <para></para>
        ''' The meaning of this value depends on <paramref name="mode"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreateRecoveryVolumesCommand.RecoveryVolumes"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="mode"/> is not a valid value of the <see cref="RarCreateRecoveryVolumesMode"/> enumeration.
        ''' <para></para>
        ''' This can occurs when an undefined or out-of-range value is passed to the method.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' <para></para>
        ''' Also thrown when <paramref name="recoveryVolumes"/> is less than zero.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarCreateRecoveryVolumesMode, archivePath As String, recoveryVolumes As Short)

            If Not [Enum].IsDefined(GetType(RarCreateRecoveryVolumesMode), mode) Then
                Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(RarCreateRecoveryVolumesMode))
            End If

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            If recoveryVolumes < 0 Then
                Throw New ArgumentException("Value for recovery volumes must be zero or greater.", NameOf(recoveryVolumes))
            End If

            Me.Archive = archivePath
            Me.RecoveryVolumes = recoveryVolumes
            Me.RecoveryVolumesMode = mode
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarCreateRecoveryVolumesCommand"/>
        ''' for creating recovery volumes (.rev files) for a multi-volume RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Create Recovery Volumes
            sb.Append($"rv{Me.RecoveryVolumes}")
            If Me.RecoveryVolumesMode = RarCreateRecoveryVolumesMode.Percentage Then
                sb.Append("p"c)
            End If

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarCreateRecoveryVolumesCommand arguments "

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
        ''' Returns a string that represents the current <see cref="RarCreateRecoveryVolumesCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarCreateRecoveryVolumesCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
