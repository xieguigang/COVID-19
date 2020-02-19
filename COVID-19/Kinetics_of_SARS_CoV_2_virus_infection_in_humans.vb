﻿Imports SMRUCC.Rsharp.Interpreter
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.System.Configuration

Public Class Kinetics_of_SARS_CoV_2_virus_infection_in_humans

    Dim modelScript$
    ''' <summary>
    ''' Config file path of the R# interpreter
    ''' </summary>
    Dim config$

    Sub New(modelScript As String, Optional config$ = Nothing)
        Me.config = config
        Me.modelScript = modelScript
    End Sub

    ''' <summary>
    ''' 创建一个行政区域的动力学模型对象实例
    ''' </summary>
    ''' <param name="y0"></param>
    ''' <returns></returns>
    Public Function CreateInstance(y0 As InitialStatus)
        Dim config As Options = If(Me.config.FileExists, New Options(Me.config), Nothing)
        Dim engine As New RInterpreter(config) With {.debug = True}
        Dim handle As Action(Of Environment) =
            Sub(env As Environment)

            End Sub

        Call engine.Evaluate(modelScript)
        Call engine.Add("y0", engine.Evaluate($"list(
            T = {y0.T},  # 当前行政区域的总人口
            C = {y0.C},  # 初期没有人拥有抗体
            S = {y0.S},  # 最初只有一个潜伏期病人
            I = {y0.I},  # 最初没有患者
            D = {y0.D}
        )"))
        Call engine.Add("tick", CObj(handle))

        Dim iterator = engine.Evaluate("deSolve(
            Kinetics_of_SARS_CoV_2_virus_infection_in_humans, y0, 
            a = 0, 
            b = 31, 
            resolution = 10000,
            tick = tick
        );")

        Return iterator
    End Function
End Class

Public Class InitialStatus

    Public Property T As Integer
    Public Property C As Integer
    Public Property S As Integer = 1
    Public Property I As Integer
    Public Property D As Integer

End Class