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

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Text

'Imports DevCase.Core.IO.FileSystem
'Imports DevCase.Runtime.Attributes
'Imports DevCase.Runtime.TypeConverters

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarCommandBase "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR.Commands

    ''' <summary>
    ''' Represents the base <c>rar.exe</c> options that can be extended by specialized classes. 
    ''' <para></para>
    ''' This class must be inherited.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public MustInherit Class RarCommandBase : Implements IRarCommand

#Region " Properties "

#If Not NETCOREAPP Then
        ''' <summary>
        ''' Gets or sets the path to the <c>rar.exe</c> executable file (not <c>WinRAR.exe</c>).
        ''' <para></para>
        ''' Default value is <c>".\rar.exe"</c>
        ''' </summary>
        ''' 
        ''' <value>
        ''' Path to the <c>rar.exe</c> executable file.
        ''' </value>
        <DisplayName("rar.exe File Path")>
        <Description("Absolute or relative path to the rar.exe executable file (not WinRAR.exe)")>
        <DefaultValue(".\rar.exe")>
        Public Property RarExecPath As String = $"{My.Application.Info.DirectoryPath}\rar.exe" Implements IRarCommand.RarExecPath
        ' UtilFileSystem.GetRelativePath(My.Application.Info.DirectoryPath, $"{My.Application.Info.DirectoryPath}\rar.exe") Implements IRarCommand.RarExecPath
#Else
        ''' <summary>
        ''' Gets or sets the path to the <c>rar.exe</c> executable file (not <c>WinRAR.exe</c>).
        ''' <para></para>
        ''' Default value is <c>".\rar.exe"</c>
        ''' </summary>
        ''' 
        ''' <value>
        ''' Path to the <c>rar.exe</c> executable file.
        ''' </value>
        <DisplayName("rar.exe File Path")>
        <Description("Absolute or relative path to the rar.exe executable file (not WinRAR.exe)")>
        <DefaultValue(".\rar.exe")>
        Public Property RarExecPath As String = $"{AppContext.BaseDirectory}\rar.exe" Implements IRarCommand.RarExecPath
        ' UtilFileSystem.GetRelativePath(AppContext.BaseDirectory, $"{AppContext.BaseDirectory}\rar.exe") Implements IRarCommand.RarExecPath
