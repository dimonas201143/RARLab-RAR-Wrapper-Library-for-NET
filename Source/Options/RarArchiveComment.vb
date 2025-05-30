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
Imports System.IO
Imports System.Text

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " RarArchiveComment "

' ReSharper disable once CheckNamespace

Namespace DevCase.RAR

    ''' <summary>
    ''' Represents a comment for a RAR archive.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://www.win-rar.com/">RARLab's rar.exe (from WinRAR)</see>
    ''' </remarks>
    Public Class RarArchiveComment

#Region " Properties "

        ''' <summary>
        ''' Gets the commentary string to add it to the RAR archive.
        ''' <para></para>
        ''' </summary>
        ''' 
        ''' <value>
        ''' The commentary string to add it to the RAR archive.
        ''' </value>
        <DisplayName("Comment")>
        <Description("The commentary string to add it to the RAR archive.")>
        Public ReadOnly Property Comment As String

        ''' <summary>
        ''' Gets the text encoding to write the <see cref="RarArchiveComment.Comment"/> string 
        ''' to the temporary <see cref="RarArchiveComment.FilePath"/>.
        ''' </summary>
        ''' 
        ''' <value>
        ''' The text encoding to write the <see cref="RarArchiveComment.Comment"/> string 
        ''' to the temporary <see cref="RarArchiveComment.FilePath"/>
        ''' </value>
        <DisplayName("Encoding")>
        <Description("The text encoding to read/write the comment string to the temporary file.")>
        Public ReadOnly Property Encoding As Encoding

        ''' <summary>
        ''' Gets the unique path to the temporary file where the <see cref="RarArchiveComment.Comment"/> string 
        ''' is written using the specified <see cref="RarArchiveComment.Encoding"/>.
        ''' </summary>
        <Browsable(False)>
        Public ReadOnly Property FilePath As String = Path.GetTempFileName()

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="RarArchiveComment"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RarArchiveComment"/> class.
        ''' </summary>
        ''' 
        ''' <param name="comment">
        ''' The commentary string to add it to the RAR archive.
        ''' </param>
        ''' 
        ''' <param name="encoding">
        ''' Optional. The text encoding to write the <paramref name="comment"/> string 
        ''' to the temporary <see cref="RarArchiveComment.FilePath"/>
        ''' <para></para>
        ''' Default value is <see cref="System.Text.Encoding.Default"/>.
        ''' </param>
        ''' 
        ''' <exception cref="System.ArgumentException">
        ''' Comment cannot be null, empty, or whitespace.
        ''' </exception>
        <DebuggerStepThrough>
        Public Sub New(comment As String, Optional encoding As Encoding = Nothing)

            If String.IsNullOrWhiteSpace(comment) Then
                Throw New ArgumentException("Comment cannot be null, empty, or whitespace.", NameOf(comment))
            End If

            Me.Encoding = If(encoding, Encoding.Default)
            Me.Comment = comment

            Me.WriteCommentToFile()
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Writes the <see cref="RarArchiveComment.Comment"/> string to the temporary <see cref="RarArchiveComment.FilePath"/>.
        ''' </summary>
        ''' 
        ''' <exception cref="InvalidOperationException">
        ''' Failed to write the RAR comment to the temporary file.
        ''' </exception>
        <DebuggerStepThrough>
        Protected Sub WriteCommentToFile()

            Try
                File.WriteAllText(Me.FilePath, Me.Comment, Me.Encoding)

            Catch ex As IOException
                Throw New InvalidOperationException("Failed to write the RAR comment string to the temporary file.", ex)

            Catch ex As Exception
                Throw

            End Try
        End Sub

#End Region

    End Class

End Namespace

#End Region
