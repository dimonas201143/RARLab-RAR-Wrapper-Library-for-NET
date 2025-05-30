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

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarPriorityOptions "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents the options for process priority and system load of <c>rar.exe</c> process in multitasking environment.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Class RarPriorityOptions

        ''' <summary>
        ''' Gets or sets the <c>rar.exe</c> process priority.
        ''' <para></para>
        ''' Possible priority values is a value from 0 to 15. 
        ''' 1 meaning the lowest possible priority, and 15 the highest possible. 
        ''' <para></para>
        ''' If 0 (zero), <c>rar.exe</c> uses the default task priority.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The <c>rar.exe</c> process priority.
        ''' </value>
        <DisplayName("Priority")>
        <Description("Specifies the rar.exe process priority. If set to 0, rar.exe uses the default task priority.")>
        Public Property Priority As Short

        ''' <summary>
        ''' Gets or sets the period of time, in milliseconds, that <c>rar.exe</c> gives back to the 
        ''' system after read or write operations while compressing or extracting.
        ''' <para></para>
        ''' A non-zero value may be useful if you need to reduce system load even 
        ''' more than can be achieved with a low <see cref="RarPriorityOptions.Priority"/>.
        ''' <para></para>
        ''' Default value is 0 (zero).
        ''' </summary>
        ''' 
        ''' <value>
        ''' The period of time that <c>rar.exe</c> gives back to the system after read or write operations while compressing or extracting.
        ''' </value>
        <DisplayName("Sleep Time (in milliseconds)")>
        <Description("Specifies the period of time, in milliseconds, that <c>rar.exe</c> gives back to the system after read or write operations while compressing or extracting.")>
        Public Property SleepTime As Short

    End Class

End Namespace

#End Region