#End If

        ''' <summary>
        ''' Gets or sets the RARLabs's WinRAR product license string ('rareg.key').
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The RARLabs's WinRAR product license string ('rareg.key'); or <c>null</c> if no product license is specified.
        ''' </value>
        <DisplayName("RAR License Data")>
        <Description("The RARLabs's WinRAR product license string ('rareg.key').")>
        <DefaultValue("")>
        Public Property RarLicenseData As String

        ''' <summary>
        ''' Gets or sets the file path of the RAR archive to process.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The file path of the RAR archive to process.
        ''' </value>
        <DisplayName("Archive")>
        <Description("The file path of the RAR archive to process.")>
        Public MustOverride Property Archive As String

        ''' <summary>
        ''' Gets or sets a value indicating whether logging <c>rar.exe</c> messages to a log file is enabled. 
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to enable logging <c>rar.exe</c> messages to a log file; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Enable Error Logging")>
        <Description("Enables or disables logging rar.exe messages to a log file.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property LogEnabled As Boolean = False

#If Not NETCOREAPP Then
        ''' <summary>
        ''' Gets or sets the path to the log file where <c>rar.exe</c> error messages are stored 
        ''' when <see cref="LogEnabled"/> is <see langword="True"/>.
        ''' <para></para>
        ''' Default value is <c>".\RAR.log"</c>
        ''' </summary>
        ''' 
        ''' <value>
        ''' Path to the log file where <c>rar.exe</c> error messages are stored.
        ''' </value>
        <DisplayName("Log File Path")>
        <Description("Absolute or relative path to the error log file when logging is enabled.")>
        <DefaultValue(".\RAR.log")>
        Public Property LogFilePath As String = $"{My.Application.Info.DirectoryPath}\RAR.log"
        'UtilFileSystem.GetRelativePath(My.Application.Info.DirectoryPath, $"{My.Application.Info.DirectoryPath}\RAR.log")
#Else
        ''' <summary>
        ''' Gets or sets the path to the log file where <c>rar.exe</c> error messages are stored 
        ''' when <see cref="LogEnabled"/> is <see langword="True"/>.
        ''' <para></para>
        ''' Default value is <c>".\RAR.log"</c>
        ''' </summary>
        ''' 
        ''' <value>
        ''' Path to the log file where <c>rar.exe</c> error messages are stored.
        ''' </value>
        <DisplayName("Log File Path")>
        <Description("Absolute or relative path to the error log file when logging is enabled.")>
        <DefaultValue(".\RAR.log")>
        Public Property LogFilePath As String = $"{AppContext.BaseDirectory}\RAR.log"
        ' UtilFileSystem.GetRelativePath(AppContext.BaseDirectory, $"{AppContext.BaseDirectory}\RAR.log")
#End If

        ''' <summary>
        ''' Gets or sets a value indicating whether notification sounds are enabled in <c>rar.exe</c> process.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to enable notification sounds in <c>rar.exe</c> process; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Enable Notification Sounds")>
        <Description("Enables or disables notification sounds in rar.exe process.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property EnableNotificationSounds As Boolean = False

        ''' <summary>
        ''' Gets or sets a value indicating whether comments should be shown in <c>rar.exe</c> output. 
        ''' <para></para>
        ''' Default value is <see langword="True"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to show comments in <c>rar.exe</c> output; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Enable Comments Show")>
        <Description("Enables or disables comments show in rar.exe output.")>
        <DefaultValue(True)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property CommentsShow As Boolean = True

        ''' <summary>
        ''' Gets or sets the messages to display in <c>rar.exe</c> output.
        ''' <para></para>
        ''' Default value is <see cref="RarDisplayMessages.None"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating the messages to display in <c>rar.exe</c> output.
        ''' </value>
        <DisplayName("Display Messages")>
        <Description("Indicates the messages to display in rar.exe output.")>
        <DefaultValue(GetType(RarDisplayMessages), "All")> ' <TypeConverter(GetType(EnumDescriptionConverter))>
        Public Property DisplayMessages As RarDisplayMessages = RarDisplayMessages.All

        ''' <summary>
        ''' Gets or sets a value indicating whether to disable all messages in <c>rar.exe</c> output.
        ''' <para></para>
        ''' If <see langword="True"/>, <c>rar.exe</c> output will become completely empty (null) regardless of <see cref="RarCommandBase.DisplayMessages"/> value. 
        ''' <para></para>
        ''' This is useful if you don't care about retrieving standard and error messages from <c>rar.exe</c> output.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to to disable all messages in <c>rar.exe</c> output; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Silent Mode")>
        <Description("Enables or disables all messages in rar.exe output.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property SilentMode As Boolean = False

        ''' <summary>
        ''' Gets or sets a value indicating whether to ignore the RAR configuration file and RARINISWITCHES environment variable.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to ignore RAR configuration file and RARINISWITCHES environment variable; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Ignore Configuration File")>
        <Description("Indicates whether to ignore RAR configuration file and RARINISWITCHES environment variable.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property IgnoreConfigurationFile As Boolean = False

        ''' <summary>
        ''' Gets or sets the directory path where <c>rar.exe</c> creates temporary files during certain operations,
        ''' such as modifying an existing archive.
        ''' <para></para>
        ''' Note: The specified directory must already exist.
        ''' <para></para>
        ''' If this value is <c>null</c>, the working directory defaults to the same location as the RAR archive.
        ''' <para></para>
        ''' Default value is <c>null</c>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The full path to the working directory used by <c>rar.exe</c> for temporary files; 
        ''' or <c>null</c>, meaning the archive's directory is used.
        ''' </value>
        <DisplayName("Work Directory")>
        <Description("The directory used by rar.exe to store temporary files during certain operations like archive modification.")>
        Public Property WorkDirectory As String = Nothing

        ''' <summary>
        ''' Gets or sets the recommended maximum number of active threads for compression algorithm also as for other RAR modules, 
        ''' which can start several threads. While RAR attempts to follow this recommendation, 
        ''' sometimes the real number of active threads can exceed the specified value
        ''' <para></para>
        ''' This value slightly affects the compression ratio, so archives created with 
        ''' different thread count will not be exactly the same even if all other compression settings are equal.
        ''' <para></para>
        ''' If this value is 0 (zero), RAR will try to detect the number of 
        ''' available processors and select the optimal number of threads automatically.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The recommended maximum number of active threads for compression algorithm also as for other RAR modules, which can start several threads.
        ''' </value>
        <DisplayName("Thread Count")>
        <Description("The recommended maximum number of active threads for compression algorithm also as for other RAR modules, which can start several threads.")>
        <DefaultValue(0)>
        Public Property ThreadCount As Integer = 0

        ''' <summary>
        ''' Gets or sets a value indicating whether to use large memory pages when archiving and extracting.
        ''' <para></para>
        ''' When this option is <see langword="True"/>, RAR uses larger memory allocation units, 
        ''' which can improve archiving and, in some cases, extraction speed. 
        ''' <para></para>
        ''' Typically the performance gain is more significant for bigger compression dictionaries and slower compression methods.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <remarks>
        ''' Using this option requires "Lock pages in memory" privilege and if it is missing, 
        ''' RAR proposes to assign it to the current user account, making it available for other software too. 
        ''' Windows restart is necessary to activate the newly assigned privilege.
        ''' <para></para>
        ''' Large memory pages can't be placed to Windows swap file and always occupy the physical memory. 
        ''' When this switch is present, Windows Task Manager can display incorrect memory usage values for RAR.
        ''' </remarks>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to enables using large memory pages when archiving and extracting; 
        ''' otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Use Large Memory Pages")>
        <Description("Enables or disables using large memory pages when archiving and extracting.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property UseLargeMemoryPages As Boolean = False

        ''' <summary>
        ''' Gets or sets a value indicating whether to process files in all sub-directories as well as the current working directory. 
        ''' <para></para>
        ''' When used with the commands 'a', 'u', 'f', 'm', 
        ''' will process files in all sub-directories as well as the current working directory.
        ''' <para></para>
        ''' When used with the commands x, e, t, p, v, l, c, cf or s, 
        ''' will process all archives in sub-directories as well as the current working directory.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' A value indicating whether to process files in all sub-directories as well as the current working directory.
        ''' </value>
        <DisplayName("Recurse Subdirectories")>
        <Description("Enables or disables file recursion.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property RecurseSubdirectories As Boolean = False

        ''' <summary>
        ''' Gets or sets an object that regulates priority and system load of <c>rar.exe</c> process. 
        ''' </summary>
        ''' 
        ''' <value>
        ''' An object that regulates priority and system load of <c>rar.exe</c> process. 
        ''' </value>
        <DisplayName("Priority Options")>
        <Description("An object that regulates priority and system load of rar.exe process.")>
        Public Property PriorityOptions As New RarPriorityOptions()

        ''' <summary>
        ''' Gets or sets a value indicating whether to assume 'Yes' on all <c>rar.exe</c> queries. 
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' <see langword="True"/> to assume 'Yes' on all <c>rar.exe</c> queries; otherwise, <see langword="False"/>.
        ''' </value>
        <DisplayName("Assume Yes on All Queries")>
        <Description("Assume 'Yes' on all rar.exe queries.")>
        <DefaultValue(False)> ' <LocalizableBoolean> <TypeConverter(GetType(LocalizableBooleanConverter))>
        Public Property AssumeYesOnAllQueries As Boolean = False

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Returns a <c>rar.exe</c> command-line arguments string 
        ''' based on the property values of this <see cref="RarCommandBase"/>.
        ''' <para></para>
        ''' Note that these arguments does not perform any operation on <c>rar.exe</c>;
        ''' This function should be overriden by derived class to retrieve these base arguments 
        ''' and add more to have a fully functional <c>rar.exe</c> command-line arguments string 
        ''' that performs operations like compression, extraction, etc.
        ''' </summary>
        ''' 
        ''' <returns>
        ''' A string with all configured <c>rar.exe</c> command-line base arguments.
        ''' </returns>
        <DebuggerStepThrough>
        Protected Overridable Function GetRarCommandlineArguments() As String Implements IRarCommand.GetRarCommandlineArguments

            Dim sb As New StringBuilder()

            ' Assume 'Yes' on All Queries
            If Me.AssumeYesOnAllQueries Then
                sb.Append(" -y")
            End If

            ' Log Enabled
            If Me.LogEnabled Then
                sb.Append($" -ilog""{Me.LogFilePath}""")
            End If

            ' Enable Notification Sounds
            If Me.EnableNotificationSounds Then
                sb.Append(" -isnd")
            Else
                sb.Append(" -isnd-")
            End If

            ' Comments Show
            If Not Me.CommentsShow Then
                sb.Append(" -c-")
            End If

            ' Ignore Configuration File
            If Me.IgnoreConfigurationFile Then
                sb.Append(" -cfg-")
            End If

            ' Display Messages
            If Me.DisplayMessages = RarDisplayMessages.None Then
                sb.Append($" -idq")

            ElseIf Me.DisplayMessages <> RarDisplayMessages.All Then
                sb.Append($" -id")

                If Not Me.DisplayMessages.HasFlag(RarDisplayMessages.Copyright) Then
                    sb.Append($"c")
                End If

                If Not Me.DisplayMessages.HasFlag(RarDisplayMessages.Done) Then
                    sb.Append($"d")
                End If

                If Not Me.DisplayMessages.HasFlag(RarDisplayMessages.ArchivedNames) Then
                    sb.Append($"n")
                End If

                If Not Me.DisplayMessages.HasFlag(RarDisplayMessages.PercentageIndicator) Then
                    sb.Append($"p")
                End If

            End If

            ' Silent Mode
            If Me.SilentMode Then
                sb.Append($" -inul")
            End If

            ' Priority and Sleep Time
            If Me.PriorityOptions IsNot Nothing Then
                sb.Append($" -ri{Me.PriorityOptions.Priority}:{Me.PriorityOptions.SleepTime}")
            End If

            ' Thread Count
            If Me.ThreadCount <> 0 Then
                sb.Append($" -mt{Me.ThreadCount}")
            End If

            ' Use Large Memory Pages
            If Me.UseLargeMemoryPages Then
                sb.Append(" -mlp")
            End If

            ' Recurse Subdirectories
            If Me.RecurseSubdirectories Then
                sb.Append(" -r")
            Else
                sb.Append(" -r-")
            End If

            ' Work Directory.
            If Not String.IsNullOrWhiteSpace(Me.WorkDirectory) Then
                sb.Append($" -w""{Me.WorkDirectory}""")
            End If

            Return sb.ToString()
        End Function

#End Region

#Region " Constructos "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarCommandBase"/> class.
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

    End Class

End Namespace

#End Region
