' ***********************************************************************
' Author   : ElektroStudios
' Modified : 19-May-2025
' ***********************************************************************

#Region " Usage Examples "

' Dim creationCommand As New RarCreationCommand(RarCreationMode.Add, "C:\Directory\New Archive.rar", "C:\Directory to add\") With {
'     .RarExecPath = ".\rar.exe",
'     .RarLicenseData = "(Your license key)",
'     .RecurseSubdirectories = True,
'     .EncryptionProperties = Nothing,
'     .SolidCompression = False,
'     .CompressionMode = RarCompressionMode.Normal,
'     .DictionarySize = RarDictionarySize.Mb__128,
'     .OverwriteMode = RarOverwriteMode.Overwrite,
'     .FilePathMode = RarFilePathMode.ExcludeBaseDirFromFileNames,
'     .FileTimestamps = RarFileTimestamps.All,
'     .AddQuickOpenInformation = TriState.True,
'     .ProcessHardLinksAsLinks = True,
'     .ProcessSymbolicLinksAsLinks = TriState.True,
'     .DuplicateFileMode = RarDuplicateFileMode.Enabled,
'     .FileChecksumMode = RarFileChecksumMode.BLAKE2sp,
'     .ArchiveComment = New RarArchiveComment("Hello world!"),
'     .RecoveryRecordPercentage = 0,
'     .SplitIntoVolumes = Nothing,
'     .FileTypesToStore = Nothing
' }
' 
' Using rarExecutor As New RarCommandExecutor(creationCommand)
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
'     Console.WriteLine($"Command-line to execute: {creationCommand}")
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
Imports System.IO
Imports System.Linq
Imports System.Text

