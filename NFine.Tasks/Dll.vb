Option Strict Off
Option Explicit On 
Module Dll
    Public Declare Function Connect Lib "commpro.dll" (ByVal Parameters As String) As Integer
    Public Declare Function Disconnect Lib "commpro.dll" (ByVal handle As Integer) As Integer
    Public Declare Function DataQuery Lib "commpro.dll" (ByVal handle As Integer, ByVal Buffer As String, ByVal BufferSize As Integer, ByVal TableName As String, ByVal FieldNames As String, ByVal Filter As String, ByVal Options As String) As Integer
    Public Declare Function DataCount Lib "commpro.dll" (ByVal handle As Integer, ByVal TableName As String, ByVal Filter As String, ByVal Options As String) As Integer
    Public Declare Function DataAppend Lib "commpro.dll" (ByVal handle As Integer, ByVal TableName As String, ByVal Data As String, ByVal Options As String) As Integer
    Public Declare Function DataUpdate Lib "commpro.dll" (ByVal handle As Integer, ByVal TableName As String, ByVal DataValues As String, ByVal Filter As String, ByVal Options As String) As Integer
    Public Declare Function DataDelete Lib "commpro.dll" (ByVal handle As Integer, ByVal TableName As String, ByVal Filter As String, ByVal Options As String) As Integer
    Public Declare Function InfoQuery Lib "commpro.dll" (ByVal handle As Integer, ByVal Buffer As String, ByVal BufferSize As Integer, ByVal Items As String) As Integer
    Public Declare Function InfoUpdate Lib "commpro.dll" (ByVal handle As Integer, ByVal ItemAndValues As String) As Integer
    Public Declare Function SynTime Lib "commpro.dll" (ByVal handle As Integer) As Integer
    Public Declare Function GetRealTimeDatas Lib "commpro.dll" (ByVal handle As Integer, ByVal TableName As String, ByVal Filter As String, ByVal Buffer As String, ByVal BufferSize As Integer) As Integer
    Public Declare Function GetRTLog Lib "commpro.dll" (ByVal handle As Integer, ByVal Buffer As String, ByVal BufferSize As Integer) As Integer

End Module
