' ***********************************************************************
' Author   : ElektroStudios
' Modified : 12-July-2023
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Security

#End Region

#Region " SecureString Extensions "

' ReSharper disable once CheckNamespace

Namespace DevCase.Extensions.SecureStringExtensions

    ''' <summary>
    ''' Provides method extensions for the <see cref="SecureString"/> type.
    ''' </summary>
    <HideModuleName>
    Public Module SecureStringExtensions

#Region " Public Extension Methods "

        ''' <summary>
        ''' Converts the source <see cref="SecureString"/> to a managed <see cref="String"/>.
        ''' </summary>
        '''
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim secStr As New SecureString()
        ''' With secStr
        '''     .AppendChar("q"c)
        '''     .AppendChar("w"c)
        '''     .AppendChar("e"c)
        '''     .AppendChar("r"c)
        '''     .AppendChar("t"c)
        '''     .AppendChar("y"c)
        ''' End With
        ''' 
        ''' MessageBox.Show(secStr.ToManagedString())
        ''' </code>
        ''' </example>
        '''
        ''' <param name="secureString">
        ''' The source <see cref="SecureString"/>.
        ''' </param>
        '''
        ''' <returns>
        ''' The resulting <see cref="String"/>.
        ''' </returns>
        <DebuggerStepThrough>
        <Extension>
        <EditorBrowsable(EditorBrowsableState.Always)>
        Public Function ToManagedString(secureString As SecureString) As String

            If secureString Is Nothing Then
                Throw New ArgumentNullException(NameOf(secureString))
            End If

            If secureString.Length = 0 Then
                Return ""

            Else
                Dim ptr As IntPtr = Global.System.IntPtr.Zero

                Try
                    ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString)
                    Return Marshal.PtrToStringUni(ptr)

                Finally
                    If ptr <> IntPtr.Zero Then
                        Marshal.ZeroFreeGlobalAllocUnicode(ptr)
                    End If

                End Try

            End If

        End Function

#End Region

    End Module

End Namespace

#End Region
