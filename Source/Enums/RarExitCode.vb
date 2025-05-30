' ***********************************************************************
' Author   : ElektroStudios
' Modified : 17-May-2025
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarExitCode "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Specifies the exit codes returned by the <c>rar.exe</c> process.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Enum RarExitCode As Integer

        ''' <summary>
        ''' Successful operation.
        ''' </summary>
        Successful = 0

        ''' <summary>
        ''' Non fatal error(s) occurred.
        ''' </summary>
        NonFatalError = 1

        ''' <summary>
        ''' A fatal error occurred.
        ''' </summary>
        FatalError = 2

        ''' <summary>
        ''' Invalid checksum. Data is damaged.
        ''' </summary>
        InvalidChecksum = 3

        ''' <summary>
        ''' Attempt to modify an archive locked by '-k' command.
        ''' </summary>
        ArchiveLocked = 4

        ''' <summary>
        ''' Write error.
        ''' </summary>
        WriteError = 5

        ''' <summary>
        ''' File open error.
        ''' </summary>
        FileOpenError = 6

        ''' <summary>
        ''' Wrong command-line option.
        ''' </summary>
        WrongCommandlineOption = 7

        ''' <summary>
        ''' Not enough memory.
        ''' </summary>
        NotEnoughMemory = 8

        ''' <summary>
        ''' File create error.
        ''' </summary>
        FileCreateError = 9

        ''' <summary>
        ''' No files matching the specified mask and options were found.
        ''' </summary>
        NoFileMaskMatch = 10

        ''' <summary>
        ''' Wrong password.
        ''' </summary>
        WrongPassword = 11

        ''' <summary>
        ''' Read error.
        ''' </summary>
        ReadError = 12

        ''' <summary>
        ''' Bad archive.
        ''' </summary>
        BadArchive = 13

        ''' <summary>
        ''' User stopped the process.
        ''' </summary>
        UserStop = 255

    End Enum

End Namespace

#End Region