Imports DevCase.Extensions
'Imports DevCase.Runtime.Attributes
'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarCreationCommand "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the command-line arguments for RAR archiving.
    ''' <para></para>
    ''' This command adds (creates), freshen or update files in the specified RAR archive.
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB">
    ''' Dim creationCommand As New RarCreationCommand(RarCreationMode.Add, "C:\Directory\New Archive.rar", "C:\Directory to add\") With {
    '''     .RarExecPath = ".\rar.exe",
    '''     .RarLicenseData = "(Your license key)",
    '''     .RecurseSubdirectories = True,
    '''     .EncryptionProperties = Nothing,
    '''     .CompressionMode = RarCompressionMode.Normal,
    '''     .DictionarySize = RarDictionarySize.Mb__128,
    '''     .SolidCompression = False,
    '''     .OverwriteMode = RarOverwriteMode.Overwrite,
    '''     .FilePathMode = RarFilePathMode.ExcludeBaseDirFromFileNames,
    '''     .FileTimestamps = RarFileTimestamps.All,
    '''     .AddQuickOpenInformation = TriState.True,
    '''     .ProcessHardLinksAsLinks = True,
    '''     .ProcessSymbolicLinksAsLinks = TriState.True,
    '''     .DuplicateFileMode = RarDuplicateFileMode.Enabled,
    '''     .FileChecksumMode = RarFileChecksumMode.BLAKE2sp,
    '''     .ArchiveComment = New RarArchiveComment("Hello world!"),
    '''     .RecoveryRecordPercentage = 0,
    '''     .SplitIntoVolumes = Nothing,
    '''     .FileTypesToStore = Nothing
    ''' }
    ''' 
    ''' Using rarExecutor As New RarCommandExecutor(creationCommand)
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
    '''     Console.WriteLine($"Command-line to execute: {creationCommand}")
    '''     Dim exitcode As RarExitCode = rarExecutor.ExecuteRarAsync().Result
    ''' End Using
    ''' </code>
    ''' </example>
    Public Class RarCreationCommand : Inherits RarCommandBase

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive to create or update.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive to create or update.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive to create or update.")>
        Public Overrides Property Archive As String

        ''' <summary>
        ''' Gets or sets a value indicating whether to lock the archive. 
        ''' <para></para>
        ''' RAR cannot modify locked archives, so locking important archives prevents their accidental modification by RAR.
        ''' <para></para>
        ''' Such protection might be especially useful in case of RAR commands processing archives in groups.
        ''' <para></para>
        ''' This option is not intended or able to prevent modification by other tools or willful third party. 
        ''' It implements a safety measure only for accidental data change by RAR.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        <DisplayName("Lock Archive")>
        <Description("Enables or disables locking the archive.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property LockArchive As Boolean

        ''' <summary>
        ''' Gets or sets the collection of file and/or directory path masks to include in the archive.
        ''' <para></para>
        ''' Paths can be relative or absulute, and include wildcards.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A <see cref="HashSet(Of String)"/> containing the file and/or directory path masks to include in the archive.
        ''' </value>
        <DisplayName("Files and Directories")>
        <Description("The collection of file and/or directory path masks to include in the archive.")>
        Public Property FileAndDirMasks As New HashSet(Of String)(StringComparer.Ordinal)

        ''' <summary>
        ''' Gets or sets the creation mode determining whether to add (create), freshen or update files in the archive.
        ''' <para></para>
        ''' Default value is <see cref="RarCreationMode.Add"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The archive creation mode.
        ''' </value>
        <DisplayName("Creation Mode")>
        <Description("Creation mode determining whether to add (create), freshen or update files in the archive.")>
        <DefaultValue(GetType(RarCreationMode), "Add")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property CreationMode As RarCreationMode = RarCreationMode.Add

        ''' <summary>
        ''' Gets or sets the compression mode for archiving, determining the compression time and ratio.
        ''' <para></para>
        ''' Default value is <see cref="RarCompressionMode.Normal"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The compression mode for archiving.
        ''' </value>
        <DisplayName("Compression Mode")>
        <Description("Compression mode determining the compression time and ratio.")>
        <DefaultValue(GetType(RarCompressionMode), "Normal")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property CompressionMode As RarCompressionMode = RarCompressionMode.Normal

        ''' <summary>
        ''' Gets or sets the size of the dictionary used for file compression.
        ''' <para></para>
        ''' Sliding dictionary is the memory area used by compression algorithm to find and compress repeated data patterns
        ''' <para></para>
        ''' Higher values may improve compression but require more memory during compression and extraction.
        ''' <para></para>
        ''' If size of file to compress, or total files size in case of solid archive, is larger than dictionary size, 
        ''' increasing the dictionary is likely to increase the compression ratio, 
        ''' reduce the archiving speed and increase memory requirements. 
        ''' <para></para>
        ''' Compression memory requirements vary depending on the dictionary size. Rough estimate is 7x of dictionary size for 1 GB.
        ''' <para></para>
        ''' Default value is <see cref="RarDictionarySize.Mb__128"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The size of the dictionary used for file compression.
        ''' </value>
        <DisplayName("Dictionary Size")>
        <Description("Size of the dictionary used for file compression. Higher values may improve compression but require more memory during compression and extraction.")>
        <DefaultValue(GetType(RarDictionarySize), "Mb__128")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property DictionarySize As RarDictionarySize = RarDictionarySize.Mb__128

        ''' <summary>
        ''' Gets or sets a value indicating whether to create a solid archive.
        ''' <para></para>
        ''' A solid archive is an archive packed by a special compression method, 
        ''' which treats several or all files, within the archive, as one continuous data stream.
        ''' <para></para>
        ''' Solid archiving significantly increases compression when adding a large number of small, similar files. 
        ''' But it also has a few important disadvantages: slower updating of existing solid archives, 
        ''' slower access to individual files, and lower damage resistance.
        ''' <para></para>
        ''' Note that for extracting a single file in a solid archive, it will require decompressing the entire archive.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to create a solid archive; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Solid Compression")>
        <Description("Enables or disables solid compression. A solid archive may potentially improve compression ratio, but for extracting a single file will require decompressing the entire archive.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SolidCompression As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to sort files by extension when creating a solid archive.
        ''' <para></para>
        ''' If this value and <see cref="RarCreationCommand.SolidCompression"/> are <see langword="True"/>, 
        ''' this configuration may potentially improve compression ratio. 
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to sort files by extension when creating a solid archive; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Sort Files By Extension")>
        <Description("Enables or disables sorting files by extension when creating a solid archive.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SortFilesByExtension As Boolean

        ''' <summary>
        ''' Gets or sets an object that enables splitting the archive into volumes (*.part1.rar, *.part2.rar, etc) 
        ''' of the specified size, and with the specified number of recovery volumes.
        ''' <para></para>
        ''' Default value is <c>null</c>, meaning the archive will not be splitted into volumes.
        ''' </summary>
        ''' 
        ''' <value>
        ''' An object that enables splitting the archive into volumes (*.part1.rar, *.part2.rar, etc) 
        ''' of the specified size, and with the specified number of recovery volumes.
        ''' </value>
        <DisplayName("Volume Split Options")>
        <Description("Enables or disables splitting the archive into volumes (*.part1.rar, *.part2.rar, etc).")>
        Public Property VolumeSplitOptions As RarVolumeSplitOptions

        ''' <summary>
        ''' Gets or sets a value indicating whether to add 'Quick Open Information' record to files when archiving.
        ''' <para></para>
        ''' Accepted values are:
        ''' <list type="bullet">
        '''   <item>
        '''     <term><see cref="TriState.False"/></term>
        '''     <description>
        '''     Excludes adding the 'Quick Open Information' record completely for all files. 
        '''     <para></para> 
        '''     Minimizes archive size, with no concern for open speed.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.True"/></term>
        '''     <description>
        '''     Adds the 'Quick Open Information' record for all files.  
        '''     <para></para> 
        '''     Maximizes archive open speed, regardless of archive size.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.UseDefault"/></term>
        '''     <description>
        '''     Adds the 'Quick Open Information' record only for relatively large files.
        '''     <para></para> 
        '''     Offers a reasonable tradeoff between archive open speed and size.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' <para></para>
        ''' Default value is <see cref="TriState.UseDefault"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' RAR archives store every file header containing information such as file name, time, 
        ''' size and attributes immediately before data of described file. 
        ''' This approach is more damage resistant than storing all file headers in a single continuous block, 
        ''' which if broken or truncated would destroy the entire archive contents. 
        ''' <para></para>
        ''' But while being more reliable, such file headers scattered around the entire archive are slower to 
        ''' access if we need to quickly open the archive contents in a shell like WinRAR graphical interface.
        ''' <para></para>
        ''' To improve archive open speed and still not make the entire archive dependent on a single damaged block, 
        ''' RAR 5.0 archives can include an optional quick open record. 
        ''' Such record is added to the end of archive and contains copies of file names and 
        ''' other file information stored in a single continuous block additionally to
        ''' normal file headers inside of archive. 
        ''' Since the block is continuous, its contents can be read quickly, 
        ''' without necessity to perform a lot of disk seek operations. 
        ''' Every file header in this block is protected with a checksum. 
        ''' If RAR detects that quick open information is damaged, 
        ''' it resorts to reading individual headers from inside of archive, 
        ''' so damage resistance is not lessened.
        ''' </remarks>
        ''' 
        ''' <value>
        ''' <see cref="TriState.False"/> to exclude adding the 'Quick Open Information' record completely for all files, 
        ''' <see cref="TriState.True"/> to add the 'Quick Open Information' record for all files, or 
        ''' <see cref="TriState.UseDefault"/> to add the 'Quick Open Information' record only for relatively large files; 
        ''' </value>
        <DisplayName("Add Quick Open Information")>
        <Description("Enables or disables adding Quick Open Information to the archive, which can speed up access.")>
        <DefaultValue(GetType(TriState), "UseDefault")> ' <LocalizableTriState> <TypeConverter(GetType(LocalizableTriStateConverter))>
        Public Property AddQuickOpenInformation As TriState = TriState.UseDefault

        ''' <summary>
        ''' Gets or sets a value indicating the file overwrite mode when archiving.
        ''' <para></para>
        ''' Default value is <see cref="RarOverwriteMode.Overwrite"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating the file overwrite mode when archiving.
        ''' </value>
        <DisplayName("Overwrite Mode")>
        <Description("Indicates the file overwrite mode when archiving.")>
        <DefaultValue(GetType(RarOverwriteMode), "Overwrite")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property OverwriteMode As RarOverwriteMode = RarOverwriteMode.Overwrite

        ''' <summary>
        ''' Gets or sets the deletion behavior for files when archiving.
        ''' <para></para>
        ''' Default value is <see cref="RarFileDeletionMode.DoNotDelete"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The deletion behavior for files when archiving.
        ''' </value>
        <DisplayName("File Deletion Mode")>
        <Description("Specifies the deletion behavior for files when archiving.")>
        <DefaultValue(GetType(RarFileDeletionMode), "DoNotDelete")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FileDeletionMode As RarFileDeletionMode = RarFileDeletionMode.DoNotDelete

        ''' <summary>
        ''' Gets or sets a value indicating whether to test files for checksum errors after archiving.
        ''' <para></para>
        ''' This option is especially useful in combination with the <see cref="RarFileDeletionMode.SendToRecycleBin"/>,
        ''' <see cref="RarFileDeletionMode.PermanentDeletion"/> and <see cref="RarFileDeletionMode.Wipe"/> options, 
        ''' so files will be deleted only if the archive had been successfully tested.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' <value>
        ''' <see langword="True"/> to test files for checksum errors after archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Test Files After Archiving")>
        <Description("Enables or disables testing files for checksum errors after archiving.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property TestFilesAfterArchiving As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating how to process symbolic links when achiving. 
        ''' <para></para>
        ''' Accepted values are:
        ''' <list type="bullet">
        '''   <item>
        '''     <term><see cref="TriState.True"/></term>
        '''     <description>
        '''     Save symbolic links as links, so file or directory contents is not archived.
        '''     <para></para>
        '''     It also saves reparse points as links. Such archive entries are restored as symbolic links or reparse points when extracting.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.False"/></term>
        '''     <description>
        '''     Adds the contents of the symbolic links as is.
        '''     </description>
        '''   </item>
        '''   <item>
        '''     <term><see cref="TriState.UseDefault"/></term>
        '''     <description>
        '''     Completely skips symbolic links when archiving.
        '''     </description>
        '''   </item>
        ''' </list>
        ''' <para></para>
        ''' Default value is <see cref="TriState.UseDefault"/>.
        ''' </summary>
        ''' <value>
        ''' <see cref="TriState.True"/> to save symbolic links as links,
        ''' <see cref="TriState.False"/> to add the contents of the symbolic links as is, or 
        ''' <see cref="TriState.UseDefault"/> to completely skips symbolic links when archiving.
        ''' </value>
        <DisplayName("Process Symbolic Links as Links")>
        <Description("Enables or disables processing symbolic links as the link instead of the actual file.")>
        <DefaultValue(GetType(TriState), "UseDefault")> ' <LocalizableTriState> <TypeConverter(GetType(LocalizableTriStateConverter))>
        Public Property ProcessSymbolicLinksAsLinks As TriState = TriState.UseDefault

        ''' <summary>
        ''' Gets or sets a value indicating to save hard links as the link instead of the actual file. 
        ''' <para></para>
        ''' If <see langword="True"/> and archiving files include several hard links, 
        ''' store the first archived hard link as usual file and the rest of hard links in the same set as links to this first file. 
        ''' <para></para>
        ''' When extracting such files, RAR will create hard links instead of usual files.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' You must not delete or rename the first hard link in archive after the archive was created, 
        ''' because it will make extraction of following links impossible. 
        ''' If you modify the first link, all following links will also have the modified contents after extracting.
        ''' Extraction command must involve the first hard link to create following hard links successfully.
        ''' </remarks>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to save hard links as the link instead of the actual file; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Process Hard Links as Links")>
        <Description("Enables or disables saving hard links as the link instead of the actual file.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property ProcessHardLinksAsLinks As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to save NTFS streams for files when archiving.
        ''' <para></para>
        ''' This option has meaning only for NTFS file system and allows to save alternate data streams associated with a file. 
        ''' <para></para>
        ''' You may need to specify it when archiving if you use software storing data in alternative streams and wish to preserve these streams.
        ''' <para></para>
        ''' Streams are not saved for NTFS encrypted files.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to save NTFS streams for files when archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Save NTFS Streams")>
        <Description("Enables or disables saving NTFS streams for files when archiving.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SaveNtfsStreams As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to save file security information for files when archiving.
        ''' <para></para>
        ''' Enabling this option stores owner, group, file permissions and audit information, 
        ''' but only if you have necessary privileges to read them.
        ''' <para></para>
        ''' Note that only NTFS file system supports file based security under Windows.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to save file security information for files when archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Save file security information")>
        <Description("Enables or disables saving file security information for files when archiving.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SaveFileSecurityInfo As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to allow to process files opened by other applications for writing.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' Allows to process files opened by other applications for writing. 
        ''' <para></para>
        ''' This option helps if an external application allowed read access to file, 
        ''' but if all types of file access are prohibited, the file open operation will still fail. 
        ''' <para></para>
        ''' This option could be dangerous, because it allows to archive a file, 
        ''' which at the same time is modified by another application, so use it carefully.
        ''' </remarks>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to allow to process files opened by other applications for writing; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Open Shared Files")>
        <Description("Enables or disables allowing to process files opened by other applications for writing.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property OpenSharedFiles As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to clear "Archive" file attribute of files after archiving.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to clear "Archive" file attribute of files after archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Clear ""Archive"" Attribute After Archiving")>
        <Description("Enables or disables clearing ""Archive"" file attribute of files after archiving.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property ClearArchiveAttributeAfterArchiving As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to ignore file attributes when archiving.
        ''' <para></para>
        ''' If <see langword="True"/>, predefined values, typical for file and directory, 
        ''' are stored instead of actual attributes.
        ''' <para></para>
        ''' It affects <c>Archive</c>, <c>System</c>, <c>Hidden</c> and <c>Read-Only</c> attributes.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to ignore file attributes when archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Ignore File Attributes")>
        <Description("Enables or disables ignoring file attributes when archiving.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property IgnoreFileAttributes As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to save the RAR archive metadata, 
        ''' which includes the original archive name and creation time.
        ''' <para></para>
        ''' This only affects the RAR file metadata itself, not the archived files within the archive.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to save the RAR archive metadata; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Save Archive Name and Time")>
        <Description("Enables or disables saving the RAR archive metadata, which includes the original archive name and creation time.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SaveArchiveNameAndTime As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to synchronize archive contents.
        ''' <para></para>
        ''' If <see langword="True"/>, those archived files which are not present in 
        ''' the list of the currently added files, will be deleted from the archive. 
        ''' <para></para>
        ''' It is convenient to use this option in combination with <see cref="RarCreationMode.Update"/> 
        ''' to synchronize contents of archive and archiving directory.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to synchronize archive contents; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Synchronize Archive Contents")>
        <Description("Enables or disables synchronizing archive contents.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SynchronizeArchiveContents As Boolean

        ''' <summary>
        ''' Gets or sets a value indicating whether to add empty directories when archiving.
        ''' <para></para>
        ''' Default value is <see langword="True"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to add empty directories when archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Add Empty Directories")>
        <Description("Enables or disables adding empty directories when archiving.")>
        <DefaultValue(True)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property AddEmptyDirectories As Boolean = True

        ''' <summary>
        ''' Gets or sets a value indicating how file paths are stored when adding files to an archive.
        ''' <para></para>
        ''' Default value is <see cref="RarFilePathMode.[Default]"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating how file paths are stored when adding files to an archive.
        ''' </value>
        <DisplayName("File Path Mode")>
        <Description("Specifies how file paths are stored when adding files to an archive.")>
        <DefaultValue(GetType(RarFilePathMode), "Default")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FilePathMode As RarFilePathMode = RarFilePathMode.[Default]

        ''' <summary>
        ''' Gets or sets a value indicating the type of file checksum to use when archiving.
        ''' <para></para>
        ''' File data integrity in RAR archive is protected by checksums calculated and stored for every archived file.
        ''' <para></para>
        ''' Default value is <see cref="RarFileChecksumMode.CRC32"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The type of file checksum to use when archiving.
        ''' </value>
        <DisplayName("File Checksum Mode")>
        <Description("Indicates the type of file checksum to use when archiving.")>
        <DefaultValue(GetType(RarFileChecksumMode), "CRC32")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FileChecksumMode As RarFileChecksumMode = RarFileChecksumMode.CRC32

        ''' <summary>
        ''' Gets or sets a value indicating how RAR processes identical (duplicate) files when archiving.
        ''' <para></para>
        ''' Default value is <see cref="RarDuplicateFileMode.Disabled"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating how RAR processes identical (duplicate) files when archiving.
        ''' </value>
        <DisplayName("Duplicate Files Behavior")>
        <Description("Indicates how RAR processes identical files when archiving.")>
        <DefaultValue(GetType(RarDuplicateFileMode), "Disabled")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property DuplicateFileMode As RarDuplicateFileMode = RarDuplicateFileMode.Disabled

        ''' <summary>
        ''' Gets or sets a value indicating what to do with the RAR archive date when modifying a RAR archive.
        ''' <para></para>
        ''' Default value is <see cref="RarArchiveTimestampMode.Keep"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating what to do with the RAR archive date when modifying a RAR archive.
        ''' </value>
        <DisplayName("Archive Timestamp Mode")>
        <Description("Indicates what to do with the RAR archive date when modifying the archive.")>
        <DefaultValue(GetType(RarArchiveTimestampMode), "Keep")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property ArchiveTimestampMode As RarArchiveTimestampMode = RarArchiveTimestampMode.Keep

        ''' <summary>
        ''' Gets or sets a collection of file types to store without compression when archiving.
        ''' <para></para>
        ''' This option may be used to store already compressed files (e.g. "rar", "zip", "mp3", etc), 
        ''' which helps to increase archiving speed without noticeable loss in the compression ratio.
        ''' <para></para>
        ''' It is also allowed to specify wildcard file masks In the list (so "*.rar", "*.zip", "*.mp3", etc will work too).
        ''' <para></para>
        ''' Default value contemplates a comprehensive list of already compressed 
        ''' archive formats, images, music, video, documents and gaming file types.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to exclude base directory path from file names when archiving; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("File Types to Store")>
        <Description("File types to store without compression when archiving.")>
        Public Property FileTypesToStore As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        "3gp", "7z", "aac", "ac3", "ace", "arc", "arj", "asf", "asx", "avc", "avi",
        "bdmov", "bk2", "bz2", "bzip", "bzip2", "divx", "dts", "dv", "evo", "f4v",
        "fla", "flac", "flv", "fsb", "gif", "gz", "gz2", "gza", "gzi", "gzip",
        "h264", "h265", "ifo", "jpeg", "jpg", "kgb", "lha", "lz", "lz4", "lzh",
        "lzma", "lzo", "m1v", "m2t", "m2ts", "m2v", "m4a", "m4v", "mid", "midi",
        "mkv", "mov", "mp3", "mp4", "mpeg", "mpg", "mpv4", "mts", "ogg", "ogm", "ogv",
        "p7z", "pam", "paq6", "paq7", "paq8", "par", "par2", "pdf", "pea", "pls",
        "psarc", "qt", "rar", "rma", "rmvb", "sfark", "sfd", "sfx", "tbz", "tbz2",
        "tgz", "tlz", "tlzma", "ts", "txb", "txz", "uc0", "uc2", "uha", "usm",
        "vob", "vp6", "vpl", "w64", "wad", "webm", "wma", "wmv", "zip",
        "zipx", "zix", "zpaq", "zpi", "zz"}

        ''' <summary>
        ''' Gets or sets a list of file and directory path masks to exclude from archiving.
        ''' <para></para>
        ''' You can use <c>"path\filename"</c> (full or relative path) syntax to exclude this copy of "filename".
        ''' <para></para>
        ''' If mask contains wildcards, it applies to files in current directory and its subdirectories. 
        ''' <para></para>
        ''' It is not recursive without wildcards, so <c>"filename"</c> mask will exclude 'filename' file 
        ''' only in current directory when archiving.
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
        ''' A list of file and directory path masks to exclude from archiving; 
        ''' or <c>null</c>, menaning no file exclusion is performed.
        ''' </value>
        <DisplayName("File Exclusion Masks")>
        <Description("File and directory path masks to exclude from archiving.")>
        Public Property FileExclusionMasks As HashSet(Of String)

        ''' <summary>
        ''' Gets or sets the RAR encryption properties used to encrypt files when archiving.
        ''' <para></para>
        ''' If this value is <c>null</c>, no encryption is performed.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The RAR encryption properties used to encrypt files when archiving; 
        ''' or <c>null</c> if no encryption is specified.
        ''' </value>
        <DisplayName("Encryption Properties")>
        <Description("RAR encryption properties used to encrypt files when archiving.")>
        Public Property EncryptionProperties As RarEncryptionProperties

        ''' <summary>
        ''' Gets or sets the percentage of recovery record to add when creating or modifying a RAR archive.
        ''' <para></para>
        ''' Valid value is from 0% to 1000%. A value of zero means no recovery record is added. 
        ''' If value exceeds 1000, it gets automatically adjusted by <c>rar.exe</c> process.
        ''' <para></para>
        ''' A high recovery record percentage increase redundancy and archive size, 
        ''' but improves the chance of repairing a damaged archive.
        ''' <para></para>
        ''' Default value is 0% (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The percentage of recovery record to add when creating or modifying a RAR archive.
        ''' </value>
        <DisplayName("Recovery Record Percentage")>
        <Description("The percentage of recovery record to add when creating or modifying a RAR archive. Valid value is from 0 to 1000.")>
        <DefaultValue(0)>
        Public Property RecoveryRecordPercentage As Short = 0

        ''' <summary>
        ''' Gets or sets the timestamps to preserve in files when archiving.
        ''' <para></para>
        ''' Default value is <see cref="RarFileTimestamps.All"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The timestamps to preserve in files when archiving.
        ''' </value>
        <DisplayName("File Timestamps")>
        <Description("The timestamps to preserve in files when archiving.")>
        <DefaultValue(GetType(RarFileTimestamps), "All")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property FileTimestamps As RarFileTimestamps = RarFileTimestamps.All

        ''' <summary>
        ''' Gets or sets an object that enables to add a commentary string to the RAR archive.
        ''' <para></para>
        ''' Default value is <c>null</c> (no comment is added to the RAR archive).
        ''' </summary>
        ''' 
        ''' <value>
        ''' An object that enables to add a commentary string to the RAR archive, or <c>null</c> if no comment is to be added.
        ''' </value>
        <DisplayName("Archive Comment")>
        <Description("An object that enables to add a commentary string to the RAR archive.")>
        Public Property ArchiveComment As RarArchiveComment

        ''' <summary>
        ''' Gets or sets the file path that points to the Self-Extracting (SFX) module to be used for creating a SFX archive.
        ''' <para></para>
        ''' Default value is <c>null</c>, menaning no SFX archive will be created.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path that points to the Self-Extracting (SFX) module to be used for creating a SFX archive; 
        ''' or <c>null</c>, meaning no SFX archive will be created.
        ''' </value>
        <DisplayName("SFX Module Path")>
        <Description("The file path to the SFX module to be used for creating a SFX archive.")>
        Public Property SfxModulePath As String

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreationCommand"/> class 
        ''' with default value for <see cref="RarCreationCommand.CreationMode"/> property, 
        ''' and empty values for <see cref="RarCreationCommand.Archive"/> and 
        ''' <see cref="RarCreationCommand.FileAndDirMasks"/> properties.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreationCommand"/> class with 
        ''' the specified creation mode and archive file path.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The creation mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.CreationMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path where the RAR archive will be created or updated depending on the <paramref name="mode"/> parameter value.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="InvalidEnumArgumentException">
        ''' Thrown when <paramref name="mode"/> is not a valid value of the <see cref="RarCreationMode"/> enumeration.
        ''' <para></para>
        ''' This can occurs when an undefined or out-of-range value is passed to the method.
        ''' </exception>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="archivePath"/> is null, empty, or contains only whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarCreationMode, archivePath As String)

            If Not [Enum].IsDefined(GetType(RarCreationMode), mode) Then
                Throw New InvalidEnumArgumentException(NameOf(mode), mode, GetType(RarCreationMode))
            End If

            If String.IsNullOrWhiteSpace(archivePath) Then
                Throw New ArgumentException("The archive path cannot be null or empty.", NameOf(archivePath))
            End If

            Me.CreationMode = mode
            Me.Archive = archivePath
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreationCommand"/> class with 
        ''' the specified creation mode, archive file path, and a list of file and/or directory path masks to include in the archive.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The creation mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.CreationMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path where the RAR archive will be created or updated depending on the <paramref name="mode"/> parameter value.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="fileAndDirMasks">
        ''' An array of file and/or directory path masks to include in the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' Paths can be relative or absulute, and include wildcards.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.FileAndDirMasks"/> property.
        ''' </param>
        ''' 
        ''' <exception cref="ArgumentException">
        ''' Thrown when <paramref name="fileAndDirMasks"/> is null or empty.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(mode As RarCreationMode, archivePath As String, ParamArray fileAndDirMasks As String())

            Me.New(mode, archivePath)

            If fileAndDirMasks Is Nothing OrElse fileAndDirMasks.Length = 0 Then
                Throw New ArgumentException("At least one file or directory path must be specified.", NameOf(fileAndDirMasks))
            End If

            Dim maskSet As New HashSet(Of String)(StringComparer.Ordinal)
            For Each mask As String In fileAndDirMasks
                If Directory.Exists(mask) Then
                    mask = StringExtensions.EnsureEndsWith(mask, "\"c, StringComparison.OrdinalIgnoreCase)
                End If

                maskSet.Add(mask)
            Next
            Me.FileAndDirMasks = maskSet
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreationCommand"/> class with 
        ''' the specified creation mode, archive file path, and a list of file and/or directory path masks to include in the archive.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The creation mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.CreationMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path where the RAR archive will be created or updated depending on the <paramref name="mode"/> parameter value.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="fileAndDirMasks">
        ''' An array of file and/or directory path masks to include in the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' Paths can be relative or absulute, and include wildcards.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.FileAndDirMasks"/> property.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(mode As RarCreationMode, archivePath As String, fileAndDirMasks As IEnumerable(Of String))

            Me.New(mode, archivePath, fileAndDirMasks?.ToArray())
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCreationCommand"/> class with 
        ''' the specified creation mode, archive file path, and a list of files and directories to include in the archive.
        ''' </summary>
        ''' 
        ''' <param name="mode">
        ''' The creation mode for the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.CreationMode"/> property.
        ''' </param>
        ''' 
        ''' <param name="archivePath">
        ''' The file path where the RAR archive will be created or updated depending on the <paramref name="mode"/> parameter value.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.Archive"/> property.
        ''' </param>
        ''' 
        ''' <param name="filesAndDirs">
        ''' A collection of files and/or directories to include in the RAR archive specified in <paramref name="archivePath"/> parameter.
        ''' <para></para>
        ''' This value will be assigned to <see cref="RarCreationCommand.FileAndDirMasks"/> property.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(mode As RarCreationMode, archivePath As String, ParamArray filesAndDirs As FileSystemInfo())

            Me.New(mode, archivePath, filesAndDirs?.Select(Function(fsi As FileSystemInfo) fsi.FullName).ToArray())
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <see cref="String"/> with the <c>rar.exe</c> command-line arguments 
        ''' based on the values of <see cref="RarCommandBase"/> and this <see cref="RarCreationCommand"/> 
        ''' to perform the operation specified in <see cref="RarCreationCommand.CreationMode"/> property.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' The fully configured <c>rar.exe</c> command-line arguments string.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overrides Function GetRarCommandlineArguments() As String

            Dim sb As New StringBuilder()

#Region " Starting required argument "

            ' Creation Mode
            Select Case Me.CreationMode

                Case RarCreationMode.Add
                    sb.Append("a"c)

                Case RarCreationMode.Freshen
                    sb.Append("f"c)

                Case RarCreationMode.Update
                    sb.Append("u"c)

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.CreationMode), Me.CreationMode, GetType(RarCreationMode))
            End Select

#End Region

#Region " RarCommandBase arguments "

            sb.Append(MyBase.GetRarCommandlineArguments())

#End Region

#Region " RarCreationCommand arguments "

            ' Test Files After Archiving
            If Me.TestFilesAfterArchiving Then
                sb.Append(" -t")
            End If

            ' Lock Archive
            If Me.LockArchive Then
                sb.Append(" -k")
            End If

            ' Encryption Properties
            If Me.EncryptionProperties IsNot Nothing Then

                Dim unsafePassword As String = Me.EncryptionProperties.GetUnsafePassword()

                If Me.EncryptionProperties.EncryptFileHeader Then
                    sb.Append($" -hp""{unsafePassword}""")
                Else
                    sb.Append($" -p""{unsafePassword}""")
                End If
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

            ' Compression Mode
            sb.Append($" -m{CInt(Me.CompressionMode)}")

            ' Dictionary Size
            sb.Append($" -md{CInt(Me.DictionarySize)}k") ' in kilobytes (because long values written in bytes will give error in rar.exe)

            ' Save RAR Archive Name and Time
            If Me.SaveArchiveNameAndTime Then
                sb.Append(" -ams")
            End If

            ' Archive Timestamp Mode
            Select Case Me.ArchiveTimestampMode

                Case RarArchiveTimestampMode.Keep
                    sb.Append(" -tk")

                Case RarArchiveTimestampMode.Update
                    sb.Append(" -tl")

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.ArchiveTimestampMode), Me.ArchiveTimestampMode, GetType(RarArchiveTimestampMode))
            End Select

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

                sb.Append("+"c)
            End If

            ' Open Shared Files
            If Me.OpenSharedFiles Then
                sb.Append(" -dh")
            End If

            ' Clear "Archive" Attribute After Archiving
            If Me.ClearArchiveAttributeAfterArchiving Then
                sb.Append(" -ac")
            End If

            ' Ignore File Attributes
            If Me.IgnoreFileAttributes Then
                sb.Append(" -ai")
            End If

            ' Synchronize Archive Contents
            If Me.SynchronizeArchiveContents Then
                sb.Append(" -as")
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

            ' Don't add Empty Directories
            If Not Me.AddEmptyDirectories Then
                sb.Append(" -ed")
            End If

            ' File Checksum Type
            If Me.FileChecksumMode = RarFileChecksumMode.BLAKE2sp Then
                sb.Append(" -htb")
            Else
                ' Ignore. rar.exe defaults to CRC-32 (command: "-htc")
            End If

            ' Solid Compression
            If Me.SolidCompression Then
                sb.Append(" -s")
            Else
                sb.Append(" -s-")
            End If

            ' Don't Sort Files by Extension
            If Not Me.SortFilesByExtension Then
                sb.Append(" -ds")
            End If

            ' Save NTFS Streams
            If Me.SaveNtfsStreams Then
                sb.Append(" -os")
            End If

            ' Save File Security Info
            If Me.SaveFileSecurityInfo Then
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

            ' Process Hard Links as Links
            If Me.ProcessHardLinksAsLinks Then
                sb.Append(" -oh")
            End If

            ' Duplicate Files Behavior
            Select Case Me.DuplicateFileMode

                Case RarDuplicateFileMode.Disabled
                    sb.Append(" -oi0")

                Case RarDuplicateFileMode.Enabled
                    sb.Append(" -oi1")

                Case RarDuplicateFileMode.EnabledAndShownInOutput
                    sb.Append(" -oi2")

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.DuplicateFileMode), Me.DuplicateFileMode, GetType(RarDuplicateFileMode))
            End Select

            ' Add "Quick Open Information"
            Select Case Me.AddQuickOpenInformation

                Case TriState.True
                    sb.Append(" -qo+")

                Case TriState.False
                    sb.Append(" -qo-")

                Case Else
                    sb.Append(" -qo")
            End Select

            ' File Deletion Mode
            Select Case Me.FileDeletionMode

                Case RarFileDeletionMode.SendToRecycleBin
                    sb.Append(" -dr")

                Case RarFileDeletionMode.PermanentDeletion
                    sb.Append(" -df")

                Case RarFileDeletionMode.Wipe
                    sb.Append(" -dw")

                Case RarFileDeletionMode.DoNotDelete
                    ' Ignore. rar.exe does not delete files by default.

                Case Else
                    Throw New InvalidEnumArgumentException(NameOf(Me.FileDeletionMode), Me.FileDeletionMode, GetType(RarFileDeletionMode))
            End Select

            ' Split Into Volumes
            If Me.VolumeSplitOptions IsNot Nothing Then

                ' Dim byteSize As Double = Me.VolumeSplitOptions.VolumeSize.Size(DevCase.Core.IO.DigitalInformation.DigitalStorageUnits.Byte)
                Dim byteSize As Long = Me.VolumeSplitOptions.VolumeSize
                If byteSize <> 0 Then
                    sb.Append($" -v{byteSize}b")
                Else
                    sb.Append($" -v")
                End If

                ' Number of Recovery Volumes
                If Me.VolumeSplitOptions.NumberOfRecoveryVolumes > 0 Then
                    sb.Append($" -rv{Me.VolumeSplitOptions.NumberOfRecoveryVolumes}")
                End If
            End If

            ' Recovery Record Percentage
            If Me.RecoveryRecordPercentage > 0 Then
                sb.Append($" -rr{Me.RecoveryRecordPercentage}")
            End If

            ' SFX Module Path
            If Not String.IsNullOrWhiteSpace(Me.SfxModulePath) Then
                sb.Append($" -sfx""{Me.SfxModulePath}""")
            End If

            ' Archive Comment
            If Me.ArchiveComment IsNot Nothing Then

                If Not String.IsNullOrWhiteSpace(Me.ArchiveComment.Comment) Then

                    Dim encoding As Encoding = If(Me.ArchiveComment.Encoding, Encoding.Default)

                    ' Specify the character set to read comment file.
                    sb.Append(" -sc")
                    Select Case encoding.CodePage
                        Case Encoding.UTF8.CodePage
                            sb.Append("f"c)
                        Case Encoding.Unicode.CodePage
                            sb.Append("u"c)
                        Case Else
                            sb.Append("a"c)
                    End Select
                    sb.Append("c"c)

                    ' Specify the comment file path.
                    sb.Append($" -z""{Me.ArchiveComment.FilePath}""")
                End If

            End If

            ' File types to store without compression.
            If Me.FileTypesToStore IsNot Nothing AndAlso Me.FileTypesToStore.Count <> 0 Then

                If Me.CompressionMode <> RarCompressionMode.Store Then
                    sb.Append(" -ms""")
                    sb.Append(String.Join(";"c, Me.FileTypesToStore.Select(Function(ext As String) ext.Trim())))
                    sb.Append(""""c)
                Else
                    sb.Append(" -ms""*""")
                End If
            End If

            ' File Exclusion Masks
            If Me.FileExclusionMasks IsNot Nothing AndAlso Me.FileExclusionMasks.Count <> 0 Then

                For Each exclusionMask As String In Me.FileExclusionMasks
                    sb.Append($" -x""{exclusionMask}""")
                Next exclusionMask
            End If

#End Region

#Region " Switch termination arguments "

            ' Disable file lists (all such parameters found after this switch will be considered as file names, not file lists.
            sb.Append(" -@")

            ' Stop switches scanning (e.g. 'rar.exe a -s -- "-StrangeName.rar"').
            sb.Append(" --")

#End Region

#Region " Ending required argument: Path to the RAR archive, and file(s) to include in the archive "

            ' Archive File Path
            If Not String.IsNullOrWhiteSpace(Me.Archive) Then
                sb.Append($" ""{Me.Archive}""")
            End If

            ' File and Dir Masks
            If Me.FileAndDirMasks IsNot Nothing AndAlso Me.FileAndDirMasks.Count <> 0 Then
                Dim paths As String = String.Join(" "c, Array.ConvertAll(Me.FileAndDirMasks.ToArray(), Function(x As String) $"""{x}"""))
                sb.Append($" {paths}")
            End If

#End Region

            Return sb.ToString()
        End Function

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Returns a string that represents the current <see cref="RarCreationCommand"/> instance.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A <see cref="String"/> that represents the current <see cref="RarCreationCommand"/> instance.
        ''' </returns>
        Public Overrides Function ToString() As String

            Return $"""{Me.RarExecPath}"" {Me.GetRarCommandlineArguments()}"
        End Function

#End Region

    End Class

End Namespace

#End Region
