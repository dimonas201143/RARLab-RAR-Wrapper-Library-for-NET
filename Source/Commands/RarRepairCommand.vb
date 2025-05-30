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

' Dim repairCommand As New RarRepairCommand("C:\Directory\*.rar") With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = False
' }
' 
' Using rarExecutor As New RarCommandExecutor(repairCommand)
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
'     Console.WriteLine($"Command-line to execute: {repairCommand}")
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

#Region " RarRepairCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for listing the archive contents of a RAR archive.
    ''' <para></para>
    ''' This command tries to repair a RAR archive.
    ''' </summary>
    ''' 
    ''' <remarks>
    ''' Archive repairing is performed in two stages. 
    ''' First, the damaged archive is searched for a recovery record (see 'rr' command). 
    ''' If archive contains the previously added recovery record and if damaged data area is 
    ''' continuous and smaller than error correction code size in recovery record, 
    ''' chance of successful archive reconstruction is high. 
    ''' When this stage has been completed, a new archive is created, named as fixed.arcname.rar, 
    ''' where 'arcname' is the original (damaged) archive name.
    ''' <para></para>
    ''' If broken archive does not contain a recovery record or if archive is not completely recovered due to major damage, 
    ''' second stage is performed. During this stage only the archive structure is reconstructed and it is 
    ''' impossible to recover files which fail checksum validation, it is still possible, 
    ''' however, to recover undamaged files, which were inaccessible due to the broken archive structure. 
    ''' Mostly this is useful for non-solid archives. 
    ''' This stage is never efficient for archives with encrypted file headers, 
    ''' which can be repaired only if recovery record is present.
    ''' <para></para>
    ''' When the second stage is completed, the reconstructed archive is saved as rebuilt.arcname.rar, 
    ''' where 'arcname' is the original archive name.
    ''' </remarks>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim repairCommand As New RarRepairCommand("C:\Directory\*.rar") With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = False
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(repairCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {repairCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarRepairCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive(s) to repair.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive(s) to repair.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive(s) to repair.")>
        Public Overrides Property Archive As String

        ''' <summary>
        ''' Gets or sets the output directory to place repaired archives.
        ''' <para></para>
        ''' By default, repaired archives are created in the current directory.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The output directory to place repaired files.
        ''' </value>
        <DisplayName("Output Directory Path")>
        <Description("The output directory to place extracted files.")>
        Public Property OutputDirectoryPath As String

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarRepairCommand"/> class 
        ''' with empty values for <see cref="RarRepairCommand.Archive"/> and 
        ''' <see cref="RarRepairCommand.OutputDirectoryPath"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarRepairCommand"/> class with 
        ''' the specified find mode, archive file path and search string.
        ''' </summary>
        ''' 
        ''' <param name="archivePath">
        ''' The file path of the RAR archive(s) to repair.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarRepairCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="outputPath">
        ''' Optional. The output directory to place repaired archives.
        ''' <para></para>
        ''' By default, repaired archives are created in the current directory.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarRepairCommand.OutputDirectoryPath"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(archivePath As String, Optional outputPath As String = Nothing)

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            Me.Archive = archivePath
            Me.OutputDirectoryPath = outputPath
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarRepairCommand"/>
        ''' to repair a RAR archive.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Repair
            sb.Append("r"c)

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " Switch termination arguments "

            ' Disable file lists (all such parameters found after this switch will be considered as file names, not file lists.
            sb.Append(" -@")

            ' Stop switches scanning (e.g. 'rar.exe a -s -- "-StrangeName.rar"').
            sb.Append(" --")

#End Region

#Region " Ending required argument: Path to the RAR archive(s), and output directory path "

            ' Archive File Path
            If Not String.IsNullOrWhiteSpace(Me.Archive) Then
                sb.Append($" ""{Me.Archive}""")
            End If

            ' Output Directory Path
            If Not String.IsNullOrWhiteSpace(Me.OutputDirectoryPath) Then
                sb.Append($" ""{Me.OutputDirectoryPath}""")
            End If

#End Region

            Return sb.ToString()
        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Returns a string that represents the current <see cref="RarRepairCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarRepairCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
