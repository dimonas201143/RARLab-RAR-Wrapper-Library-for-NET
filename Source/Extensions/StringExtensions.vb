' ***********************************************************************
' Author   : ElektroStudios
' Modified : 24-April-2024
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Security
Imports System.Diagnostics

#If Not NETCOREAPP Then
'Imports SupportedOSPlatform = DevCase.ProjectMigration.SupportedOSPlatformAttribute
#Else
'Imports System.Runtime.Versioning
#End If

#End Region

#Region " String Extensions "

' ReSharper disable once CheckNamespace

Namespace DevCase.Extensions.StringExtensions

    ''' <summary>
    ''' Provides method extensions for a <see cref="String"/> type.
    ''' </summary>
    <HideModuleName>
    Public Module StringExtensions

#Region " Public Extension Methods "

        ''' <summary>
        ''' Determines whether the end of the source string matches the specified string.
        ''' <para></para>
        ''' If does not, it appends the specified string at the end of the source string.
        ''' </summary>
        '''
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim str As String = "Hello ".EnsureEndsWith("world.", StringComparison.OrdinalIgnoreCase)
        ''' </code>
        ''' </example>
        '''
        ''' <param name="str">
        ''' The source <see cref="String"/>.
        ''' </param>
        ''' 
        ''' <param name="value">
        ''' The string to match.
        ''' </param>
        ''' 
        ''' <param name="comparisonType">
        ''' One of the enumeration values that determines how this string and value are compared.
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' value
        ''' </exception>
        '''
        ''' <returns>
        ''' <see langword="True"/> if the end of the source string matches the specified string; 
        ''' otherwise, <see langword="False"/>.
        ''' </returns>
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function EnsureEndsWith(str As String, value As String,
                                       comparisonType As StringComparison) As String

            If String.IsNullOrEmpty(value) Then
                Throw New ArgumentNullException(paramName:=NameOf(value))

            Else
                Return If(str.EndsWith(value, comparisonType), str, str & value)

            End If

        End Function

        ''' <summary>
        ''' Converts the source <see cref="String"/> to a <see cref="SecureString"/>.
        ''' </summary>
        '''
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim secstr As SecureString = "PASSWORD".ToSecureString()
        ''' </code>
        ''' </example>
        '''
        ''' <param name="sender">
        ''' The source <see cref="String"/>.
        ''' </param>
        ''' 
        ''' <param name="readOnly">
        ''' If <see langword="True"/>, makes read-only the resulting <see cref="SecureString"/>.
        ''' <para></para>
        ''' Default value is <see langword="False"/>.
        ''' </param>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' value
        ''' </exception>
        '''
        ''' <returns>
        ''' The resulting <see cref="SecureString"/>.
        ''' </returns>
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ToSecureString(sender As String, Optional [readOnly] As Boolean = False) As SecureString

            Dim secureString As New SecureString()

            For Each c As Char In sender
                secureString.AppendChar(c)
            Next

            If [readOnly] Then
                secureString.MakeReadOnly()
            End If

            Return secureString
        End Function

#End Region

    End Module

End Namespace

#End Region
