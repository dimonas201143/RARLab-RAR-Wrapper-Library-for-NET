' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim extractionCommand As New RarExtractionCommand(RarExtractionMode.ExtractWithPath, "C:\Directory\*.rar", outputDirectoryPath:="") With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = False,
'     .Password = Nothing,
'     .DictionarySize = RarDictionarySize.Gb4,
'     .OverwriteMode = RarOverwriteMode.Skip,
'     .UpdateMode = RarUpdateMode.Normal,
'     .FilePathMode = RarFilePathMode.ExcludeBaseDirFromFileNames,
'     .FileTimestamps = RarFileTimestamps.All
' }
' 
' Using rarExecutor As New RarCommandExecutor(extractionCommand)
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
'     Console.WriteLine($"Command-line to execute: {extractionCommand}")
'     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
' End Using

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Text

'Imports DevCase.Runtime.Attributes
'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarExtractionCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for RAR archive extraction.
    ''' <para></para>
    ''' This command extract files to disk from the specified RAR archive(s).
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim extractionCommand As New RarExtractionCommand(RarExtractionMode.ExtractWithPath, "C:\Directory\*.rar", outputDirectoryPath:="") With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = False,
    '''     .Password = Nothing,
    '''     .DictionarySize = RarDictionarySize.Gb4,
    '''     .OverwriteMode = RarOverwriteMode.Skip,
    '''     .UpdateMode = RarUpdateMode.Normal,
    '''     .FilePathMode = RarFilePathMode.ExcludeBaseDirFromFileNames,
    '''     .FileTimestamps = RarFileTimestamps.All
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(extractionCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {extractionCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarExtractionCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive(s) to extract.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive to extract.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive(s) to extract.")>
        Public Overrides Property Archive As String

        ''' <summary>
        ''' Gets or sets the output directory to place extracted files. This directory is created by RAR if it does not exist yet.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The output directory to place extracted files.
        ''' </value>
        <DisplayName("Output Directory Path")>
        <Description("The output directory to place extracted files.")>
        Public Property OutputDirectoryPath As String

        ''' <summary>
        ''' Gets or sets the mode for extracting the contents of a RAR archive.
        ''' <para></para>
        ''' Default value is <see cref="RarExtractionMode.ExtractWithPath"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The mode for extracting the contents of a RAR archive.
        ''' </value>
        <DisplayName("Extraction Mode")>
        <Description("The mode for extracting the contents of a RAR archive.")>
        <DefaultValue(GetType(RarExtractionMode), "ExtractWithPath")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property ExtractionMode As RarExtractionMode = RarExtractionMode.ExtractWithPath

        ''' <summary>
        ''' Gets or sets the size of the dictionary used for archive extraction.
        ''' <para></para>
        ''' By default, RAR refuses to unpack archives with dictionary exceeding 4 GB. 
        ''' It is done to prevent the unexpected large memory allocation. 
        ''' <para></para>
        ''' Use this value to allow unpacking dictionaries up to and including the specified dictionary size.
        ''' <para></para>
        ''' Default value is <see cref="RarDictionarySize.Gb4"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The size of the dictionary used for archive extraction.
        ''' </value>
        <DisplayName("Dictionary Size")>
        <Description("Size of the dictionary used for archive extraction. Use this value to allow unpacking dictionaries up to and including the specified dictionary size.")>
        <DefaultValue(GetType(RarDictionarySize), "Gb4")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property DictionarySize As RarDictionarySize = RarDictionarySize.Gb4

        ''' <summary>
        ''' Gets or sets a value indicating how to process symbolic links when extracting an archive. 
        ''' <para></para>
        ''' Accepted values are:
        ''' <list type="bullet">
        '''   <item>
        '''     <term><see cref="TriState.True"/></term>
        '''     <description>
        '''     Extract symbolic links as links, so file or directory contents is not extracted.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.False"/></term>
        '''     <description>
        '''     Extract the contents of the symbolic links as is.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.UseDefault"/></term>
        '''     <description>
        '''     Completely skips symbolic links when extracting.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' <para></para>
        ''' Default value is <see cref="TriState.UseDefault"/>.
        ''' </summary>
        ''' <value>
        ''' <see cref="TriState.True"/> to extract symbolic links as links,
        ''' <see cref="TriState.False"/> to extract the contents of the symbolic links as is, or 
        ''' <see cref="TriState.UseDefault"/> to completely skips symbolic links when extracting.
        ''' </value>
        <DisplayName("Process Symbolic Links as Links")>
        <Description("Enables or disables processing symbolic links as the link.")>
        <DefaultValue(GetType(TriState), "UseDefault")> ' <LocalizableTriState>  <TypeConverter(GetType(LocalizableTriStateConverter))>
        Public Property ProcessSymbolicLinksAsLinks As TriState = TriState.UseDefault

        ''' <summary>
        ''' Gets or sets a value indicating whether to restore file security information for extracted files.
        ''' <para></para>
        ''' Enabling this option restores owner, group, file permissions and audit information, 
        ''' but only if you have necessary privileges to read them.
        ''' <para></para>
        ''' Note that only NTFS file system supports file based security under Windows.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to restore file security information for extracted files; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Restore NTFS Streams")>
        <Description("Enables or disables restoring file security information for extracted files.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property RestoreFileSecurityInfo As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to clear "Archive" file attribute for extracted files.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to clear "Archive" file attribute for extracted files; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Clear ""Archive"" Attribute After Extracting")>
        <Description("Enables or disables clearing ""Archive"" file attribute for extracted files.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property ClearArchiveAttributeAfterExtracting As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to ignore file attributes for extracted files.
        ''' <para></para>
        ''' If <see langword="True"/>, RAR does not set general file attributes stored in archive to extracted files. 
        ''' This value preserves attributes assigned by operating system to a newly created file.
        ''' <para></para>
        ''' It affects <c>Archive</c>, <c>System</c>, <c>Hidden</c> and <c>Read-Only</c> attributes.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to ignore file attributes for extracted files; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Ignore File Attributes")>
        <Description("Enables or disables ignoring file attributes for extracted files.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property IgnoreFileAttributes As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating how file paths are restored for extracted files.
        ''' <para></para>
        ''' Default value is <see cref="RarFilePathMode.[Default]"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating how file paths are restored for extracted files.
        ''' </value>
        <DisplayName("File Path Mode")>
        <Description("Specifies how file paths are restored for extracted files.")>
        <DefaultValue(GetType(RarFilePathMode), "Default")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FilePathMode As RarFilePathMode = RarFilePathMode.[Default]

        ''' <summary>
        ''' Gets or sets a list of file and directory path masks to exclude from extraction.
        ''' <para></para>
        ''' The path must be inside of archive, not the file path on the disk after extracting it.
        ''' <para></para>
        ''' You can use <c>"path\filename"</c> (full or relative path) syntax to exclude this copy of "filename".
        ''' <para></para>
        ''' It is not recursive without wildcards, so <c>"filename"</c> mask will exclude 'filename' file 
        ''' only in root archive directory when extracting.
        ''' <para></para>
        ''' Use <c>"*\filename"</c> syntax to exclude "filename" recursively in all directories.
        ''' <para></para>
        ''' By default, masks containing wildcards are applied only to files. 
        ''' If you need a mask with wildcards to exclude several directories, use the special syntax for directory exclusion masks.
        ''' Such masks must have the trailing path separator character (<c>'\'</c>). 
        ''' For example, <c>"*tmp*\"</c> mask will exclude all directories matching "*tmp*", 
        ''' and <c>"*\tmp\"</c> mask will exclude all 'tmp' directories. 
        ''' Since wildcards are present, both masks will be applied to contents of current directory and all its subdirectories.
        ''' <para></para>
        ''' If you wish to exclude only one directory, specify the exact name of directory 
        ''' including the absolute or relative path without any wildcards. 
        ''' In this case you do not need to append the path separator to mask, 
        ''' which is required only for directory exclusion masks containing wildcards 
        ''' to distinguish them from file exclusion masks.
        ''' <para></para>
        ''' Default value is <c>null</c>, menaning no file and directory exclusion is performed.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A list of file and directory path masks to exclude from extraction; 
        ''' or <c>null</c>, menaning no file exclusion is performed.
        ''' </value>
        <DisplayName("File Exclusion Masks")>
        <Description("File and directory path masks to exclude from extraction.")>
        Public Property FileExclusionMasks As HashSet(Of String)

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
        ''' Gets or sets the timestamps to retore for extracted files.
        ''' <para></para>
        ''' Default value is <see cref="RarFileTimestamps.All"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The timestamps to retore for extracted files.
        ''' </value>
        <DisplayName("File Timestamps")>
        <Description("The timestamps to retore for extracted files.")>
        <DefaultValue(GetType(RarFileTimestamps), "All")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FileTimestamps As RarFileTimestamps = RarFileTimestamps.All

        ''' <summary>
        ''' Gets or sets a value indicating whether to allow potentially incompatible names for extracted files.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' While NTFS file system permits file names with trailing spaces and dots, 
        ''' also as reserved device names, a lot of Windows programs fail to process such names correctly. 
        ''' <para></para>
        ''' If this switch is not specified, RAR removes trailing spaces and dots, if any, from file names when extracting.
        ''' It also inserts the underscore character in the beginning of reserved device names, such as aux.
        ''' <para></para>
        ''' Enable this option if you need to extract such names as is. 
        ''' It might be associated with compatibility or even security risks.
        ''' </remarks>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to allow potentially incompatible names for extracted files; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Allow Potentially Incompatible Names")>
        <Description("Enables or disables allowing potentially incompatible names for extracted files.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property AllowPotentiallyIncompatibleNames As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to keep broken extracted files.
        ''' <para></para>
        ''' RAR, by default, deletes files with checksum errors after extraction. 
        ''' Setting this value to <see langword="True"/> specifies that files with checksum errors should not be deleted.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to keep broken extracted files; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Keep Broken Extracted Files")>
        <Description("Enables or disables keeping broken extracted files.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property KeepBrokenExtractedFiles As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to rename extracted files automatically if file with the same name already exists.
        ''' <para></para>
        ''' Renamed file will get the name like 'filename(N).txt', 
        ''' where 'filename.txt' is the original file name and 
        ''' 'N' is a number starting from 1 and incrementing if file exists.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to rename extracted files automatically if file with the same name already exists; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Rename Files")>
        <Description("Enables or disables renaming extracted files automatically if file with the same name already exists.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property RenameFiles As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating the file overwrite mode for extracted files.
        ''' <para></para>
        ''' Default value is <see cref="RarOverwriteMode.Overwrite"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating the file overwrite mode for extracted files.
        ''' </value>
        <DisplayName("Overwrite Mode")>
        <Description("Indicates the file overwrite mode for extracted files.")>
        <DefaultValue(GetType(RarOverwriteMode), "Skip")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property OverwriteMode As RarOverwriteMode = RarOverwriteMode.Skip

        ''' <summary>
        ''' Gets or sets a value indicating the mode for updating extracted files to disk.
        ''' <para></para>
        ''' Default value is <see cref="RarUpdateMode.Normal"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating the mode for updating extracted files to disk.
        ''' </value>
        <DisplayName("Update Mode")>
        <Description("Indicates the mode for updating extracted files to disk.")>
        <DefaultValue(GetType(RarUpdateMode), "Normal")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property UpdateMode As RarUpdateMode = RarUpdateMode.Normal

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarExtractionCommand"/> class 
        ''' with default value for <see cref="RarExtractionCommand.ExtractionMode"/> property, 
        ''' and empty values for <see cref="RarExtractionCommand.Archive"/> and 
        ''' <see cref="RarExtractionCommand.OutputDirectoryPath"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarExtractionCommand"/> class with 
        ''' the specified extraction mode and archive file path.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The extraction mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarExtractionCommand.ExtractionMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path to the RAR archive(s) to extract.
        ''' <para></para>
        ''' The path can be a mask with wildcards (e.g. "<c>*.rar</c>") and also pointing to multiple RAR archives.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarExtractionCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="outputDirectoryPath">
        ''' Optional. The output directory to place extracted files. This directory is created by RAR if it does not exist yet.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarExtractionCommand.OutputDirectoryPath"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="mode"/> is not a valid value of the <see cref="RarExtractionMode"/> enumeration.
        ''' <para></para>
        ''' This can occurs when an undefined or out-of-range value is passed to the method.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarExtractionMode, archivePath As String, Optional outputDirectoryPath As String = "")

            If Not [Enum].IsDefined(GetType(RarExtractionMode), mode) Then
                Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(RarExtractionMode))
            End If

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            Me.ExtractionMode = mode
            Me.Archive = archivePath
            Me.OutputDirectoryPath = outputDirectoryPath
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarExtractionCommand"/> 
        ''' to perform the operation specified in <see cref="RarExtractionCommand.ExtractionMode"/> property.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Extraction Mode
            Select Case Me.ExtractionMode

                Case RarExtractionMode.ExtractWithoutPath
                    sb.Append("e"c)

                Case RarExtractionMode.ExtractWithPath
                    sb.Append("x"c)

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.ExtractionMode), Me.ExtractionMode, GetType(RarExtractionMode))
            End Select

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarExtractionCommand arguments "

            ' Update Mode
            If Me.UpdateMode <> RarUpdateMode.Normal Then

                Select Case Me.UpdateMode

                    Case RarUpdateMode.Freshen
                        sb.Append(" -f")

                    Case RarUpdateMode.Update
                        sb.Append(" -u")

                    Case Else
                        Throw New InvalidEnumArgumentException(NameOf(Me.UpdateMode), Me.UpdateMode, GetType(RarUpdateMode))
                End Select
            End If

            ' Password
            If Me.Password IsNot Nothing Then

                Dim unsafePassword As String = Me.Password.GetUnsafePassword()
                sb.Append($" -p""{unsafePassword}""")
            End If

            ' Overwrite Mode
            Select Case Me.OverwriteMode

                Case RarOverwriteMode.Ask
                    sb.Append(" -o")

                Case RarOverwriteMode.Overwrite
                    sb.Append(" -o+")

                Case RarOverwriteMode.Skip
                    sb.Append(" -o-")

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.OverwriteMode), Me.OverwriteMode, GetType(RarOverwriteMode))
            End Select

            ' Dictionary Size
            sb.Append($" -mdx{CInt(Me.DictionarySize)}k") ' in kilobytes (because long values written in bytes will give error in rar.exe)

            ' Allow Potentially Incompatible Names
            If Me.AllowPotentiallyIncompatibleNames Then
                sb.Append(" -oni")
            End If

            ' Keep Broken Extracted Files
            If Me.KeepBrokenExtractedFiles Then
                sb.Append(" -kb")
            End If

            ' Rename Files
            If Me.RenameFiles Then
                sb.Append(" -or")
            End If

            ' File Timestamps
            If Me.FileTimestamps = RarFileTimestamps.None Then

                sb.Append($" -ts-")
            Else

                sb.Append($" -ts")

                If Me.FileTimestamps.HasFlag(RarFileTimestamps.CreationTime) Then
                    sb.Append("c"c)
                End If

                If Me.FileTimestamps.HasFlag(RarFileTimestamps.ModificationTime) Then
                    sb.Append("m"c)
                End If

                If Me.FileTimestamps.HasFlag(RarFileTimestamps.LastAccessTime) Then
                    sb.Append("a"c)
                End If
            End If

            ' Clear "Archive" Attribute After Extracting
            If Me.ClearArchiveAttributeAfterExtracting Then
                sb.Append(" -ac")
            End If

            ' Ignore File Attributes
            If Me.IgnoreFileAttributes Then
                sb.Append(" -ai")
            End If

            ' File Path Mode
            If Me.FilePathMode <> RarFilePathMode.Default Then

                Select Case Me.FilePathMode

                    Case RarFilePathMode.ExcludePathsFromFileNames
                        sb.Append(" -ep")

                    Case RarFilePathMode.ExcludeBaseDirFromFileNames
                        sb.Append(" -ep1")

                    Case RarFilePathMode.ExpandPathsToFullWithoutDriveLetter
                        sb.Append(" -ep2") ' This is the default RAR behavior if an "-ep" argument is not specified.

                    Case RarFilePathMode.ExpandPathsToFullWithDriveLetter
                        sb.Append(" -ep3")

                    Case Else
                        Throw New InvalidEnumArgumentException(NameOf(Me.FilePathMode), Me.FilePathMode, GetType(RarFilePathMode))
                End Select
            End If

            ' Restore File Security Info
            If Me.RestoreFileSecurityInfo Then
                sb.Append(" -ow")
            End If

            ' Process Symbolic Links as Links
            Select Case Me.ProcessSymbolicLinksAsLinks

                Case TriState.True
                    sb.Append(" -ol")

                Case TriState.False
                    sb.Append(" -ola")

                Case Else
                    sb.Append(" -ol-")
            End Select

            ' File Exclusion Masks
            If Me.FileExclusionMasks?.Any() Then

                For Each exclusionMask As String In Me.FileExclusionMasks
                    sb.Append($" -x""{exclusionMask}""")
                Next exclusionMask
            End If

            ' Output Directory Path
            If Not String.IsNullOrWhiteSpace(Me.OutputDirectoryPath) Then
                sb.Append($" -op""{Me.OutputDirectoryPath}""")
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
        ''' Returns a string that represents the current <see cref="RarExtractionCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarExtractionCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
